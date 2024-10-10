using System;
using Server.Items;

namespace Server.Items
{
    public class InvisShield : BaseShield
    {
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Plate;

        [Constructable]
        public InvisShield() : base(0xC48E) // Shield item ID
        {
            Name = "Invis Shield";
            Hue = 0; // Set the hue if you want to change its color
            Weight = 1.0; // Set the weight of the shield
        }

        public InvisShield(Serial serial) : base(serial)
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
