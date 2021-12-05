using System;
 
namespace Server.Items
{
    public class PlaygroundAnkhTrophy : Item
    {
        [Constructable]
        public PlaygroundAnkhTrophy() : base( 39367 )
        {
            this.Name = "Playground Ankh Trophy";
            this.Hue = 0;
        }
 
        public PlaygroundAnkhTrophy( Serial serial ) : base( serial )
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
