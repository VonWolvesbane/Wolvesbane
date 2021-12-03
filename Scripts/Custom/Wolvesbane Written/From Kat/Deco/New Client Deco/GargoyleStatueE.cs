using System;
 
namespace Server.Items
{
    public class GargoyleStatueE : Item
    {
        [Constructable]
        public GargoyleStatueE() : base( 18766 )
        {
            this.Name = "Gargoyle Statue E";
            this.Hue = 0;
        }
 
        public GargoyleStatueE( Serial serial ) : base( serial )
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
