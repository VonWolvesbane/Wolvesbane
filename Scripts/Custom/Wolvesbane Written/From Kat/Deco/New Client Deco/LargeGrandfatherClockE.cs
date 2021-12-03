using System;
 
namespace Server.Items
{
    public class LargeGrandfatherClockE : Item
    {
        [Constructable]
        public LargeGrandfatherClockE() : base( 17633 )
        {
            this.Name = "Large Grandfather Clock E";
            this.Hue = 0;
        }
 
        public LargeGrandfatherClockE( Serial serial ) : base( serial )
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
