using System;
using System.Collections;
using System.Collections.Generic;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using Server.ContextMenus;
using Server.Spells;
using Server.Targeting;
using Server.Misc;

namespace Server.Items
{
	public class SpellChannelRune : Item
	{
		[Constructable]
		public SpellChannelRune() : base( 0x1F14 )
		{
			Weight = 0.2;  // ?
			Name = "Spell Channeling Rune";
			Hue = 1266;
		}

		public override void OnDoubleClick( Mobile from ) 
		{
			double minSkill = 70.0;
		 
			PlayerMobile pm = from as PlayerMobile;
		
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}

			else if ( pm == null || from.Skills[SkillName.Inscribe].Base < 70.0 )
			{
				from.SendMessage( "You are not skilled enough to attempt this enhancement." );
			}

		        else if( from.InRange( this.GetWorldLocation(), 5 ) ) 
		        {
				double maxSkill = minSkill + 40.0;

				if ( !from.CheckSkill( SkillName.Inscribe, minSkill, maxSkill ) )
				{
					from.SendMessage( "The rune shatters, releasing the magic energy." );
					from.PlaySound( 65 );
					from.PlaySound( 0x1F8 );
					Delete();
					return;
				}
				else
				{
					from.SendMessage( "Select the item to enhance." );
					from.Target = new InternalTarget( this );
				}
		        } 

		        else 
		        { 
		        	from.SendLocalizedMessage( 500446 ); // That is too far away. 
		        } 
		} 
		
		private class InternalTarget : Target 
		{
			private SpellChannelRune m_SpellChannelRune;

			public InternalTarget( SpellChannelRune runeaug ) : base( 1, false, TargetFlags.None )
			{
				m_SpellChannelRune = runeaug;
			}

		 	protected override void OnTarget( Mobile from, object targeted ) 
		 	{ 
				int DestroyChance = Utility.Random( 3 ); // 3 chance 0 1 2  if rolls 0 then fail
						// most other runes will have a random 0 - x increase + 1 (no 0) here this is a true or false
						// rune so dont needa  random stat bonus just set to 1 

				int NewNameHere = Utility.Random( 4 );	// you could define a new interger here above the tree like this 
									// then call it later, goto line 116 for example
                    	    if ( targeted is Item  )  // protects from crash if targeting a Mobile. 
			    {
				Item item = (Item) targeted;
 
				if ( !from.InRange( ((Item)targeted).GetWorldLocation(), 3 ) ) 
				{ 
			          	from.SendLocalizedMessage( 500446 ); // That is too far away. 
		       		}

				else if (( ((Item)targeted).Parent != null ) && ( ((Item)targeted).Parent is Mobile ) ) 
			       	{ 
			          	from.SendMessage( "You cannot enhance that in it's current location." ); 
		       		}

			    	else if ( targeted is BaseWeapon ) 
				{ 
			       		BaseWeapon Weapon = targeted as BaseWeapon; 
		       			{

						if ( DestroyChance > 0 ) // Success
						{
							Weapon.Attributes.SpellChanneling = 1; 
							from.SendMessage( "The Rune enhances your weapon." );
				                  	from.PlaySound( 0x1F5 );
			        	          	m_SpellChannelRune.Delete();
			          		}

						else // Fail
						{
					  		from.SendMessage( "You have failed to enhance the weapon!" );
							m_SpellChannelRune.Delete();
							from.PlaySound( 42 );
							if ( NewNameHere < 1 ) // this is the random interger we made, for a seperate success chance to determine
							{			 // if the item to break with the rune or not. see line 139 for example without.
							 from.SendMessage( "The weapon is damaged beyond repair!" ); 
						  	 Weapon.Delete();
							}
							else
							{
 							 from.SendMessage( "The rune crumbles in the attempt, but the weapon survives." );
							}
							
				  		}
					}
				}

			    	else if ( targeted is BaseShield ) 
				{ 
			       		BaseShield Shield = targeted as BaseShield; 
		       			{
						if ( DestroyChance > 0 ) // Success
						{
							Shield.Attributes.SpellChanneling = 1; 
							from.SendMessage( "The Rune enhances your shield." );
				                  	from.PlaySound( 0x1F5 );
			        	          	m_SpellChannelRune.Delete();
			          		}

						else // Fail
						{
					  		from.SendMessage( "You have failed to enhance the shield!" );
							from.SendMessage( "The shield is damaged beyond repair!" );  // see how this "fail" is structered it will always 
							from.PlaySound( 42 );					     // break the item if you fail the rune to change
						  	Shield.Delete();					     // do what i did above. for Each Layer Defined.
							m_SpellChannelRune.Delete();
				  		}
					}
				}
			    }
		    	 else // if not a item or slot we approved (mobile crash) this says nope nope nope
		    	 { 
		          from.SendMessage( "You cannot enhance that." );
		    	 } 
		  	}
		
		}

		public override bool DisplayLootType{ get{ return false; } }  // ha ha!

		public SpellChannelRune( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}