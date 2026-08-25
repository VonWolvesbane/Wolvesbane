using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Custom.BossDrops
{
    public static class BossDropCommands
    {
        public static void Initialize()
        {
            BossDropRegistry.Initialize();
            CommandSystem.Register("BossDropGump", AccessLevel.GameMaster, new CommandEventHandler(OnBossDropGump));
            CommandSystem.Register("RemoveBossDropDisplay", AccessLevel.GameMaster, new CommandEventHandler(OnRemoveDisplay));
            CommandSystem.Register("RefreshBossDropDisplay", AccessLevel.GameMaster, new CommandEventHandler(OnRefreshDisplay));
            CommandSystem.Register("RefreshAllBossDropDisplays", AccessLevel.GameMaster, new CommandEventHandler(OnRefreshAllDisplays));
            CommandSystem.Register("AuditBossDropDisplays", AccessLevel.GameMaster, new CommandEventHandler(OnAuditDisplays));
            CommandSystem.Register("ValidateBossDropRegistry", AccessLevel.Administrator, new CommandEventHandler(OnValidateRegistry));
        }

        private static void OnBossDropGump(CommandEventArgs e)
        {
            e.Mobile.CloseGump(typeof(BossDropGump));
            e.Mobile.SendGump(new BossDropGump(e.Mobile, 0));
        }

        private static void OnAuditDisplays(CommandEventArgs e) { RunAudit(e.Mobile); }
        private static void OnValidateRegistry(CommandEventArgs e) { ValidateRegistry(e.Mobile, true); }

        public static void RunAudit(Mobile from)
        {
            BossDropRegistry.Initialize();
            Dictionary<string, int> counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            int totalPlaced = 0, orphanCases = 0, orphanPlaques = 0, legacyStatues = 0;

            foreach (Mobile m in World.Mobiles.Values)
            {
                BossDropMannequin mannequin = m as BossDropMannequin;
                if (mannequin == null || mannequin.Deleted) continue;
                totalPlaced++;
                int count; counts.TryGetValue(mannequin.DisplayKey ?? String.Empty, out count);
                counts[mannequin.DisplayKey ?? String.Empty] = count + 1;
            }

            foreach (Item item in World.Items.Values)
            {
                BossDropDisplayCase c = item as BossDropDisplayCase;
                if (c != null && !c.Deleted && (c.Mannequin == null || c.Mannequin.Deleted)) orphanCases++;
                BossDropInfoPlaque p = item as BossDropInfoPlaque;
                if (p != null && !p.Deleted && (p.Mannequin == null || p.Mannequin.Deleted)) orphanPlaques++;
            }

            foreach (Mobile m in World.Mobiles.Values)
            {
                BossDropBossStatue statue = m as BossDropBossStatue;
                if (statue != null && !statue.Deleted) legacyStatues++;
            }

            int missing = 0, duplicates = 0;
            IList<BossDropDefinition> defs = BossDropRegistry.Definitions;
            for (int i = 0; i < defs.Count; i++)
            {
                int count; counts.TryGetValue(defs[i].Key, out count);
                if (count == 0) missing++;
                if (count > 1) duplicates += count - 1;
            }

            int invalid = ValidateRegistry(from, false);
            from.SendMessage(68, "Boss Drop audit: {0} registered, {1} placed, {2} missing, {3} duplicate placement(s), {4} orphan case(s), {5} orphan plaque(s), {6} legacy statue(s), {7} invalid registry item(s).", defs.Count, totalPlaced, missing, duplicates, orphanCases, orphanPlaques, legacyStatues, invalid);
        }

        public static int ValidateRegistry(Mobile from, bool verbose)
        {
            int invalid = 0;
            IList<BossDropDefinition> defs = BossDropRegistry.Definitions;
            for (int i = 0; i < defs.Count; i++)
            {
                BossDropDefinition def = defs[i];
                if (def.ItemTypes == null || def.ItemTypes.Length == 0)
                {
                    invalid++;
                    if (verbose) from.SendMessage(33, "{0}: no item types registered.", def.Label);
                    continue;
                }

                for (int j = 0; j < def.ItemTypes.Length; j++)
                {
                    Type t = def.ItemTypes[j];
                    if (t == null || !typeof(Item).IsAssignableFrom(t))
                    {
                        invalid++;
                        if (verbose) from.SendMessage(33, "{0}: invalid item type at position {1}.", def.Label, j + 1);
                    }
                }
            }
            if (verbose) from.SendMessage(invalid == 0 ? 68 : 33, "Boss Drop registry validation complete: {0} invalid item reference(s).", invalid);
            return invalid;
        }

        private static void OnRefreshAllDisplays(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            List<BossDropMannequin> displays = new List<BossDropMannequin>();
            foreach (Mobile m in World.Mobiles.Values)
            {
                BossDropMannequin mannequin = m as BossDropMannequin;
                if (mannequin != null && !mannequin.Deleted) displays.Add(mannequin);
            }

            int refreshed = 0, skipped = 0;
            for (int i = 0; i < displays.Count; i++)
            {
                BossDropMannequin oldMannequin = displays[i];
                if (oldMannequin == null || oldMannequin.Deleted) continue;
                BossDropDefinition def = BossDropRegistry.Find(oldMannequin.DisplayKey);
                if (def == null) { skipped++; continue; }

                Point3D loc = oldMannequin.Location; Map map = oldMannequin.Map;
                oldMannequin.Delete();
                BossDropMannequin mannequin = BossDropRegistry.Create(def, null);
                if (mannequin != null) { BossDropRegistry.PlaceDisplay(mannequin, loc, map); refreshed++; }
                else skipped++;
            }
            from.SendMessage(68, "Boss Drop global refresh complete: {0} display(s) refreshed, {1} skipped.", refreshed, skipped);
        }

        private static void OnRemoveDisplay(CommandEventArgs e)
        {
            e.Mobile.SendMessage(68, "Target the boss drop mannequin to remove.");
            e.Mobile.Target = new RemoveBossDropDisplayTarget();
        }

        private static void OnRefreshDisplay(CommandEventArgs e)
        {
            e.Mobile.SendMessage(68, "Target the boss drop mannequin to rebuild from its current registry definition.");
            e.Mobile.Target = new RefreshBossDropDisplayTarget();
        }
    }

    public class RemoveBossDropDisplayTarget : Target
    {
        public RemoveBossDropDisplayTarget() : base(-1, false, TargetFlags.None) { }
        protected override void OnTarget(Mobile from, object targeted)
        {
            BossDropMannequin mannequin = targeted as BossDropMannequin;
            if (mannequin == null) { from.SendMessage(33, "That is not a Boss Drop display mannequin."); return; }
            string name = mannequin.Name; mannequin.Delete(); from.SendMessage(68, "Removed {0}.", name);
        }
    }

    public class RefreshBossDropDisplayTarget : Target
    {
        public RefreshBossDropDisplayTarget() : base(-1, false, TargetFlags.None) { }
        protected override void OnTarget(Mobile from, object targeted)
        {
            BossDropMannequin oldMannequin = targeted as BossDropMannequin;
            if (oldMannequin == null) { from.SendMessage(33, "That is not a Boss Drop display mannequin."); return; }
            BossDropDefinition def = BossDropRegistry.Find(oldMannequin.DisplayKey);
            if (def == null) { from.SendMessage(33, "No registry entry exists for display key '{0}'.", oldMannequin.DisplayKey); return; }
            Point3D loc = oldMannequin.Location; Map map = oldMannequin.Map; oldMannequin.Delete();
            BossDropMannequin mannequin = BossDropRegistry.Create(def, from); BossDropRegistry.PlaceDisplay(mannequin, loc, map);
            from.SendMessage(68, "Refreshed {0}.", def.Label);
        }
    }
}
