using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cute shade corpse")]
    public class CuteShade : BaseCreature
    {
        [Constructable]
        public CuteShade() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Cute Shade";
            Body = 0x1A; // Body ID for a shade
            Hue = 2253; // Custom hue

            SetStr(300);
            SetDex(300);
            SetInt(100); // Optional: Adjust as needed

            SetHits(20000);
            SetStam(300);
            SetMana(0); // Optional: Adjust if needed

            SetDamage(25, 50); // Min and Max damage

            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Parry, 100.0);
            SetSkill(SkillName.Anatomy, 100.0);

            Fame = 1000;
            Karma = 0;

            VirtualArmor = 30; // Adjust based on desired difficulty
        }

        public CuteShade(Serial serial) : base(serial)
        {
        }

        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.BleedAttack; // Use BleedAttack ability
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
