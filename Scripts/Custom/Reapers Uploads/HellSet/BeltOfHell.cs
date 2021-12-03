using System;

namespace Server.Items
{
    public class BeltOfHell : BaseArmor
	{
				public override int ArtifactRarity{ get{ return 666; } }
        [Constructable]
        public BeltOfHell() : base (10128)
        {
			this.Name = "Belt of HELL";
			this.Hue = 0x27;
			this.Weight = 1;


			Attributes.BonusHits = Utility.RandomMinMax(15, 30);
			Attributes.BonusMana = Utility.RandomMinMax(15, 30);
			Attributes.BonusStam = Utility.RandomMinMax(15, 30);
			Attributes.BonusStr = Utility.RandomMinMax(10, 30); 
            Attributes.BonusDex = Utility.RandomMinMax(10, 30); 
            Attributes.BonusInt = Utility.RandomMinMax(10, 30); 
			Attributes.Luck = Utility.RandomMinMax(10, 200);
			this.Attributes.WeaponDamage = 25;
			this.Attributes.DefendChance = 25;
			this.Attributes.CastRecovery = 3;
			this.Attributes.CastSpeed = 3;
			this.Attributes.LowerManaCost = 10;
			this.Attributes.LowerRegCost = 20;

		}

		public BeltOfHell(Serial serial)
			: base(serial)
        {
        }
		
		public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Cloth;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}