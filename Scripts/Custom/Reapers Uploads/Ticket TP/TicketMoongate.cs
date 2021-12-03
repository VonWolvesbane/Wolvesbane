	#region References
using System;

using System.Collections;
using Server.Factions;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Regions;
using Server.Spells;
#endregion

namespace Server.Items
{
	public class TicketMoongate : Moongate
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public int GumpWidth { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int GumpHeight { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int TitleColor { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int MessageColor { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int TitleNumber { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public string TitleString { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int MessageNumber { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public string MessageString { get; set; }

		[Constructable]
		public TicketMoongate()
			: this(Point3D.Zero, null)
		{ }

		[Constructable]
		public TicketMoongate(Point3D target, Map targetMap)
			: base(target, targetMap)
		{ }

		public TicketMoongate(Serial serial)
			: base(serial)
		{ }
		 public static TeleporterTicket GetTeleporterTicket(Mobile m)
        {
            for (int i = 0; i < m.Items.Count; i ++)
            {
                if (m.Items[i] is TeleporterTicket)
                    return (TeleporterTicket)m.Items[i];
            }
			
            if (m.Backpack != null)
                return m.Backpack.FindItemByType(typeof(TeleporterTicket), true) as TeleporterTicket;
				
            return null;
        }
		public virtual void EndConfirmation(Mobile from)
		{
			if (!ValidateUse(from, true))
				return;

			UseGate(from);
		}
		public virtual void Warning_Callback(Mobile from, bool okay, object state)
		{
			 TeleporterTicket ticket = GetTeleporterTicket(from);
			if (okay)
			{
				ticket.Delete();
				from.SendMessage("Your ticket disappears as you step into the Gate.");
				EndConfirmation(from);
				DelayedTeleport(from);
			}
			else 
			{
				from.SendMessage("You decided not to travel at this time.");
			}
		}

		public override void BeginConfirmation(Mobile from)
		{
			TeleporterTicket ticket = GetTeleporterTicket(from);
				
            if (ticket != null)
            {
				from.CloseGump(typeof(TicketWarningGump));

				from.SendGump(
					new TicketWarningGump(
						new TextDefinition(TitleNumber, TitleString),
						TitleColor,
						new TextDefinition(MessageNumber, MessageString),
						MessageColor,
						GumpWidth,
						GumpHeight,
						Warning_Callback,
						from));
			}
			else
			{
				from.SendMessage("You need The Champion Ticket to use this Portal!");
			}
		}
		
		 private void DelayedTeleport(Mobile m)
        {    

			Timer.DelayCall(TimeSpan.FromMinutes(60), DelayedTeleportCallback, m);

        }

        private void DelayedTeleportCallback(Mobile m)
        {
            m.Frozen = false;

            DoTeleport(m);
        }
		 public virtual void DoTeleport(Mobile m)
        {
            BaseCreature.TeleportPets(m, this.Location, this.Map);

            m.MoveToWorld( this.Location, this.Map );

        }
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(1); // version

			//writer.Write(TitleString);

			//writer.WriteEncodedInt(GumpWidth);
			//writer.WriteEncodedInt(GumpHeight);

			//writer.WriteEncodedInt(TitleColor);
			//writer.WriteEncodedInt(MessageColor);

			//writer.WriteEncodedInt(TitleNumber);
			//writer.WriteEncodedInt(MessageNumber);

			//writer.Write(MessageString);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
			/*switch (version)
			{
				case 1:
				{
					TitleString = reader.ReadString();
					goto case 0;
				}
				case 0:
				{
					GumpWidth = reader.ReadEncodedInt();
					GumpHeight = reader.ReadEncodedInt();

					TitleColor = reader.ReadEncodedInt();
					MessageColor = reader.ReadEncodedInt();

					TitleNumber = reader.ReadEncodedInt();
					MessageNumber = reader.ReadEncodedInt();

					MessageString = reader.ReadString();

					break;
				}*/
			//}
		}
	}

    public delegate void TicketWarningGumpCallback(Mobile from, bool okay, object state);

    public class TicketWarningGump : Gump
    {
        private readonly WarningGumpCallback m_Callback;
        private readonly object m_State;
        private readonly bool m_CancelButton;

        public TicketWarningGump(TextDefinition header, int headerColor, TextDefinition content, int contentColor, int width, int height, WarningGumpCallback callback, object state)
            : this(header, headerColor, content, contentColor, width, height, callback, state, true)
        {
        }
		
        public TicketWarningGump(TextDefinition header, int headerColor, TextDefinition content, int contentColor, int width, int height, WarningGumpCallback callback, object state, bool cancelButton, int ok = 1011036, int cancel = 1011012)
            : base((640 - width) / 2, (480 - height) / 2)
        {
            m_Callback = callback;
            m_State = state;
            m_CancelButton = cancelButton;

            Closable = false;

            AddPage(0);

            AddBackground(12, 9, 394, 180, 2620);

			AddHtml( 20, 20, 384, 130, "<BODY>" +
			//----------------------------------------/-----------------------------------------------/
			"<BASEFONT COLOR=YELLOW>Stepping through this Gate Consumes 1 Champion Ticket!<BR>" +
			"<BASEFONT COLOR=YELLOW><BR>" +
			"<BASEFONT COLOR=YELLOW>We highly Recommed you do not enter Alone. " +
			"<BASEFONT COLOR=YELLOW>Once you step through the Gate you have 1 hour " +
			"<BASEFONT COLOR=YELLOW>before you will be teleported out, wether you have " +
			"<BASEFONT COLOR=YELLOW>Or have not killed the champion and claimed " +
			"<BASEFONT COLOR=YELLOW>your reward.<BR> " +
			"<BASEFONT COLOR=YELLOW>Do you still Wish to continue?" +
			"</BODY>", false, true);
            
			/*if (header.Number > 0)
            {
                AddHtmlLocalized(10, 10, width - 20, 20, header.Number, headerColor, false, false);
            }
            else if (header.String != null)
            {
                AddHtml(10, 10, width - 20, height - 80, String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", headerColor, header.String), false, true);
            }
            if (content.Number > 0)
            {
                AddHtmlLocalized(10, 40, width - 20, height - 80, content.Number, contentColor, false, true);
            }
            else if (content.String != null)
            {
                AddHtml(10, 40, width - 20, height - 80, String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", contentColor, content.String), false, true);
            }
*/
			AddButton(365, 155, 4005, 4007, 1, GumpButtonType.Reply, 0);
            AddHtmlLocalized(325, 160, 170, 20, ok, 32767, false, false); // OKAY

            if (m_CancelButton)
            {
				AddButton(70, 155, 4005, 4007, 0, GumpButtonType.Reply, 0);
                AddHtmlLocalized(20, 160, 170, 20, cancel, 32767, false, false); // CANCEL
            }
        }

        public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
        {
            if (info.ButtonID == 1 && m_Callback != null)
                m_Callback(sender.Mobile, true, m_State);
            else if (m_Callback != null)
                m_Callback(sender.Mobile, false, m_State);
        }
    }
}