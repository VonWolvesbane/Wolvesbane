using System;
 
namespace Server.Items
{
    public class LargeGrandfatherClockS : Item
    {
        [Constructable]
        public LargeGrandfatherClockS() : base( 17629 )
        {
            this.Name = "Large Grandfather Clock S";
            this.Hue = 0;
        }
 
        public LargeGrandfatherClockS( Serial serial ) : base( serial )
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
