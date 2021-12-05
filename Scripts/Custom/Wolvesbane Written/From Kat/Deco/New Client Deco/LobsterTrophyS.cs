using System;
 
namespace Server.Items
{
    public class LobsterTrophyS : Item
    {
        [Constructable]
        public LobsterTrophyS() : base( 18108 )
        {
            this.Name = "Lobster Troyphy S";
            this.Hue = 38;
        }
 
        public LobsterTrophyS( Serial serial ) : base( serial )
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
