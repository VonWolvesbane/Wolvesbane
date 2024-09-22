using System;

namespace Server.Mobiles
{
    [CorpseName("a majestic wyrm corpse")]
    public class MajesticWyrm : BaseCreature
    {
        public override double AverageThreshold { get { return 0.25; } }

        [Constructable]
        public MajesticWyrm()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.4)
        {
            Body = Utility.RandomBool() ? 180 : 49;
            Name = "a majestic wyrm";
            BaseSoundID = 362;
            Hue = Utility.RandomBool() ? 2400 : 2801;


            SetStr(800);
            SetDex(800);
            SetInt(2000);
            SetMana(30000);

            SetHits(120000);
            SetStam(120000);
            SetDamage(75, 75);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 60);
            SetResistance(ResistanceType.Fire, 60);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 60);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.EvalInt, 200);
            SetSkill(SkillName.Magery, 200);
            SetSkill(SkillName.Meditation, 200);
            SetSkill(SkillName.MagicResist, 200);
            SetSkill(SkillName.Tactics, 200);
            SetSkill(SkillName.Wrestling, 400);
            SetSkill(SkillName.DetectHidden, 100.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 64;

            Tamable = false;
            ControlSlots = 3;
            MinTameSkill = 96.3;
        }

        public MajesticWyrm(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement
        {
            get
            {
                return true;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 4;
            }
        }

        public override bool CanAngerOnTame
        {
            get
            {
                return true;
            }
        }
        public override bool CanFly
        {
            get
            {
                return true;
            }
        }
        public override bool HasBreath { get { return true; } }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems, Utility.Random(1, 5));
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