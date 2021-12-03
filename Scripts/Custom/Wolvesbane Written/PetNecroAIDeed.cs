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
   	public class PetNecroAIDeed : Item 
   	{ 
    
      		[Constructable] 
      		public PetNecroAIDeed() : base( 0x14F0 ) 
      		{ 
         		Weight = 1.0;  
         		Movable = true;
         		Name="Pet Necromancy AI Deed";   
      		} 

      		public PetNecroAIDeed( Serial serial ) : base( serial ) 
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
           			from.Target = new AITarget( this );

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


  		private class AITarget : Target 
      		{ 
         		private Mobile m_Owner; 
      
         		private PetNecroAIDeed m_Powder; 

         		public AITarget( PetNecroAIDeed charge ) : base ( 10, false, TargetFlags.None ) 
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
						c.AI = AIType.AI_Necro;
						c.SetSkill(SkillName.Necromancy, 50);
						c.SetSkill(SkillName.SpiritSpeak, 50);
						c.Skills[SkillName.Necromancy].Cap = 200;
						c.Skills[SkillName.SpiritSpeak].Cap = 200;
						c.Karma = -35000;
						c.RawInt += 100;
						c.Mana += 200;
						from.SendMessage( "You have chosen the path of darkness for this creature!" );
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
