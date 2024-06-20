using System;
using Server.Items;

namespace Server.Items
{
    public class SwordBeltSkin : BaseArmor
    {
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Plate;

        [Constructable]
        public SwordBeltSkin() : base(0xA40D)
        {
            Name = "Sword Belt Skin";
            Hue = 0; // Set the hue if you want to change its color
            Weight = 1.0;
            Layer = Layer.Waist;
        }

        public SwordBeltSkin(Serial serial) : base(serial)
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
