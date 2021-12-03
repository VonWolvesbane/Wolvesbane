using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xF5C, 0xF5D )]
	public class ElvenNoblesMace : BaseBashing
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Disarm; } }

		public override int StrengthReq{ get{ return 45; } }
		public override int MinDamage{ get{ return 12; } }
		public override int MaxDamage{ get{ return 14; } }

		public override int InitMinHits{ get{ return 150; } }
		public override int InitMaxHits{ get{ return 150; } }

		[Constructable]
		public ElvenNoblesMace() : base( 0xF5C )
		{
			Weight = 14.0;
			Name = "Elven Noble's Mace";
			Hue = 2212;
			Attributes.SpellChanneling = 1;
			Attributes.BonusDex = 5;
			Attributes.BonusStam = 5;
			Attributes.WeaponSpeed = 15;
		}

		public ElvenNoblesMace( Serial serial ) : base( serial )
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