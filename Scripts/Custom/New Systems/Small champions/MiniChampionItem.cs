//Crafted By ReApEr
using System;
using System.Collections;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{
    public class MiniChampionStone : Item
    {
        private bool m_Active = false;
        private string m_MiniChampname = "No name";

        [CommandProperty(AccessLevel.GameMaster)]
        public string MiniChampName
        {
            get { return m_MiniChampname; }
            set { m_MiniChampname = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get { return m_Active; }
            set { m_Active = value; InvalidateProperties(); }
        }

        public MiniChampionStone() : base(0x9A1C)
        {
            Name = "Summon Shredder";
            Movable = false;
        }

        public MiniChampionStone(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write(m_Active);
            writer.Write(m_MiniChampname);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            m_Active = reader.ReadBool();
            m_MiniChampname = reader.ReadString();
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, m_MiniChampname);

            base.OnSingleClick(from);

            if (Active)
                LabelTo(from, "Active - DO NOT DELETE!");
            else
                LabelTo(from, "Not Active - Safe for delete");

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel == AccessLevel.Player)
            {
                return;
            }

            if (Active)
            {
                this.PublicOverheadMessage(MessageType.Regular, 0, false, "This is already activated.");
            }
            else
            {
                this.PublicOverheadMessage(MessageType.Regular, 0, false, "Beginning activation of MiniChamp...");
                this.Active = true;
                this.ActivateStone();
            }
        }

        public virtual void ActivateStone()
        {
            base.PublicOverheadMessage(MessageType.Regular, 0, false, "...finished!");
        }

        public virtual void AnnounceMiniChamp(string announce)
        {
            ArrayList mobs = new ArrayList(World.Mobiles.Values);
            foreach (Mobile m in mobs)
            {
                if (m is TownCrier)
                {
                    m.Say(announce);
                }
            }
        }
    }
}
