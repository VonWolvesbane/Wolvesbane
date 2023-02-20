/*
Training Elemental script by Murzin @ RunUO with contribution by jjarmis
Instructions: drop into your scripts/custom/monster folder and
	you can either add it in-game or on a spawner but since
	he wont die, it doesnt need to be on a spawner.  modify
	the hits as much as you want but if you change other things
	it may not work as designed.
*/
using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a training elemental corpse" )]
	public class timedtrainer2h : BaseCreature
	{
		private int m_Lifespan;
		private Timer m_Timer;
		public override double DispelDifficulty{ get{ return 117.5; } }
		public override double DispelFocus{ get{ return 45.0; } }

		[Constructable]
		public timedtrainer2h() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0, 0 )
		{
			Name = "a Timed Training Elemental";
			Body = 14;
			BaseSoundID = 268;
			Hue = 0x21;
			CantWalk = true;

			SetStr( 50, 50 );
			SetDex( 350, 350 );
			SetInt( 71, 92 );

			SetHits( 30000, 30000 );

			SetDamage( 0, 0 );

			SetDamageType( ResistanceType.Physical, 0 );
			SetDamageType( ResistanceType.Fire, 0 );
			SetDamageType( ResistanceType.Cold, 0 );
			SetDamageType( ResistanceType.Poison, 0 );
			SetDamageType( ResistanceType.Energy, 0 );

			SetResistance( ResistanceType.Physical, 150 );
			SetResistance( ResistanceType.Fire, 150 );
			SetResistance( ResistanceType.Cold, 150 );
			SetResistance( ResistanceType.Poison, 150 );
			SetResistance( ResistanceType.Energy, 150 );

			SetSkill( SkillName.MagicResist, 120.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Wrestling, 100.0 );

			Fame = 0;
			Karma = 0;

			VirtualArmor = 350;
			ControlSlots = 0;
			
			if (Lifespan > 0)
            {
                m_Lifespan = Lifespan;
                StartTimer();
            }

		}

		public override void GenerateLoot()
		{
		}

		public override bool AutoDispel{ get{ return true; } }
		public override bool BardImmune{ get{ return false; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		      
		public override bool DeleteCorpseOnDeath { get { return true; }
		}
		public override void OnThink()
		{
			if ( Hits != HitsMax )
			{
				Hits = HitsMax;
			}
		}
			
		public timedtrainer2h( Serial serial ) : base( serial )
		{
		}
		
        public virtual int Lifespan { get { return Utility.RandomMinMax(600, 28800); } }
        public virtual bool UseSeconds { get { return true; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TimeLeft
        {
            get { return m_Lifespan; }
            set
            {
                m_Lifespan = value;
                InvalidateProperties();
            }
        }
		
		public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
			
			            if (Lifespan > 0)
						{
                if (UseSeconds)
					
					{ 	
					TimeSpan t = TimeSpan.FromSeconds(TimeLeft);
				    int days = t.Days;
                    int hours = t.Hours;
                    int minutes = t.Minutes;

						//list.Add((m_Lifespan));
						list.Add(1153090, t.Hours.ToString()); // Lifespan: ~1_val~ hours
						list.Add(1153089, t.Minutes.ToString()); // Lifespan: ~1_val~ minutes
						list.Add(1072517, t.Seconds.ToString()); // Lifespan: ~1_val~ seconds
						Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerCallback( InvalidateProperties ) );  
					}
				//list.Add(1072517, m_Lifespan.ToString()); // Lifespan: ~1_val~ seconds
                else
                {
                    TimeSpan t = TimeSpan.FromSeconds(TimeLeft);

                    int weeks = (int)t.Days / 7;
                    int days = t.Days;
                    int hours = t.Hours;
                    int minutes = t.Minutes;

                    if (weeks > 1)
                        list.Add(1153092, (t.Days / 7).ToString()); // Lifespan: ~1_val~ weeks
                    else if (days > 1)
                        list.Add(1153091, t.Days.ToString()); // Lifespan: ~1_val~ days
                    else if (hours > 1)
                        list.Add(1153090, t.Hours.ToString()); // Lifespan: ~1_val~ hours
                    else if (minutes > 1)
                        list.Add(1153089, t.Minutes.ToString()); // Lifespan: ~1_val~ minutes
                    else
                        list.Add(1072517, t.Seconds.ToString()); // Lifespan: ~1_val~ seconds
                }
            }
        }

        public virtual void StartTimer()
        {
            if (m_Timer != null)
                return;

            m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), new TimerCallback(Slice));
            m_Timer.Priority = TimerPriority.OneSecond;
        }

        public virtual void StopTimer()
        {
            if (m_Timer != null)
                m_Timer.Stop();

            m_Timer = null;
        }

        public virtual void Slice()
        {
            m_Lifespan -= 1;

            InvalidateProperties();

            if (m_Lifespan <= 0)
                Delete();
        }

        public virtual void Delete()
        {
 
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
            Effects.PlaySound(Location, Map, 0x201);
			
            StopTimer();
            Delete();
        
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write((int)m_Lifespan);
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			 m_Lifespan = reader.ReadInt();
			 StartTimer();
		}
	}
}