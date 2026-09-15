using System;
using Server;
using Server.Items;

namespace Server.Items
{
    /// <summary>
    /// Dedicated editable vendor stone for the verified voting reward currency.
    /// Uses the existing TokenVendorStone system and defaults its currency to NewVoteToken.
    /// </summary>
    public class NewVoteTokenStone : TokenVendorStone
    {
        [Constructable]
        public NewVoteTokenStone() : base()
        {
            Name = "Vote Token Stone";
            Hue = 2065;
            Currency = "NewVoteToken";
        }

        public NewVoteTokenStone(Serial serial) : base(serial)
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
