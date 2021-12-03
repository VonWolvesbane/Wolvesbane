//Created By Milva
using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "Mechanical Pet corpse" )]
	public class MechanicalPet : BaseCreature
	{
		[Constructable]
		public MechanicalPet() : this( "a Mechanical Pet" )
		{
		}

		[Constructable]
        public MechanicalPet(string name)
            : base(AIType.AI_Animal, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a Mechanical Pet";
            Body = 0x2F5;
            
            Hue =  Utility.RandomList ( 2878, 2842, 2956 );
			


                     SetStr(496, 525);
		     SetDex( 156, 225 );
		     SetInt( 250, 280 );

                     SetHits(300, 310);

                        SetDamage( 21, 25 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Fire, 25 );

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Fire, 80, 90 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 60, 70 );

            SetSkill(SkillName.Anatomy, 80.1, 100.0);
            SetSkill(SkillName.Healing, 80.1, 100.0);
			SetSkill( SkillName.MagicResist, 100.5, 150.0 );
			SetSkill( SkillName.Tactics, 97.6, 100.0 );
			SetSkill( SkillName.Wrestling, 97.6, 100.0 );

			Fame = 14000;
			Karma = -14000;

			VirtualArmor = 100;

			Tamable = true;
			ControlSlots = 4;
			MinTameSkill = 101.1;
			{
                             
			}

			
			PackGem();
			PackGold( 250, 350 );
			PackItem( new SulfurousAsh( Utility.RandomMinMax( 3, 5 ) ) );
			//PackScroll( 1, 5 );
			//PackPotion();
          }

        

        public override int GetIdleSound()
		{
            if ( !Controlled )
				
			return 0x218;
            return base.GetIdleSound();
		}

		public override int GetAngerSound()
		{
            if (!Controlled)
			return 0x26C;
            return base.GetAngerSound();
		}

		public override int GetDeathSound()
		{
            if (!Controlled)
			return 0x211;
            return base.GetDeathSound();
		}

		public override int GetAttackSound()
		{
            if (!Controlled)
			return 0x232;
            return base.GetAttackSound();
		}

		public override int GetHurtSound()
		{
            if (!Controlled)
			return 0x140;
            return base.GetHurtSound();
		}

       
       public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
        public virtual bool CanHeal { get { return true; } }
        public override bool CanHealOwner { get { return true; } }
	   public override int Meat{ get{ return 5; } }
		public override int Hides{ get{ return 10; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
        public override bool CanAngerOnTame { get { return true; } }

        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.BleedAttack;
        }

		public MechanicalPet( Serial serial ) : base( serial )
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
