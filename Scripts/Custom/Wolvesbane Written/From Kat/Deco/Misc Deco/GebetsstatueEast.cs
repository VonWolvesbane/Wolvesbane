using System;

namespace Server.Items
{
	public class GebetsstatueEast : Item
	{
		[Constructable]
		public GebetsstatueEast() : base(6472)
		{
			Name = "Gebetsstatue";
			Weight = 1.0;
		}

		public GebetsstatueEast(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}
}
