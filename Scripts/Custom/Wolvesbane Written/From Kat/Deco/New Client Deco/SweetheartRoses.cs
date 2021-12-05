using System;
 
namespace Server.Items
{
    public class SweetheartRoses : Item
    {
        [Constructable]
        public SweetheartRoses() : base( 19467 )
        {
            this.Name = "Sweetheart Roses";
            this.Hue = 38;
        }
 
        public SweetheartRoses( Serial serial ) : base( serial )
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
