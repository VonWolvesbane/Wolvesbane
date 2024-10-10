using System;
using Server.Items;

namespace Server.Items
{
    public class InvisLegArmor : BaseArmor
    {
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Leather;

        [Constructable]
        public InvisLegArmor() : base(0xC48C) // Legs armor item ID
        {
            Name = "Invis Leg Armor";
            Hue = 0; // Set the hue if you want to change its color
            Weight = 1.0; // Set weight to 1
            Layer = Layer.InnerLegs; // Correct layer for leg armor
        }

        public InvisLegArmor(Serial serial) : base(serial)
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
