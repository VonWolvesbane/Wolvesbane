using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cute rotting corpse")]
    public class CuteRottingCorpse : BaseCreature
    {
        [Constructable]
        public CuteRottingCorpse() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Cute Rotting Corpse";
            Body = 0x9B; // Corpse-like body ID
            Hue = 1153; // Custom hue

            SetStr(1000);
            SetDex(1200);
            SetInt(4000); // High intelligence for magic
            SetHits(250000); // Total hits
            SetStam(2000); // Stamina

            SetDamage(75, 110); // Min and Max damage

            SetSkill(SkillName.Wrestling, 250.0);
            SetSkill(SkillName.Tactics, 250.0);
            SetSkill(SkillName.Parry, 250.0);
            SetSkill(SkillName.Anatomy, 250.0);
            SetSkill(SkillName.Magery, 500.0); // Strong magic capability
            SetSkill(SkillName.Swords, 200.0); // New weapon skill
            SetSkill(SkillName.Macing, 200.0); // New weapon skill
            SetSkill(SkillName.Fencing, 200.0); // New weapon skill

            Fame = 5000; // Optional: Adjust based on desired difficulty
            Karma = -5000; // Optional: Adjust as needed

            VirtualArmor = 100; // Adjust based on desired difficulty
        }

        public CuteRottingCorpse(Serial serial) : base(serial)
        {
        }

        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.BleedAttack; // Attach BleedAttack weapon ability
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
