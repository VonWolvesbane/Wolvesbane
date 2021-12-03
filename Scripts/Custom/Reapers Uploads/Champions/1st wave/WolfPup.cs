using System;
using System.Collections;
using Server.Mobiles;
namespace Server.Mobiles
{
    [CorpseName("remains of a puppy")]

    public class WolfPup : BaseCreature
    {
        
        [Constructable]

        public WolfPup() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a wolf pup";
            Hue = 2050;
            Body = 0xD9;

            SetDamage(15, 25);

            SetStr(400);
            SetDex(100);
            SetInt(1450, 1555);

            SetHits(200);
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
        public WolfPup(Serial serial) : base(serial)
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
