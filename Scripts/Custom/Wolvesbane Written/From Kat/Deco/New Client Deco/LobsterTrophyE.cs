using System;
 
namespace Server.Items
{
    public class LobsterTrophyE : Item
    {
        [Constructable]
        public LobsterTrophyE() : base( 18109 )
        {
            this.Name = "Lobster Trophy E";
            this.Hue = 38;
        }
 
        public LobsterTrophyE( Serial serial ) : base( serial )
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
