using System;
 
namespace Server.Items
{
    public class RuinedTapestryE : Item
    {
        [Constructable]
        public RuinedTapestryE() : base( 18074 )
        {
            this.Name = "Ruined Tapestry E";
            this.Hue = 0;
        }
 
        public RuinedTapestryE( Serial serial ) : base( serial )
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
