using System;
using Server;
using Server.Engines.Craft;
namespace Server.Items
{
	public class WitchesGloves : LeatherGloves
	{
		public override int ArtifactRarity{ get{ return 100; } }
		public override bool IsArtifact { get { return true; } }
		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public WitchesGloves()
		{

			this.SkillBonuses.SetValues(0, SkillName.EvalInt, 10);
			this.SkillBonuses.SetValues(1, SkillName.Inscribe, 10);

			Hue = 2342; 
            Name = "<Body bgcolor=Black; text=#9400D3><Big><center>Mage Fists</Body>";

			Attributes.BonusHits = 50;
			Attributes.BonusMana = 150;
			Attributes.BonusStr = 25; 
            Attributes.BonusDex = 25; 
            Attributes.BonusInt = 100; 
			//Attributes.RegenStam = Utility.RandomMinMax(10, 30);
			Attributes.RegenMana = 125;
			//Attributes.RegenHits = Utility.RandomMinMax(10, 30);
			Attributes.Luck = 200;
			AbsorptionAttributes.CastingFocus = 25;
			ArmorAttributes.SelfRepair = 10;
			this.Attributes.SpellDamage = 200;
			this.Attributes.WeaponDamage = 10;
			this.Attributes.DefendChance = 10;
			this.Attributes.CastRecovery = 5;
			this.Attributes.CastSpeed = 5;
			this.Attributes.LowerManaCost = 20;
			this.Attributes.LowerRegCost = 30;
			FireBonus = 4;
			ColdBonus = 5;
            PoisonBonus = 11;
            PhysicalBonus = 1;
            EnergyBonus = 14;
		}

		public WitchesGloves( Serial serial ) : base( serial )
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