using System; 
using Server;
using Server.Commands; 
using Server.Gumps; 
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{ 
   public class SteveIrwinGump : Gump 
   { 
      public static void Initialize() 
      { 
         CommandSystem.Register( "SteveIrwinGump", AccessLevel.GameMaster, new CommandEventHandler( SteveIrwinGump_OnCommand ) ); 
      } 

      private static void SteveIrwinGump_OnCommand( CommandEventArgs e ) 
      { 
         e.Mobile.SendGump( new SteveIrwinGump( e.Mobile ) ); 
      } 

      public SteveIrwinGump( Mobile owner ) : base( 50,50 ) 
      { 
//----------------------------------------------------------------------------------------------------

				AddPage( 0 );
			AddImageTiled(  54, 33, 369, 400, 2624 );
			AddAlphaRegion( 54, 33, 369, 400 );

			AddImageTiled( 416, 39, 44, 389, 203 );
//--------------------------------------Window size bar--------------------------------------------
			
			AddImage( 97, 49, 9005 );
			AddImageTiled( 58, 39, 29, 390, 10460 );
			AddImageTiled( 412, 37, 31, 389, 10460 );
			AddLabel( 140, 60, 0x34, "The Lost Egg" );
			

			AddHtml( 107, 140, 300, 230, "<BODY>" +
//----------------------/----------------------------------------------/
"<BASEFONT COLOR=YELLOW>*Steve Glances Up With A Worried Look In His Eye.*<BR><BR>G'day Mate, Feel Like Lending Me A Hand??<BR>" +
"<BASEFONT COLOR=YELLOW>I Was Just Feeding The Crocs, Nice Little Beauts, When A Pesky Drongo Lizard Man Had It Away With One Of My Precious Croc Eggs..<BR>" +
"<BASEFONT COLOR=YELLOW>So Mate, If You Wanna Go Find The Blighter And Rip His Bloody Head Off, And Bring Me The Egg Back<BR><BR>There Will Be A Little Something In It For You.<BR>" +
"<BASEFONT COLOR=YELLOW>He Was Last Seen Heading To The Bog Of Desolation Near Compassion Desert ..." +						     
                                              "</BODY>", false, true);
			
//			<BASEFONT COLOR=#7B6D20>			

//			AddLabel( 113, 135, 0x34, "Steve looks at you in disbelief..." );
//			AddLabel( 113, 150, 0x34, "Steve looks at you in disbelief..." );
//			AddLabel( 113, 165, 0x34, "Eh?" );
//			AddLabel( 113, 180, 0x34, "Bah!" );
//			AddLabel( 113, 195, 0x34, "Aha?" );
//			AddLabel( 113, 210, 0x34, "Bring me the required item?" );
//			AddLabel( 113, 235, 0x34, "Bleh!" );
//			AddLabel( 113, 250, 0x34, "The item I require is the egg," );
//			AddLabel( 113, 265, 0x34, "The monster was last seen at swamp." );
//			AddLabel( 113, 280, 0x34, "Not a easy kill--" );
//			AddLabel( 113, 295, 0x34, "felled. Should you retreive the egg I shall" );
//			AddLabel( 113, 310, 0x34, "give you a nice reward" );
			AddImage( 430, 9, 10441);
			AddImageTiled( 40, 38, 17, 391, 9263 );
			AddImage( 6, 25, 10421 );
			AddImage( 34, 12, 10420 );
			AddImageTiled( 94, 25, 342, 15, 10304 );
			AddImageTiled( 40, 427, 415, 16, 10304 );
			AddImage( -10, 314, 10402 );
			AddImage( 56, 150, 10411 );
			AddImage( 155, 120, 2103 );
			AddImage( 136, 84, 96 );

			AddButton( 225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0 ); 

//--------------------------------------------------------------------------------------------------------------
      } 

      public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons 
      { 
         Mobile from = state.Mobile; 

         switch ( info.ButtonID ) 
         { 
            case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0 
            { 
               //Cancel 
               from.SendMessage( "Ripper Mate, Go Get Him !" );
               break; 
            } 

         }
      }
   }
}