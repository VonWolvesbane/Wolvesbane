

using System;
using Server;

namespace Server.Items
{
    public class VIPCloak : Cloak
    {
       

       

        [Constructable]
        public VIPCloak()
        {
            Name = "VIP Cloak";
            Hue = 2050;
            LootType = LootType.Blessed;
            Attributes.NightSight = 1;
            Attributes.BonusStr = 350;
            Attributes.BonusDex = 350;
            Attributes.BonusInt = 350;
            Attributes.RegenHits = 25;
            Attributes.RegenStam = 25;
            Attributes.RegenMana = 25;



        }

        
        public override void OnSingleClick(Mobile from)
        {
            this.LabelTo(from, Name);
        }

        public VIPCloak(Serial serial) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    } // End Class
} // End Namespace
