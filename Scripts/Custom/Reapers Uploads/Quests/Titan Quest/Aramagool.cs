using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "Aramagool's Corpse" )]
	public class Aramagool : Titan
	{
		[Constructable]
		public Aramagool()
		{
			Name = "Aramagool";
			Body = 76;
			BaseSoundID = 609;


			SetStr( 500 );
			SetDex( 300 );
			SetInt( 200 );

			SetHits( 5250 );
			SetMana( 100 );

			SetDamage( 25, 30 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Poison, 25 );

			SetResistance( ResistanceType.Physical, 60 );
			SetResistance( ResistanceType.Fire, 80 );
			SetResistance( ResistanceType.Cold, 90 );
			SetResistance( ResistanceType.Poison, 90 );
			SetResistance( ResistanceType.Energy, 85 );

			SetSkill( SkillName.MagicResist, 500.0 );
			SetSkill( SkillName.Tactics, 150.0 );
			SetSkill( SkillName.Wrestling, 150.0 );
			SetSkill( SkillName.Magery, 50.0 );
			SetSkill( SkillName.Parry, 50.0 );

			Fame = 1000;
			Karma = -8000;

			VirtualArmor = 50;

			PackItem( new AncientTitanHelm() );

		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich );
		}
		
		public override double BonusPetDamageScalar { get { return (Core.SE) ? 100.0 : 1.0; } }
		
		public Aramagool( Serial serial ) : base( serial )
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
