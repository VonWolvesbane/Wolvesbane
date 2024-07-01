using System;
 
namespace Server.Items
{
    public class JulyThingy24 : Item
    {
        [Constructable]
        public JulyThingy24() : base()
        {
            this.Name = "July Special Thingy 2024";
			this.ItemID = 49161;
			LootType = LootType.Blessed;
		}
 
        public JulyThingy24( Serial serial ) : base( serial )
        {
        }
 
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
 
            writer.Write( (int) 0 ); // version
        }
 
        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
 
            int version = reader.ReadInt();
        }
    }
}
