using System;
using Server.Items;

namespace Server.Items
{
    public class InvisHelmSkin : BaseArmor
    {
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Plate;

        [Constructable]
        public InvisHelmSkin() : base(0xC48F)
        {
            Name = "Invis Helm Skin";
            Hue = 0; // Set the hue if you want to change its color
            Weight = 1.0;
            Layer = Layer.Helm; // Correct layer for the helmet slot
        }

        public InvisHelmSkin(Serial serial) : base(serial)
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
