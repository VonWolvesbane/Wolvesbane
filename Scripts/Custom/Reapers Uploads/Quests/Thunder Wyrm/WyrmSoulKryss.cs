using System;
using Server.Network;
using Server;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x1401, 0x1400 )]
	public class WyrmSoulKryss : BaseSword 
	{
		public override int ArtifactRarity{ get{ return 11; } }

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.InfectiousStrike; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.BleedAttack; } }

		public override int MinDamage{ get{ return 18; } }
		public override int MaxDamage{ get{ return 22; } }
		public override float Speed{ get{ return 2.50f; } }

		public override int InitMinHits{ get{ return 100; } }
		public override int InitMaxHits{ get{ return 100; } }

		public override SkillName DefSkill{ get{ return SkillName.Fencing; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }
		
		[Constructable]
		public WyrmSoulKryss() : base( 0x1401 )
		{
			Weight = 2.0;
            		Name = "a Wyrm Soul's Kryss";
            		Hue = 1154;
                         
			Attributes.AttackChance = 45;
			Attributes.BonusDex = 25;
	//		Attributes.BonusHits = 10;
	//		Attributes.BonusInt = 7;
	//		Attributes.BonusMana = 10;
	//		Attributes.BonusStam = 10;
	//		Attributes.BonusStr = 7;
	//		Attributes.CastRecovery = x;
	//		Attributes.CastSpeed = x;
	//		Attributes.DefendChance = 15;
	//		Attributes.EnhancePotions = x;
	//		Attributes.LowerManaCost = 10;
	//		Attributes.LowerRegCost = 10;
			Attributes.Luck = 250;
	//		Attributes.ReflectPhysical = x;
	//		Attributes.RegenHits = 2;
	//		Attributes.RegenMana = 2;
	//		Attributes.RegenStam = 2;
			Attributes.SpellChanneling = 1; // 1 for true, 0 for false
	//		Attributes.SpellDamage = x;
			Attributes.WeaponDamage = 45;
			Attributes.WeaponSpeed = 30;
	//		Attributes.ReflectPhysical = x;
	//		Attributes.RegenHits = x;
	//		WeaponAttributes.DurabilityBonus = x; 
	//		WeaponAttributes.HitColdArea = x;
	//		WeaponAttributes.HitDispel = 45;
	//		WeaponAttributes.HitEnergyArea = x;
	//		WeaponAttributes.HitFireArea = x;
	//		WeaponAttributes.HitFireball = x;
	//		WeaponAttributes.HitHarm = x;
			WeaponAttributes.HitLeechHits = 75;
	//		WeaponAttributes.HitLeechMana = x;
	//		WeaponAttributes.HitLeechStam = 56;
			WeaponAttributes.HitLightning = 75;
	//		WeaponAttributes.HitLowerAttack = 48;
			WeaponAttributes.HitLowerDefend = 100;
			WeaponAttributes.HitMagicArrow = 55;
	//		WeaponAttributes.HitPhysicalArea = x;
	//		WeaponAttributes.HitPoisonArea = x;
	//		WeaponAttributes.LowerStatReq = x;
			WeaponAttributes.MageWeapon = 30; // 1 for true, 0 for false.
	//		WeaponAttributes.ResistColdBonus = x;
	//		WeaponAttributes.ResistEnergyBonus = x;
	//		WeaponAttributes.ResistFireBonus = x;
	//		WeaponAttributes.ResistPhysicalBonus = x;
	//		WeaponAttributes.ResistPoisonBonus = x;
			WeaponAttributes.SelfRepair = 10;
			WeaponAttributes.UseBestSkill = 1; // 1 for true, 0 for false.
	//		PhysicalBonus = 20;
	//		FireBonus = -5;
	//		ColdBonus = 10;
	//		PoisonBonus = -10;
	//		EnergyBonus = 10;
			LootType = LootType.Blessed;
		}

		public WyrmSoulKryss( Serial serial ) : base( serial )
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

			if ( Weight == 1.0 )
				Weight = 2.0;
		}
	}
}