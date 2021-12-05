using System;
 
namespace Server.Items
{
    public class RottieBasketPuppy2 : Item
    {
        [Constructable]
        public RottieBasketPuppy2() : base( 25585 )
        {
            this.Name = "RottieBasketPuppy2";
            this.Hue = 0;
        }
 
        public RottieBasketPuppy2( Serial serial ) : base( serial )
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
