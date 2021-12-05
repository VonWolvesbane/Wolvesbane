using System;
 
namespace Server.Items
{
    public class DragonfishTrophyE : Item
    {
        [Constructable]
        public DragonfishTrophyE() : base( 17640 )
        {
            this.Name = "Dragon Fish Trophy East";
            this.Hue = 0;
        }
 
        public DragonfishTrophyE( Serial serial ) : base( serial )
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
