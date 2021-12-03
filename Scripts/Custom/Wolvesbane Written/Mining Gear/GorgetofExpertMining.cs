// Created by GreyWolf
// Created On: 10/4/2007
// Modified Nov. 4, 2007

using System;
using Server;

namespace Server.Items
{
    public class GorgetofExpertMining : LeatherGorget
    {
        public override int BasePhysicalResistance{ get{ return 5; } }
        public override int BaseColdResistance{ get{ return 5; } }
        public override int BaseFireResistance{ get{ return 5; } }
        public override int BaseEnergyResistance{ get{ return 5; } }
        public override int BasePoisonResistance{ get{ return 5; } }
        public override int InitMinHits{ get{ return 50; } }
        public override int InitMaxHits{ get{ return 100; } }
        

       

        [Constructable]
        public GorgetofExpertMining()
        {
            Name = "Gorget of Expert Mining";
            Hue = 93;
            LootType = LootType.Regular;
            Attributes.NightSight = 1;
            Attributes.BonusStr = 5;
            Attributes.BonusDex = 5;
            Attributes.RegenStam = 5;
            this.SkillBonuses.SetValues(0, SkillName.Mining, 10.0);
            

        }

        public override void OnSingleClick(Mobile from)
        {
            this.LabelTo(from, Name);
        }

        public GorgetofExpertMining(Serial serial) : base( serial )
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
