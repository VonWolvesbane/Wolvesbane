using System;
using Server;
using Server.Engines.Craft;
namespace Server.Items
{
	public class GlovesOfHell : LeatherGloves
	{
		public override int ArtifactRarity{ get{ return 666; } }
public override bool IsArtifact { get { return true; } }
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public GlovesOfHell()
		{
			Hue = 0x27; 
            Name = "Gloves Of HELL";

			Attributes.BonusHits = Utility.RandomMinMax(20, 40);
			Attributes.BonusMana = Utility.RandomMinMax(20, 40);
			Attributes.BonusStam = Utility.RandomMinMax(20, 40);
			Attributes.BonusStr = Utility.RandomMinMax(10, 30); 
            Attributes.BonusDex = Utility.RandomMinMax(10, 30); 
            Attributes.BonusInt = Utility.RandomMinMax(10, 30); 
			Attributes.RegenStam = Utility.RandomMinMax(10, 30);
			Attributes.RegenMana = Utility.RandomMinMax(10, 30);
			Attributes.RegenHits = Utility.RandomMinMax(10, 30);
			Attributes.Luck = Utility.RandomMinMax(10, 200);
			this.Attributes.SpellDamage = 35;
			this.Attributes.WeaponDamage = 25;
			this.Attributes.DefendChance = 25;
			this.Attributes.CastRecovery = 3;
			this.Attributes.CastSpeed = 3;
			this.Attributes.LowerManaCost = 20;
			this.Attributes.LowerRegCost = 30;
			FireBonus = 25;
			ColdBonus = 25;
            PoisonBonus = 25;
            PhysicalBonus = 25;
            EnergyBonus = 15;
		}

		public GlovesOfHell( Serial serial ) : base( serial )
		{
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