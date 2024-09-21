using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a skeletal knight corpse")]
    public class CuteSkeletalKnight : BaseCreature
    {
        [Constructable]
        public CuteSkeletalKnight() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Cute Skeletal Knight";
            Body = 0x93; // Adjust body ID as needed
            Hue = 2253; // Custom hue

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

            Fame = 10000; // Optional: Adjust based on desired difficulty
            Karma = -10000; // Optional: Adjust as needed

            VirtualArmor = 150; // Adjust based on desired difficulty
        }

        public CuteSkeletalKnight(Serial serial) : base(serial)
        {
        }

        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.CrushingBlow; // Change to an available weapon ability
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            // Here, you could add any additional effects on damage if needed
            base.OnDamage(amount, from, willKill);
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
