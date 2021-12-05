using System;
 
namespace Server.Items
{
    public class ObsidianSkull2 : Item
    {
        [Constructable]
        public ObsidianSkull2() : base( 39454 )
        {
            this.Name = "Obsidian Skull 2";
            this.Hue = 0;
        }
 
        public ObsidianSkull2( Serial serial ) : base( serial )
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
