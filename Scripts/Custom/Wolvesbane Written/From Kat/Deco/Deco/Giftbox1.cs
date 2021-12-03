using System;
 
namespace Server.Items
{
    public class Giftbox1 : GiftBoxCube
    {
        [Constructable]
        public Giftbox1() : base( 39570 )
        {
            this.Name = "Gift Box 1";
            this.Hue = 1173;
        }
 
        public Giftbox1( Serial serial ) : base( serial )
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
