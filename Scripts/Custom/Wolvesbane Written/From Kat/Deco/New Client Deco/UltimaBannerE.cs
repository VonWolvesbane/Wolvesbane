using System;
 
namespace Server.Items
{
    public class UltimaBannerE : Item
    {
        [Constructable]
        public UltimaBannerE() : base( 17098 )
        {
            this.Name = "Ultima Banner East";
            this.Hue = 0;
        }
 
        public UltimaBannerE( Serial serial ) : base( serial )
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
