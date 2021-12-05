using System;
 
namespace Server.Items
{
    public class WhiteGrandfatherClockE : Item
    {
        [Constructable]
        public WhiteGrandfatherClockE() : base( 18648 )
        {
            this.Name = "White Grandfather Clock E";
            this.Hue = 0;
        }
 
        public WhiteGrandfatherClockE( Serial serial ) : base( serial )
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
