using System;
 
namespace Server.Items
{
    public class LargeDyingPlant : Item
    {
        [Constructable]
        public LargeDyingPlant() : base( 17081 )
        {
            this.Name = "";
            this.Hue = 0;
        }
 
        public LargeDyingPlant( Serial serial ) : base( serial )
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
