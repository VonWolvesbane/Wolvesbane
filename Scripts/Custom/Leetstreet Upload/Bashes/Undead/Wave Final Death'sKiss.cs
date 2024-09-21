using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("dead death's kiss")]
    public class DeathsKiss : BaseCreature
    {
        [Constructable]
        public DeathsKiss() : base(AIType.AI_NecroMage, FightMode.Weakest, 10, 1, 0.01, 0.02)
        {
            Name = "Death's Kiss";
            Body = 0x18; // Skeleton dragon body ID
            Hue = 1153; // Custom hue

            SetStr(1100);
            SetDex(1200);
            SetInt(4000); // High intelligence for magic
            SetHits(2500000); // Total hits
            SetStam(20000); // Stamina

            SetDamage(75, 110); // Min and Max damage

            SetDamageType(ResistanceType.Physical, 100);
            SetDamageType(ResistanceType.Fire, 100);

            SetResistance(ResistanceType.Physical, 80, 80);
            SetResistance(ResistanceType.Fire, 80, 80);
            SetResistance(ResistanceType.Cold, 80, 80);
            SetResistance(ResistanceType.Poison, 80, 80);
            SetResistance(ResistanceType.Energy, 80, 80);

            SetSkill(SkillName.EvalInt, 250.0);
            SetSkill(SkillName.Magery, 250.0);
            SetSkill(SkillName.MagicResist, 250.0);
            SetSkill(SkillName.Tactics, 250.0);
            SetSkill(SkillName.Wrestling, 400.0);
            SetSkill(SkillName.Necromancy, 500.0);
            SetSkill(SkillName.SpiritSpeak, 500.0);
            SetSkill(SkillName.Parry, 300.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80;
        }

        public DeathsKiss(Serial serial) : base(serial)
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
        public override int Hides { get { return 20; } }
        public override int Meat { get { return 19; } } // where's it hiding these? :)
        public override HideType HideType { get { return HideType.Barbed; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override TribeType Tribe { get { return TribeType.Undead; } }

        public override WeaponAbility GetWeaponAbility()
        {
            // Randomly select one of the two abilities
            int ability = Utility.Random(2);
            if (ability == 0)
                return WeaponAbility.BleedAttack; // First weapon ability
            else
                return WeaponAbility.ConcussionBlow; // Second weapon ability
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 4);
            AddLoot(LootPack.Gems, 5);
        }

        public override bool OnBeforeDeath()
        {
            this.Hue = 16385;

            if (!this.NoKillAwards)
            {
                Map map = this.Map;

                if (map != null)
                {
                    for (int x = -7; x <= 7; ++x)
                    {
                        for (int y = -7; y <= 3; ++y)
                        {
                            double dist = Math.Sqrt(x * x + y * y);

                            if (dist <= 12)
                                new GoodiesTimer(map, this.X + x, this.Y + y).Start();
                        }
                    }
                }
            }

            return base.OnBeforeDeath();
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.6)
                c.DropItem(new ParrotItem());

            if (Utility.RandomDouble() < 0.025)
                c.DropItem(new CrimsonCincture());
        }

        private class GoodiesTimer : Timer
        {
            private readonly Map m_Map;
            private readonly int m_X;
            private readonly int m_Y;

            public GoodiesTimer(Map map, int x, int y) : base(TimeSpan.FromSeconds(Utility.RandomDouble() * 10.0))
            {
                this.m_Map = map;
                this.m_X = x;
                this.m_Y = y;
            }

            protected override void OnTick()
            {
                int z = this.m_Map.GetAverageZ(this.m_X, this.m_Y);
                bool canFit = this.m_Map.CanFit(this.m_X, this.m_Y, z, 6, false, false);

                for (int i = -3; !canFit && i <= 3; ++i)
                {
                    canFit = this.m_Map.CanFit(this.m_X, this.m_Y, z + i, 6, false, false);

                    if (canFit)
                        z += i;
                }

                if (!canFit)
                    return;

                Gold g = new Gold(10000, 60000);
                g.MoveToWorld(new Point3D(this.m_X, this.m_Y, z), this.m_Map);

                if (0.5 >= Utility.RandomDouble())
                {
                    switch (Utility.Random(3))
                    {
                        case 0: // Fire column
                            Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, 5052);
                            Effects.PlaySound(g, g.Map, 0x208);
                            break;

                        case 1: // Explosion
                            Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
                            Effects.PlaySound(g, g.Map, 0x307);
                            break;

                        case 2: // Ball of fire
                            Effects.SendLocationParticles(EffectItem.Create(g.Location, g.Map, EffectItem.DefaultDuration), 0x36FE, 10, 10, 5052);
                            break;
                    }
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
