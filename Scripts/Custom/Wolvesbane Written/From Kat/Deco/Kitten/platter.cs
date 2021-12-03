using System;
 
namespace Server.Items
{
    public class platter : Static
    {
        [Constructable]
        public platter() : base( 39265 )
        {
            this.Name = "platter";
            this.Hue = 1150;
        }
 
        public platter( Serial serial ) : base( serial )
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
