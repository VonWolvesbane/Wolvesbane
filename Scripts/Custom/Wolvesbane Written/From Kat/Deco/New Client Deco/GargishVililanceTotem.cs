using System;
 
namespace Server.Items
{
    public class GargishVililanceTotem : Item
    {
        [Constructable]
        public GargishVililanceTotem() : base( 17084 )
        {
            this.Name = "Gargish Vililance Totem";
            this.Hue = 0;
        }
 
        public GargishVililanceTotem( Serial serial ) : base( serial )
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
