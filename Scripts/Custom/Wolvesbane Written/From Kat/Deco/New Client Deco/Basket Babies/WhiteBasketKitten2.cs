using System;
 
namespace Server.Items
{
    public class WhiteBasketKitten2 : Item
    {
        [Constructable]
        public WhiteBasketKitten2() : base( 25561 )
        {
            this.Name = "WhiteBaskeKitten2";
            this.Hue = 0;
        }
 
        public WhiteBasketKitten2( Serial serial ) : base( serial )
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
