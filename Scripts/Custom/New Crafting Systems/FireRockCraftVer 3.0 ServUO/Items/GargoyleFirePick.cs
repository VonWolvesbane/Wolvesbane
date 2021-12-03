/* Created by Hammerhand*/

using System;
using Server;
using Server.Engines.Harvest;

namespace Server.Items
{
    public class GargoyleFirePick : BaseAxe, IUsesRemaining
	{

        public override HarvestSystem HarvestSystem { get { return FireRockMining.System; } }

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Disarm; } }

		public override int StrengthReq{ get{ return 50; } }
		public override int MinDamage{ get{ return 13; } }
		public override int MaxDamage{ get{ return 15; } }
        public override float Speed { get { return 3.00f; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Slash1H; } }

		[Constructable]
		public GargoyleFirePick() : this( 50 )
		{
		}

		[Constructable]
		public GargoyleFirePick( int uses ) : base( 0xE86 )
		{
            Name = "GargoyleFirePick";
			Weight = 11.0;
			Hue = 1358;
			UsesRemaining = uses;
			ShowUsesRemaining = true;
		}

        public GargoyleFirePick(Serial serial): base(serial)
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