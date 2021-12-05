using System;
 
namespace Server.Items
{
    public class LgCrystalBall : Item
    {
        [Constructable]
        public LgCrystalBall() : base( 18059 )
        {
            this.Name = "Large Crystal Ball";
            this.Hue = 0;
        }
 
        public LgCrystalBall( Serial serial ) : base( serial )
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
