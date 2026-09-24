using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    /*
     * Wolvesbane Gargoyle Gear Bosses
     *
     * ObsidianWingGuardian
     *   - Body 1579
     *   - Approximately 70% of Hephastos core stats/skills/resists/damage
     *   - 10% overall chance to drop exactly one Obsidian item
     *   - If the 10% roll succeeds, all six possible drops are equally likely
     *
     * AeternumStoneGuardian
     *   - Body randomly chosen between 1577 and 1578 when first created
     *   - Hephastos-class stats/skills/resists/damage
     *   - 10% overall chance to drop exactly one Aeternum item
     *   - If the 10% roll succeeds, all six possible drops are equally likely
     *
     * Both bosses retain Hephastos-style boss protections:
     * AutoDispel, BardImmune, Unprovokable, Lethal hit poison,
     * AlwaysMurderer, and immunity to controlled/provoked creature damage.
     */

    public class ObsidianWingGuardian : BaseCreature
    {
        [Constructable]
        public ObsidianWingGuardian()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Obsidian Wing Guardian";
            Title = "Keeper of the Obsidian Wing";
            Body = 1579;
            Hue = 0;

            // ~70% of Hephastos
            SetStr(4662);
            SetDex(4662);
            SetInt(4662);
            SetHits(466666);
            SetDamage(105, 466);

            // Mirrors Hephastos' damage-type configuration
            SetDamageType(ResistanceType.Physical, 100);
            SetDamageType(ResistanceType.Cold, 100);
            SetDamageType(ResistanceType.Fire, 100);
            SetDamageType(ResistanceType.Energy, 100);
            SetDamageType(ResistanceType.Poison, 100);

            // 70% of Hephastos resistances
            SetResistance(ResistanceType.Physical, 140);
            SetResistance(ResistanceType.Cold, 140);
            SetResistance(ResistanceType.Fire, 140);
            SetResistance(ResistanceType.Energy, 56);
            SetResistance(ResistanceType.Poison, 140);

            // 70% of Hephastos skills
            SetSkill(SkillName.EvalInt, 224.0);
            SetSkill(SkillName.Magery, 413.0);
            SetSkill(SkillName.Meditation, 448.0);
            SetSkill(SkillName.Poisoning, 336.0);
            SetSkill(SkillName.MagicResist, 413.0);
            SetSkill(SkillName.Tactics, 553.0);
            SetSkill(SkillName.Wrestling, 315.0);
            SetSkill(SkillName.Swords, 280.0);
            SetSkill(SkillName.Anatomy, 490.0);
            SetSkill(SkillName.Parry, 245.0);
            SetSkill(SkillName.Healing, 315.0);
        }

        public override void GenerateLoot()
        {
            // Mirror Hephastos: 10% overall chance to receive one special item.
            if (Utility.RandomDouble() >= 0.10)
                return;

            // Equal chance among the six Obsidian rewards.
            switch (Utility.Random(6))
            {
                case 0: PackItem(new ObsidianWingChest()); break;
                case 1: PackItem(new ObsidianWingArms()); break;
                case 2: PackItem(new ObsidianWingLegs()); break;
                case 3: PackItem(new ObsidianWingKilt()); break;
                case 4: PackItem(new ObsidianWingArmor()); break;
                case 5: PackItem(new ObsidianBoomerang()); break;
            }
        }

        public override bool AutoDispel { get { return true; } }
        public override bool BardImmune { get { return true; } }
        public override bool Unprovokable { get { return true; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }
        public override bool AlwaysMurderer { get { return true; } }

        public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
        {
            if (from is BaseCreature)
            {
                BaseCreature bc = (BaseCreature)from;

                if (bc.Controlled || bc.BardTarget == this)
                    damage = 0;
            }
        }

        public ObsidianWingGuardian(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
