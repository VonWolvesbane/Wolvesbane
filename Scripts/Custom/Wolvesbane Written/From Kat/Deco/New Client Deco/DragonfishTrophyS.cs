using System;
 
namespace Server.Items
{
    public class DragonfishTrophyS : Item
    {
        [Constructable]
        public DragonfishTrophyS() : base( 17639 )
        {
            this.Name = "Dragon Fish Trophy South";
            this.Hue = 0;
        }
 
        public DragonfishTrophyS( Serial serial ) : base( serial )
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
