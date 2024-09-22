using System;

namespace Server.Mobiles
{
    [CorpseName("a dead dead dragon corpse")]
    public class DeadDragon : BaseCreature
    {
        [Constructable]
        public DeadDragon()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dead dragon";
            Body = 104;
            BaseSoundID = 0x488;
            Hue = Utility.RandomBool() ? 2400 : 2801;

            SetStr(1000);
            SetDex(1000);
            SetInt(4000);
            SetMana(30000);

            SetHits(200000);
            SetStam(200000);
            SetDamage(100, 100);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Fire, 25);

            SetResistance(ResistanceType.Physical, 70);
            SetResistance(ResistanceType.Fire, 70);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 70);
            SetResistance(ResistanceType.Energy, 70);

   
            SetSkill(SkillName.Magery, 200);
            SetSkill(SkillName.Meditation, 200);
            SetSkill(SkillName.EvalInt, 200);
            SetSkill(SkillName.Necromancy, 400);
            SetSkill(SkillName.SpiritSpeak, 400);
            SetSkill(SkillName.MagicResist, 200);
            SetSkill(SkillName.Tactics, 200);
            SetSkill(SkillName.Wrestling, 400);
            SetSkill(SkillName.Anatomy, 400);
            SetSkill(SkillName.DetectHidden, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80;
        }

        public DeadDragon(Serial serial)
            : base(serial)
        {
        }

        public override bool AutoDispel { get { return !Controlled; } }
        public override bool BleedImmune { get { return true; } }
        public override bool HasBreath { get { return true; } } // fire breath enabled
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override double BonusPetDamageScalar { get { return (Core.SE) ? 3.0 : 1.0; } }
        public override int BreathFireDamage { get { return 0; } }
        public override int BreathColdDamage { get { return 100; } }
        public override int BreathEffectHue { get { return 0x480; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override TribeType Tribe { get { return TribeType.Undead; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 4);
            AddLoot(LootPack.Gems, 5);
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
