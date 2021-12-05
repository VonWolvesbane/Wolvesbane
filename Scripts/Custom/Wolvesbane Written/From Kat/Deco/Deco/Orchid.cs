using System;
 
namespace Server.Items
{
    public class Orchid : Item
    {
        [Constructable]
        public Orchid() : base( 39696 )
        {
            this.Name = "Orchid";
            this.Hue = 0;
        }
 
        public Orchid( Serial serial ) : base( serial )
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
