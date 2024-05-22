using System;
using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Items;
using System.Collections.Generic;
using System.Linq;

namespace Server.Mobiles
{
    [CorpseName("an Aliens corpse")]
	public class Alien1 : BaseCreature
	{
        public static TimeSpan TalkDelay = TimeSpan.FromSeconds(10.0); //the delay between talks is 10 seconds
        public DateTime m_NextTalk;

      //  public override WeaponAbility GetWeaponAbility()
       // {
     //       return Utility.RandomBool() ? WeaponAbility.ConcussionBlow : WeaponAbility.FrenziedWhirlwind; // for diffent weaponabiltys look in WeaponAbility.cs
      //  }
        private static string[] m_Names = new string[]
		{
          "???",
          "???????"
		};

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (DateTime.Now >= m_NextTalk && InRange(m, 4) && InLOS(m)) // check if it's time to talk & mobile in range & in los.
            {
                m_NextTalk = DateTime.Now + TalkDelay; // set next talk time 
                switch (Utility.Random(7))
                {
                    case 0: Say("I'm going to Probe you"); //make it say ...
                        PlaySound(1066); //play giggle sound
                        break;
                    case 1: Say("You will all be my test subjects!");
                        PlaySound(1071); //play huh sound
                        break;
                    case 2: Say("One finger or five?");
                        PlaySound(1055); //play clear throat sound
                        break; //
                    case 3: Say("You don't have any friends; nobody likes you!");
                        PlaySound(1074); //play no!! sound
                        break;
                    case 4: Say("You may aswell just come with me");
                        PlaySound(1067); //play groan sound
                        break;
                    case 5: Say("Haha That tickles");
                        PlaySound(1073); //play lough sound
                        break;
                    case 6: Say("You vile Creature");
                        PlaySound(1094); //play spit sound
                        break;
                };
            }
        }

        public override WeaponAbility GetWeaponAbility() 
        {
            int ability = Utility.Random(3);
            if (ability == 1)
                return WeaponAbility.MortalStrike;
            else if (ability == 2)
                return WeaponAbility.MortalStrike;
            else
                return WeaponAbility.FrenziedWhirlwind;
        }
		
		[Constructable]
        public Alien1()
            : base(AIType.AI_Predator, FightMode.Weakest, 18, 10, 0.01, 0.2)    
		{
           
            Title = "The Alien";
            Name = m_Names[Utility.Random(m_Names.Length)];
            Body = 777;
			Hue = 0;
            BaseSoundID = 357;

			SetStr( 5000, 6000 );
			SetDex( 1500, 2500 );
			SetInt( 3500, 4500 );

            SetHits(350000, 500000);
            SetMana(900000);
            SetStam(502, 550);
	
			SetDamage( 50, 50 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 98, 100 );
			SetResistance( ResistanceType.Fire, 98, 100 );
			SetResistance( ResistanceType.Cold, 98, 100 );
			SetResistance( ResistanceType.Poison, 98, 100 );
			SetResistance( ResistanceType.Energy, 98, 100 );

            SetSkill(SkillName.EvalInt, 120.0, 150.0);
			SetSkill( SkillName.Magery, 120.0, 150.0 );
			SetSkill( SkillName.MagicResist, 250.0, 250.0 );
			SetSkill( SkillName.Swords, 130.0, 130.0 );
			SetSkill( SkillName.Tactics, 130.0, 130.0 );
			SetSkill( SkillName.Wrestling, 150.0, 200.0 );
			SetSkill( SkillName.Parry, 115.1, 150.0 );

			Fame = 5000;
			Karma = -5000;

            PackGold(10000, 25000);
			PackGold(10000, 25000);
			PackGold(10000, 25000);

			CanSwim = true;
			GuardImmune = true;
			
            VirtualArmor = 30;
          
		}

        public override void GenerateLoot()
        {
            AddLoot( LootPack.Poor );
            AddLoot( LootPack.Average, 2 ); 
        }
		
