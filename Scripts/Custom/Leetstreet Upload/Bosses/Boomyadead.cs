using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("boom I dead")]
    public class BoomYouDead : BaseCreature
    {
        private int m_TransformationStage;
        private Timer m_AoETimer;
        public override WeaponAbility GetWeaponAbility()
        {
            if (m_TransformationStage == 0)
            {
                // Early stage abilities
                return Utility.RandomBool() ? WeaponAbility.ParalyzingBlow : WeaponAbility.ConcussionBlow;
            }
            else if (m_TransformationStage == 1)
            {
                // Second stage abilities
                return Utility.RandomBool() ? WeaponAbility.MortalStrike : WeaponAbility.BleedAttack;
            }
            else
            {
                // Final stage abilities
                return Utility.RandomBool() ? WeaponAbility.FrenziedWhirlwind : WeaponAbility.CrushingBlow;
            }
        }
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            // Cap the total incoming damage to 1000 per hit.
            int maxDamage = 1000;

            // Limit the amount of damage to the cap.
            if (amount > maxDamage)
            {
                amount = maxDamage;
            }

            // Proceed with the usual damage handling.
            base.OnDamage(amount, from, willKill);
        }
        public override bool IgnoreYoungProtection { get { return Core.ML; } }
        public override bool BardImmune { get { return !Core.SE; } }
        public override bool Unprovokable { get { return Core.SE; } }
        public override bool AreaPeaceImmune { get { return Core.SE; } }
        public override bool CanBeParagon { get { return false; } }
        public override bool AlwaysMurderer { get { return true; } }
        public override bool TeleportsTo { get { return true; } }
        public override TimeSpan TeleportDuration { get { return TimeSpan.FromSeconds(2); } }
        public override int TeleportRange { get { return 16; } }
        public override double TeleportProb { get { return 1.0; } }
        public override bool TeleportsPets { get { return false; } }

        [Constructable]
        public BoomYouDead() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.001, 0.002) // Faster AI timing
        {
            Name = "Boom You Dead";
            Body = 752; // A new body ID for uniqueness
            BaseSoundID = 0x5A1; // New sound ID

            m_TransformationStage = 0;

            SetStr(1200);
            SetDex(500); // Increased dexterity for faster actions
            SetInt(900);

            SetHits(800000); // Initial health
            SetStam(1000000);
            SetMana(500000);

            SetDamage(35, 45);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 70);
            SetResistance(ResistanceType.Cold, 90);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 85);

            SetSkill(SkillName.MagicResist, 130.0);
            SetSkill(SkillName.Tactics, 250.0);
            SetSkill(SkillName.Wrestling, 250.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 80;
        }

        public BoomYouDead(Serial serial) : base(serial)
        {
        }



        public override void OnDeath(Container c)
        {
            if (m_TransformationStage == 0)
            {
                TransformToSecondStage();
            }
            else if (m_TransformationStage == 1)
            {
                TransformToThirdStage();
            }
            else
            {
 
        DistributeBoomScrolls();
                m_AoETimer?.Stop();
                base.OnDeath(c);
            }
        }

        public void TransformToSecondStage()
        {
            m_TransformationStage = 1;
            Body = 79; // Different form ID
            BaseSoundID = 0x300; // Different sound ID

            SetStr(1800);
            SetDex(2000); // Increased dexterity
            SetInt(1500);

            SetHits(1200000);
            SetStam(1500000);
            SetMana(1200000);

            SetDamage(45, 55);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);

            PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, true, "Boom You Dead transforms!");

            Map map = this.Map;
            Point3D location = this.Location;
            Effects.SendLocationEffect(location, map, 0x3709, 30, 10, 0, 0);
            PlaySound(0x208);
            this.MoveToWorld(location, map);
        }

        public void TransformToThirdStage()
        {
            m_TransformationStage = 2;
            Body = 46; // Dragon form
            BaseSoundID = 0x16A;

            SetStr(3000);
            SetDex(4500); // Increased dexterity for the third stage
            SetInt(3000);

            SetHits(2200000);
            SetStam(4500);
            SetMana(4500);

            SetDamage(80, 100);

            PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, true, "Boom You Dead becomes unstoppable!");

            m_AoETimer = new AoETimer(this);
            m_AoETimer.Start();

            Map map = this.Map;
            Point3D location = this.Location;
            Effects.SendLocationEffect(location, map, 0x3709, 30, 10, 0, 0);
            PlaySound(0x208);
            this.MoveToWorld(location, map);
        }

        private void DistributeBoomScrolls()
        {
            // Your existing logic for distributing boom scrolls here

            // Gold explosion logic
            Map map = this.Map;

            if (map != null)
            {
                for (int x = -7; x <= 7; ++x)
                {
                    for (int y = -7; y <= 3; ++y)
                    {
                        double dist = Math.Sqrt(x * x + y * y);
                        if (dist <= 12)
                        {
                            new GoodiesTimer(map, this.X + x, this.Y + y).Start();
                        }
                    }
                }
            }
        }

        // Keep the GoodiesTimer class as it is
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

                Gold g = new Gold(60000, 60000);
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

        private class AoETimer : Timer
        {
            private BoomYouDead m_Harrower;

            public AoETimer(BoomYouDead harrower) : base(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3))
            {
                m_Harrower = harrower;
            }

            protected override void OnTick()
            {
                Map map = m_Harrower.Map;

                if (map == null)
                    return;

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in m_Harrower.GetMobilesInRange(5))
                {
                    if (m != m_Harrower && m.Player && m.Alive)
                    {
                        targets.Add(m);
                    }
                }

                foreach (Mobile m in targets)
                {
                    double percentage = 0.5;
                    int damage = (int)(m.Hits * percentage);
                    m_Harrower.DoHarmful(m);
                    m.Damage(damage, m_Harrower);
                    m.SendMessage("Boom! Shots Fired");
                }

                Effects.PlaySound(m_Harrower.Location, map, 0x208);
                Effects.SendLocationEffect(m_Harrower.Location, map, 0x3709, 30, 10, 0, 0);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 4);
            AddLoot(LootPack.Gems, 5);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_TransformationStage);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_TransformationStage = reader.ReadInt();
        }
    }
}
