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
   	public class PetDamageDeed : Item 
   	{ 
    
      		[Constructable] 
      		public PetDamageDeed() : base( 0x14F0 ) 
      		{ 
         		Weight = 1.0;  
         		Movable = true;
         		Name="Pet Damage Increase Deed";   
      		} 
			
			public override void GetProperties(ObjectPropertyList list)
			{
			base.GetProperties(list);
	
			list.Add("<Body bgcolor=; text=#ff0000><Big><center>Max Damage can not exceed 200</Body>");
			}
      		public PetDamageDeed( Serial serial ) : base( serial ) 
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
           			from.Target = new statTarget( this );

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


  		private class statTarget : Target 
      		{ 
         		private Mobile m_Owner; 
      
         		private PetDamageDeed m_Powder; 

         		public statTarget( PetDamageDeed charge ) : base ( 10, false, TargetFlags.None ) 
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
						if (c.DamageMax <= 195 )
						{
						c.DamageMin += 5;
						c.DamageMax += 5;
						from.SendMessage( "Your pet absorbs the power" );
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
