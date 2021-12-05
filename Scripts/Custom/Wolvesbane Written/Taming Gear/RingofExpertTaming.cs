// Created by GreyWolf
// Created On: 10/4/2007
// Modified Nov. 4, 2007

using System;
using Server;

namespace Server.Items
{
    public class RingofExpertAnimalTaming : GoldRing
    {
        
        
        // For skill mods above Ring without changing everything to above Ring - GreyWolf.
        

        [Constructable]
        public RingofExpertAnimalTaming()
        {
            Name = "Ring of Expert Animal Taming";
            Hue = 93;
            LootType = LootType.Regular;
            Attributes.NightSight = 1;
            Attributes.BonusStr = 25;
            Attributes.BonusDex = 25;
            Attributes.RegenStam = 25;
            this.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            this.SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);

        }

       
       

        public override void OnSingleClick(Mobile from)
        {
            this.LabelTo(from, Name);
        }

        public RingofExpertAnimalTaming(Serial serial) : base( serial )
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
