using System;
 
namespace Server.Items
{
    public class EmptyGargishBottle : Item
    {
        [Constructable]
        public EmptyGargishBottle() : base( 18054 )
        {
            this.Name = "Empty Gargish Bottle";
            this.Hue = 0;
        }
 
        public EmptyGargishBottle( Serial serial ) : base( serial )
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
