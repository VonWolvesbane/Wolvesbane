using System;
using Server;
using System.Collections;
using Server.Mobiles;
using System.Linq;
using Server.Items;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a vorpal bunny corpse")]
    public class EasterBunny1 : BaseCreature
    {
        [Constructable]
        public EasterBunny1()
            : base(AIType.AI_Melee, FightMode.Weakest, 15, 2, 0.05, 0.05)
        {
            Name = "An Easter bunny";
            Body = 302;
            Hue = 1166;

            SetStr(450);
            SetDex(2100, 3000);
            SetInt(1000, 2000);

            SetHits(300000);
            SetStam(20000);
            SetMana(10000);

            SetDamage(150);

            SetDamageType(ResistanceType.Energy, 100);
			
			SetResistance( ResistanceType.Physical, 75, 75 );
			SetResistance( ResistanceType.Fire, 75, 75 );
			SetResistance( ResistanceType.Cold, 75, 75 );
			SetResistance( ResistanceType.Poison, 100, 100 );
			SetResistance( ResistanceType.Energy, 75, 75 );

            SetSkill(SkillName.MagicResist, 200.0);
            SetSkill(SkillName.Tactics, 115.0);
			SetSkill(SkillName.Anatomy, 90, 120);
            SetSkill(SkillName.Wrestling, 115.0);

            Fame = 1000;
            Karma = 0;

            VirtualArmor = 10;

            BeginPoop();

        }
		
		public void ManaDrain()
		{
			ArrayList list = new ArrayList();

			foreach ( Mobile m in this.GetMobilesInRange( 20 ) )
			{
				if ( m == this || !CanBeHarmful( m ) )
					continue;
				
				if ( m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team) )
					list.Add( m );
				if ( m is PlayerMobile )
					list.Add( m );
				
			}

			foreach ( Mobile m in list )
			{
				DoHarmful( m );

				m.FixedParticles( 0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist );
				m.PlaySound( 0x231 );

				m.SendMessage( "You feel your mana drain from you!" );

				int toDrain = Utility.RandomMinMax( 70, 100 ); //how much mana are we going to take?

                m.Mana -= toDrain;
				m.Damage( toDrain, this );
			}
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( 0.1 >= Utility.RandomDouble() )
				ManaDrain();
			
			if ( 0.3 >= Utility.RandomDouble() )
                defender.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                defender.PlaySound(0x307);
				defender.Damage(Utility.Random(50, 60), this); //how much damage is popping candy gonna do?
                defender.SendMessage("The Easter bunny threw some popping candy");
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if ( 0.1 >= Utility.RandomDouble() )
				ManaDrain();
		}
		public override void OnKilledBy( Mobile mob )
		{
			if(mob is PlayerMobile)
			{
                int chance = 1 + (int)Math.Min(10, ((PlayerMobile)mob).Luck / 30); //chance to obtain easteregg. minimum is 10% maximum is players luck / 30+1 (2000 luck would give a 67.6% chance)

                if (chance >= Utility.Random(100))
                {
                    Type t = m_Loot[Utility.Random(m_Loot.Length)];

                    if (t != null)
                    {
                        Item loot = Loot.Construct(t);

                        if (loot != null)
                        {
                            Container pack = mob.Backpack;

                            if (pack == null || !pack.TryDropItem(mob, loot, false))
                            {
                                mob.BankBox.DropItem(loot);
                                mob.SendMessage("An EasterEgg Has been placed in your Bank!");
                            }
                            else
                                mob.SendMessage("An EasterEgg has been placed in your Backpack!");
                        }
                    }
                }
			}
		}
		public override bool ReacquireOnMovement{ get{ return true; } }
		public virtual void SpawnPackItems()
        {
            int carrots = Utility.RandomMinMax(5, 10);
            PackItem(new Carrot(carrots));

            if (Utility.Random(5) == 0)
                PackItem(new EasterEgg1());

            PackStatue();
        }

        public EasterBunny1(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 1; } }
        public override bool BardImmune { get { return !Core.AOS; } }
		
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 4);
            AddLoot(LootPack.Rich, 2);
        }

		public static Type[] Artifacts { get { return m_Loot; } }

		private static Type[] m_Loot = new Type[]
		{
			typeof(EasterEgg1), //artifacts for bunny to drop
		};
        public virtual void BeginPoop()
        {
			Timer.DelayCall(TimeSpan.FromSeconds( Utility.RandomMinMax( 10, 30 )), new TimerCallback(BeginPoop));
            new Poop().MoveToWorld(Location, Map);
            PlaySound(1064);
        }

        public override int GetAttackSound() { return 0xC9; }
        public override int GetHurtSound() { return 0xCA; }
        public override int GetDeathSound() { return 0xCB; }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

        }
   
        public class Poop : BrightlyColoredEggs
        {
            public Poop()
                : base()
            {
                Movable = false;
                Name = "Jelly Bean Poop";
                Timer.DelayCall(TimeSpan.FromSeconds(15.0), new TimerCallback(Delete));
            }

            public Poop(Serial serial)
                : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write((int)0);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                int version = reader.ReadInt();

                Delete();
            }
        }
    }
}

