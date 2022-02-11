using System;
using Server;

namespace Server.Items

{
              
              public class CupidsBow : CompositeBow
              { 
              public override int AosMinDamage{ get{ return 35; } }
              public override int AosMaxDamage{ get{ return 45; } }
              
                      [Constructable]
                      public CupidsBow() 
                      {
                                      
                                        Name = "Cupid's Bow of Love";
                                       
										ItemID = 0xC516;
										WeaponAttributes.HitFireArea = 95;
                                        WeaponAttributes.HitLeechHits = 35;
                                        WeaponAttributes.HitLeechMana = 35;
                                        WeaponAttributes.HitLeechStam = 35;
                                        WeaponAttributes.HitFireball = 90;
                                        WeaponAttributes.HitHarm = 90;
                                        WeaponAttributes.SelfRepair = 5;
              
                                        Attributes.AttackChance = 25;
                                        Attributes.BonusStr = 30;
                                        Attributes.DefendChance = 20;
                                        Attributes.Luck = 1000;
                                        Attributes.RegenHits = 5;
                                        Attributes.SpellChanneling = 1;
                                        Attributes.SpellDamage = 55;
                                        Attributes.WeaponSpeed = 50;
			
              
                                    }
              
                      public CupidsBow( Serial serial ) : base( serial )  
                                    {
                                    }
              
                      public override void Serialize( GenericWriter writer )
                                    {
                                                      base.Serialize( writer );
              
                                                      writer.Write( (int) 0 );
                                    }
              
                      public override void Deserialize(GenericReader reader)
                                    {
                                                      base.Deserialize( reader );
                            
                                                      int version = reader.ReadInt();
                                    }
                  }
}
