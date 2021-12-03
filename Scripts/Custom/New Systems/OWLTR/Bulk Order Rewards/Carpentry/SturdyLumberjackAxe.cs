/*    
-<>>--<<>>--<0>>--<< <<2005>> >>--<<0>--<<>>--<<>-
|        ____________________________            |
|     -=(_)__________________________)=-         |
|          \_    All Crafts 1.0.0    _\          |
|           \_  -------------------   _\         |
|            )       Created By:        )        |
|           /_  Sirsly & Lucid Nagual _/         |
|         _/__________________________/          |
|      -=(_)__________________________)=-        |
|                                                |
-<>>-<< Based off of Daat99's OWLTR system >>-<<>-
*/
using System;
using Server;
using Server.Engines.Harvest;

namespace Server.Items
{
	public class SturdyLumberjackAxe : BaseAxe
	{
		
		public override HarvestSystem HarvestSystem{ get{ return Lumberjacking.System; } }

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.CrushingBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Dismount; } }

		public override int StrengthReq{ get{ return 35; } }
		public override int MinDamage{ get{ return 14; } }
		public override int MaxDamage{ get{ return 16; } }
		public override float Speed{ get{ return 37; } }

			
		[Constructable]
		public SturdyLumberjackAxe() : this( 180 )
		{
		}

		[Constructable]
		public SturdyLumberjackAxe( int uses ) : base( 0xF49 )
		{
                  Name = "a sturdy lumberjacks axe";
			Weight = 11.0;
			Hue = 0x973;
			UsesRemaining = uses;
			ShowUsesRemaining = true;
		}

		public SturdyLumberjackAxe( Serial serial ) : base( serial )
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