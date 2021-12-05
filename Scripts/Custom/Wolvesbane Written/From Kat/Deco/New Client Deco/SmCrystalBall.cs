using System;
 
namespace Server.Items
{
    public class SmCrystalBall : Item
    {
        [Constructable]
        public SmCrystalBall() : base( 18058 )
        {
            this.Name = "Small Crystal Ball";
            this.Hue = 0;
        }
 
        public SmCrystalBall( Serial serial ) : base( serial )
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
