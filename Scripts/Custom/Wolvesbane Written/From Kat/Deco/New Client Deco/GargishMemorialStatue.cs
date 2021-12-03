using System;
 
namespace Server.Items
{
    public class GargishMemorialStatue : Item
    {
        [Constructable]
        public GargishMemorialStatue() : base( 17091 )
        {
            this.Name = "Gargish Memorial Statue";
            this.Hue = 0;
        }
 
        public GargishMemorialStatue( Serial serial ) : base( serial )
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
