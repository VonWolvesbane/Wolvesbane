using System;

namespace Server.Mobiles
{
    [CorpseName("a weiner dragon corpse")]
    public class WeinerDragontwo : BaseCreature
    {
        [Constructable]
        public WeinerDragontwo()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a weiner dragon";
            Body = 103;
            BaseSoundID = 362;
            Hue = Utility.RandomBool() ? 2400 : 2801;

            SetStr(650);
            SetDex(650);
            SetInt(650);
            SetStam(80000);

            SetHits(80000);

            SetDamage(32, 60);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Poison, 25);

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

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 36;

            if (Core.ML && Utility.RandomDouble() < .33)
                PackItem(Engines.Plants.Seed.RandomPeculiarSeed(3));

            Tamable = false;
            ControlSlots = 3;
            MinTameSkill = 108.0;
        }

        public WeinerDragontwo(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override bool HasBreath { get { return true; } } // fire breath enabled

        public override double BonusPetDamageScalar { get { return Controlled ? 1.0 : (Core.SE) ? 3.0 : 1.0; } }
        public override bool AutoDispel { get { return !Controlled; } }
        public override HideType HideType { get { return HideType.Barbed; } }
        public override int Hides { get { return 20; } }
        public override int Meat { get { return 19; } }
        public override int Scales { get { return 6; } }

        public override ScaleType ScaleType
        {
            get
            {
                return (Utility.RandomBool() ? ScaleType.Black : ScaleType.White);
            }
        }
        public override int TreasureMapLevel { get { return 4; } }
        public override bool CanAngerOnTame { get { return true; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 2);
        }

        public override int GetIdleSound()
        {
            return 0x2C4;
        }

        public override int GetAttackSound()
        {
            return 0x2C0;
        }

        public override int GetDeathSound()
        {
            return 0x2C1;
        }

        public override int GetAngerSound()
        {
            return 0x2C4;
        }

        public override int GetHurtSound()
        {
            return 0x2C3;
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (!Core.SE && 0.2 > Utility.RandomDouble() && attacker is BaseCreature)
            {
                BaseCreature c = (BaseCreature)attacker;

                if (c.Controlled && c.ControlMaster != null)
                {
                    c.ControlTarget = c.ControlMaster;
                    c.ControlOrder = OrderType.Attack;
                    c.Combatant = c.ControlMaster;
                }
            }
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