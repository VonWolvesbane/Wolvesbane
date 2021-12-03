using System;
 
namespace Server.Items
{
    public class CherryPie : Static
    {
        [Constructable]
        public CherryPie() : base( 19468 )
        {
            this.Name = "CherryPie";
            this.Hue = 0;
        }
 
        public CherryPie( Serial serial ) : base( serial )
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
