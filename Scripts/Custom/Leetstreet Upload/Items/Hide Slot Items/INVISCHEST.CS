using System;
using Server.Items;

namespace Server.Items
{
    public class InvisChest : BaseArmor
    {
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Plate;

        [Constructable]
        public InvisChest() : base(0xC48D) // Chest armor item ID
        {
            Name = "Invis Chest";
            Hue = 0; // Set the hue if you want to change its color
            Weight = 1.0; // Set weight to 1
            Layer = Layer.InnerTorso; // Correct layer for chest armor
        }

        public InvisChest(Serial serial) : base(serial)
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
