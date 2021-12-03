using System;
 
namespace Server.Items
{
    public class CottonTree : Item
    {
        [Constructable]
        public CottonTree() : base( 39695 )
        {
            this.Name = "Cotton";
            this.Hue = 0;
        }
 
        public CottonTree( Serial serial ) : base( serial )
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
