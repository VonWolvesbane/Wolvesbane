using System;
 
namespace Server.Items
{
    public class BulldogBasketPuppy2 : Item
    {
        [Constructable]
        public BulldogBasketPuppy2() : base( 25579 )
        {
            this.Name = "BulldogBasketPuppy2";
            this.Hue = 0;
        }
 
        public BulldogBasketPuppy2( Serial serial ) : base( serial )
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
