using System;

namespace Server.Voting
{
    public sealed class VoteSiteInfo
    {
        public string Name { get; private set; }
        public string Url { get; private set; }
        public TimeSpan Cooldown { get; private set; }
        public bool VerifiedReward { get; private set; }

        public VoteSiteInfo(string name, string url, TimeSpan cooldown, bool verifiedReward)
        {
            Name = name;
            Url = url;
            Cooldown = cooldown;
            VerifiedReward = verifiedReward;
        }
    }

    public static class VoteSites
    {
        // VerifiedReward means this site is intended to award tokens only after
        // a verified website callback/postback is implemented.
        public static readonly VoteSiteInfo[] Sites = new VoteSiteInfo[]
        {
            new VoteSiteInfo("Nostalgic.gg", "https://nostalgic.gg/vote/1977", TimeSpan.FromHours(12.0), true),
            new VoteSiteInfo("MMOHub", "https://mmohub.com/site/433/vote", TimeSpan.FromHours(12.0), true),
            new VoteSiteInfo("TopG", "https://topg.org/ultima-private-servers/server-589367", TimeSpan.FromHours(6.0), true),
            new VoteSiteInfo("XtremeTop100", "https://www.xtremetop100.com/in.php?site=1132368555", TimeSpan.FromHours(24.0), false),
            new VoteSiteInfo("UOGateway", "https://www.uogateway.com/shard.php?id=1248&act=vote", TimeSpan.FromHours(24.0), false),
            new VoteSiteInfo("GTop100", "https://gtop100.com/Ultima-Online/Wolvesbane-96694", TimeSpan.FromHours(24.0), true),
            new VoteSiteInfo("Top100Arena", "https://www.top100arena.com/listing/96148/vote", TimeSpan.FromHours(24.0), true),
            new VoteSiteInfo("GameTop.gg", "https://gametop.gg/vote/wolvesbane-uo", TimeSpan.FromHours(24.0), true)
        };
    }
}
