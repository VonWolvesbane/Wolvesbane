using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.BossDrops
{
    // A vendor-style glass display case used for ALL alternate drops belonging to one mannequin.
    public class BossDropDisplayCase : Container
    {
        private string m_DisplayKey;
        private Mobile m_Mannequin;

        [CommandProperty(AccessLevel.GameMaster)]
        public string DisplayKey
        {
            get { return m_DisplayKey; }
            set { m_DisplayKey = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Mannequin
        {
            get { return m_Mannequin; }
            set { m_Mannequin = value; }
        }

        public BossDropDisplayCase(string displayKey, string bossName) : base(0x2FEB)
        {
            m_DisplayKey = displayKey;
            Name = bossName + " - Alternate Drops";
            Movable = false;
            LootType = LootType.Blessed;
        }

        // Backward-compatible constructor for v2.4/v2.5 saves/code.
        public BossDropDisplayCase(string displayKey, string bossName, int sectionIndex) : this(displayKey, bossName)
        {
        }

        public BossDropDisplayCase(Serial serial) : base(serial)
        {
        }

        public void AddDisplayItem(Item item)
        {
            if (item == null || item.Deleted)
                return;

            item.Movable = false;
            item.LootType = LootType.Blessed;
            DropItem(item);
            InvalidateProperties();
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Alternate boss drops");
            list.Add("Double-click to inspect the items inside.");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(3);
            writer.Write(m_DisplayKey);
            writer.Write(m_Mannequin);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 1)
            {
                m_DisplayKey = reader.ReadString();
                m_Mannequin = reader.ReadMobile();

                // v2 stored a section index. Read and discard it for save compatibility.
                if (version == 2)
                    reader.ReadInt();
            }
            else
            {
                m_DisplayKey = String.Empty;
                m_Mannequin = null;
            }

            Movable = false;
            LootType = LootType.Blessed;
        }
    }
}
