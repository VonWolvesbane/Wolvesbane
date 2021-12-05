using System;
 
namespace Server.Items
{
    public class BeagleBasketPuppy : Item
    {
        [Constructable]
        public BeagleBasketPuppy() : base( 25588 )
        {
            this.Name = "BeagleBasketPuppy";
            this.Hue = 0;
        }
 
        public BeagleBasketPuppy( Serial serial ) : base( serial )
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
