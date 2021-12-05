using System;
 
namespace Server.Items
{
    public class ChaosBannerE : Item
    {
        [Constructable]
        public ChaosBannerE() : base( 17100 )
        {
            this.Name = "Chaos Banner East";
            this.Hue = 0;
        }
 
        public ChaosBannerE( Serial serial ) : base( serial )
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
