

using System;
using Server;

namespace Server.Items
{
    public class CyclopsVisor : Glasses
    {
        public override int BasePhysicalResistance{ get{ return 15; } }
        public override int BaseColdResistance{ get{ return 15; } }
        public override int BaseFireResistance{ get{ return 15; } }
        public override int BaseEnergyResistance{ get{ return 15; } }
        public override int BasePoisonResistance{ get{ return 15; } }
        public override int InitMinHits{ get{ return 50; } }
        public override int InitMaxHits{ get{ return 100; } }
       

       

        [Constructable]
        public CyclopsVisor()
        {
            Name = "Visor of Cyclops";
            Hue = 1986;
            LootType = LootType.Regular;
            Attributes.NightSight = 1;
            Attributes.BonusStr = 5;
            Attributes.BonusDex = 5;
            Attributes.RegenStam = 5;
            
            

        }

        
        public override void OnSingleClick(Mobile from)
        {
            this.LabelTo(from, Name);
        }

        public CyclopsVisor(Serial serial) : base( serial )
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
