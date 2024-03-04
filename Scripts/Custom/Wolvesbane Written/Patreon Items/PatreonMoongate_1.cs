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
		public override void OnDoubleClick(Mobile m)
		{
			if (m is PlayerMobile player && player.IsPatreon)
			{
				base.OnDoubleClick(m); // Allow player to use moongate if they are Patreon
			}
			else
			{
				m.SendMessage("You are not authorized to use this moongate.");
			}
		}
		public override void UseGate(Mobile m)
		{
			// Perform authorization check before calling base method
			if (m is PlayerMobile player && !player.IsPatreon)
			{
				player.SendMessage("You are not a Wolvesbane UO Patreon Member, and therefore are not authorized to use this moongate.");
				return;
			}

			// Call the base method to handle regular gate usage
			base.UseGate(m);
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

    
