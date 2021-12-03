using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class DRoboBlaster : HeavyCrossbow
	{

		public override int DefMaxRange{ get{ return 20; } }

		public override int EffectID{ get{ return 0xF42; } }
		public override Type AmmoType{ get{ return typeof( BlasterAmmo ); } }

		public override int StrengthReq{ get{ return 50; } }
		public override int MinDamage{ get{ return 20; } }
		public override int MaxDamage{ get{ return 25; } }

		public override int InitMinHits{ get{ return 250; } }
		public override int InitMaxHits{ get{ return 250; } }

		[Constructable]
		public DRoboBlaster() : base()
		{
			Weight = 5.0;
			Name = "Robo Blaster";
			Layer = Layer.OneHanded;
                        Attributes.AttackChance = 50;
			Attributes.DefendChance = 25;
			WeaponAttributes.HitFireball = 10;
			WeaponAttributes.HitFireArea = 10;
			WeaponAttributes.HitLeechMana = Utility.Random( 1, 10 );
			WeaponAttributes.HitLeechStam = Utility.Random( 1, 10 );
			WeaponAttributes.HitLeechHits = Utility.Random( 1, 10 );
			WeaponAttributes.HitHarm = 10;
			WeaponAttributes.HitColdArea = 10;
			WeaponAttributes.SelfRepair = 5;
			LootType = LootType.Regular;
			Hue = 1985;
		}

		public DRoboBlaster( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}