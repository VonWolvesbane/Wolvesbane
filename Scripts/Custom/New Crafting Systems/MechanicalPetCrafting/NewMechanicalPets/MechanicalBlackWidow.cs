using System;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a mechanical black widow spider corpse" )] // stupid corpse name
	public class MechanicalBlackWidow : BaseCreature
	{
		[Constructable]
		public MechanicalBlackWidow() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "a mechanical black widow";
			Body =  0x9D;
                                                Hue = 981;
			BaseSoundID = 0x388; // TODO: validate

			SetStr( 280, 500 );
			SetDex( 216, 515 );
			SetInt( 236, 360 );

			SetHits( 1146, 2150 );

			SetDamage( 215, 420 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 20, 30 );
			SetResistance( ResistanceType.Fire, 10, 20 );
			SetResistance( ResistanceType.Cold, 10, 20 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.Anatomy, 60.3, 75.0 );
			SetSkill( SkillName.Poisoning, 70.1, 80.0 );
			SetSkill( SkillName.MagicResist, 50.1, 60.0 );
			SetSkill( SkillName.Tactics, 75.1, 80.0 );
			SetSkill( SkillName.Wrestling, 80.1, 85.0 );

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 30;

            Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 0;
            {
			}
            }

		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
        public override bool CanAngerOnTame { get { return true; } }
		

		public MechanicalBlackWidow( Serial serial ) : base( serial )
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
