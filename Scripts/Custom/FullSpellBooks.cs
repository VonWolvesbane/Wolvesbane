using System;

namespace Server.Items
{
	public class FullRegularSpellbook : Spellbook
	{
		[Constructable]
		public FullRegularSpellbook()
			: base(UInt64.MaxValue)
		{ }

		public FullRegularSpellbook(Serial serial)
			: base(serial)
		{ }

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
	
	public class FullNecroSpellbook : NecromancerSpellbook
	{
		[Constructable]
		public FullNecroSpellbook()
			: base((UInt64)0xFFFF)
		{ }

		public FullNecroSpellbook(Serial serial)
			: base(serial)
		{ }

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
	
	public class FullBookOfChivalry : BookOfChivalry
	{
		[Constructable]
		public FullBookOfChivalry()
			: base((UInt64)0x3FF)
		{ }

		public FullBookOfChivalry(Serial serial)
			: base(serial)
		{ }

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
