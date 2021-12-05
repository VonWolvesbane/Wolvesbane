using System;
 
namespace Server.Items
{
    public class SmIciclesS : Item
    {
        [Constructable]
        public SmIciclesS() : base( 17779 )
        {
            this.Name = "Small Icicle S";
            this.Hue = 0;
        }
 
        public SmIciclesS( Serial serial ) : base( serial )
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
