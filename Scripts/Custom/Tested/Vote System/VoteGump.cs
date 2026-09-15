using System;
using Server;
using Server.Accounting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Voting
{
    public class VoteGump : Gump
    {
        private Mobile _From;

        public VoteGump(Mobile from) : base(80, 60)
        {
            _From = from;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 610, 510, 9270);
            AddAlphaRegion(12, 12, 586, 486);

            AddLabel(205, 22, 1153, "WOLVESBANE VOTING");
            AddHtml(40, 52, 530, 42,
                "<CENTER><BASEFONT COLOR=#E8D7A5>Support Wolvesbane by voting on the sites below.<BR>Verified rewards are delivered separately after the vote site confirms your vote.</BASEFONT></CENTER>",
                false, false);

            int tokenCount = GetTokenCount(from);
            AddHtml(405, 96, 165, 24,
                String.Format("<BASEFONT COLOR=#F2CC6B>Vote Tokens: {0}</BASEFONT>", tokenCount),
                false, false);

            AddLabel(48, 101, 1153, "Voting Site");
            AddLabel(267, 101, 1153, "Reward");
            AddLabel(420, 101, 1153, "Vote");

            int y = 130;

            for (int i = 0; i < VoteSites.Sites.Length; i++)
            {
                VoteSiteInfo site = VoteSites.Sites[i];

                if ((i % 2) == 0)
                    AddImageTiled(28, y - 4, 550, 36, 2624);

                AddLabel(48, y + 4, 0x481, site.Name);

                if (site.VerifiedReward)
                    AddLabel(267, y + 4, 0x40, "Verified Token");
                else
                    AddLabel(267, y + 4, 0x3B2, "Vote Site");

                AddButton(430, y, 4005, 4007, 1000 + i, GumpButtonType.Reply, 0);
                AddLabel(465, y + 4, 0x34, "VOTE");

                y += 38;
            }

            AddHtml(35, 455, 540, 35,
                "<CENTER><BASEFONT COLOR=#9E9E9E>Opening a voting page does not itself award a token. This prevents vote rewards from being exploited.</BASEFONT></CENTER>",
                false, false);
        }

        private static int GetTokenCount(Mobile from)
        {
            if (from == null)
                return 0;

            int amount = 0;

            if (from.Backpack != null)
                amount += from.Backpack.GetAmount(typeof(NewVoteToken));

            if (from.BankBox != null)
                amount += from.BankBox.GetAmount(typeof(NewVoteToken));

            return amount;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (from == null || from.Deleted)
                return;

            int index = info.ButtonID - 1000;

            if (index >= 0 && index < VoteSites.Sites.Length)
            {
                VoteSiteInfo site = VoteSites.Sites[index];

                string url = site.Url;

                // Verified providers require an account-bound opaque parameter in the
                // vote URL so their postback can identify which Wolvesbane account to reward.
                if (site.Name.Equals("MMOHub", StringComparison.OrdinalIgnoreCase))
                    url = MMOHubVoteRewards.GetVoteUrl(from);
                else if (site.Name.Equals("TopG", StringComparison.OrdinalIgnoreCase))
                    url = TopGVoteRewards.GetVoteUrl(from);
                else if (site.Name.Equals("GTop100", StringComparison.OrdinalIgnoreCase))
                    url = GTop100VoteRewards.GetVoteUrl(from);
                else if (site.Name.Equals("Top100Arena", StringComparison.OrdinalIgnoreCase))
                    url = Top100ArenaVoteRewards.GetVoteUrl(from);
                else if (site.Name.Equals("GameTop.gg", StringComparison.OrdinalIgnoreCase))
                    url = GameTopVoteRewards.GetVoteUrl(from);

                if (String.IsNullOrEmpty(url))
                {
                    from.SendMessage(0x22, "Unable to create your verified voting link for {0}. Please try again shortly.", site.Name);
                    return;
                }

                from.LaunchBrowser(url);

                if (site.Name.Equals("Nostalgic.gg", StringComparison.OrdinalIgnoreCase))
                {
                    from.SendMessage(0x59, "Opening Nostalgic.gg. For your verified reward, enter this character name exactly: {0}", from.Name);
                    from.SendMessage(0x59, "After Nostalgic confirms the vote, you will receive 1 New Vote Token automatically.");
                }
                else
                {
                    from.SendMessage(0x59, "Opening {0}. Thank you for supporting Wolvesbane!", site.Name);
                }

                // Re-open the hub so the player can vote on additional sites.
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), delegate
                {
                    if (from != null && !from.Deleted && from.NetState != null)
                        from.SendGump(new VoteGump(from));
                });
            }
        }
    }
}
