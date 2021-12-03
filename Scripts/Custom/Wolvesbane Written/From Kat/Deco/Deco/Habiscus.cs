using System;
 
namespace Server.Items
{
    public class Habiscus : Item
    {
        [Constructable]
        public Habiscus() : base( 39697 )
        {
            this.Name = "Habiscus";
            this.Hue = 0;
        }
 
        public Habiscus( Serial serial ) : base( serial )
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
