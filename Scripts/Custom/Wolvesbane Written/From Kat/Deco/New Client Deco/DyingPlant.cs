using System;
 
namespace Server.Items
{
    public class DyingPlant : Item
    {
        [Constructable]
        public DyingPlant() : base( 17082 )
        {
            this.Name = "Dying Plant";
            this.Hue = 0;
        }
 
        public DyingPlant( Serial serial ) : base( serial )
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
