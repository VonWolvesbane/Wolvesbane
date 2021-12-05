using System;
 
namespace Server.Items
{
    public class SmNutcrackerS : Item
    {
        [Constructable]
        public SmNutcrackerS() : base( 18159 )
        {
            this.Name = "Sm Nutcracker S";
            this.Hue = 0;
        }
 
        public SmNutcrackerS( Serial serial ) : base( serial )
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
