using System;
using Server.Items;

namespace Server.Items
{
    public class InvisKilt : BaseOuterLegs
    {
        [Constructable]
        public InvisKilt() : base(0xC48B)
        {
            Name = "Invis Kilt";
            Hue = 0;
            Weight = 1.0;
        }

        public InvisKilt(Serial serial) : base(serial)
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
