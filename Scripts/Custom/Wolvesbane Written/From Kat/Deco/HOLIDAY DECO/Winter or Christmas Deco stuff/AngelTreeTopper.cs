using System;
 
namespace Server.Items
{
    public class AngelTreeTopper : Item
    {
        [Constructable]
        public AngelTreeTopper() : base( 18171 )
        {
            this.Name = "";
            this.Hue = 0;
        }
 
        public AngelTreeTopper( Serial serial ) : base( serial )
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
