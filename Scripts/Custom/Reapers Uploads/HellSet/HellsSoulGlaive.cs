using System;
using Server;

namespace Server.Items

{
              
              public class HellsSoulGlaive : SoulGlaive
              { 
              public override int MinDamage{ get{ return 35; } }
              public override int MaxDamage{ get{ return 45; } }
              
                      [Constructable]
                      public HellsSoulGlaive() 
                      {
                                      
                                        Name = "Lucifers SoulGlaive";
                                       
										Hue = 0x27;
										WeaponAttributes.HitFireArea = 100;
                                        WeaponAttributes.HitLeechHits = 35;
                                        WeaponAttributes.HitLeechMana = 35;
                                        WeaponAttributes.HitLeechStam = 35;
                                        WeaponAttributes.HitFireball = 100;
                                        WeaponAttributes.HitHarm = 100;
                                        WeaponAttributes.SelfRepair = 10;
              
                                        Attributes.AttackChance = 25;
                                        Attributes.BonusStr = 30;
                                        Attributes.DefendChance = 20;
                                        Attributes.Luck = 666;
                                        Attributes.RegenHits = 5;
                                        Attributes.SpellChanneling = 1;
                                        Attributes.SpellDamage = 55;
                                        Attributes.WeaponSpeed = 50;
              
                                    }
              
                      public HellsSoulGlaive( Serial serial ) : base( serial )  
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
