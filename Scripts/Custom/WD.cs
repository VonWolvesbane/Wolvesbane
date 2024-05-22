using System;
 
namespace Server.Items
{
    public class WDollar : Item
    {
        [Constructable]
        public WDollar() : base()
        {
            this.Name = "Wolvesbane Dollar";
			this.ItemID = 49160;
			this.Stackable = true;
			LootType = LootType.Blessed;
		}
 
        public WDollar( Serial serial ) : base( serial )
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
