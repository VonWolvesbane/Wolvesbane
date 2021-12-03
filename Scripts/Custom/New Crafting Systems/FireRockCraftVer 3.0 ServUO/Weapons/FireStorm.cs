/* Created by Hammerhand*/

using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class FireStorm : BaseRanged
	{
        public override int Hue { get { return 1359; } }
		public override int EffectID{ get{ return 0x1BFE; } }
		public override Type AmmoType{ get{ return typeof( FireBolt ); } }
		public override Item Ammo{ get{ return new FireBolt(); } }

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MovingShot; } }

		public override int StrengthReq{ get{ return 30; } }
		public override int MinDamage{ get{ return 15; } }
		public override int MaxDamage{ get{ return 22; } }
        public override float Speed { get { return 4.50f; } }


		public override int DefMaxRange{ get{ return 7; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }

		[Constructable]
		public FireStorm() : base( 0x26C3 )
		{
            Name = "Fire Storm";
			Weight = 6.0;

            WeaponAttributes.HitFireArea = Utility.RandomMinMax(15, 45);
            Attributes.WeaponSpeed = Utility.RandomMinMax(10, 25);
		}

        public FireStorm(Serial serial)
            : base(serial)
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