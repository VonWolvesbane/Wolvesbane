using System;
 
namespace Server.Items
{
    public class GreyBasketKitten : Item
    {
        [Constructable]
        public GreyBasketKitten() : base( 25564 )
        {
            this.Name = "GreyBasketKitten";
            this.Hue = 0;
        }
 
        public GreyBasketKitten( Serial serial ) : base( serial )
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
