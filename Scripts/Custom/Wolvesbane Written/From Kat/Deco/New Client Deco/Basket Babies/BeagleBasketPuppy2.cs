using System;
 
namespace Server.Items
{
    public class BeagleBasketPuppy2 : Item
    {
        [Constructable]
        public BeagleBasketPuppy2() : base( 25591 )
        {
            this.Name = "BeagleBasketPuppy2";
            this.Hue = 0;
        }
 
        public BeagleBasketPuppy2( Serial serial ) : base( serial )
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
