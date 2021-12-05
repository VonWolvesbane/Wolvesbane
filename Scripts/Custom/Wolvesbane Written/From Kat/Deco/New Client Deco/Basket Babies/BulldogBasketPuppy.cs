using System;
 
namespace Server.Items
{
    public class BulldogBasketPuppy : Item
    {
        [Constructable]
        public BulldogBasketPuppy() : base( 25576 )
        {
            this.Name = "BulldogBasketPuppy";
            this.Hue = 0;
        }
 
        public BulldogBasketPuppy( Serial serial ) : base( serial )
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
