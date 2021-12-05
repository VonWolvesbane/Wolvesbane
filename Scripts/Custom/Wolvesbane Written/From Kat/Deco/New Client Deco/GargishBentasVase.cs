using System;
 
namespace Server.Items
{
    public class GargishBentasVase : Item
    {
        [Constructable]
        public GargishBentasVase() : base( 17075 )
        {
            this.Name = "Gargish Bentas Vase";
            this.Hue = 0;
        }
 
        public GargishBentasVase( Serial serial ) : base( serial )
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
