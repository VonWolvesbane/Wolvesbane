using System;

namespace Server.Items
{
	[FlipableAttribute(0xC001, 0xC002)]
	public class ValentineCookies : Food
    {
        [Constructable]
        public ValentineCookies()
			:base (0xC001)
        {
			Weight = 1.0;
			FillFactor = 5;
			Stackable = false;

		}

        public ValentineCookies(Serial serial)
            : base(serial)
        {
        }
		public override string DefaultName
		{
			get
			{
				return "Valentine's Day Cookies";
			}
		}

		public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if (version == 0)
				Stackable = false;
		}
    }
}