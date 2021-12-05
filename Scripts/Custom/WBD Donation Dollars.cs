using System;
 
namespace Server.Items
{
    public class WolvesbaneDollar : BlankScroll
    {
        [Constructable]
        public WolvesbaneDollar() : base()
        {
            this.Name = "Wolvesbane Dollar";
            this.Hue = 2044;
			LootType = LootType.Blessed;
		}
 
        public WolvesbaneDollar( Serial serial ) : base( serial )
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
