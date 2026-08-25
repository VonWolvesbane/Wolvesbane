using System;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Custom.BossDrops
{
    public class BossDropInfoPlaque : Item
    {
        private string m_DisplayKey;
        private Mobile m_Mannequin;

        [CommandProperty(AccessLevel.GameMaster)]
        public string DisplayKey { get { return m_DisplayKey; } set { m_DisplayKey = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Mannequin { get { return m_Mannequin; } set { m_Mannequin = value; } }

        public BossDropInfoPlaque(string key, string bossName) : base(19232)
        {
            m_DisplayKey = key;
            Name = bossName + " - Boss Information";
            Movable = false;
            LootType = LootType.Blessed;
        }

        public BossDropInfoPlaque(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !from.InRange(this, 4))
            {
                if (from != null) from.SendMessage("You are too far away to read the display.");
                return;
            }

            BossDropDefinition def = BossDropRegistry.Find(m_DisplayKey);
            if (def != null)
            {
                from.CloseGump(typeof(BossDropInfoGump));
                from.SendGump(new BossDropInfoGump(from, def, m_Mannequin as BossDropMannequin));
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Double-click for boss and set information.");
            list.Add("Boss location is intentionally not shown.");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(m_DisplayKey);
            writer.Write(m_Mannequin);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_DisplayKey = reader.ReadString();
            m_Mannequin = reader.ReadMobile();
            Movable = false;
            LootType = LootType.Blessed;
        }
    }

    public class BossDropInfoGump : Gump
    {
        private readonly BossDropDefinition m_Def;
        private readonly BossDropMannequin m_Mannequin;

        public BossDropInfoGump(Mobile from, BossDropDefinition def, BossDropMannequin mannequin) : base(90, 70)
        {
            m_Def = def;
            m_Mannequin = mannequin;

            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(0, 0, 520, 385, 9270);
            AddAlphaRegion(12, 12, 496, 361);
            AddLabel(25, 20, 1152, def.BossName);
            AddLabel(25, 50, 0x34, def.SetName);
            AddLabel(25, 85, 1153, "Category:");
            AddLabel(145, 85, 1152, def.Category);
            AddLabel(25, 110, 1153, "Display Race:");
            AddLabel(145, 110, 1152, def.Race.ToString());
            AddLabel(25, 135, 1153, "Set Pieces:");
            AddLabel(145, 135, 1152, def.ItemTypes == null ? "0" : def.ItemTypes.Length.ToString());
            AddLabel(25, 160, 1153, "Progression:");
            AddLabel(145, 160, 1152, def.MaxEvolution ? "Fully Leveled Evolution Display (1001)" : (def.SetName.IndexOf("Evolution", StringComparison.OrdinalIgnoreCase) >= 0 ? "Unleveled Evolution Display" : "Standard Equipment"));

            BossDropBossMeta meta = BossDropBossData.Find(def.Key);
            AddLabel(25, 185, 1153, "Difficulty:");
            AddLabel(145, 185, 1152, meta == null ? "Not Rated" : ("Wolvesbane " + new String('*', meta.Difficulty) + " (" + meta.Difficulty + "/5)"));

            AddHtml(25, 220, 470, 55, "<BASEFONT COLOR=#DDDDDD>This exhibit shows the actual scripted equipment used by Wolvesbane. Boss locations are deliberately omitted so discovery remains part of the game.</BASEFONT>", false, false);

            if (mannequin != null && !mannequin.Deleted)
            {
                AddButton(25, 305, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddLabel(60, 305, 1152, "View Complete Set Stats");
                AddButton(265, 305, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddLabel(300, 305, 1152, "View Equipment Paperdoll");
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;
            if (from == null || m_Def == null) return;

            if (info.ButtonID == 1 && m_Mannequin != null && !m_Mannequin.Deleted)
                from.SendGump(new BossDropSetStatsGump(m_Def, m_Mannequin));
            else if (info.ButtonID == 2 && m_Mannequin != null && !m_Mannequin.Deleted)
                m_Mannequin.DisplayPaperdollTo(from);
        }
    }

    public class BossDropSetStatsGump : Gump
    {
        private class StatLine
        {
            public string Name;
            public int Value;
            public StatLine(string name, int value) { Name = name; Value = value; }
        }

        public BossDropSetStatsGump(BossDropDefinition def, BossDropMannequin mannequin) : base(110, 80)
        {
            List<StatLine> stats = CollectStats(mannequin);
            int height = Math.Min(650, 145 + Math.Max(1, stats.Count) * 22);

            AddBackground(0, 0, 500, height, 9270);
            AddAlphaRegion(12, 12, 476, height - 24);
            AddLabel(25, 20, 1152, def.BossName + " - Complete Set Stats");
            AddLabel(25, 48, 0x34, def.SetName);
            AddHtml(25, 73, 450, 38, "<BASEFONT COLOR=#BBBBBB>Combined numeric bonuses detected on the equipped display items and alternate drops. Item-specific tooltips remain the authoritative detail.</BASEFONT>", false, false);

            int y = 118;
            if (stats.Count == 0)
            {
                AddLabel(25, y, 1152, "No additive numeric bonuses were detected for this set.");
            }
            else
            {
                for (int i = 0; i < stats.Count && y < height - 28; i++)
                {
                    AddLabel(30, y, 1153, stats[i].Name);
                    AddLabel(345, y, 1152, stats[i].Value.ToString());
                    y += 22;
                }
            }
        }

        private static List<StatLine> CollectStats(BossDropMannequin mannequin)
        {
            Dictionary<string, int> totals = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            List<Item> items = new List<Item>();

            if (mannequin != null)
            {
                for (int i = 0; i < mannequin.Items.Count; i++)
                    if (mannequin.Items[i] != null) items.Add(mannequin.Items[i]);

                BossDropDisplayCase c = mannequin.DisplayCase;
                if (c != null && !c.Deleted)
                    for (int i = 0; i < c.Items.Count; i++)
                        if (c.Items[i] != null) items.Add(c.Items[i]);
            }

            for (int i = 0; i < items.Count; i++)
                AccumulateItem(items[i], totals);

            List<StatLine> result = new List<StatLine>();
            foreach (KeyValuePair<string, int> kv in totals)
                if (kv.Value != 0) result.Add(new StatLine(Pretty(kv.Key), kv.Value));

            result.Sort(delegate(StatLine a, StatLine b) { return String.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase); });
            return result;
        }

        private static void Add(Dictionary<string, int> totals, string name, int value)
        {
            if (value == 0 || String.IsNullOrEmpty(name)) return;
            int old;
            totals.TryGetValue(name, out old);
            totals[name] = old + value;
        }

        private static void AccumulateItem(Item item, Dictionary<string, int> totals)
        {
            string[] direct = { "PhysicalResistance", "FireResistance", "ColdResistance", "PoisonResistance", "EnergyResistance" };
            Type t = item.GetType();

            for (int i = 0; i < direct.Length; i++)
            {
                PropertyInfo p = t.GetProperty(direct[i], BindingFlags.Instance | BindingFlags.Public);
                if (p != null && p.PropertyType == typeof(int) && p.GetIndexParameters().Length == 0)
                {
                    try { Add(totals, direct[i], (int)p.GetValue(item, null)); } catch { }
                }
            }

            string[] groups = { "Attributes", "ArmorAttributes", "WeaponAttributes", "ClothingAttributes" };
            for (int g = 0; g < groups.Length; g++)
            {
                PropertyInfo gp = t.GetProperty(groups[g], BindingFlags.Instance | BindingFlags.Public);
                if (gp == null || gp.GetIndexParameters().Length != 0) continue;

                object obj = null;
                try { obj = gp.GetValue(item, null); } catch { }
                if (obj == null) continue;

                PropertyInfo[] props = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                for (int p = 0; p < props.Length; p++)
                {
                    PropertyInfo pi = props[p];
                    if (pi.PropertyType != typeof(int) || !pi.CanRead || pi.GetIndexParameters().Length != 0) continue;
                    if (String.Equals(pi.Name, "Count", StringComparison.OrdinalIgnoreCase)) continue;
                    try { Add(totals, pi.Name, (int)pi.GetValue(obj, null)); } catch { }
                }
            }
        }

        private static string Pretty(string s)
        {
            if (String.IsNullOrEmpty(s)) return s;
            System.Text.StringBuilder b = new System.Text.StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (i > 0 && Char.IsUpper(s[i]) && !Char.IsUpper(s[i - 1])) b.Append(' ');
                b.Append(s[i]);
            }
            return b.ToString();
        }
    }
}
