using System;
 
namespace Server.Items
{
    public class SmNutcrackerE : Item
    {
        [Constructable]
        public SmNutcrackerE() : base( 18156 )
        {
            this.Name = "Sm Nutcracker E";
            this.Hue = 0;
        }
 
        public SmNutcrackerE( Serial serial ) : base( serial )
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
