using System;
using Server;
using Server.Mobiles;

namespace Server.Custom.BossDrops
{
    // Legacy v2.12 compatibility type. New displays no longer create boss statues.
    public class BossDropBossStatue : Mobile
    {
        private string m_DisplayKey;
        private Mobile m_Mannequin;

        [CommandProperty(AccessLevel.GameMaster)]
        public string DisplayKey { get { return m_DisplayKey; } set { m_DisplayKey = value; InvalidateProperties(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Mannequin { get { return m_Mannequin; } set { m_Mannequin = value; } }

        public BossDropBossStatue(string displayKey, string bossName, int body, int hue)
        {
            m_DisplayKey = displayKey;
            Name = bossName;
            Title = "boss exhibit";
            Body = body;
            Hue = hue;
            Blessed = true;
            CantWalk = true;
            Direction = Direction.East;
        }

        public BossDropBossStatue(Serial serial) : base(serial) { }

        public override bool CanBeDamaged() { return false; }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !from.InRange(this, 4))
            {
                if (from != null) from.SendMessage("You are too far away to inspect this exhibit.");
                return;
            }

            BossDropDefinition def = BossDropRegistry.Find(m_DisplayKey);
            BossDropMannequin mannequin = m_Mannequin as BossDropMannequin;
            if (def != null)
            {
                from.CloseGump(typeof(BossDropInfoGump));
                from.SendGump(new BossDropInfoGump(from, def, mannequin));
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Decorative boss replica - no combat AI or loot.");
            list.Add("Double-click for boss and set information.");
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
            Blessed = true;
            CantWalk = true;
            Direction = Direction.East;
        }
    }
}
