using System;
 
namespace Server.Items
{
    public class LgiciclesS : Item
    {
        [Constructable]
        public LgiciclesS() : base( 17778 )
        {
            this.Name = "Lg Icicles S";
            this.Hue = 0;
        }
 
        public LgiciclesS( Serial serial ) : base( serial )
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
