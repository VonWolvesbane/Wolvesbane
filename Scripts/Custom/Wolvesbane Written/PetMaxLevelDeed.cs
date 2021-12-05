using Server.Targeting; 
using System; 
using Server; 
using Server.Gumps; 
using Server.Network; 
using Server.Menus; 
using Server.Menus.Questions; 
using Server.Mobiles; 
using System.Collections; 
using Server.Engines.XmlSpawner2;

namespace Server.Items 
{ 
   	public class PetMaxLevelDeed : Item 
   	{ 
    
      		[Constructable] 
      		public PetMaxLevelDeed() : base( 0x14F0 ) 
      		{ 
         		Weight = 1.0;  
         		Movable = true;
         		Name="Pet Max Level Deed";   
      		} 

      		public PetMaxLevelDeed( Serial serial ) : base( serial ) 
      		{ 
      		} 
      		public override void OnDoubleClick( Mobile from ) 
      		{ 

			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if( from.InRange( this.GetWorldLocation(), 1 ) ) 
			{

        			this.SendLocalizedMessageTo(from, 1010086); 
           			from.Target = new MaxLevelTarget( this );

			} 
			else 
			{ 
				from.SendLocalizedMessage( 500446 ); // That is too far away. 
			}

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


  		private class MaxLevelTarget : Target 
      		{ 
         		private Mobile m_Owner; 
      
         		private PetMaxLevelDeed m_Powder; 

         		public MaxLevelTarget( PetMaxLevelDeed charge ) : base ( 10, false, TargetFlags.None ) 
         		{ 
            			m_Powder=charge; 
         		} 
          
         		protected override void OnTarget( Mobile from, object target ) 
         		{ 

            			if( target == from ) 
				{
               				from.SendMessage( "You cant do that." );
				}
          			else if( target is BaseCreature ) 
          			{ 
            
          				BaseCreature c = (BaseCreature)target;
					if ( c.Controlled == false )
					{
						from.SendMessage( "That Creature is not tamed." );
					}	
					else if ( c.ControlMaster != from )
					{
						from.SendMessage( "This is not your pet." );
					}
					else if ( c.Controlled == true && c.ControlMaster == from)
					{
						
						if((c.MaxLevel ) > 34)
                            {
                                from.SendMessage("The level on this creature is already too high to use this scroll!");
                            }
							else
                            {
							c.MaxLevel += 1;
						from.SendMessage( "Your pet has gained 1 max level." );
						from.PlaySound( 503 );
            					m_Powder.Delete(); 
							}
                           }
            			}
				else
				{
					from.SendMessage( "You cant do that." );
				}
         		} 
      		} 
   	} 
} 
