using System;
 
namespace Server.Items
{
    public class ChampagneFlute : Item
    {
        [Constructable]
        public ChampagneFlute() : base( 39594 )
        {
            this.Name = "Champagne Flute";
            this.Hue = 0;
        }
 
        public ChampagneFlute( Serial serial ) : base( serial )
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
