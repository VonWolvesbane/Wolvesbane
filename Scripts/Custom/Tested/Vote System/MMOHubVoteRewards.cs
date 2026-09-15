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
    /// Verified MMOHub vote rewards for Wolvesbane.
    ///
    /// Flow:
    /// 1. [vote creates/reuses an opaque account token and appends it to the MMOHub URL.
    /// 2. MMOHub calls the private PHP postback after a successful vote.
    /// 3. This class polls the PHP queue and awards one existing New Vote Token.
    /// 4. The queue event is acknowledged only after the token is successfully delivered.
    ///
    /// The opaque token is stored as an Account tag. No account name is exposed in the URL.
    /// </summary>
    public static class MMOHubVoteRewards
    {
        private const string AccountTokenTag = "WB_MMOHUB_VOTE_TOKEN";
        private const string MMOHubVoteBaseUrl = "https://mmohub.com/site/433/vote/";

        // This is the private PHP endpoint included with this package.
        // If you rename/move it, update EndpointUrl here as well.
        private const string EndpointUrl = "https://wolvesbaneuo.com/vote/wbv_86270b1a377c4b91.php";
        private const string ClaimKey = "e4fa47716c42f9d38247d291d72cf2475130f5d3c36f4bb9";

        private static readonly TimeSpan PollInterval = TimeSpan.FromMinutes(1.0);
        private static readonly TimeSpan InitialDelay = TimeSpan.FromSeconds(30.0);

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
            if (from == null || from.Deleted)
                return null;

            Account account = from.Account as Account;

            if (account == null)
                return null;

            string token = account.GetTag(AccountTokenTag);

            if (!IsValidToken(token))
            {
                token = CreateToken();
                account.SetTag(AccountTokenTag, token);
            }

            return MMOHubVoteBaseUrl + token;
        }

        private static bool IsValidToken(string token)
        {
            if (String.IsNullOrEmpty(token) || token.Length > 32)
                return false;

            for (int i = 0; i < token.Length; i++)
            {
                char c = token[i];
                bool valid = (c >= 'a' && c <= 'z') ||
                             (c >= 'A' && c <= 'Z') ||
                             (c >= '0' && c <= '9') ||
                             c == '-' || c == '_';

                if (!valid)
                    return false;
            }

            return true;
        }

        private static string CreateToken()
        {
            // Guid N format is 32 alphanumeric characters and satisfies MMOHub's rules.
            return Guid.NewGuid().ToString("N").ToUpperInvariant();
        }

        private static void BeginPoll()
        {
            if (_Polling)
                return;

            _Polling = true;

            ThreadPool.QueueUserWorkItem(delegate
            {
                List<RemoteVoteEvent> events = null;

                try
                {
                    events = PullPendingEvents();
                }
                catch (Exception e)
                {
                    Console.WriteLine("[Vote System] MMOHub poll failed: {0}", e.Message);
                }

                Timer.DelayCall(TimeSpan.Zero, delegate
                {
                    try
                    {
                        if (events != null)
                            ProcessEvents(events);
                    }
                    finally
                    {
                        _Polling = false;
                    }
                });
            });
        }

        private static List<RemoteVoteEvent> PullPendingEvents()
        {
            string url = EndpointUrl + "?mode=pull&key=" + Uri.EscapeDataString(ClaimKey);
            string response = HttpGet(url);
            List<RemoteVoteEvent> list = new List<RemoteVoteEvent>();

            if (String.IsNullOrEmpty(response))
                return list;

            using (StringReader reader = new StringReader(response))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.Length == 0)
                        continue;

                    string[] parts = line.Split('|');

                    if (parts.Length < 2)
                        continue;

                    string id = parts[0].Trim();
                    string token = parts[1].Trim();
                    string ip = parts.Length > 2 ? parts[2].Trim() : String.Empty;

                    if (id.Length > 0 && IsValidToken(token))
                        list.Add(new RemoteVoteEvent(id, token, ip));
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

                // If the account/character cannot currently be resolved, leave the event
                // unacknowledged. It remains queued and will be tried again next poll.
                if (recipient == null)
                    continue;

                if (GiveNewVoteToken(recipient))
                {
                    Account account = recipient.Account as Account;

                    if (account != null)
                        account.SetTag("WB_MMOHUB_LAST_VERIFIED", DateTime.UtcNow.ToString("o"));

                    if (recipient.NetState != null)
                    {
                        recipient.SendMessage(0x59, "MMOHub confirmed your vote. You received 1 Vote Token. Thank you for supporting Wolvesbane!");
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
                if (m == null || m.Deleted)
                    continue;

                Account account = m.Account as Account;

                if (account == null)
                    continue;

                string accountToken = account.GetTag(AccountTokenTag);

                if (!String.Equals(accountToken, token, StringComparison.OrdinalIgnoreCase))
                    continue;

                // Prefer an online character on the account; otherwise use any loaded
                // character so the reward can safely be placed in the account bank.
                if (m.NetState != null)
                    return m;

                if (firstMatch == null)
                    firstMatch = m;
            }

            return firstMatch;
        }

        private static bool GiveNewVoteToken(Mobile recipient)
        {
            if (recipient == null || recipient.Deleted)
                return false;

            NewVoteToken reward = new NewVoteToken();

            // Online players receive it in their backpack when possible.
            if (recipient.NetState != null && recipient.Backpack != null && recipient.Backpack.TryDropItem(recipient, reward, false))
                return true;

            // Offline rewards and backpack failures go to the bank.
            if (recipient.BankBox != null)
            {
                recipient.BankBox.DropItem(reward);
                return true;
            }

            reward.Delete();
            return false;
        }

        private static void QueueAck(string eventId)
        {
            if (String.IsNullOrEmpty(eventId))
                return;

            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    string url = EndpointUrl + "?mode=ack&key=" + Uri.EscapeDataString(ClaimKey) +
                                 "&id=" + Uri.EscapeDataString(eventId);
                    HttpGet(url);
                }
                catch (Exception e)
                {
                    // IMPORTANT: if ACK fails after the item was delivered, the event would
                    // be returned again. The PHP side also de-duplicates postbacks, but an
                    // ACK transport failure can still cause a retry. Log loudly so staff can
                    // investigate. A durable local receipt ledger can be added if desired.
                    Console.WriteLine("[Vote System] MMOHub ACK failed for {0}: {1}", eventId, e.Message);
                }
            });
        }

        private static string HttpGet(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

            if (request == null)
                return String.Empty;

            request.Method = "GET";
            request.Timeout = 7000;
            request.ReadWriteTimeout = 7000;
            request.UserAgent = "Wolvesbane-UO-VoteSystem/2.0";
            request.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null)
                    return String.Empty;

                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    return reader.ReadToEnd();
            }
        }

        private sealed class RemoteVoteEvent
        {
            public readonly string Id;
            public readonly string Token;
            public readonly string IP;

            public RemoteVoteEvent(string id, string token, string ip)
            {
                Id = id;
                Token = token;
                IP = ip;
            }
        }

        private sealed class RewardPollTimer : Timer
        {
            public RewardPollTimer() : base(InitialDelay, PollInterval)
            {
                Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                BeginPoll();
            }
        }
    }
}
