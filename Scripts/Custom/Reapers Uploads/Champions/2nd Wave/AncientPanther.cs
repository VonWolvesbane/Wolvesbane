using System;
using System.Collections;
using Server.Mobiles;
namespace Server.Mobiles
{
    [CorpseName("remains of a Panther")]

    public class AncientPanther : BaseCreature
    {
        
        [Constructable]

        public AncientPanther() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Ancient Panther";
            Hue = 2053;
            Body = 0xD6;

            SetDamage(25, 35);

            SetStr(400, 700);
            SetDex(400);
            SetInt(140, 155);

            SetHits(700);
            SetMana(1400, 1500);

            SetDamageType(ResistanceType.Physical, 75);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 50);
            SetResistance(ResistanceType.Cold, 10, 35);
            SetResistance(ResistanceType.Poison, 10, 35);
            SetResistance(ResistanceType.Energy, 10, 30);

            SetSkill(SkillName.MagicResist, 100.1, 120.0);
            SetSkill(SkillName.Tactics, 100.2, 120.0);
            SetSkill(SkillName.Wrestling, 100.2, 120.0);
            Fame = 300;
            Karma = -159;


            Tamable = false;
        }
        public AncientPanther(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
