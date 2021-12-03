using System;
 
namespace Server.Items
{
    public class SunDial : Item
    {
        [Constructable]
        public SunDial() : base( 39866 )
        {
            this.Name = "SunDial";
            this.Hue = 0;
        }
 
        public SunDial( Serial serial ) : base( serial )
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
