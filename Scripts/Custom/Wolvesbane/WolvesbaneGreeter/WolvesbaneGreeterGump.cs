using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
    public class WolvesbaneGreeterGump : Gump
    {
        private readonly Mobile m_From;
        private readonly WolvesbaneGreeter m_Greeter;
        private readonly int m_PageIndex;

        public WolvesbaneGreeterGump(Mobile from, WolvesbaneGreeter greeter, int pageIndex)
            : base(60, 40)
        {
            m_From = from;
            m_Greeter = greeter;

            if (WolvesbaneGreeterConfig.Pages.Count == 0)
                WolvesbaneGreeterConfig.Load();

            if (pageIndex < 0 || pageIndex >= WolvesbaneGreeterConfig.Pages.Count)
                pageIndex = 0;

            m_PageIndex = pageIndex;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 720, 520, 9270);
            AddAlphaRegion(15, 15, 690, 490);

            AddLabel(28, 22, 1152, "WOLVESBANE UO");
            AddLabel(28, 44, 68, "Newcomer Guide");
            AddHtml(28, 70, 180, 38, "<BASEFONT COLOR=#DDDDDD>Choose a topic:</BASEFONT>", false, false);

            AddAlphaRegion(22, 105, 200, 375);

            int y = 114;
            int maxVisible = Math.Min(WolvesbaneGreeterConfig.Pages.Count, 14);

            for (int i = 0; i < maxVisible; i++)
            {
                bool selected = (i == m_PageIndex);
                int buttonID = 100 + i;

                AddButton(32, y + 2, selected ? 4006 : 4005, selected ? 4007 : 4007, buttonID, GumpButtonType.Reply, 0);
                AddLabel(63, y, selected ? 68 : 1152, WolvesbaneGreeterConfig.Pages[i].Title);
                y += 25;
            }

            if (WolvesbaneGreeterConfig.Pages.Count > maxVisible)
            {
                AddHtml(30, 462, 180, 20, "<BASEFONT COLOR=#AAAAAA>More sections may be added in config.</BASEFONT>", false, false);
            }

            AddAlphaRegion(235, 70, 455, 410);

            WolvesbaneGreeterConfig.GreeterPage page = WolvesbaneGreeterConfig.Pages[m_PageIndex];

            AddLabel(255, 88, 68, page.Title);

            string html = BuildPageHtml(page);
            AddHtml(255, 120, 415, 320, html, true, true);

            AddButton(548, 455, 4017, 4018, 1, GumpButtonType.Reply, 0);
            AddLabel(582, 456, 1152, "Close");

            if (from != null && from.AccessLevel >= AccessLevel.GameMaster)
            {
                AddButton(250, 455, 4029, 4030, 2, GumpButtonType.Reply, 0);
                AddLabel(284, 456, 88, "Reload Greeter Config");
            }
        }

        private string BuildPageHtml(WolvesbaneGreeterConfig.GreeterPage page)
        {
            if (page == null || page.Lines == null || page.Lines.Count == 0)
                return "<BASEFONT COLOR=#FFFFFF>No information has been added to this page yet.</BASEFONT>";

            string result = "<BASEFONT COLOR=#FFFFFF>";

            for (int i = 0; i < page.Lines.Count; i++)
            {
                string line = page.Lines[i] ?? String.Empty;
                line = line.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                result += line + "<BR><BR>";
            }

            result += "</BASEFONT>";
            return result;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (from == null || from.Deleted)
                return;

            if (info.ButtonID == 0 || info.ButtonID == 1)
                return;

            if (info.ButtonID == 2 && from.AccessLevel >= AccessLevel.GameMaster)
            {
                WolvesbaneGreeterConfig.Load();
                from.SendMessage(68, "Wolvesbane Greeter configuration reloaded.");
                from.SendGump(new WolvesbaneGreeterGump(from, m_Greeter, Math.Min(m_PageIndex, WolvesbaneGreeterConfig.Pages.Count - 1)));
                return;
            }

            if (info.ButtonID >= 100)
            {
                int page = info.ButtonID - 100;

                if (page >= 0 && page < WolvesbaneGreeterConfig.Pages.Count)
                    from.SendGump(new WolvesbaneGreeterGump(from, m_Greeter, page));
            }
        }
    }
}
