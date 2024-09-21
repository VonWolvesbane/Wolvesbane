using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cute mummy corpse")]
    public class CuteMummy : BaseCreature
    {
        [Constructable]
        public CuteMummy() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Cute Mummy";
            Body = 0x9A; // Body ID for a mummy
            Hue = 1153; // Custom hue

            SetStr(600);
            SetDex(800);
            SetInt(100); // Optional: Adjust as needed

            SetHits(40000);
            SetStam(800);
            SetMana(0); // Optional: Adjust if needed

            SetDamage(55, 80); // Min and Max damage

            SetSkill(SkillName.Wrestling, 175.0);
            SetSkill(SkillName.Tactics, 175.0);
            SetSkill(SkillName.Parry, 175.0);
            SetSkill(SkillName.Anatomy, 175.0);

            Fame = 2500; // Optional: Adjust based on desired difficulty
            Karma = -2500; // Optional: Adjust as needed

            VirtualArmor = 50; // Adjust based on desired difficulty
        }

        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.FrenziedWhirlwind; // Optional: Assign weapon ability if needed
        }

        public CuteMummy(Serial serial) : base(serial)
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
