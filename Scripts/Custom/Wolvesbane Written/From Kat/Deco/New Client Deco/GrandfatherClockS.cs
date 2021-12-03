using System;
 
namespace Server.Items
{
    public class GrandfatherClockS : Item
    {
        [Constructable]
        public GrandfatherClockS() : base( 17621 )
        {
            this.Name = "";
            this.Hue = 0;
        }
 
        public GrandfatherClockS( Serial serial ) : base( serial )
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
