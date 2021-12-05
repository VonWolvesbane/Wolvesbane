using System;
 
namespace Server.Items
{
    public class geode : Static
    {
        [Constructable]
        public geode() : base( 19274 )
        {
            this.Name = "";
            this.Hue = 1266;
        }
 
        public geode( Serial serial ) : base( serial )
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
