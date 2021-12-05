using System;
 
namespace Server.Items
{
    public class OrangeBasketKitten2 : Item
    {
        [Constructable]
        public OrangeBasketKitten2() : base( 25573 )
        {
            this.Name = "OrangeBasketKitten2";
            this.Hue = 0;
        }
 
        public OrangeBasketKitten2( Serial serial ) : base( serial )
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
