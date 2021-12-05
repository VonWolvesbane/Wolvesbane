// Script: [getlore
// Version: 1.0
// Author: ReApEr
// 
// Date: 04/05/2020
// Purpose: 
// Player Command. New animal lore gump
using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Gumps;

namespace Server.Commands
{
  public class GetLore
	{
		public static void Initialize()
			{
	       CommandSystem.Register( "GetLore", AccessLevel.Player, new CommandEventHandler( GetLore_OnCommand ) );
			}
			
		[Usage( "GetLore" )]
		[Description( "New animal lore gump" )]
		public static void GetLore_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( GetLore_OnTarget ) );
			e.Mobile.SendMessage("Target the pet you wish to know more about");
		}	
		
		
		public static void GetLore_OnTarget( Mobile from, object targeted )	
		{	
			if ( targeted is BaseCreature )
			 {
				 BaseCreature targ = (BaseCreature)targeted;		
						if ( targ.ControlMaster == from ) 
							{	
										from.SendGump( new MercLoreGump( ((BaseCreature)targ), from, MercLorePage.Stats ) );
										from.SendMessage( "You take a closer look at " + ((BaseCreature)targ).Name + "'s stats." );
								}
						else	
						{
							from.SendMessage("You must target a creature you Control" );
						}									
		}			
 		else
			{
				from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( GetLore_OnTarget ) );
			 	from.SendMessage("That is not a pet!" ); 
		 }
	}
 }
}