using System;
 
namespace Server.Items
{
    public class GargishBottle : Item
    {
        [Constructable]
        public GargishBottle() : base( 17076 )
        {
            this.Name = "Gargish Bottle";
            this.Hue = 0;
        }
 
        public GargishBottle( Serial serial ) : base( serial )
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
