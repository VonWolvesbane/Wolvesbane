using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a harrower corpse")]
    public class UltraHarrower : BaseCreature
    {
        private int m_TransformationStage;
        private Timer m_AoETimer;

        [Constructable]
        public UltraHarrower() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.02, 0.04)
        {
            Name = "The Ultra Harrower";
            Body = 146; // initial form body ID
            BaseSoundID = 0x165; // initial form sound ID

            m_TransformationStage = 0;

            SetStr(1000);
            SetDex(150);
            SetInt(1000);

            SetHits(1000000); // initial health points
            SetStam(1000000);
            SetMana(1000000);

            SetDamage(40, 50);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 300.0);
            SetSkill(SkillName.Wrestling, 300.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 75;
        }

        public UltraHarrower(Serial serial) : base(serial)
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
                DistributePowerScrolls();
                m_AoETimer?.Stop();
                base.OnDeath(c);
            }
        }

        public void TransformToSecondStage()
        {
            m_TransformationStage = 1;
            Body = 0x2B; // second form body ID
            BaseSoundID = 0x300; // second form sound ID

            SetStr(1500);
            SetDex(1500);
            SetInt(1500);

            SetHits(1500000); // second form health points
            SetStam(1500000);
            SetMana(1500000);

            SetDamage(50, 60);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            SetSkill(SkillName.MagicResist, 200.0);
            SetSkill(SkillName.Tactics, 400.0);
            SetSkill(SkillName.Wrestling, 400.0);

            VirtualArmor = 85;

            PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, true, "The Ultra Harrower transforms!");

            // Resurrect the creature with new stats
            Map map = this.Map;
            Point3D location = this.Location;
            Effects.SendLocationEffect(location, map, 0x3709, 30, 10, 0, 0);
            PlaySound(0x208);
            this.MoveToWorld(location, map);
        }

        public void TransformToThirdStage()
        {
            m_TransformationStage = 2;
            Body = 0x4E0;
            BaseSoundID = 0x16A;

            SetStr(2500);
            SetDex(4500);
            SetInt(2500);

            SetHits(2000000);
            SetStam(4500);
            SetMana(4500);

            SetDamage(250, 300);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            SetSkill(SkillName.MagicResist, 400.0);
            SetSkill(SkillName.Tactics, 500.0);
            SetSkill(SkillName.Wrestling, 500.0);

            VirtualArmor = 95;

            PublicOverheadMessage(Network.MessageType.Regular, 0x3B2, true, "The Ultra Harrower transforms!");

            // Start AoE attack timer
            m_AoETimer = new AoETimer(this);
            m_AoETimer.Start();

            // Resurrect the creature with new stats
            Map map = this.Map;
            Point3D location = this.Location;
            Effects.SendLocationEffect(location, map, 0x3709, 30, 10, 0, 0);
            PlaySound(0x208);
            this.MoveToWorld(location, map);
        }
        public override bool IgnoreYoungProtection { get { return Core.ML; } }
        public override bool BardImmune { get { return !Core.SE; } }
        public override bool Unprovokable { get { return Core.SE; } }
        public override bool AreaPeaceImmune { get { return Core.SE; } }
        public override bool HasBreath { get { return true; } }
        public override double BonusPetDamageScalar { get { return (Core.SE) ? 100.0 : 1.0; } }
        public override bool AlwaysMurderer { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }

        private void DistributePowerScrolls()
        {
            List<Mobile> toGive = new List<Mobile>();

            // Add all valid aggressors
            foreach (AggressorInfo info in Aggressors)
            {
                if (info.Attacker.Player && info.Attacker.Alive && !toGive.Contains(info.Attacker))
                    toGive.Add(info.Attacker);
            }

            // Add all valid aggressed
            foreach (AggressorInfo info in Aggressed)
            {
                if (info.Defender.Player && info.Defender.Alive && !toGive.Contains(info.Defender))
                    toGive.Add(info.Defender);
            }

            if (toGive.Count == 0)
                return;

            foreach (Mobile m in toGive)
            {
                for (int i = 0; i < 2; ++i) // Giving 2 Power Scrolls to each player
                {
                    int level;
                    double random = Utility.RandomDouble();

                    if (0.05 >= random)
                        level = 150;
                    else if (0.10 >= random)
                        level = 145;
                    else if (0.15 >= random)
                        level = 140;
                    else if (0.20 >= random)
                        level = 135;
                    else if (0.25 >= random)
                        level = 130;
                    else if (0.30 >= random)
                        level = 105;
                    else if (0.35 >= random)
                        level = 125;
                    else if (0.45 >= random)
                        level = 110;
                    else if (0.55 >= random)
                        level = 120;
                    else
                        level = 115;

                    switch (Utility.Random(32)) // Updated to 32 skills
                    {
                        case 0: m.AddToBackpack(new PowerScroll(SkillName.Swords, level)); break;
                        case 1: m.AddToBackpack(new PowerScroll(SkillName.Fencing, level)); break;
                        case 2: m.AddToBackpack(new PowerScroll(SkillName.Macing, level)); break;
                        case 3: m.AddToBackpack(new PowerScroll(SkillName.Archery, level)); break;
                        case 4: m.AddToBackpack(new PowerScroll(SkillName.Wrestling, level)); break;
                        case 5: m.AddToBackpack(new PowerScroll(SkillName.Parry, level)); break;
                        case 6: m.AddToBackpack(new PowerScroll(SkillName.Tactics, level)); break;
                        case 7: m.AddToBackpack(new PowerScroll(SkillName.Anatomy, level)); break;
                        case 8: m.AddToBackpack(new PowerScroll(SkillName.Healing, level)); break;
                        case 9: m.AddToBackpack(new PowerScroll(SkillName.Magery, level)); break;
                        case 10: m.AddToBackpack(new PowerScroll(SkillName.Meditation, level)); break;
                        case 11: m.AddToBackpack(new PowerScroll(SkillName.EvalInt, level)); break;
                        case 12: m.AddToBackpack(new PowerScroll(SkillName.MagicResist, level)); break;
                        case 13: m.AddToBackpack(new PowerScroll(SkillName.AnimalTaming, level)); break;
                        case 14: m.AddToBackpack(new PowerScroll(SkillName.AnimalLore, level)); break;
                        case 15: m.AddToBackpack(new PowerScroll(SkillName.Veterinary, level)); break;
                        case 16: m.AddToBackpack(new PowerScroll(SkillName.Musicianship, level)); break;
                        case 17: m.AddToBackpack(new PowerScroll(SkillName.Provocation, level)); break;
                        case 18: m.AddToBackpack(new PowerScroll(SkillName.Discordance, level)); break;
                        case 19: m.AddToBackpack(new PowerScroll(SkillName.Peacemaking, level)); break;
                        case 20: m.AddToBackpack(new PowerScroll(SkillName.Chivalry, level)); break;
                        case 21: m.AddToBackpack(new PowerScroll(SkillName.Necromancy, level)); break;
                        case 22: m.AddToBackpack(new PowerScroll(SkillName.Bushido, level)); break;
                        case 23: m.AddToBackpack(new PowerScroll(SkillName.Ninjitsu, level)); break;
                        case 24: m.AddToBackpack(new PowerScroll(SkillName.Spellweaving, level)); break;
                        case 25: m.AddToBackpack(new PowerScroll(SkillName.Mysticism, level)); break;
                        case 26: m.AddToBackpack(new PowerScroll(SkillName.Imbuing, level)); break;
                        case 27: m.AddToBackpack(new PowerScroll(SkillName.Throwing, level)); break;
                        case 28: m.AddToBackpack(new PowerScroll(SkillName.Focus, level)); break;
                        case 29: m.AddToBackpack(new PowerScroll(SkillName.Begging, level)); break;
                        case 30: m.AddToBackpack(new PowerScroll(SkillName.Inscribe, level)); break;
                        case 31: m.AddToBackpack(new PowerScroll(SkillName.RemoveTrap, level)); break;
                    }
                }
            }
        }

        private class AoETimer : Timer
        {
            private UltraHarrower m_Harrower;

            public AoETimer(UltraHarrower harrower) : base(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5))
            {
                m_Harrower = harrower;
            }

            protected override void OnTick()
            {
                Map map = m_Harrower.Map;

                if (map == null)
                    return;

                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in m_Harrower.GetMobilesInRange(10))
                {
                    if (m != m_Harrower && m.Player && m.Alive)
                    {
                        targets.Add(m);
                    }
                }

                foreach (Mobile m in targets)
                {
                    int damage = Utility.RandomMinMax(50, 100);
                    m_Harrower.DoHarmful(m);
                    AOS.Damage(m, m_Harrower, damage, 100, 0, 0, 0, 0);
                    m.SendMessage("You are struck by a powerful force!");
                }

                Effects.PlaySound(m_Harrower.Location, map, 0x208);
                Effects.SendLocationEffect(m_Harrower.Location, map, 0x3709, 30, 10, 0, 0);
            }
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
