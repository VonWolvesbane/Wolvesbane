// Created by GreyWolf
// Created On: 10/4/2007
// Modified Nov. 4, 2007

using System;
using Server;

namespace Server.Items
{
    public class TunicofXMenUniform : LeatherChest
    {
        public override int BasePhysicalResistance{ get{ return 15; } }
        public override int BaseColdResistance{ get{ return 15; } }
        public override int BaseFireResistance{ get{ return 15; } }
        public override int BaseEnergyResistance{ get{ return 15; } }
        public override int BasePoisonResistance{ get{ return 15; } }
        public override int InitMinHits{ get{ return 50; } }
        public override int InitMaxHits{ get{ return 100; } }
       

        [Constructable]
        public TunicofXMenUniform()
        {
            Name = "Tunic of X-men Uniform";
            Hue = 1980;
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

        public TunicofXMenUniform(Serial serial) : base( serial )
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
