using System;

namespace Server.Items
{
	[FlipableAttribute(0xC001, 0xC002)]
	public class ValentineCookies : Cookies
    {
        [Constructable]
        public ValentineCookies()
			:base(0xC001)
        {
			
			
        }

        public ValentineCookies(Serial serial)
            : base(serial)
        {
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
		}
    }
}