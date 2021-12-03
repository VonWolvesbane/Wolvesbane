using System;
 
namespace Server.Items
{
    public class LgNutcrackerS : Item
    {
        [Constructable]
        public LgNutcrackerS() : base( 18165 )
        {
            this.Name = "Lg Nutcracker S";
            this.Hue = 0;
        }
 
        public LgNutcrackerS( Serial serial ) : base( serial )
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
