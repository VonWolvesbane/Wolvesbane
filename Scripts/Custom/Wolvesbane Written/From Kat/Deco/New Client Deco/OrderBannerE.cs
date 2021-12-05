using System;
 
namespace Server.Items
{
    public class OrderBannerE : Item
    {
        [Constructable]
        public OrderBannerE() : base( 17102 )
        {
            this.Name = "Order Banner E";
            this.Hue = 0;
        }
 
        public OrderBannerE( Serial serial ) : base( serial )
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
