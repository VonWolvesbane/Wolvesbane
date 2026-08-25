using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public enum BossDropDisplayRace
    {
        Human,
        Elf,
        Gargoyle
    }

    public class BossDropMannequin : Mobile
    {
        private string m_DisplayKey;
        private BossDropDisplayRace m_DisplayRace;
        private bool m_Female;
        private List<Server.Custom.BossDrops.BossDropDisplayCase> m_DisplayCases;
        private Server.Custom.BossDrops.BossDropInfoPlaque m_InfoPlaque;
        private Server.Custom.BossDrops.BossDropBossStatue m_BossStatue;

        [CommandProperty(AccessLevel.GameMaster)]
        public string DisplayKey
        {
            get { return m_DisplayKey; }
            set { m_DisplayKey = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public BossDropDisplayRace DisplayRace
        {
            get { return m_DisplayRace; }
            set { m_DisplayRace = value; ApplyBody(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool FemaleDisplay
        {
            get { return m_Female; }
            set { m_Female = value; Female = value; ApplyBody(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Server.Custom.BossDrops.BossDropInfoPlaque InfoPlaque
        {
            get { return m_InfoPlaque; }
            set { m_InfoPlaque = value; }
        }



        [CommandProperty(AccessLevel.GameMaster)]
        public Server.Custom.BossDrops.BossDropBossStatue BossStatue
        {
            get { return m_BossStatue; }
            set { m_BossStatue = value; }
        }

        public List<Server.Custom.BossDrops.BossDropDisplayCase> DisplayCases
        {
            get
            {
                if (m_DisplayCases == null)
                    m_DisplayCases = new List<Server.Custom.BossDrops.BossDropDisplayCase>();

                return m_DisplayCases;
            }
        }

        // Kept so older v2.3 mannequins can still deserialize their single linked case.
        [CommandProperty(AccessLevel.GameMaster)]
        public Server.Custom.BossDrops.BossDropDisplayCase DisplayCase
        {
            get { return DisplayCases.Count > 0 ? DisplayCases[0] : null; }
            set
            {
                DisplayCases.Clear();
                if (value != null)
                    DisplayCases.Add(value);
            }
        }

        public BossDropMannequin(string displayKey, string name, BossDropDisplayRace race, bool female)
        {
            m_DisplayKey = displayKey;
            m_DisplayRace = race;
            m_Female = female;
            m_DisplayCases = new List<Server.Custom.BossDrops.BossDropDisplayCase>();

            Name = name;
            Title = "boss drop display";
            Blessed = true;
            CantWalk = true;
            Female = female;

            ApplyBody();
        }

        public BossDropMannequin(Serial serial) : base(serial)
        {
            m_DisplayCases = new List<Server.Custom.BossDrops.BossDropDisplayCase>();
        }

        private void ApplyBody()
        {
            Female = m_Female;

            switch (m_DisplayRace)
            {
                case BossDropDisplayRace.Elf:
                    Body = m_Female ? 0x25E : 0x25D;
                    break;
                case BossDropDisplayRace.Gargoyle:
                    Body = m_Female ? 0x29B : 0x29A;
                    break;
                default:
                    Body = m_Female ? 0x191 : 0x190;
                    break;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null)
                return;

            if (!from.InRange(this, 4))
            {
                from.SendMessage("You are too far away to inspect this display.");
                return;
            }

            DisplayPaperdollTo(from);
        }

        public void ClearDisplayItems()
        {
            for (int i = Items.Count - 1; i >= 0; --i)
            {
                Item item = Items[i];

                if (item != null && !item.Deleted)
                    item.Delete();
            }
        }

        public bool AddDisplayItem(Item item)
        {
            if (item == null || item.Deleted)
                return false;

            item.Movable = false;
            item.LootType = LootType.Blessed;

            try
            {
                AddItem(item);
                return item.Parent == this;
            }
            catch
            {
                return false;
            }
        }

        public void AddDisplayCase(Server.Custom.BossDrops.BossDropDisplayCase displayCase)
        {
            if (displayCase != null && !displayCase.Deleted && !DisplayCases.Contains(displayCase))
                DisplayCases.Add(displayCase);
        }

        public override bool CanBeDamaged()
        {
            return false;
        }

        public override void OnDelete()
        {
            ClearDisplayItems();

            for (int i = DisplayCases.Count - 1; i >= 0; --i)
            {
                Server.Custom.BossDrops.BossDropDisplayCase c = DisplayCases[i];
                if (c != null && !c.Deleted)
                    c.Delete();
            }

            DisplayCases.Clear();

            if (m_InfoPlaque != null && !m_InfoPlaque.Deleted)
                m_InfoPlaque.Delete();

            m_InfoPlaque = null;

            if (m_BossStatue != null && !m_BossStatue.Deleted)
                m_BossStatue.Delete();

            m_BossStatue = null;
            base.OnDelete();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Double-click to inspect this boss drop set.");

            if (DisplayCases.Count > 0)
                list.Add("{0} alternate drop display case(s) beside this mannequin.", DisplayCases.Count);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(5);
            writer.Write(m_DisplayKey);
            writer.Write((int)m_DisplayRace);
            writer.Write(m_Female);
            writer.Write(m_InfoPlaque);
            writer.Write(m_BossStatue);

            writer.Write(DisplayCases.Count);
            for (int i = 0; i < DisplayCases.Count; i++)
                writer.Write(DisplayCases[i]);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_DisplayCases = new List<Server.Custom.BossDrops.BossDropDisplayCase>();

            if (version >= 1)
            {
                m_DisplayKey = reader.ReadString();
                m_DisplayRace = (BossDropDisplayRace)reader.ReadInt();
                m_Female = reader.ReadBool();

                if (version >= 4)
                    m_InfoPlaque = reader.ReadItem() as Server.Custom.BossDrops.BossDropInfoPlaque;

                if (version >= 5)
                    m_BossStatue = reader.ReadMobile() as Server.Custom.BossDrops.BossDropBossStatue;

                if (version >= 3)
                {
                    int count = reader.ReadInt();
                    for (int i = 0; i < count; i++)
                    {
                        Server.Custom.BossDrops.BossDropDisplayCase c = reader.ReadItem() as Server.Custom.BossDrops.BossDropDisplayCase;
                        if (c != null)
                            m_DisplayCases.Add(c);
                    }
                }
                else if (version >= 2)
                {
                    Server.Custom.BossDrops.BossDropDisplayCase oldCase = reader.ReadItem() as Server.Custom.BossDrops.BossDropDisplayCase;
                    if (oldCase != null)
                        m_DisplayCases.Add(oldCase);
                }
            }
            else
            {
                m_DisplayKey = String.Empty;
                m_DisplayRace = BossDropDisplayRace.Human;
                m_Female = false;
            }

            Blessed = true;
            CantWalk = true;
            ApplyBody();
        }
    }
}
