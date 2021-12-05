using System;
 
namespace Server.Items
{
    public class JadeSkull2 : Item
    {
        [Constructable]
        public JadeSkull2() : base( 39453 )
        {
            this.Name = "Jade Skull 2";
            this.Hue = 0;
        }
 
        public JadeSkull2( Serial serial ) : base( serial )
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
