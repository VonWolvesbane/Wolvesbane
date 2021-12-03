/*
 created by:
     /\            888                   888     .d8888b.   .d8888b.  
____/_ \____       888                   888    d88P  Y88b d88P  Y88b 
\  ___\ \  /       888                   888    888    888 888    888 
 \/ /  \/ /    .d88888  8888b.   8888b.  888888 Y88b. d888 Y88b. d888 
 / /\__/_/\   d88" 888     "88b     "88b 888     "Y888P888  "Y888P888 
/__\ \_____\  888  888 .d888888 .d888888 888           888        888 
    \  /      Y88b 888 888  888 888  888 Y88b.  Y88b  d88P Y88b  d88P 
     \/        "Y88888 "Y888888 "Y888888  "Y888  "Y8888P"   "Y8888P"  
*/
using Server.Engines.Harvest;

namespace Server.Items
{
	[FlipableAttribute( 0xf45, 0xf46 )]
	public class GargoylesAxe : BaseAxe, IUsesRemaining
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.BleedAttack; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MortalStrike; } }

		public override int StrengthReq{ get{ return 40; } }
		public override int MinDamage{ get{ return 15; } }
		public override int MaxDamage{ get{ return 17; } }
		public override float Speed{ get{ return 33; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		public override HarvestSystem HarvestSystem{ get{ return Lumberjacking.System; } }

		[Constructable]
		public GargoylesAxe() : this( Utility.RandomMinMax( 101, 125 ) )
		{
		}

		[Constructable]
		public GargoylesAxe( int uses ) : base( 0xf45 )
		{
			Weight = 4.0;
			//Hue = 0x973; //removed in RunUO
			UsesRemaining = uses;
			ShowUsesRemaining = true;
			Name = "Gargoyles Axe";
		}

		public GargoylesAxe( Serial serial ) : base( serial )
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