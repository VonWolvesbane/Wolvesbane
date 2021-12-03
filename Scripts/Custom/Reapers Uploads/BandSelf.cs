using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Scripts.Commands
{
	public class BandSelf
	{
		public static void Initialize()
		{
			CommandSystem.Register( "bandself", AccessLevel.Player, new CommandEventHandler( BandSelf_OnCommand ) );
			CommandSystem.Register( "bs", AccessLevel.Player, new CommandEventHandler( BandSelf_OnCommand ) );
		}
		//Command will search the userers backpack for TypeOf(Bandage), and if true, applies to self.
		//If false, will return a msg announcing to the player they are out of bandage.
		//Should also give a warning if bandage count is under ??
		public static void BandSelf_OnCommand(CommandEventArgs e )
		{
			Mobile pm = e.Mobile;
			Item band = pm.Backpack.FindItemByType(typeof( EverlastingBandage ));				

			if ( band != null )
			{
				EverlastingBandage.BandSelfCommandCall( pm, band );
			}
			else
			{
				pm.SendMessage( "This command requires the Everlasting bandage." );
			}
		}
	}
}