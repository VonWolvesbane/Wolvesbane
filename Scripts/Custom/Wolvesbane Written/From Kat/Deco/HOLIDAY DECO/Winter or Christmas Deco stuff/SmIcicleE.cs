using System;
 
namespace Server.Items
{
    public class SmIcicleE : Item
    {
        [Constructable]
        public SmIcicleE() : base( 17782 )
        {
            this.Name = "Small Icicle E";
            this.Hue = 0;
        }
 
        public SmIcicleE( Serial serial ) : base( serial )
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
