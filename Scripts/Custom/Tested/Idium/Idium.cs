// Created by Neptune

using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Mobiles

              {
              [CorpseName( " corpse of Idium" )]
              public class Idium : BaseCreature
              {

                                 [Constructable]
                                    public Idium() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2 )
                            {
                                               Name = "Idium";
                                               Hue = 1911;
								Paralyzed = false;
                                               Body = 46; // Uncomment these lines and input values
                                               BaseSoundID = 357; // To use your own custom body and sound.
                                               SetStr( 2000, 2500 );
                                               SetDex( 1500, 2000 );
                                               SetInt( 1500, 2000 );
                                               SetHits( 200000, 250000 );
                                               SetDamage( 45, 55 );
                                               SetDamageType( ResistanceType.Cold, 120 );
                                               SetDamageType( ResistanceType.Fire, 120 );
                                               SetDamageType( ResistanceType.Energy, 120 );
                                               SetDamageType( ResistanceType.Poison, 120 );

                                               SetResistance( ResistanceType.Physical, 70 );
                                               SetResistance( ResistanceType.Cold, 80 );
                                               SetResistance( ResistanceType.Fire, 80 );
                                               SetResistance( ResistanceType.Energy, 80 );
                                               SetResistance( ResistanceType.Poison, 80 );

			SetSkill( SkillName.EvalInt, 320.1, 330.0 );
			SetSkill( SkillName.Magery, 290.1, 300.0 );
			SetSkill( SkillName.Meditation, 200.1, 301.0 );
			SetSkill( SkillName.Poisoning, 200.1, 301.0 );
			SetSkill( SkillName.MagicResist, 575.2, 600.0 );
			SetSkill( SkillName.Tactics, 390.1, 400.0 );
			SetSkill( SkillName.Wrestling, 375.1, 400.0 );
			SetSkill( SkillName.Swords, 375.1, 400.0 );
			SetSkill( SkillName.Anatomy, 375.1, 400.0 );
			SetSkill( SkillName.Parry, 250.1, 300.0 );


                                               Fame = 40000;
                                               Karma = -45000;
                                               VirtualArmor = 70;
		PackGold( 11120, 11130 );

}
public override void GenerateLoot()
		{		
			/*switch ( Utility.Random( 75 ))
			{
				
				case 0: PackItem( new DragonArmsOfEvolution() ); break;
				case 1: PackItem( new DragonChestOfEvolution() ); break;
				case 2: PackItem( new DragonGlovesOfEvolution() ); break;
				case 3: PackItem( new DragonLegsOfEvolution() ); break;
				case 4: PackItem( new OrderShieldOfEvolution() ); break;
				case 5: PackItem( new DragonHelmOfEvolution() ); break;
                case 6: PackItem(new DragonGorgetOfEvolution()); break;


            }*/
					if ( Utility.RandomDouble() < 0.10 )
					{
					switch ( Utility.Random( 7 ) )
					{
				case 0: PackItem( new DragonArmsOfEvolution() ); break;
				case 1: PackItem( new DragonChestOfEvolution() ); break;
				case 2: PackItem( new DragonGlovesOfEvolution() ); break;
				case 3: PackItem( new DragonLegsOfEvolution() ); break;
				case 4: PackItem( new OrderShieldOfEvolution() ); break;
				case 5: PackItem( new DragonHelmOfEvolution() ); break;
                case 6: PackItem(new DragonGorgetOfEvolution()); break;
					}
				}
				else
				{
					Item item = new DragonTorphySouthAddon();
						 item.Hue = 1911;
						 item.Movable = true;
						 item.Name = "<Body bgcolor=Black; text=Red><Big><center>I killed Idium and all I got was this!</Body>";
					   //this.AddItem(item);
					   
					switch ( Utility.Random( 8 ) )
					{
						
				case 0: item = new GildedDress(); break;
				case 1: item = new LongPants(); break;
				case 2: item = new BodySash(); break;
				case 3: item = new HalfApron(); break;
				case 4: item = new Cloak(); break;
				case 5: item = new Robe(); break;
				case 6: item = new DragonTorphyEastAddon(); break;
					}
				}							
             }

                                 public override bool HasBreath{ get{ return true ; } }
				 public override int BreathFireDamage{ get{ return 9; } }
				 public override int BreathColdDamage{ get{ return 9; } }
                                 public override bool IsScaryToPets{ get{ return true; } }
				 public override bool AutoDispel{ get{ return true; } }
                                 public override bool BardImmune{ get{ return true; } }
                                 public override bool Unprovokable{ get{ return true; } }
                                 public override Poison HitPoison{ get{ return Poison. Lethal ; } }
                                 public override bool AlwaysMurderer{ get{ return true; } }
				



		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
		{
			if ( from is BaseCreature )
			{
				BaseCreature bc = (BaseCreature)from;

				if ( bc.Controlled || bc.BardTarget == this )
					damage = 0; // Immune to pets and provoked creatures
			}
		}



public Idium( Serial serial ) : base( serial )
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

