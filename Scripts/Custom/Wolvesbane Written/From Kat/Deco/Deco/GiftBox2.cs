using System;
 
namespace Server.Items
{
    public class GiftBox2 : GiftBoxCube
    {
        [Constructable]
        public GiftBox2() : base( 39571 )
        {
            this.Name = "Gift Box 2";
            this.Hue = 1173;
        }
 
        public GiftBox2( Serial serial ) : base( serial )
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
