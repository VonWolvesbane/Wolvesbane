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
    public class PatreonMoongate : Moongate
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
        public PatreonMoongate()
            : this(Point3D.Zero, null)
        { }

        [Constructable]
        public PatreonMoongate(Point3D target, Map targetMap)
            : base(target, targetMap)
        { }

        public PatreonMoongate(Serial serial)
            : base(serial)
        { }
        public static PatreonTicket GetPatreonTicket(Mobile m)
        {
            for (int i = 0; i < m.Items.Count; i++)
            {
                if (m.Items[i] is PatreonTicket)
                    return (PatreonTicket)m.Items[i];
            }

            if (m.Backpack != null)
                return m.Backpack.FindItemByType(typeof(PatreonTicket), true) as PatreonTicket;

            return null;
        }
        public virtual void EndConfirmation(Mobile from)
        {
            if (!ValidateUse(from, true))
                return;

            UseGate(from);
        }
        public virtual void Warning_Callback(Mobile from)
        {
            PatreonTicket ticket = GetPatreonTicket(from);
            if (ticket != null)
            {
                //ticket.Delete();
                from.SendMessage("You are transferred to the Patreon Dungeon as you step into the Gate.");
                EndConfirmation(from);
                
            }
            else
            {
                from.SendMessage("You decided not to travel at this time.");
            }
        }
		
		public override void BeginConfirmation(Mobile from)
		{
			PatreonTicket ticket = GetPatreonTicket(from);
				
            if (ticket != null)
            {
				Warning_Callback(from);
			}
			else
			{
				from.SendMessage("You need The Patreon Ticket to use this Portal!");
			}
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

}

    
