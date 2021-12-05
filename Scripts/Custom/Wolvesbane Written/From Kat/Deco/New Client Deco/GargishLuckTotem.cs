using System;
 
namespace Server.Items
{
    public class GargishLuckTotem : Item
    {
        [Constructable]
        public GargishLuckTotem() : base( 17088 )
        {
            this.Name = "Gargish Luck Totem";
            this.Hue = 0;
        }
 
        public GargishLuckTotem( Serial serial ) : base( serial )
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
