using System;

using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Regions;

namespace Wolvesbane.TreasureMaps
{
    /// <summary>
    /// Server-side treasure chart used only for Map.NewWolvesbane.
    ///
    /// NewWolvesbane remains a real treasure facet with a real ChestLocation.
    /// This gump replaces only the client-side cartography artwork, which does
    /// not reliably render custom facet 6.
    /// </summary>
    public class WBNewWolvesbaneTreasureChartGump : Gump
    {
        private readonly TreasureMap m_Map;

        private const int ButtonDig = 1;
        private const int ButtonRefresh = 2;

        public WBNewWolvesbaneTreasureChartGump(Mobile from, TreasureMap map)
            : base(80, 60)
        {
            m_Map = map;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            // Parchment-like frame using stock client gump art only.
            AddBackground(0, 0, 520, 430, 9270);
            AddAlphaRegion(18, 18, 484, 394);

            AddLabel(122, 28, 1153, "WOLVESBANE EXPLORER'S TREASURE CHART");
            AddLabel(190, 52, 1150, "New Wolvesbane");

            AddImageTiled(38, 82, 444, 2, 2624);

            if (map == null || map.Deleted)
            {
                AddLabel(70, 120, 33, "This treasure chart is no longer valid.");
                return;
            }

            Point2D treasure = map.ChestLocation;

            int approxX = RoundTo(treasure.X, 50);
            int approxY = RoundTo(treasure.Y, 50);

            Region region = Region.Find(new Point3D(treasure.X, treasure.Y, 0), Map.NewWolvesbane);
            string regionName = region != null && !String.IsNullOrEmpty(region.Name)
                ? region.Name
                : "the wilderness";

            string tier = GetTierName(map);
            string package = GetPackageName(map);

            AddLabel(48, 102, 88, "Treasure:");
            AddLabel(145, 102, 1153, tier);

            AddLabel(48, 126, 88, "Cache Type:");
            AddLabel(145, 126, 1153, package);

            AddLabel(48, 150, 88, "Region:");
            AddLabel(145, 150, 1153, regionName);

            AddImageTiled(38, 181, 444, 2, 2624);

            AddLabel(48, 201, 88, "Cartographer's Survey");
            AddLabel(48, 230, 1150,
                String.Format("Approximate position: {0}, {1}", approxX, approxY));

            AddLabel(48, 254, 1150,
                String.Format("From your current position: {0}", GetDirectionText(from, treasure)));

            int distance = GetTileDistance(from, treasure);
            AddLabel(48, 278, 1150,
                String.Format("Estimated distance: about {0} paces", RoundTo(distance, 25)));

            AddLabel(48, 310, 53, "The survey is intentionally approximate.");
            AddLabel(48, 332, 53, "Search the area and use your shovel when you arrive.");

            // Begin Dig simply enters the normal TreasureMap targeting path.
            AddButton(48, 370, 4005, 4007, ButtonDig, GumpButtonType.Reply, 0);
            AddLabel(82, 371, 68, "Begin Dig");

            AddButton(230, 370, 4011, 4013, ButtonRefresh, GumpButtonType.Reply, 0);
            AddLabel(264, 371, 1150, "Refresh Bearings");

            AddButton(414, 370, 4017, 4019, 0, GumpButtonType.Reply, 0);
            AddLabel(448, 371, 33, "Close");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (from == null || m_Map == null || m_Map.Deleted)
                return;

            if (!from.InRange(m_Map.GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(1019045); // I can't reach that.
                return;
            }

            switch (info.ButtonID)
            {
                case ButtonDig:
                    m_Map.OnBeginDig(from);
                    break;

                case ButtonRefresh:
                    from.CloseGump(typeof(WBNewWolvesbaneTreasureChartGump));
                    from.SendGump(new WBNewWolvesbaneTreasureChartGump(from, m_Map));
                    break;
            }
        }

        private static string GetTierName(TreasureMap map)
        {
            if (TreasureMapInfo.NewSystem)
                return map.TreasureLevel.ToString();

            switch (map.Level)
            {
                case 0:
                case 1: return "Stash";
                case 2:
                case 3: return "Supply";
                case 4:
                case 5: return "Cache";
                case 6: return "Hoard";
                default: return "Trove";
            }
        }

        private static string GetPackageName(TreasureMap map)
        {
            if (TreasureMapInfo.NewSystem)
                return map.Package.ToString();

            return "Legacy Treasure";
        }

        private static int GetTileDistance(Mobile from, Point2D target)
        {
            if (from == null || from.Map != Map.NewWolvesbane)
                return 0;

            int dx = Math.Abs(target.X - from.X);
            int dy = Math.Abs(target.Y - from.Y);

            // UO movement distance is effectively Chebyshev distance.
            return Math.Max(dx, dy);
        }

        private static string GetDirectionText(Mobile from, Point2D target)
        {
            if (from == null || from.Map != Map.NewWolvesbane)
                return "travel to New Wolvesbane";

            int dx = target.X - from.X;
            int dy = target.Y - from.Y;

            if (Math.Abs(dx) <= 15 && Math.Abs(dy) <= 15)
                return "very close by";

            string vertical = dy < 0 ? "north" : (dy > 0 ? "south" : "");
            string horizontal = dx < 0 ? "west" : (dx > 0 ? "east" : "");

            if (vertical.Length > 0 && horizontal.Length > 0)
                return vertical + "-" + horizontal;

            if (vertical.Length > 0)
                return vertical;

            if (horizontal.Length > 0)
                return horizontal;

            return "very close by";
        }

        private static int RoundTo(int value, int step)
        {
            if (step <= 1)
                return value;

            return ((value + (step / 2)) / step) * step;
        }
    }
}
