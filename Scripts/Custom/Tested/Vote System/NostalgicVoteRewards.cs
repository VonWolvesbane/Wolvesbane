using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

using Server;
using Server.Accounting;
using Server.Items;
using Server.Mobiles;

namespace Server.Voting
{
    /// <summary>
    /// Verified Nostalgic.gg vote rewards for Wolvesbane.
    /// Nostalgic returns pb_name, the in-game character name entered by the voter.
    /// </summary>
    public static class NostalgicVoteRewards
    {
        private const string EndpointUrl = "https://wolvesbaneuo.com/vote/wbnost_390ad1112832ec35.php";
        private const string ClaimKey = "3f5203c7617c902977081e702aa8cbbfbac5790c24ccdee7";

        private static readonly TimeSpan PollInterval = TimeSpan.FromMinutes(1.0);
        private static readonly TimeSpan InitialDelay = TimeSpan.FromSeconds(40.0);
        private static bool _Polling;
        private static RewardPollTimer _Timer;

        public static void Initialize()
        {
            if (_Timer == null)
            {
                _Timer = new RewardPollTimer();
                _Timer.Start();
            }
        }

        private static void BeginPoll()
        {
            if (_Polling) return;
            _Polling = true;
            ThreadPool.QueueUserWorkItem(delegate
            {
                List<RemoteVoteEvent> events = null;
                try { events = PullPendingEvents(); }
                catch (Exception e) { Console.WriteLine("[Vote System] Nostalgic.gg poll failed: {0}", e.Message); }

                Timer.DelayCall(TimeSpan.Zero, delegate
                {
                    try { if (events != null) ProcessEvents(events); }
                    finally { _Polling = false; }
                });
            });
        }

        private static List<RemoteVoteEvent> PullPendingEvents()
        {
            string url = EndpointUrl + "?mode=pull&key=" + Uri.EscapeDataString(ClaimKey);
            string response = HttpGet(url);
            List<RemoteVoteEvent> list = new List<RemoteVoteEvent>();
            if (String.IsNullOrEmpty(response)) return list;

            using (StringReader reader = new StringReader(response))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.Length == 0) continue;
                    string[] parts = line.Split('|');
                    if (parts.Length < 2) continue;
                    string id = parts[0].Trim();
                    string playerName = Uri.UnescapeDataString(parts[1].Trim());
                    string ipHash = parts.Length > 2 ? parts[2].Trim() : String.Empty;
                    if (id.Length > 0 && IsValidPlayerName(playerName))
                        list.Add(new RemoteVoteEvent(id, playerName, ipHash));
                }
            }
            return list;
        }

        private static bool IsValidPlayerName(string name)
        {
            if (String.IsNullOrEmpty(name) || name.Length > 64) return false;
            for (int i = 0; i < name.Length; i++)
            {
                if (Char.IsControl(name[i]) || name[i] == '|') return false;
            }
            return true;
        }

        private static void ProcessEvents(List<RemoteVoteEvent> events)
        {
            for (int i = 0; i < events.Count; i++)
            {
                RemoteVoteEvent vote = events[i];
                Mobile recipient = FindRecipient(vote.PlayerName);
                if (recipient == null)
                {
                    Console.WriteLine("[Vote System] Nostalgic.gg vote {0} could not resolve character '{1}'; leaving queued.", vote.Id, vote.PlayerName);
                    continue;
                }

                if (GiveNewVoteToken(recipient))
                {
                    Account account = recipient.Account as Account;
                    if (account != null)
                        account.SetTag("WB_NOSTALGIC_LAST_VERIFIED", DateTime.UtcNow.ToString("o"));

                    if (recipient.NetState != null)
                    {
                        recipient.SendMessage(0x59, "Nostalgic.gg confirmed your vote. You received 1 New Vote Token. Thank you for supporting Wolvesbane!");
                        recipient.FixedEffect(0x376A, 10, 16);
                    }
                    QueueAck(vote.Id);
                }
            }
        }

        private static Mobile FindRecipient(string playerName)
        {
            Mobile firstMatch = null;
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m == null || m.Deleted || String.IsNullOrEmpty(m.Name)) continue;
                if (!String.Equals(m.Name, playerName, StringComparison.OrdinalIgnoreCase)) continue;
                if (!(m.Account is Account)) continue;
                if (m.NetState != null) return m;
                if (firstMatch == null) firstMatch = m;
            }
            return firstMatch;
        }

        private static bool GiveNewVoteToken(Mobile recipient)
        {
            if (recipient == null || recipient.Deleted) return false;
            NewVoteToken reward = new NewVoteToken();
            if (recipient.NetState != null && recipient.Backpack != null && recipient.Backpack.TryDropItem(recipient, reward, false)) return true;
            if (recipient.BankBox != null) { recipient.BankBox.DropItem(reward); return true; }
            reward.Delete();
            return false;
        }

        private static void QueueAck(string eventId)
        {
            if (String.IsNullOrEmpty(eventId)) return;
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    string url = EndpointUrl + "?mode=ack&key=" + Uri.EscapeDataString(ClaimKey) + "&id=" + Uri.EscapeDataString(eventId);
                    HttpGet(url);
                }
                catch (Exception e) { Console.WriteLine("[Vote System] Nostalgic.gg ACK failed for {0}: {1}", eventId, e.Message); }
            });
        }

        private static string HttpGet(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null) return String.Empty;
            request.Method = "GET";
            request.Timeout = 7000;
            request.ReadWriteTimeout = 7000;
            request.UserAgent = "Wolvesbane-UO-VoteSystem/8.0";
            request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null) return String.Empty;
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8)) return reader.ReadToEnd();
            }
        }

        private sealed class RemoteVoteEvent
        {
            public readonly string Id;
            public readonly string PlayerName;
            public readonly string IPHash;
            public RemoteVoteEvent(string id, string playerName, string ipHash) { Id = id; PlayerName = playerName; IPHash = ipHash; }
        }

        private sealed class RewardPollTimer : Timer
        {
            public RewardPollTimer() : base(InitialDelay, PollInterval) { Priority = TimerPriority.OneMinute; }
            protected override void OnTick() { BeginPoll(); }
        }
    }
}
