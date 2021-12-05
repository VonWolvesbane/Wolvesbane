//Crafted By ReApEr
using System;
using System.Collections;
using Server.Mobiles;
namespace Server.Mobiles
{
	[CorpseName( "Skeletal Remains of A cat" )]
	public class DeadCat : BaseMount
	{
		[Constructable]
		public DeadCat() : this( "A Dead Cat" )
		{
			Hue = 0;
		}

		[Constructable]
		public DeadCat( string name ) : base( name, 0x5A1 , 0x3ED0 , AIType.AI_Necro, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0x69;


			SetDamage( 25, 35 );
			
			SetStr(400);
            SetDex(100);
            SetInt(1450, 1555);

            SetHits(540);
            SetMana(1400, 1500);

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 70, 90 );
			SetResistance( ResistanceType.Fire, 10, 50 );
			SetResistance( ResistanceType.Cold, 70, 95 );
			SetResistance( ResistanceType.Poison, 90, 95 );
			SetResistance( ResistanceType.Energy, 10, 50 );

			SetSkill( SkillName.MagicResist, 100.1, 120.0 );
			SetSkill( SkillName.Tactics, 100.2, 120.0 );
			SetSkill( SkillName.Wrestling, 100.2, 120.0 );
			SetSkill( SkillName.SpiritSpeak, 100.1, 120 );
			SetSkill( SkillName.Necromancy, 100.1, 120 );
			
			Skills[SkillName.Anatomy].Cap = 200;
			Skills[SkillName.Necromancy].Cap = 200;
			Skills[SkillName.SpiritSpeak].Cap = 200;
			Skills[SkillName.MagicResist].Cap = 200;
			Skills[SkillName.Tactics].Cap = 200;
			Skills[SkillName.Wrestling].Cap = 200;

			Fame = 300;
			Karma = -15990;


			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 110.0;
		}

		public override int Meat{ get{ return 0; } }
		public override int Hides{ get{ return 0; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }
		
		public void DrainLife()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 4 ) )
			{
				Mobile combatant = this.Combatant as Mobile;
				if (m != this && m != this.ControlMaster && IsEnemy(m));		
			}

			foreach ( Mobile m in list )
			{
				DoHarmful( m );

				m.FixedParticles( 0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				m.SendMessage( "You feel yourself Weaken as the Cat gets stronger!" );

				int toDrain = Utility.RandomMinMax( 10, 15 );

				Hits += toDrain;
				m.Damage( toDrain, this );
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.25 >= Utility.RandomDouble() )
				DrainLife();
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.1 >= Utility.RandomDouble() )
				DrainLife();
		}

		public override bool CanAngerOnTame
        {
            get
            {
                return true;
            }
        }
		public DeadCat( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}