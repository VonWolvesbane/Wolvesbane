using System;
 
namespace Server.Items
{
    public class CrabTrophyS : Item
    {
        [Constructable]
        public CrabTrophyS() : base( 18106 )
        {
            this.Name = "Crab Trophy South";
            this.Hue = 38;
        }
 
        public CrabTrophyS( Serial serial ) : base( serial )
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
