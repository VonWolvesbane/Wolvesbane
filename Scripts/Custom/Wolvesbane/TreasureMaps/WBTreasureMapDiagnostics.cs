using System;
using Server;
using Server.Commands;
using Server.Items;
using Server.Targeting;

namespace Wolvesbane.TreasureMaps
{
    public static class WBTreasureMapDiagnostics
    {
        public static void Initialize()
        {
            CommandSystem.Register("WBTMapCreate", AccessLevel.GameMaster, OnCreate);
            CommandSystem.Register("WBTMapLegacy", AccessLevel.GameMaster, OnLegacy);
            CommandSystem.Register("WBTMapAudit", AccessLevel.GameMaster, OnAudit);
            CommandSystem.Register("WBTMapLocationTest", AccessLevel.GameMaster, OnLocationTest);
        }

        [Usage("WBTMapCreate <tier> [package]")]
        [Description("Creates a new-system treasure map. Tier may be 0-4 or Stash/Supply/Cache/Hoard/Trove; package may be Artisan/Assassin/Mage/Ranger/Warrior.")]
        private static void OnCreate(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            int level;

            if (e.Arguments.Length < 1 || e.Arguments.Length > 2 || !TryParseTier(e.Arguments[0], out level))
            {
                SendCreateUsage(from);
                return;
            }

            TreasurePackage package = TreasurePackage.Artisan;
            bool forcePackage = false;

            if (e.Arguments.Length == 2)
            {
                if (!TryParsePackage(e.Arguments[1], out package))
                {
                    SendCreateUsage(from);
                    return;
                }

                forcePackage = true;
            }

            Map map = from.Map;

            if (map == null || map == Map.Internal)
                map = Map.Trammel;

            if (map == Map.NewWolvesbane && WBTreasureMapAreas.GetAreas().Length == 0)
            {
                from.SendMessage(33, "NewWolvesbane has no approved treasure areas yet.");
                from.SendMessage(68, "Stand in a safe hunting area and use [WBTMapAreaAdd 250 first.");
                return;
            }

            bool eodon = map == Map.TerMur && Server.Spells.SpellHelper.IsEodon(map, from.Location);

            TreasureMap tmap = new TreasureMap(level, map, eodon);

            // The normal constructor intentionally assigns a random profession
            // package. Staff testing can override it without changing normal
            // treasure-map generation behavior.
            if (forcePackage)
                tmap.Package = package;

            if (from.Backpack != null)
                from.Backpack.DropItem(tmap);
            else
                tmap.MoveToWorld(from.Location, from.Map);

            from.SendMessage(
                68,
                "Created {0} / {1} treasure map (Level {2}) on {3}.",
                tmap.TreasureLevel,
                tmap.Package,
                tmap.Level,
                tmap.Facet);
        }

        private static bool TryParseTier(string value, out int level)
        {
            level = -1;

            if (String.IsNullOrEmpty(value))
                return false;

            int numeric;

            if (Int32.TryParse(value, out numeric))
            {
                if (numeric >= 0 && numeric <= 4)
                {
                    level = numeric;
                    return true;
                }

                return false;
            }

            switch (value.Trim().ToLowerInvariant())
            {
                case "stash":
                    level = 0;
                    return true;

                case "supply":
                    level = 1;
                    return true;

                case "cache":
                    level = 2;
                    return true;

                case "hoard":
                    level = 3;
                    return true;

                case "trove":
                    level = 4;
                    return true;
            }

            return false;
        }

        private static bool TryParsePackage(string value, out TreasurePackage package)
        {
            package = TreasurePackage.Artisan;

            if (String.IsNullOrEmpty(value))
                return false;

            switch (value.Trim().ToLowerInvariant())
            {
                case "artisan":
                case "craft":
                case "crafter":
                    package = TreasurePackage.Artisan;
                    return true;

                case "assassin":
                    package = TreasurePackage.Assassin;
                    return true;

                case "mage":
                case "wizard":
                    package = TreasurePackage.Mage;
                    return true;

                case "ranger":
                case "archer":
                    package = TreasurePackage.Ranger;
                    return true;

                case "warrior":
                case "fighter":
                    package = TreasurePackage.Warrior;
                    return true;
            }

            return false;
        }

