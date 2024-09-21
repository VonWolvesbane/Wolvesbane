using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cute ghoul corpse")]
    public class CuteGhoul : BaseCreature
    {
        [Constructable]
        public CuteGhoul() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Cute Ghoul";
            Body = 0x99; // Body ID for a ghoul
            Hue = 2253; // Custom hue

            SetStr(600);
            SetDex(800);
            SetHits(40000); // Hits
            SetStam(800); // Stamina

            SetDamage(55, 80); // Min and Max damage

            SetSkill(SkillName.Wrestling, 175.0);
            SetSkill(SkillName.Tactics, 175.0);
            SetSkill(SkillName.Parry, 175.0);
            SetSkill(SkillName.Anatomy, 175.0);

            Fame = 2500; // Optional: Adjust based on desired difficulty
            Karma = -2500; // Optional: Adjust as needed

            VirtualArmor = 50; // Adjust based on desired difficulty
        }

        public CuteGhoul(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
