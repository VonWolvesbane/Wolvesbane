using System;
using Server;

namespace Server.Items
{
	public class WyrmSoulMace : Mace 
	{
		public override int ArtifactRarity{ get{ return 11; } }

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Disarm; } }

		public override int AosMinDamage{ get{ return 18; } }
		public override int AosMaxDamage{ get{ return 22; } }
		public override int AosSpeed{ get{ return 40; } }
		public override float MlSpeed{ get{ return 2.75f; } }

		public override int InitMinHits{ get{ return 100; } }
		public override int InitMaxHits{ get{ return 100; } }

		[Constructable]
		public WyrmSoulMace()
		{
			Weight = 5.0;
            		Name = "a Wyrm Soul's Mace";
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

		public WyrmSoulMace( Serial serial ) : base( serial )
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