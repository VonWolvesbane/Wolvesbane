using System;
 
namespace Server.Items
{
    public class Champagne : Item
    {
        [Constructable]
        public Champagne() : base( 39597 )
        {
            this.Name = "Champagne";
            this.Hue = 0;
        }
 
        public Champagne( Serial serial ) : base( serial )
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
