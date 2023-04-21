using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "an mechanical ostard corpse" )]
	public class MechanicalFrenziedOstard : BaseMount
	{
		[Constructable]
		public MechanicalFrenziedOstard() : this( "a mechanical frenzied ostard" )
		{
		}

		[Constructable]
		public MechanicalFrenziedOstard( string name ) : base( name, 0xDA, 0x3EA4, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Hue = 981;

			BaseSoundID = 0x275;

			SetStr( 200, 470 );
			SetDex( 156, 315 );
			SetInt( 110, 215 );

			SetHits( 1171, 2110 );
			SetMana( 2000 );

			SetDamage( 125, 230 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 35 );
			SetResistance( ResistanceType.Fire, 15, 20 );
			SetResistance( ResistanceType.Poison, 40, 45 );
			SetResistance( ResistanceType.Energy, 20, 25 );

			SetSkill( SkillName.MagicResist, 75.1, 80.0 );
			SetSkill( SkillName.Tactics, 79.3, 94.0 );
			SetSkill( SkillName.Wrestling, 79.3, 94.0 );
            SetSkill( SkillName.Anatomy, 60.3, 75.0 );
            SetSkill( SkillName.Poisoning, 70.1, 80.0 );

			Fame = 1500;
			Karma = -1500;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 0;
		}

		public override int Meat{ get{ return 3; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish | FoodType.Eggs | FoodType.FruitsAndVegies; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Ostard; } }

		public MechanicalFrenziedOstard( Serial serial ) : base( serial )
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
