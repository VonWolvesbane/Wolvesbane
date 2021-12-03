using System;
 
namespace Server.Items
{
    public class GiftBox3 : GiftBoxCube
    {
        [Constructable]
        public GiftBox3() : base( 39572 )
        {
            this.Name = "Gift Box 3";
            this.Hue = 1173;
        }
 
        public GiftBox3( Serial serial ) : base( serial )
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
