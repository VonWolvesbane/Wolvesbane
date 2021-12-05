using System;
 
namespace Server.Items
{
    public class GargishTraditionalVase : Item
    {
        [Constructable]
        public GargishTraditionalVase() : base( 17074 )
        {
            this.Name = "Gargish Traditional Vase";
            this.Hue = 0;
        }
 
        public GargishTraditionalVase( Serial serial ) : base( serial )
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
