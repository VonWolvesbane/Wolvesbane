

using System;
using Server;

namespace Server.Items
{
    public class UberRobe : Robe
    {
       

       

        [Constructable]
        public UberRobe()
        {
            Name = "Uber Robe";
            Hue = 2050;
            ItemID = 30742;
            LootType = LootType.Blessed;
            Attributes.NightSight = 1;
            
            this.SkillBonuses.SetValues(0, SkillName.Veterinary, 10.0);

            this.SkillBonuses.SetValues(1, SkillName.AnimalTaming, 10.0);

            this.SkillBonuses.SetValues(2, SkillName.AnimalLore, 10.0);
        }

        
        public override void OnSingleClick(Mobile from)
        {
            this.LabelTo(from, Name);
        }

        public UberRobe(Serial serial) : base( serial )
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
