using System;
 
namespace Server.Items
{
    public class DragonEasterEgg : Item
    {
        [Constructable]
        public DragonEasterEgg() : base( 18406 )
        {
            this.Name = "Dragon Easter Egg";
            this.Hue = 0;
        }
 
        public DragonEasterEgg( Serial serial ) : base( serial )
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
