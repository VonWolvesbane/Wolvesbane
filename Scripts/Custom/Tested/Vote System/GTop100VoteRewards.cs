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
    /// <summary>Verified GTop100 vote rewards for Wolvesbane.</summary>
    public static class GTop100VoteRewards
    {
        private const string AccountTokenTag = "WB_GTOP100_VOTE_TOKEN";
        private const string GTop100VoteBaseUrl = "https://gtop100.com/Ultima-Online/Wolvesbane-96694?vote=1&pingUsername=";
        private const string EndpointUrl = "https://wolvesbaneuo.com/vote/wbgt_4f5fb1b4ff817495.php";
        private const string ClaimKey = "8f6d97217bd3c005ff6f91e73339bbd1d55880d3148efe52";

        private static readonly TimeSpan PollInterval = TimeSpan.FromMinutes(1.0);
        private static readonly TimeSpan InitialDelay = TimeSpan.FromSeconds(50.0);
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

        public static string GetVoteUrl(Mobile from)
        {
            if (from == null || from.Deleted) return null;
            Account account = from.Account as Account;
            if (account == null) return null;

            string token = account.GetTag(AccountTokenTag);
            if (!IsValidToken(token))
            {
                // GTop100 recommends A-Z and 0-9 for pingUsername.
                token = Guid.NewGuid().ToString("N").ToUpperInvariant();
                account.SetTag(AccountTokenTag, token);
            }

            return GTop100VoteBaseUrl + Uri.EscapeDataString(token);
        }

        private static bool IsValidToken(string token)
        {
            if (String.IsNullOrEmpty(token) || token.Length > 32) return false;
            for (int i = 0; i < token.Length; i++)
            {
                char c = token[i];
                if (!((c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))) return false;
            }
            return true;
        }

        private static void BeginPoll()
        {
            if (_Polling) return;
            _Polling = true;
            ThreadPool.QueueUserWorkItem(delegate
            {
                List<RemoteVoteEvent> events = null;
                try { events = PullPendingEvents(); }
                catch (Exception e) { Console.WriteLine("[Vote System] GTop100 poll failed: {0}", e.Message); }

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
                    string token = parts[1].Trim();
                    string ip = parts.Length > 2 ? parts[2].Trim() : String.Empty;
                    if (id.Length > 0 && IsValidToken(token)) list.Add(new RemoteVoteEvent(id, token, ip));
                }
            }
            return list;
        }

        private static void ProcessEvents(List<RemoteVoteEvent> events)
        {
            for (int i = 0; i < events.Count; i++)
            {
                RemoteVoteEvent vote = events[i];
                Mobile recipient = FindRecipient(vote.Token);
                if (recipient == null) continue;

                if (GiveNewVoteToken(recipient))
                {
                    Account account = recipient.Account as Account;
                    if (account != null) account.SetTag("WB_GTOP100_LAST_VERIFIED", DateTime.UtcNow.ToString("o"));

                    if (recipient.NetState != null)
                    {
                        recipient.SendMessage(0x59, "GTop100 confirmed your vote. You received 1 Vote Token. Thank you for supporting Wolvesbane!");
                        recipient.FixedEffect(0x376A, 10, 16);
                    }
                    QueueAck(vote.Id);
                }
            }
        }

        private static Mobile FindRecipient(string token)
        {
            Mobile firstMatch = null;
            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m == null || m.Deleted) continue;
                Account account = m.Account as Account;
                if (account == null) continue;
                if (!String.Equals(account.GetTag(AccountTokenTag), token, StringComparison.OrdinalIgnoreCase)) continue;
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
                catch (Exception e) { Console.WriteLine("[Vote System] GTop100 ACK failed for {0}: {1}", eventId, e.Message); }
            });
        }

        private static string HttpGet(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null) return String.Empty;
            request.Method = "GET";
            request.Timeout = 7000;
            request.ReadWriteTimeout = 7000;
            request.UserAgent = "Wolvesbane-UO-VoteSystem/4.0";
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
            public readonly string Id; public readonly string Token; public readonly string IP;
            public RemoteVoteEvent(string id, string token, string ip) { Id = id; Token = token; IP = ip; }
        }

        private sealed class RewardPollTimer : Timer
        {
            public RewardPollTimer() : base(InitialDelay, PollInterval) { Priority = TimerPriority.OneMinute; }
            protected override void OnTick() { BeginPoll(); }
        }
    }
}
