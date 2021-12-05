using System; 
using Server; 
using Server.Gumps; 
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Commands;

namespace Server.Gumps
{ 
   public class DannyquestGump : Gump 
   { 
      public static void Initialize() 
      { 
         CommandSystem.Register( "DannyquestGump", AccessLevel.GameMaster, new CommandEventHandler( DannyquestGump_OnCommand ) ); 
      } 

      private static void DannyquestGump_OnCommand( CommandEventArgs e ) 
      { 
         e.Mobile.SendGump( new DannyquestGump( e.Mobile ) ); 
      } 

      public DannyquestGump( Mobile owner ) : base( 50,50 ) 
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
//----------------------------------------------------------------------------------------------------

				AddPage( 0 );
			AddImageTiled(13, 5, 382, 433, 2524);
			AddImageTiled(9, 6, 388, 7, 40);
			AddImageTiled(11, 433, 382, 9, 40);
			AddImage(13, 18, 3005, 1152);
			AddImage(389, 188, 3003, 1152);
			AddImage(13, 187, 3005, 1152);
			AddImage(389, 17, 3003, 1152);
			AddImageTiled(15, 421, 376, 12, 50);
			AddImage(46, 12, 2080);
                                                AddTextEntry(82, 25, 170, 20, 33, 0,    @"Easter Egg Hunt Quest!");
                                               // AddTextEntry(69, 52, 200, 20, 58, 0, @"Bring Me Back An Easter Eggs!");
			

			

			
			

			AddHtml(  31, 93, 346, 281, "<BODY>" +
//----------------------/----------------------------------------------/
"<BASEFONT COLOR=GREEN>Well hello there, beautiful day!!<BR><BR>" + 
"<BASEFONT COLOR=GREEN>Why would I be walking around Brit you ask?!<BR><BR>" +
"<BASEFONT COLOR=GREEN>Easter Day is not far away and I have been told<BR><BR>" +
"<BASEFONT COLOR=GREEN>There are some special easter eggs around Brit!<BR><BR>" +
"<BASEFONT COLOR=GREEN>So I have been trying to find some of these for my girl!<BR><BR>" +
"<BASEFONT COLOR=GREEN>They must be pretty well hidden because so far I have only found 2!<BR><BR>" +
"<BASEFONT COLOR=GREEN>Make you a deal! If you could bring me back lets say!<BR><BR>" +
"<BASEFONT COLOR=GREEN>10 of those special easter eggs, I could give you a very<BR><BR>" +
"<BASEFONT COLOR=GREEN>Nice easter box with an item inside, in exchange for those eggs!<BR><BR>" +
"<BASEFONT COLOR=GREEN>Before I forget to tell you, I will be here waiting for you!<BR><BR>" +
"<BASEFONT COLOR=GREEN>Here I will even give you this Easter Basket to collect the eggs!<BR><BR>" +
"<BASEFONT COLOR=GREEN>Oh,  you will need to stack 10 of one color special easter egg<BR><BR>" +
						     "</BODY>", false, true);
			

                                               AddButton(163, 385, 247, 248, 0, GumpButtonType.Reply, 0);
			AddItem(23, 66, 10248, 24);
			AddItem(327, 66, 10248, 33);
			
			

			

//--------------------------------------------------------------------------------------------------------------
      } 

      public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons 
      { 
         Mobile from = state.Mobile; 

         switch ( info.ButtonID ) 
         { 
            case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0 
            { 
                
               
               break; 
            } 

         }
      }
   }
}