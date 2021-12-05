using System;
 
namespace Server.Items
{
    public class RuinedTapestryS : Item
    {
        [Constructable]
        public RuinedTapestryS() : base( 18073 )
        {
            this.Name = "Ruined Tapestry S";
            this.Hue = 0;
        }
 
        public RuinedTapestryS( Serial serial ) : base( serial )
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
