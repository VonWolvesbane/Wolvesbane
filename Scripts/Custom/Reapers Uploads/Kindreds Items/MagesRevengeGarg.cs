using System;
using Server;
using Server.Mobiles;

namespace Server.Items

{
              
              public class MagesRevengeGarg : GlassStaff
              { 
              public override int AosMinDamage{ get{ return 25; } }
              public override int AosMaxDamage{ get{ return 37; } }
              
                      [Constructable]
                      public MagesRevengeGarg() 
                      {
                                      
									  	this.SkillBonuses.SetValues(0, SkillName.Magery, 10);
                                        Name = "Mages Revenge";
                                       
										Hue = 2342;
										//WeaponAttributes.HitEnergyArea = 100;
                                        //WeaponAttributes.HitLeechHits = 35;
                                        WeaponAttributes.HitLeechMana = 100;
                                        //WeaponAttributes.HitLeechStam = 35;
                                        WeaponAttributes.HitFireball = 100;
										WeaponAttributes.HitLightning = 100;
										WeaponAttributes.MageWeapon = 30;
										ExtendedWeaponAttributes.HitSparks = 100;
                                        WeaponAttributes.HitHarm = 100;
                                        WeaponAttributes.SelfRepair = 10;
              
                                        Attributes.AttackChance = 25;
                                        Attributes.BonusInt = 50;
                                        Attributes.DefendChance = 20;
                                        //Attributes.Luck = 666;
                                        Attributes.RegenMana = 5;
										Attributes.BonusMana = 50;
                                        Attributes.SpellChanneling = 1;
                                        Attributes.SpellDamage = 75;
                                        //Attributes.WeaponSpeed = 50;
              
                                    }
              
                      public MagesRevengeGarg( Serial serial ) : base( serial )  
                                    {
                                    }
									public override int DefMaxRange{ get{ return 5; } }
                      public override void GetDamageTypes(Mobile wielder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct)
        {
            phys = fire = cold = pois = chaos = direct = 0;
            nrgy = 100;
        }
				public override void OnHit(Mobile attacker, IDamageable defender, double Damagebonus) //On hit trigger
	{
	if (defender is BaseCreature)
            {
				if (0.4 > Utility.RandomDouble())//40% of the time this weapon will set a defenders hit points to 0 almost killing it
            {
                  defender.Hits -=120;
				  defender.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.Waist ); //FlameStrike Effect
				  defender.PlaySound( 0x208 );
                        }
			}
         base.OnHit( attacker, defender, Damagebonus );
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
