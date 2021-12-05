using System;

namespace Server.Items
{
    public class PatreonTicket : Item
    {
		[Constructable]
		public PatreonTicket() : base( 0x14F0 )
		{
			Name = "A Patreon Ticket";
			Hue = 2066;
		}

        public PatreonTicket(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadEncodedInt();
        }
    }
}
