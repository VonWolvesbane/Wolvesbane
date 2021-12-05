using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a mechanical hellsteed corpse" )]
	public class MechanicalHellSteed : BaseMount
	{
		

		[Constructable] 
		public MechanicalHellSteed() : this( "a mechanical hellsteed" )
		{
		}

		[Constructable]
		public MechanicalHellSteed( string name ) : base( name, 793, 0x3EBB, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
            Hue = 981;
			
		
        
		
		
			SetStr( 500, 810 );
			SetDex( 201, 610 );
			SetInt( 210, 515 );

			SetHits( 2010, 4220 );

			SetDamage( 300, 500 );

			SetDamageType( ResistanceType.Physical, 65 );
			SetDamageType( ResistanceType.Fire, 75 );

			SetResistance( ResistanceType.Physical, 70, 80 );
			SetResistance( ResistanceType.Fire, 90 );
			SetResistance( ResistanceType.Poison, 100 );

			SetSkill( SkillName.MagicResist, 95.1, 110.0 );
			SetSkill( SkillName.Tactics, 60.0 );
			SetSkill( SkillName.Wrestling, 90.1, 110.0 );
            SetSkill( SkillName.Anatomy, 60.3, 75.0 );

			Fame = 1500;
			Karma = -1500;

            Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 0;
		}
        public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
        public override bool CanAngerOnTame { get { return true; } }
        public override bool HasBreath{ get{ return true; } } // fire breath enabled

		public MechanicalHellSteed( Serial serial ) : base( serial )
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
