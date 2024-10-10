using System;
using Server.Items;

namespace Server.Items
{
    public class InvisEarrings : BaseJewel
    {
        [Constructable]
        public InvisEarrings() : base(0xC48A, Layer.Earrings) // Earrings item ID
        {
            Name = "Invis Earrings";
            Hue = 0; // Set the hue if you want to change its color
            Weight = 0.1; // Set a small weight for earrings
        }

        public InvisEarrings(Serial serial) : base(serial)
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
