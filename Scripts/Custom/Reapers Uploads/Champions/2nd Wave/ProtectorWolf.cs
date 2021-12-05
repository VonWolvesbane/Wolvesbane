using System;
using System.Collections;
using Server.Mobiles;
namespace Server.Mobiles
{
    [CorpseName("remains of a Wolf")]

    public class ProtectorWolf : GreyWolf
    {
        
        [Constructable]

        public ProtectorWolf() : base()
        {
            Name = "A Wolf Protector";
            Hue = 2063;

            SetDamage(25, 35);

            SetStr(500, 700);
            SetDex(200);
            SetInt(150, 155);

            SetHits(980, 1000);

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
        public ProtectorWolf(Serial serial) : base(serial)
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
