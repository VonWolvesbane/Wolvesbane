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

using System.Collections; 

namespace Server.Items 
{ 
   	public class PetMageAIDeed : Item 
   	{ 
    
      		[Constructable] 
      		public PetMageAIDeed() : base( 0x14F0 ) 
      		{ 
         		Weight = 1.0;  
         		Movable = true;
         		Name="Pet Mage AI Deed";   
      		} 

      		public PetMageAIDeed( Serial serial ) : base( serial ) 
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
      
         		private PetMageAIDeed m_Powder; 

         		public AITarget( PetMageAIDeed charge ) : base ( 10, false, TargetFlags.None ) 
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
						c.AI = AIType.AI_Mage;
						
						//if c.Skills[SkillName.Magery] <= 50;
						//{
							c.SetSkill(SkillName.Magery, 50);
						//}	
						//if c.Skills[SkillName.EvalInt] <= 50;
						//{
							c.SetSkill(SkillName.EvalInt, 50);
						//}
						if  (c.RawInt <= 500 )
						{
							c.RawInt = 500;
						}
						if ( c.Mana <= 1500 )
						{
							c.Mana = 1500;
						}	
							c.Skills[SkillName.Magery].Cap = 200;
							c.Skills[SkillName.EvalInt].Cap = 200;
							from.SendMessage( "Your pet Has now Started to learn the ways of the Magi!" );
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
