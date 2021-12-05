using System;
 
namespace Server.Items
{
    public class GargishWarriorStatue : Item
    {
        [Constructable]
        public GargishWarriorStatue() : base( 17090 )
        {
            this.Name = "Gargish Warrior Statue";
            this.Hue = 0;
        }
 
        public GargishWarriorStatue( Serial serial ) : base( serial )
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
