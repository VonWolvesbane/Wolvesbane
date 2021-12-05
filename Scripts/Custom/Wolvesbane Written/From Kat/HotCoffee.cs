/* Created by Hammerhand */

using System;
using Server;

namespace Server.Items
{
    public abstract class BaseCeramicMug : Item
    {
        public virtual int Bonus { get { return 0; } }
        public virtual StatType Type { get { return StatType.Int; } }

        public BaseCeramicMug(int hue)
            : base(0x995)
        {
            Weight = 1.0;
            Hue = 0;
        }
        public BaseCeramicMug(Serial serial)
            : base(serial)
        {
        }
        public virtual bool Apply(Mobile from)
        {
            bool applied = Spells.SpellHelper.AddStatOffset(from, Type, Bonus, TimeSpan.FromMinutes(30.0));

            if (!applied)
                from.SendLocalizedMessage(502173); // You are already under a similar effect.

            return applied;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
            else if (Apply(from))
            {
                from.FixedEffect(0x375A, 10, 15);
                from.PlaySound(0x1E7);
                from.SendMessage("You begin to feel wide awake!");
                Delete();
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class HotCoffee : BaseCeramicMug
    {
        public override int Bonus { get { return 10; } }
        public override StatType Type { get { return StatType.Int; } }

        //public override int LabelNumber{ get{ return 1041073; } } // prized fish

        [Constructable]
        public HotCoffee()
            : base(0x995)
        {
            this.Name = "Cup of Coffee";
        }

        public HotCoffee(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}