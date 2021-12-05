using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a stone elemental corpse" )]
	public class StoneElemental : BaseCreature
	{

		[Constructable]
		public StoneElemental() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.4 )
		{
			Name = "a stone elemental";
			Hue = 1000;
			Body = 14;
			BaseSoundID = 268;

			SetStr( 12666, 15566 );
			SetDex( 66, 85 );
			SetInt( 71, 92 );

			SetHits( 7600, 9300 );

			SetDamage( 9, 16 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 75, 85 );
			SetResistance( ResistanceType.Fire, 88, 99 );
			SetResistance( ResistanceType.Cold, 88, 99 );
			SetResistance( ResistanceType.Poison, 98, 100 );
			SetResistance( ResistanceType.Energy, 88, 99 );

			SetSkill( SkillName.MagicResist, 50.1, 95.0 );
			SetSkill( SkillName.Tactics, 60.1, 140.0 );
			SetSkill( SkillName.Wrestling, 60.1, 140.0 );

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 34;
			ControlSlots = 2;

			PackItem( new RoughStone( Utility.RandomMinMax( 0, 20 ) ) );
			PackItem( new Gold(4500, 7500) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.Gems, 3 );
		}

		public override bool BleedImmune{ get{ return true; } }
		public override int TreasureMapLevel{ get{ return 1; } }

		public StoneElemental( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}