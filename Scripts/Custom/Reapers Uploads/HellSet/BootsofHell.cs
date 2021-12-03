
using System;
using Server;

namespace Server.Items
{
	public class BootsOfHell : BaseArmor
	{
		public override int ArtifactRarity{ get{ return 666; } }

		[Constructable]
		public BootsOfHell() : base (5899)
		{
			Name = "Boots Of HELL";
			Hue = 0x27;
			
			LootType = LootType.Regular;
			this.Weight = 4;
			Attributes.BonusHits = Utility.RandomMinMax(10, 30);
			Attributes.BonusMana = Utility.RandomMinMax(10, 30);
			Attributes.BonusStam = Utility.RandomMinMax(10, 30);
			Attributes.BonusStr = Utility.RandomMinMax(10, 20); 
            Attributes.BonusDex = Utility.RandomMinMax(10, 20); 
            Attributes.BonusInt = Utility.RandomMinMax(10, 20); 
			Attributes.Luck = Utility.RandomMinMax(10, 200);
			this.Attributes.WeaponDamage = 25;
			this.Attributes.DefendChance = 25;
			this.Attributes.CastRecovery = 3;
			this.Attributes.CastSpeed = 3;
			this.Attributes.LowerManaCost = 10;
			this.Attributes.LowerRegCost = 20;
			PhysicalBonus = 10;
			FireBonus = 10;
			ColdBonus = 10;
			PoisonBonus = 10;
			EnergyBonus = 10;
		}


		public BootsOfHell( Serial serial ) : base( serial )
		{
		}

		public override ArmorMaterialType MaterialType
		{
			get
			{
				return ArmorMaterialType.Cloth;
			}
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

	}
}