        private static void SendCreateUsage(Mobile from)
        {
            from.SendMessage("Usage: [WBTMapCreate <tier> [package]");
            from.SendMessage("Tiers: 0/Stash, 1/Supply, 2/Cache, 3/Hoard, 4/Trove");
            from.SendMessage("Packages: Artisan, Assassin, Mage, Ranger, Warrior");
            from.SendMessage("Examples:");
            from.SendMessage("  [WBTMapCreate Stash Mage");
            from.SendMessage("  [WBTMapCreate Supply Artisan");
            from.SendMessage("  [WBTMapCreate Trove Warrior");
            from.SendMessage("  [WBTMapCreate 4 Mage");
            from.SendMessage("Omit package to keep the normal random package.");
        }

        [Usage("WBTMapLegacy <0-7>")]
        [Description("Creates a map using a legacy pre-revamp level and reports the converted new-system tier.")]
        private static void OnLegacy(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            int legacy;

            if (e.Arguments.Length != 1 || !Int32.TryParse(e.Arguments[0], out legacy) || legacy < 0 || legacy > 7)
            {
                from.SendMessage("Usage: [WBTMapLegacy <0-7>");
                return;
            }

            Map map = from.Map;

            if (map == null || map == Map.Internal)
                map = Map.Trammel;

            if (map == Map.NewWolvesbane && WBTreasureMapAreas.GetAreas().Length == 0)
            {
                from.SendMessage(33, "NewWolvesbane has no approved treasure areas yet.");
                from.SendMessage(68, "Stand in a safe hunting area and use [WBTMapAreaAdd 250 first.");
                return;
            }

            bool eodon = map == Map.TerMur && Server.Spells.SpellHelper.IsEodon(map, from.Location);

            TreasureMap tmap = TreasureMapInfo.CreateLegacyMap(legacy, map, eodon);

            if (from.Backpack != null)
                from.Backpack.DropItem(tmap);
            else
                tmap.MoveToWorld(from.Location, from.Map);

            from.SendMessage(
                68,
                "Legacy level {0} converted to {1} (Level {2}).",
                legacy,
                tmap.TreasureLevel,
                tmap.Level);
        }

        [Usage("WBTMapLocationTest")]
        [Description("Selects a NewWolvesbane treasure location using production validation and reports the result.")]
        private static void OnLocationTest(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from.Map != Map.NewWolvesbane)
            {
                from.SendMessage(33, "You must be on NewWolvesbane to run this test.");
                return;
            }

            if (WBTreasureMapAreas.GetAreas().Length == 0)
            {
                from.SendMessage(33, "No approved NewWolvesbane treasure areas are configured.");
                return;
            }

            Point2D p = TreasureMap.GetRandomLocation(Map.NewWolvesbane);

            if (p == Point2D.Zero)
            {
                from.SendMessage(33, "No dry valid treasure location could be selected.");
                return;
            }

            int z = Map.NewWolvesbane.GetAverageZ(p.X, p.Y);

            from.SendMessage(
                68,
                "Selected dry treasure candidate: {0},{1},{2}.",
                p.X,
                p.Y,
                z);

            from.SendMessage(
                TreasureMap.ValidateLocation(p.X, p.Y, Map.NewWolvesbane) ? 68 : 33,
                "Production validation: {0}.",
                TreasureMap.ValidateLocation(p.X, p.Y, Map.NewWolvesbane) ? "PASS" : "FAIL");
        }

        [Usage("WBTMapAudit")]
        [Description("Targets a TreasureMap and reports its canonical tier, package, facet and state.")]
        private static void OnAudit(CommandEventArgs e)
        {
            e.Mobile.SendMessage(68, "Target a treasure map to inspect.");
            e.Mobile.Target = new AuditTarget();
        }

        private sealed class AuditTarget : Target
        {
            public AuditTarget()
                : base(12, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                TreasureMap tmap = targeted as TreasureMap;

                if (tmap == null)
                {
                    from.SendMessage(33, "That is not a TreasureMap.");
                    return;
                }

                from.SendMessage(88, "Wolvesbane Treasure Map Audit");
                from.SendMessage("Serial: {0}", tmap.Serial);
                from.SendMessage("Level: {0} ({1})", tmap.Level, tmap.TreasureLevel);
                from.SendMessage("Package: {0}", tmap.Package);
                from.SendMessage("Facet: {0}; TreasureFacet: {1}", tmap.Facet, tmap.TreasureFacet);
                from.SendMessage("Chest location: {0},{1}", tmap.ChestLocation.X, tmap.ChestLocation.Y);
                from.SendMessage("Completed: {0}", tmap.Completed ? "yes" : "no");
                from.SendMessage("Decoder: {0}", tmap.Decoder == null ? "(none)" : tmap.Decoder.Name);
                from.SendMessage("Next reset: {0}", tmap.NextReset == DateTime.MinValue ? "(none)" : tmap.NextReset.ToString("u"));
            }
        }
    }
}
