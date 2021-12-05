using System;
 
namespace Server.Items
{
    public class WhiteBasketKitten : Item
    {
        [Constructable]
        public WhiteBasketKitten() : base( 25558 )
        {
            this.Name = "White Basket Kitten";
            this.Hue = 0;
        }
 
        public WhiteBasketKitten( Serial serial ) : base( serial )
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
