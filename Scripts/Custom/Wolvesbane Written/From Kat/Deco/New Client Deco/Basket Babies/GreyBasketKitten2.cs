using System;
 
namespace Server.Items
{
    public class GreyBasketKitten2 : Item
    {
        [Constructable]
        public GreyBasketKitten2() : base( 25568 )
        {
            this.Name = "GreyBasketKitten2";
            this.Hue = 0;
        }
 
        public GreyBasketKitten2( Serial serial ) : base( serial )
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
