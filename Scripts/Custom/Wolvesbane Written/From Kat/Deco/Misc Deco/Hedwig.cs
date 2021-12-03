using System;
 
namespace Server.Items
{
    public class Hedwig : Static
    {
        [Constructable]
        public Hedwig() : base( 39579 )
        {
            this.Name = "Hedwig";
            this.Hue = 0;
        }
 
        public Hedwig( Serial serial ) : base( serial )
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
