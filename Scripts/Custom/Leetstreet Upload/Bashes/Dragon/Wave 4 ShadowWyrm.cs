using System;

namespace Server.Mobiles
{
    [CorpseName("an ethie wyrm corpse")]
    public class EthieWyrm : BaseCreature
    {
        [Constructable]
        public EthieWyrm()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.1, 0.4)
        {
            Name = "an ethie wyrm";
            Body = 106;
            BaseSoundID = 362;

            SetStr(1000);
            SetDex(1000);
            SetInt(4000);
            SetMana(30000);

            SetHits(200000);
            SetStam(200000);
            SetDamage(150, 150);

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

            VirtualArmor = 70;

            Tamable = false;
            ControlSlots = 5;
            MinTameSkill = 105.0;
        }

        public EthieWyrm(Serial serial)
            : base(serial)
        {
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override bool HasBreath { get { return true; } } // fire breath enabled
        public override bool AutoDispel { get { return !Controlled; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override Poison HitPoison { get { return Poison.Deadly; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override int Meat { get { return 19; } }
        public override int Hides { get { return 20; } }
        public override int Scales { get { return 10; } }
        public override ScaleType ScaleType { get { return ScaleType.Black; } }
        public override HideType HideType { get { return HideType.Barbed; } }
        public override bool CanFly { get { return true; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.Gems, 5);
        }

        public override int GetIdleSound()
        {
            return 0x2D5;
        }

        public override int GetHurtSound()
        {
            return 0x2D1;
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
