//Crafter by ReApEr
using Server.Targeting; 
using System; 
using Server; 
using Server.Gumps; 
using Server.Network; 
using Server.Menus; 
using Server.Menus.Questions; 
using Server.Mobiles; 
using Server.Items;
using Server.Network;
using System.Collections; 

namespace Server.Items 
{ 
   	public class PetSexChangeDeed : Item 
   	{ 
    
      		[Constructable] 
      		public PetSexChangeDeed() : base( 0x14F0 ) 
      		{ 
         		Weight = 1.0;  
         		Movable = true;
         		Name="Pet Sex Change Deed";   
      		} 

      		public PetSexChangeDeed( Serial serial ) : base( serial ) 
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
           			from.Target = new SexTarget( this );

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


  		private class SexTarget : Target 
      		{ 
         		private Mobile m_Owner; 
      
         		private PetSexChangeDeed m_Powder; 

         		public SexTarget( PetSexChangeDeed charge ) : base ( 10, false, TargetFlags.None ) 
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
					else if ( c.Controlled == true && c.ControlMaster == from && c.BodyValue != 400 && c.BodyValue != 401)
					{						
					if  (c.Female == true )
					{
						c.Female = false;
						from.SendMessage( "Your pet has magically grown Male genitals" );
						from.PlaySound( 503 );
            			m_Powder.Delete(); 
					}
					else
					{
						c.Female = true;
						from.SendMessage( "Your pets genitalia has fallen off. Your pet is now a female!" );
						from.PlaySound( 503 );
            			m_Powder.Delete(); 
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
}