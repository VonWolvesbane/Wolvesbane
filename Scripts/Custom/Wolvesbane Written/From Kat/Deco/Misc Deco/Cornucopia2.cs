using System;
 
namespace Server.Items
{
    public class Cornucopia2 : Static
    {
        [Constructable]
        public Cornucopia2() : base( 19417 )
        {
            this.Name = "Cornucopia 2";
            this.Hue = 0;
        }
 
        public Cornucopia2( Serial serial ) : base( serial )
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
