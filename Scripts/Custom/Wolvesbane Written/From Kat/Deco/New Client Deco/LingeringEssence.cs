using System;
 
namespace Server.Items
{
    public class LingeringEssence : Item
    {
        [Constructable]
        public LingeringEssence() : base( 17103 )
        {
            this.Name = "Lingering Essence";
            this.Hue = 0;
        }
 
        public LingeringEssence( Serial serial ) : base( serial )
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
