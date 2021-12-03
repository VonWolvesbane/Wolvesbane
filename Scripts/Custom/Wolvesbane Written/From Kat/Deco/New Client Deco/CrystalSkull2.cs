using System;
 
namespace Server.Items
{
    public class CrystalSkull2 : Item
    {
        [Constructable]
        public CrystalSkull2() : base( 39451 )
        {
            this.Name = "Crystal Skull 2";
            this.Hue = 0;
        }
 
        public CrystalSkull2( Serial serial ) : base( serial )
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
