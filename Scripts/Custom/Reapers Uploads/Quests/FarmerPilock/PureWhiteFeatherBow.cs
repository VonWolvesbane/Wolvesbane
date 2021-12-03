using System;
using Server;

namespace Server.Items
{
	public class PureWhiteFeatherBow : Bow 
	{
		public override int ArtifactRarity{ get{ return 11; } }

		public override int EffectID{ get{ return 0xF42; } }
		public override Type AmmoType{ get{ return typeof( Arrow ); } }
		public override Item Ammo{ get{ return new Arrow(); } }
				public override int MinDamage{ get{ return 22; } }
		public override int MaxDamage{ get{ return 27; } }

		public override int InitMinHits{ get{ return 300; } }
		public override int InitMaxHits{ get{ return 300; } }

		[Constructable]
		public PureWhiteFeatherBow()
		{
			Weight = 5.0;
            		Name = "Pure White Feather Bow";
            		Hue = 1153;

			WeaponAttributes.DurabilityBonus = 10; 
			WeaponAttributes.HitEnergyArea = 50;
			WeaponAttributes.HitLeechHits = 100;
			WeaponAttributes.HitPoisonArea = 50;   
			WeaponAttributes.ResistEnergyBonus = 10;
			WeaponAttributes.ResistPoisonBonus = 5;
			WeaponAttributes.UseBestSkill = 1;

			Attributes.BonusDex = 25;
			Attributes.BonusStr = 25;
			Attributes.Luck = 100;
			Attributes.SpellChanneling = 1;
			Attributes.WeaponSpeed = 100;
			Attributes.WeaponDamage = 100;

			StrRequirement = 01;

			LootType = LootType.Blessed;
		}

		public PureWhiteFeatherBow( Serial serial ) : base( serial )
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
