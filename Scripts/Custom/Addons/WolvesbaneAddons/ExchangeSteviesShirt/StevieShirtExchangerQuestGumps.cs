using System;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
namespace Server.Gumps
{

	public class StevieShirtExchangerQuestGump1 : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register("StevieShirtExchangerQuestGump", AccessLevel.GameMaster, new CommandEventHandler(StevieShirtExchangerQuestGump_OnCommand1));
		}
		private static void StevieShirtExchangerQuestGump_OnCommand1(CommandEventArgs e)
		{
			e.Mobile.SendGump(new StevieShirtExchangerQuestGump1(e.Mobile));
		}
		public StevieShirtExchangerQuestGump1(Mobile owner) : base(50, 50)
		{
			//----------------------------------------------------------------------------------------------------
			AddPage(0); AddImageTiled(54, 33, 369, 400, 2624); AddAlphaRegion(54, 33, 369, 400); AddImageTiled(416, 39, 44, 389, 203);
			//--------------------------------------Window size bar--------------------------------------------
			AddImage(97, 49, 9005);
			AddImageTiled(58, 39, 29, 390, 10460);
			AddImageTiled(412, 37, 31, 389, 10460);
			AddLabel(140, 60, 0x34, "Shirt Exchange");
			//----------------------/----------------------------------------------/
			AddHtml(107, 140, 300, 230, " < BODY > " +
			"<BASEFONT COLOR=YELLOW>Greetings!<BR>" +
			"<BASEFONT COLOR=YELLOW><BR>" +
			"<BASEFONT COLOR=YELLOW>Your loyalty to the Queen is sufficient<BR>" +
			"<BASEFONT COLOR=YELLOW>You can give me a Stevie shirt <BR>" +
			"<BASEFONT COLOR=YELLOW>and I will exchange it for a Royal Gargish Tamers Sash!!!<BR>" +
			"</BODY>", false, true);
			//----------------------/----------------------------------------------/
			AddImage(430, 9, 10441);
			AddImageTiled(40, 38, 17, 391, 9263);
			AddImage(6, 25, 10421);
			AddImage(34, 12, 10420);
			AddImageTiled(94, 25, 342, 15, 10304);
			AddImageTiled(40, 427, 415, 16, 10304);
			AddImage(-10, 314, 10402);
			AddImage(56, 150, 10411);
			AddImage(155, 120, 2103);
			AddImage(136, 84, 96);
			AddButton(225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0);
		}
		//----------------------/----------------------------------------------/
		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile; switch (info.ButtonID)
			{
				case 0: { break; }
			}
		}
	}

	public class StevieShirtExchangerQuestGump2 : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register("StevieShirtExchangerQuestGump", AccessLevel.GameMaster, new CommandEventHandler(StevieShirtExchangerQuestGump_OnCommand2));
		}
		private static void StevieShirtExchangerQuestGump_OnCommand2(CommandEventArgs e)
		{
			e.Mobile.SendGump(new StevieShirtExchangerQuestGump2(e.Mobile));
		}
		public StevieShirtExchangerQuestGump2(Mobile owner) : base(50, 50)
		{
			//----------------------------------------------------------------------------------------------------
			AddPage(0); AddImageTiled(54, 33, 369, 400, 2624); AddAlphaRegion(54, 33, 369, 400); AddImageTiled(416, 39, 44, 389, 203);
			//--------------------------------------Window size bar--------------------------------------------
			AddImage(97, 49, 9005);
			AddImageTiled(58, 39, 29, 390, 10460);
			AddImageTiled(412, 37, 31, 389, 10460);
			AddLabel(140, 60, 0x34, "Shirt Exchange");
			//----------------------/----------------------------------------------/
			AddHtml(107, 140, 300, 230, " < BODY > " +
			"<BASEFONT COLOR=YELLOW>Greetings!<BR>" +
			"<BASEFONT COLOR=YELLOW><BR>" +
			"<BASEFONT COLOR=YELLOW>Your loyalty to the Queen is not sufficient<BR>" +
			"<BASEFONT COLOR=YELLOW>Return to me after you have served the Queen.<BR>" +
			"</BODY>", false, true);
			//----------------------/----------------------------------------------/
			AddImage(430, 9, 10441);
			AddImageTiled(40, 38, 17, 391, 9263);
			AddImage(6, 25, 10421);
			AddImage(34, 12, 10420);
			AddImageTiled(94, 25, 342, 15, 10304);
			AddImageTiled(40, 427, 415, 16, 10304);
			AddImage(-10, 314, 10402);
			AddImage(56, 150, 10411);
			AddImage(155, 120, 2103);
			AddImage(136, 84, 96);
			AddButton(225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0);
		}
		//----------------------/----------------------------------------------/
		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile; switch (info.ButtonID)
			{
				case 0: { break; }
			}
		}
	}

	public class StevieShirtExchangerQuestGump3 : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register("StevieShirtExchangerQuestGump", AccessLevel.GameMaster, new CommandEventHandler(StevieShirtExchangerQuestGump_OnCommand3));
		}
		private static void StevieShirtExchangerQuestGump_OnCommand3(CommandEventArgs e)
		{
			e.Mobile.SendGump(new StevieShirtExchangerQuestGump3(e.Mobile));
		}
		public StevieShirtExchangerQuestGump3(Mobile owner) : base(50, 50)
		{
			//----------------------------------------------------------------------------------------------------
			AddPage(0); AddImageTiled(54, 33, 369, 400, 2624); AddAlphaRegion(54, 33, 369, 400); AddImageTiled(416, 39, 44, 389, 203);
			//--------------------------------------Window size bar--------------------------------------------
			AddImage(97, 49, 9005);
			AddImageTiled(58, 39, 29, 390, 10460);
			AddImageTiled(412, 37, 31, 389, 10460);
			AddLabel(140, 60, 0x34, "Shirt Exchange");
			//----------------------/----------------------------------------------/
			AddHtml(107, 140, 300, 230, " < BODY > " +
			"<BASEFONT COLOR=YELLOW>Foreign scum!<BR>" +
			"<BASEFONT COLOR=YELLOW><BR>" +
			"<BASEFONT COLOR=YELLOW>Be gone before I kill you<BR>" +
			"</BODY>", false, true);
			//----------------------/----------------------------------------------/
			AddImage(430, 9, 10441);
			AddImageTiled(40, 38, 17, 391, 9263);
			AddImage(6, 25, 10421);
			AddImage(34, 12, 10420);
			AddImageTiled(94, 25, 342, 15, 10304);
			AddImageTiled(40, 427, 415, 16, 10304);
			AddImage(-10, 314, 10402);
			AddImage(56, 150, 10411);
			AddImage(155, 120, 2103);
			AddImage(136, 84, 96);
			AddButton(225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0);
		}
		//----------------------/----------------------------------------------/
		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile; switch (info.ButtonID)
			{
				case 0: { break; }
			}
		}
	}

}
