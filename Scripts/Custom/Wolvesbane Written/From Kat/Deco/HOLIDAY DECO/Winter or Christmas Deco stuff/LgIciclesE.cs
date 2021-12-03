using System;
 
namespace Server.Items
{
    public class LgIciclesE : Item
    {
        [Constructable]
        public LgIciclesE() : base( 17781 )
        {
            this.Name = "Lg Icicles E";
            this.Hue = 0;
        }
 
        public LgIciclesE( Serial serial ) : base( serial )
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
