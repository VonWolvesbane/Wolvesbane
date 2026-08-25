using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Custom.BossDrops
{
    public class BossDropGump : Gump
    {
        private const int PerPage = 8;
        private readonly Mobile m_From;
        private readonly int m_Page;
        private readonly string m_Filter;

        public BossDropGump(Mobile from, int page) : this(from, page, String.Empty) { }

        public BossDropGump(Mobile from, int page, string filter) : base(40, 40)
        {
            m_From = from;
            m_Page = page < 0 ? 0 : page;
            m_Filter = filter == null ? String.Empty : filter.Trim();

            BossDropRegistry.Initialize();
            List<BossDropDefinition> defs = GetFilteredDefinitions(m_Filter);
            int pageCount = Math.Max(1, (defs.Count + PerPage - 1) / PerPage);
            int currentPage = Math.Min(m_Page, pageCount - 1);

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 760, 520, 9270);
            AddAlphaRegion(12, 12, 736, 496);
            AddLabel(25, 20, 1152, "Wolvesbane Boss Drop Displays");
            AddLabel(605, 20, 1152, String.Format("Page {0}/{1}", currentPage + 1, pageCount));

            AddLabel(25, 50, 1153, "Search:");
            AddTextEntry(90, 48, 285, 22, 1152, 0, m_Filter);
            AddButton(385, 48, 4005, 4007, 10, GumpButtonType.Reply, 0);
            AddLabel(420, 50, 1152, "Filter");
            AddButton(485, 48, 4017, 4019, 11, GumpButtonType.Reply, 0);
            AddLabel(520, 50, 1152, "Clear");
            AddButton(610, 48, 4005, 4007, 12, GumpButtonType.Reply, 0);
            AddLabel(645, 50, 1152, "Audit");

            AddLabel(25, 82, 1152, "Boss / Set");
            AddLabel(425, 82, 1152, "Category");
            AddLabel(545, 82, 1152, "Status");
            AddLabel(650, 82, 1152, "Action");

            int start = currentPage * PerPage;
            int y = 108;

            for (int i = 0; i < PerPage && start + i < defs.Count; i++)
            {
                BossDropDefinition def = defs[start + i];
                int masterIndex = GetMasterIndex(def);
                BossDropMannequin placed = FindPlaced(def.Key);
                int placedCount = CountPlaced(def.Key);

                AddLabel(25, y, 0x34, def.BossName + " - " + def.SetName);
                AddLabel(425, y, 1153, ShortCategory(def.Category));

                if (placed != null)
                {
                    AddLabel(545, y, placedCount > 1 ? 0x21 : 0x59, placedCount > 1 ? "Placed x" + placedCount : "Placed");
                    AddButton(650, y, 4005, 4007, 3000 + masterIndex, GumpButtonType.Reply, 0);
                    AddLabel(684, y, 1152, "Go");
                }
                else
                {
                    AddLabel(545, y, 0x21, "Not Placed");
                    AddButton(650, y, 4005, 4007, 1000 + masterIndex, GumpButtonType.Reply, 0);
                    AddLabel(684, y, 1152, "Add");
                }

                AddButton(25, y + 23, 4005, 4007, 2000 + masterIndex, GumpButtonType.Reply, 0);
                AddLabel(60, y + 23, 1152, "Info");
                AddLabel(135, y + 23, def.Race == BossDropDisplayRace.Human ? 0x59 : 0x21, def.Race.ToString());
                y += 47;
            }

            if (currentPage > 0)
            {
                AddButton(25, 475, 4014, 4016, 1, GumpButtonType.Reply, 0);
                AddLabel(58, 475, 1152, "Previous");
            }

            if (currentPage < pageCount - 1)
            {
                AddButton(650, 475, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddLabel(685, 475, 1152, "Next");
            }

            AddLabel(280, 475, 1153, defs.Count + " display definition(s)");
        }

        private static List<BossDropDefinition> GetFilteredDefinitions(string filter)
        {
            List<BossDropDefinition> result = new List<BossDropDefinition>();
            IList<BossDropDefinition> defs = BossDropRegistry.Definitions;

            for (int i = 0; i < defs.Count; i++)
            {
                BossDropDefinition d = defs[i];
                if (String.IsNullOrEmpty(filter) ||
                    d.BossName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    d.SetName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    d.Category.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    d.Race.ToString().IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                    result.Add(d);
            }
            return result;
        }

        private static int GetMasterIndex(BossDropDefinition def)
        {
            IList<BossDropDefinition> defs = BossDropRegistry.Definitions;
            for (int i = 0; i < defs.Count; i++) if (Object.ReferenceEquals(defs[i], def)) return i;
            return -1;
        }

        private static BossDropMannequin FindPlaced(string key)
        {
            foreach (Mobile m in World.Mobiles.Values)
            {
                BossDropMannequin mannequin = m as BossDropMannequin;
                if (mannequin != null && !mannequin.Deleted && String.Equals(mannequin.DisplayKey, key, StringComparison.OrdinalIgnoreCase))
                    return mannequin;
            }
            return null;
        }

        private static int CountPlaced(string key)
        {
            int count = 0;
            foreach (Mobile m in World.Mobiles.Values)
            {
                BossDropMannequin mannequin = m as BossDropMannequin;
                if (mannequin != null && !mannequin.Deleted && String.Equals(mannequin.DisplayKey, key, StringComparison.OrdinalIgnoreCase)) count++;
            }
            return count;
        }

        private static string ShortCategory(string s)
        {
            if (String.IsNullOrEmpty(s)) return "Other";
            if (s.Length <= 16) return s;
            return s.Substring(0, 16);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_From == null || m_From.Deleted) return;
            TextRelay entry = info.GetTextEntry(0);
            string filter = entry == null ? m_Filter : entry.Text;

            if (info.ButtonID == 1) { m_From.SendGump(new BossDropGump(m_From, m_Page - 1, m_Filter)); return; }
            if (info.ButtonID == 2) { m_From.SendGump(new BossDropGump(m_From, m_Page + 1, m_Filter)); return; }
            if (info.ButtonID == 10) { m_From.SendGump(new BossDropGump(m_From, 0, filter)); return; }
            if (info.ButtonID == 11) { m_From.SendGump(new BossDropGump(m_From, 0, String.Empty)); return; }
            if (info.ButtonID == 12) { BossDropCommands.RunAudit(m_From); m_From.SendGump(new BossDropGump(m_From, m_Page, m_Filter)); return; }

            IList<BossDropDefinition> defs = BossDropRegistry.Definitions;

            if (info.ButtonID >= 1000 && info.ButtonID < 2000)
            {
                int index = info.ButtonID - 1000;
                if (index >= 0 && index < defs.Count)
                {
                    m_From.SendMessage(68, "Target the location for {0}.", defs[index].Label);
                    m_From.Target = new BossDropPlacementTarget(defs[index].Key, m_Page, m_Filter);
                }
                return;
            }

            if (info.ButtonID >= 2000 && info.ButtonID < 3000)
            {
                int index = info.ButtonID - 2000;
                if (index >= 0 && index < defs.Count)
                {
                    BossDropMannequin mannequin = FindPlaced(defs[index].Key);
                    m_From.SendGump(new BossDropInfoGump(m_From, defs[index], mannequin));
                }
                return;
            }

            if (info.ButtonID >= 3000 && info.ButtonID < 4000)
            {
                int index = info.ButtonID - 3000;
                if (index >= 0 && index < defs.Count)
                {
                    BossDropMannequin mannequin = FindPlaced(defs[index].Key);
                    if (mannequin != null && !mannequin.Deleted)
                    {
                        m_From.MoveToWorld(mannequin.Location, mannequin.Map);
                        m_From.SendMessage(68, "Moved to {0}.", defs[index].Label);
                    }
                }
            }
        }
    }

    public class BossDropPlacementTarget : Target
    {
        private readonly string m_Key;
        private readonly int m_Page;
        private readonly string m_Filter;

        public BossDropPlacementTarget(string key, int page, string filter) : base(-1, true, TargetFlags.None)
        {
            m_Key = key; m_Page = page; m_Filter = filter;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            try
            {
                IPoint3D p = targeted as IPoint3D;
                if (p == null) { from.SendMessage(33, "That is not a valid placement location."); return; }

                BossDropDefinition def = BossDropRegistry.Find(m_Key);
                BossDropMannequin mannequin = BossDropRegistry.Create(def, from);
                if (mannequin == null) { from.SendMessage(33, "The display could not be created."); return; }
                BossDropRegistry.PlaceDisplay(mannequin, new Point3D(p), from.Map);
            }
            finally
            {
                if (from != null && !from.Deleted)
                {
                    from.CloseGump(typeof(BossDropGump));
                    from.SendGump(new BossDropGump(from, m_Page, m_Filter));
                }
            }
        }

        protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
        {
            if (from != null && !from.Deleted)
            {
                from.CloseGump(typeof(BossDropGump));
                from.SendGump(new BossDropGump(from, m_Page, m_Filter));
            }
        }
    }
}
