// Created by Tom Sibilsky aka Neptune

using System;
using System.Collections.Generic;
using daat99;
using Server.Items;

namespace Server.Mobiles



{      [CorpseName( " corpse of Malacoda" )]
              public class Malacoda : BaseCreature
              {
		 public override WeaponAbility GetWeaponAbility()
		  {
		return Utility.RandomBool() ? WeaponAbility.CrushingBlow : WeaponAbility.ConcussionBlow;
		  
                      }
			private Timer m_Timer;

	 
	private int i_PsCount;
	public int PsCount { get { return i_PsCount; } set { i_PsCount = value; InvalidateProperties(); } }
	private bool i_ChampionSpawn;
	public bool ChampionSpawn { get { return i_ChampionSpawn; } set { i_ChampionSpawn = value; InvalidateProperties(); } }

	[Constructable]
  public Malacoda() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
                            {
                                Name = "Malacoda";
				Title = "Leader of the Malabranche";
				Body = 400;
				Female = false; 
				Hue = 33775;
			ChampionSpawn = i_ChampionSpawn; //set if it spawn with champ spawn or normal spawn

							SetStr( 8400 );
                            SetDex( 8400 );
                            SetInt( 8400 );
                            SetHits( 850000 );
                            SetDamage( 150, 200 );
                            SetDamageType( ResistanceType.Physical, 100 );
                            SetDamageType( ResistanceType.Cold, 100 );
                            SetDamageType( ResistanceType.Fire, 100 );
                            SetDamageType( ResistanceType.Energy, 100 );
                            SetDamageType( ResistanceType.Poison, 100 );

                            SetResistance( ResistanceType.Physical, 200 );
                            SetResistance( ResistanceType.Cold, 200 );
                            SetResistance( ResistanceType.Fire, 80 );
                            SetResistance( ResistanceType.Energy, 200 );
                            SetResistance( ResistanceType.Poison, 200 );

                        SetSkill( SkillName.EvalInt, 320.0 );
			SetSkill( SkillName.Magery, 590.0 );
			SetSkill( SkillName.Meditation, 640.0 );
			SetSkill( SkillName.Poisoning, 480.0 );
			SetSkill( SkillName.MagicResist, 590.0 );
			SetSkill( SkillName.Tactics, 790.0 );
			SetSkill( SkillName.Wrestling, 450.0 );
			SetSkill( SkillName.Swords, 400.0 );
			SetSkill( SkillName.Anatomy, 700.0 );
			SetSkill( SkillName.Parry, 350.0 );


			/*m_Timer = new TeleportTimer( this );
			m_Timer.Start();
			*/


            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 85;

			PackGold( 20000, 30000 );

	    		MalabrancheChest Chest = new MalabrancheChest();
			Chest.Movable = false;
			AddItem(Chest);
			
			MalabrancheArms Arms = new MalabrancheArms();
			Arms.Movable = false;
			AddItem(Arms);
			
			MalabrancheLegs Legs = new MalabrancheLegs();
			Legs.Movable = false;
			AddItem(Legs);
			
			MalabrancheGloves Gloves = new MalabrancheGloves();
			Gloves.Movable = false;
			AddItem(Gloves);
			
			MalabrancheVest HalfApron = new MalabrancheVest();
			HalfApron.Movable = false;
			AddItem(HalfApron);
			
			MalabrancheHelm Helm = new MalabrancheHelm();
			Helm.Movable = false;
			AddItem(Helm);

	    		MalabrancheRobe Robe = new MalabrancheRobe();
			Robe.Movable = false;
			AddItem(Robe);

            






                            }
		public override void GenerateLoot()
		{
			switch (Utility.Random(70))
			{
				case 0: PackItem(new MalabrancheRobe()); break;
				case 1: PackItem(new MalabrancheHelm()); break;
				case 2: PackItem(new MalabrancheLegs()); break;
				case 3: PackItem(new MalabrancheArms()); break;
				case 4: PackItem(new MalabrancheGloves()); break;
				case 5: PackItem(new MalabrancheChest()); break;
				case 6: PackItem(new MalabrancheVest()); break;


			}

			public void GiveCraftPowerScroll() //Generate power scroll routine and add to pack
			{
				List<Mobile> toGive = new List<Mobile>(); //no idea :(

				List<AggressorInfo> list = Aggressors; //I think it add list players that attacked it
				for (int i = 0; i < list.Count; ++i)
				{
					AggressorInfo info = list[i];

					if (info.Attacker.Player && info.Attacker.Alive && (DateTime.Now - info.LastCombatTime) < TimeSpan.FromSeconds(30.0) && !toGive.Contains(info.Attacker))
						toGive.Add(info.Attacker);
				}

				List<DamageStore> rights = GetLootingRights();
				for (int i = rights.Count - 1; i >= 0; --i)
				{
					DamageStore ds = rights[i];

					if (ds.m_HasRight)
						toGive.Add(ds.m_Mobile);
				}
					if (toGive.Count == 0)//if nobody attacked it and it didn't attack anybody then break operation and no ps MUAH
						return;

				// Randomize //absolutly no idea
				for (int i = 0; i < toGive.Count; ++i)
				{
					int rand = Utility.Random(toGive.Count);
					Mobile hold = toGive[i];
					toGive[i] = toGive[rand];
					toGive[rand] = hold;
				}

				PsCount = ChampionSpawn ? 2 : 1; //set how many ps to give if it spawned using champ spawn or normal spawn
				for (int i = 0; i < PsCount; ++i)
				{
					Mobile m = (Mobile)toGive[i % toGive.Count];
					int level;
					double random = Utility.RandomDouble();
					if (0.05 >= random)
						level = 150;
					else if (0.10 >= random) // select the level of the ps
						level = 145;
					else if (0.15 >= random) // select the level of the ps
						level = 140;
					else if (0.20 >= random) // select the level of the ps
						level = 135;
					else if (0.25 >= random) // select the level of the ps
						level = 130;
					else if (0.30 >= random) // select the level of the ps
						level = 105;
					else if (0.35 >= random) // select the level of the ps
						level = 125;
					else if (0.45 >= random) // select the level of the ps
						level = 110;
					else if (0.55 >= random)
						level = 120;
					else
						level = 115;

					if (OWLTROptionsManager.IsEnabled(OWLTROptionsManager.OPTIONS_ENUM.RECIPE_CRAFT))
						m.AddToBackpack(new ResourceRecipe());

					switch (Utility.Random(16)) // select which skill to use in the ps
					{
						case 0: m.AddToBackpack(new PowerScroll(SkillName.Swords, level)); break; // give blacksmith ps acording to the ps level we selected before
						case 1: m.AddToBackpack(new PowerScroll(SkillName.Fencing, level)); break;
						case 2: m.AddToBackpack(new PowerScroll(SkillName.Macing, level)); break;
						case 3: m.AddToBackpack(new PowerScroll(SkillName.Archery, level)); break;
						case 4: m.AddToBackpack(new PowerScroll(SkillName.Wrestling, level)); break;
						case 5: m.AddToBackpack(new PowerScroll(SkillName.Parry, level)); break;
						case 6: m.AddToBackpack(new PowerScroll(SkillName.Tactics, level)); break;
						case 7: m.AddToBackpack(new PowerScroll(SkillName.Anatomy, level)); break;
						case 8: m.AddToBackpack(new PowerScroll(SkillName.Healing, level)); break;
						case 9: m.AddToBackpack(new PowerScroll(SkillName.Magery, level)); break;
						case 10: m.AddToBackpack(new PowerScroll(SkillName.Meditation, level)); break;
						case 11: m.AddToBackpack(new PowerScroll(SkillName.EvalInt, level)); break;
						case 12: m.AddToBackpack(new PowerScroll(SkillName.MagicResist, level)); break;
						case 13: m.AddToBackpack(new PowerScroll(SkillName.AnimalTaming, level)); break;
						case 14: m.AddToBackpack(new PowerScroll(SkillName.AnimalLore, level)); break;
						case 15: m.AddToBackpack(new PowerScroll(SkillName.Veterinary, level)); break;
						case 16: m.AddToBackpack(new PowerScroll(SkillName.Musicianship, level)); break;
						case 17: m.AddToBackpack(new PowerScroll(SkillName.Provocation, level)); break;
						case 18: m.AddToBackpack(new PowerScroll(SkillName.Discordance, level)); break;
						case 19: m.AddToBackpack(new PowerScroll(SkillName.Peacemaking, level)); break;
						case 20: m.AddToBackpack(new PowerScroll(SkillName.Chivalry, level)); break;
						case 21: m.AddToBackpack(new PowerScroll(SkillName.Focus, level)); break;
						case 22: m.AddToBackpack(new PowerScroll(SkillName.Necromancy, level)); break;
						case 23: m.AddToBackpack(new PowerScroll(SkillName.Stealing, level)); break;
						case 24: m.AddToBackpack(new PowerScroll(SkillName.Stealth, level)); break;
						case 25: m.AddToBackpack(new PowerScroll(SkillName.SpiritSpeak, level)); break;
						case 26: m.AddToBackpack(new PowerScroll(SkillName.Ninjitsu, level)); break;
						case 27: m.AddToBackpack(new PowerScroll(SkillName.Bushido, level)); break;
						case 28: m.AddToBackpack(new PowerScroll(SkillName.Spellweaving, level)); break;
						case 29: m.AddToBackpack(new PowerScroll(SkillName.Throwing, level)); break;
						case 30: m.AddToBackpack(new PowerScroll(SkillName.Mysticism, level)); break;
						case 31: m.AddToBackpack(new PowerScroll(SkillName.Hiding, level)); break;

					}
					m.SendLocalizedMessage(1049524); // You have received a scroll of power!



				 } }
		
	public override bool HasBreath{ get{ return true ; } }
	public override int BreathFireDamage{ get{ return 20; } }
	public override int BreathColdDamage{ get{ return 20; } }
			
//      public override bool IsScaryToPets{ get{ return true; } }
	public override bool AutoDispel{ get{ return true; } }
        public override bool BardImmune{ get{ return true; } }
        public override bool Unprovokable{ get{ return true; } }
        public override Poison HitPoison{ get{ return Poison. Lethal ; } }
        public override bool AlwaysMurderer{ get{ return true; } }
//	public override bool IsScaredOfScaryThings{ get{ return false; } }






		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.BardTarget == this )
					damage = 0; // Immune to pets and provoked creatures
			}
		}
		/*private class TeleportTimer : Timer
		{
			private Mobile m_Owner;

			private static int[] m_Offsets = new int[]
			{
				-1, -1,
				-1,  0,
				-1,  1,
				0, -1,
				0,  1,
				1, -1,
				1,  0,
				1,  1
			};

			public TeleportTimer( Mobile owner ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.1 ) )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ( m_Owner.Deleted )
				{
					Stop();
					return;
				}

				Map map = m_Owner.Map;

				if ( map == null )
					return;

				if ( 0.5 < Utility.RandomDouble() )
					return;

				Mobile toTeleport = null;

				foreach ( Mobile m in m_Owner.GetMobilesInRange( 16 ) )
				{
					if ( m != m_Owner && m.Player && m_Owner.CanBeHarmful( m ) && m_Owner.CanSee( m ) )
					{
						toTeleport = m;
						break;
					}
				}

				if ( toTeleport != null )
				{
					int offset = Utility.Random( 8 ) * 2;

					Point3D to = m_Owner.Location;

					for ( int i = 0; i < m_Offsets.Length; i += 2 )
					{
						int x = m_Owner.X + m_Offsets[(offset + i) % m_Offsets.Length];
						int y = m_Owner.Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];

						if ( map.CanSpawnMobile( x, y, m_Owner.Z ) )
						{
							to = new Point3D( x, y, m_Owner.Z );
							break;
						}
						else
						{
							int z = map.GetAverageZ( x, y );

							if ( map.CanSpawnMobile( x, y, z ) )
							{
								to = new Point3D( x, y, z );
								break;
							}
						}
					}

					Mobile m = toTeleport;

					Point3D from = m.Location;

					m.Location = to;

					Server.Spells.SpellHelper.Turn( m_Owner, toTeleport );
					Server.Spells.SpellHelper.Turn( toTeleport, m_Owner );

					m.ProcessDelta();

					Effects.SendLocationParticles( EffectItem.Create( from, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 2023 );
					Effects.SendLocationParticles( EffectItem.Create(   to, m.Map, EffectItem.DefaultDuration ), 0x3728, 10, 10, 5023 );

					m.PlaySound( 0x1FE );

					m_Owner.Combatant = toTeleport;
				}
			}
		}
		*/

public Malacoda( Serial serial ) : base( serial )
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
