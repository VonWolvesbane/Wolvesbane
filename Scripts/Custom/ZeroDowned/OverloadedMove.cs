/*
	COMMAND: [OverloadedMove
	Can only be used by someone with Owner access level. 

	- Best use it to create several woodenchest in the player's home and secure them. 
	- Use command [viewequip and target player, double click the bankbox and select Move > Continue
	- Target a place in their house near the woodenchests that were created
	- Now use the command [OverloadedMove and target each of the wooden chests to move 1,000 items from the bank box to the chests until the bank box chest can be opened & viewed correctly
	
	!!! Player will need to use a banker and either just say "bank" or context menu and open bankbox to create a new bankbox for themselves
	- This is an important step in order to avoid issues with looting, etc as most other scripts/systems will not create a new one and could cause a crash

	~ created by zerodowned
*/


using System;
using Server;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Accounting; 
using System.Collections;
using System.Collections.Generic;
using Server.Network;

namespace Server.Commands 
{ 
  public class OverloadedMove
  { 
    public static void Initialize() 
    { 
      CommandSystem.Register( "OverloadedMove", AccessLevel.Owner, new CommandEventHandler( OverloadedMove_OnCommand ) ); 
    } 

    public static void OverloadedMove_OnCommand( CommandEventArgs e ) 
    { 
		Mobile from = e.Mobile; 
		from.LocalOverheadMessage( MessageType.Regular, 0x1150, true, "Select the container you want to OverloadedMove items FROM."); 
		from.Target = new PackFromTarget( from);
	}

	private class PackFromTarget : Target
	{
		public PackFromTarget( Mobile from ) : base( -1, true, TargetFlags.None )
		{
		}
			
		protected override void OnTarget( Mobile from, object o )
		{
			if(o is Container)
			{
				Container xx = o as Container;
			
				from.LocalOverheadMessage( MessageType.Regular, 0x33, true, "Select the container you want to OverloadedMove items INTO."); 
				from.Target = new PackToTarget( from, xx );

			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x22, true, "That is not a container!"); 
			}
		}
	}
	private class PackToTarget : Target
	{
		private Container FromCont;

			public PackToTarget( Mobile from, Container cont ) : base( -1, true, TargetFlags.None )
		{
			FromCont = cont;
		}
		
		protected override void OnTarget( Mobile from, object o )
		{
			if( o is Container)
			{
				Container xx = o as Container;

				if (xx == FromCont)
				{
					from.LocalOverheadMessage( MessageType.Regular, 0x22, true, "The container to OverloadedMove INTO must be different from the one you are OverloadedMoveing FROM."); 
					return;
				}
				
                if (xx == FromCont || xx.IsChildOf(FromCont))
                {
					from.LocalOverheadMessage( MessageType.Regular, 0x22, true, "You cannot sort INTO a subcontainer of the same container you are sorting FROM."); 
					return;
                }

				try
				{
					for(int i = 0; i < 1000; i++)
					{
						if(!(xx.TryDropItem( from, FromCont.Items[i], true )))
							from.SendMessage("That container is too full!");
					}

				}
				catch
				{}
				
			}
			else
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x22, true, "That is not a container!"); 
			}
		}
	}	
  } 
} 