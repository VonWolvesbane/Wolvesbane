using System;
 
namespace Server.Items
{
    public class OrderBannerS : Item
    {
        [Constructable]
        public OrderBannerS() : base( 17101 )
        {
            this.Name = "Order Banner S";
            this.Hue = 0;
        }
 
        public OrderBannerS( Serial serial ) : base( serial )
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