		public override bool IgnoreYoungProtection {  get { return Core.ML; } }
		public override bool BardImmune { get { return !Core.SE; } }
        public override bool Unprovokable { get { return Core.SE; } }
        public override bool AreaPeaceImmune { get { return Core.SE; } }
		public override bool HasBreath { get { return true; } }
		public override double BonusPetDamageScalar { get { return (Core.SE) ? 100.0 : 1.0; } }
		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } } 
		public override int TreasureMapLevel{ get{ return 6; } }
		public override int Meat{ get{ return 3; } }
		public override bool CanBeParagon { get { return false; } }
		public virtual bool TeleportsTo { get { return true; } }
        public virtual TimeSpan TeleportDuration { get { return TimeSpan.FromSeconds(2); } }
        public virtual int TeleportRange { get { return 16; } }
        public virtual double TeleportProb { get { return 1.0; } }
		public virtual bool TeleportsPets { get { return false; } }
		
		public void DrainLife()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 2 ) )
			{
				Mobile combatant = this.Combatant as Mobile;
				if (m != this && m != this.ControlMaster && IsEnemy(m));		
			}

			foreach ( Mobile m in list )
			{
				DoHarmful( m );

				m.FixedParticles( 0x37B9, 10, 15, 5013, 0x496, 0, EffectLayer.Waist );
				m.PlaySound( 0x218 );

				m.SendMessage( "You feel yourself and those around you get weaker!" );

				int toDrain = Utility.RandomMinMax( 50, 70 );

				Hits += toDrain;
				m.Damage( toDrain, this );
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.75 >= Utility.RandomDouble() )
				DrainLife();
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.1 >= Utility.RandomDouble() )
				DrainLife();
		}
		
/*		public override void OnDeath( Container c )
		{
			base.OnDeath( c );	
			
			if ( Utility.RandomDouble() < 0.95 )
            {
			switch ( Utility.Random( 14 ) )
            {
				case 0: c.DropItem(new RAD()); break;
				case 1: c.DropItem(new RAD()); break;
				case 2: c.DropItem(new RAD()); break;
				case 3: c.DropItem(new RAD()); break;
				case 4: c.DropItem(new RAD()); break;
				case 5: c.DropItem(new RAD()); break;
				case 6: c.DropItem(new RAD()); break;
				case 7: c.DropItem(new RAD()); break;
				case 8: c.DropItem(new RAD()); break;
				case 9: c.DropItem(new RAD()); break;
				case 10: c.DropItem(new RAD()); break;
				case 11: c.DropItem(new RAD()); break;
				case 12: c.DropItem(new RAD()); break;
				case 13: c.DropItem(new RAD()); break;

            }
		}
		if ( Utility.RandomDouble() < 0.01 )
		{
           switch ( Utility.Random( 6 ) )
            {
				case 0: c.DropItem(new PerfectedArms()); break;
				case 1: c.DropItem(new PerfectedCap()); break;
				case 2: c.DropItem(new PerfectedChest()); break;
				case 3: c.DropItem(new PerfectedGorget()); break;
				case 4: c.DropItem(new PerfectedGloves()); break;
				case 5: c.DropItem(new PerfectedLegs()); break;
			}
		}
	}*/
	        public override void OnDeath(Container c)
        {
					base.OnDeath(c);
            List<DamageStore> rights = GetLootingRights();            

            foreach (Mobile m in rights.Select(x => x.m_Mobile).Distinct())
            {
                if (m is PlayerMobile)
                {
					Item item = new RAD();
					
					if ( Utility.RandomDouble() < 0.01 )
					{
					switch ( Utility.Random( 6 ) )
					{
				case 0: item = new PerfectedArms(); break;
				case 1: item = new PerfectedCap(); break;
				case 2: item = new PerfectedChest(); break;
				case 3: item = new PerfectedGloves(); break;
				case 4: item = new PerfectedGorget(); break;
				case 5: item = new PerfectedLegs(); break;
					}
					}							

                        if (m.Backpack == null || !m.Backpack.TryDropItem(m, item, false))
                        {
                            m.BankBox.DropItem(item);
                        }

                        m.SendMessage("An Item has been Beamed to you!"); // You received a Quest Item!
                    }
                }
        }
		public Alien1( Serial serial ) : base( serial )
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