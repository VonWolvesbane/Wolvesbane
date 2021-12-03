using System;
 
namespace Server.Items
{
    public class IcicleE : Item
    {
        [Constructable]
        public IcicleE() : base( 17783 )
        {
            this.Name = "";
            this.Hue = 0;
        }
 
        public IcicleE( Serial serial ) : base( serial )
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
