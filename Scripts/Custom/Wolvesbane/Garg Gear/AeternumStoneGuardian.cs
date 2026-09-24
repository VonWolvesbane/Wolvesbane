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

    public class AeternumStoneGuardian : BaseCreature
    {
        [Constructable]
        public AeternumStoneGuardian()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Aeternum Stone Guardian";
            Title = "Keeper of the Aeternum Stone";

            // Random visual form. The chosen Body is saved by the base Mobile serialization.
            Body = Utility.RandomBool() ? 1577 : 1578;
            Hue = 0;

            // Hephastos-class core stats
            SetStr(6660);
            SetDex(6660);
            SetInt(6660);
            SetHits(666666);
            SetDamage(150, 666);

            // Mirrors Hephastos exactly
            SetDamageType(ResistanceType.Physical, 100);
            SetDamageType(ResistanceType.Cold, 100);
            SetDamageType(ResistanceType.Fire, 100);
            SetDamageType(ResistanceType.Energy, 100);
            SetDamageType(ResistanceType.Poison, 100);

            SetResistance(ResistanceType.Physical, 200);
            SetResistance(ResistanceType.Cold, 200);
            SetResistance(ResistanceType.Fire, 200);
            SetResistance(ResistanceType.Energy, 80);
            SetResistance(ResistanceType.Poison, 200);

            SetSkill(SkillName.EvalInt, 320.0);
            SetSkill(SkillName.Magery, 590.0);
            SetSkill(SkillName.Meditation, 640.0);
            SetSkill(SkillName.Poisoning, 480.0);
            SetSkill(SkillName.MagicResist, 590.0);
            SetSkill(SkillName.Tactics, 790.0);
            SetSkill(SkillName.Wrestling, 450.0);
            SetSkill(SkillName.Swords, 400.0);
            SetSkill(SkillName.Anatomy, 700.0);
            SetSkill(SkillName.Parry, 350.0);
            SetSkill(SkillName.Healing, 450.0);
        }

        public override void GenerateLoot()
        {
            // Mirror Hephastos: 10% overall chance to receive one special item.
            if (Utility.RandomDouble() >= 0.10)
                return;

            // Equal chance among the six Aeternum rewards.
            switch (Utility.Random(6))
            {
                case 0: PackItem(new AeternumStoneChest()); break;
                case 1: PackItem(new AeternumStoneArms()); break;
                case 2: PackItem(new AeternumStoneLegs()); break;
                case 3: PackItem(new AeternumStoneKilt()); break;
                case 4: PackItem(new AeternumStoneWingArmor()); break;
                case 5: PackItem(new AeternumCyclone()); break;
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

        public AeternumStoneGuardian(Serial serial)
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
