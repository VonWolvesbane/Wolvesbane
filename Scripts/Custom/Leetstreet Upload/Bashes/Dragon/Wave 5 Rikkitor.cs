using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;

namespace Server.Mobiles
{
    public class FireSnuggles : BaseChampion
    {
        [Constructable]
        public FireSnuggles()
            : base(AIType.AI_Mage)
        {
            Body = 172;
            Name = "Fire Snuggles";
            Hue = 2801;

            SetStr(1500);
            SetDex(2000);
            SetInt(2000);

            SetHits(1500000);
            SetStam(500000);

            SetDamage(200, 200);

            SetDamageType(ResistanceType.Physical, 100);
            SetDamageType(ResistanceType.Fire, 75);
            SetDamageType(ResistanceType.Energy, 75);
            SetDamageType(ResistanceType.Poison, 75);
            SetDamageType(ResistanceType.Cold, 75);

            SetResistance(ResistanceType.Physical, 98);
            SetResistance(ResistanceType.Fire, 98);
            SetResistance(ResistanceType.Cold, 98);
            SetResistance(ResistanceType.Poison, 98);
            SetResistance(ResistanceType.Energy, 98);

            SetSkill(SkillName.Magery, 200);
            SetSkill(SkillName.Meditation, 200);
            SetSkill(SkillName.EvalInt, 250);
            SetSkill(SkillName.Focus, 250);
            SetSkill(SkillName.Necromancy, 400);
            SetSkill(SkillName.SpiritSpeak, 400);
            SetSkill(SkillName.MagicResist, 200);
            SetSkill(SkillName.Tactics, 200);
            SetSkill(SkillName.Wrestling, 400);
            SetSkill(SkillName.Anatomy, 400);
            SetSkill(SkillName.DetectHidden, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 130;
        }

        public FireSnuggles(Serial serial)
            : base(serial)
        {
        }
        public override bool IgnoreYoungProtection { get { return Core.ML; } }
        public override bool BardImmune { get { return !Core.SE; } }
        public override bool Unprovokable { get { return Core.SE; } }
        public override bool AreaPeaceImmune { get { return Core.SE; } }
        public override bool HasBreath { get { return true; } }
        public override double BonusPetDamageScalar { get { return (Core.SE) ? 100.0 : 1.0; } }
        public override bool AlwaysMurderer { get { return true; } }

        public override ChampionSkullType SkullType
        {
            get
            {
                return ChampionSkullType.Power;
            }
        }
        public override Type[] UniqueList
        {
            get
            {
                return new Type[] { typeof(CrownOfTalKeesh) };
            }
        }
        public override Type[] SharedList
        {
            get
            {
                return new Type[]
                {
                    typeof(TheMostKnowledgePerson),
                    typeof(BraveKnightOfTheBritannia),
                    typeof(LieutenantOfTheBritannianRoyalGuard)
                };
            }
        }
        public override Type[] DecorativeList
        {
            get
            {
                return new Type[]
                {
                    typeof(LavaTile),
                    typeof(MonsterStatuette),
                    typeof(MonsterStatuette)
                };
            }
        }
        public override MonsterStatuetteType[] StatueTypes
        {
            get
            {
                return new MonsterStatuetteType[]
                {
                    MonsterStatuetteType.OphidianArchMage,
                    MonsterStatuetteType.OphidianWarrior
                };
            }
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.2 >= Utility.RandomDouble())
                this.Earthquake();
        }

        public void Earthquake()
        {
            Map map = this.Map;

            if (map == null)
                return;

            ArrayList targets = new ArrayList();

            IPooledEnumerable eable = GetMobilesInRange(8);

            foreach (Mobile m in eable)
            {
                if (m == this || !this.CanBeHarmful(m))
                    continue;

                if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
                    targets.Add(m);
                else if (m.Player)
                    targets.Add(m);
            }

            eable.Free();

            this.PlaySound(0x2F3);

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile m = (Mobile)targets[i];

                double damage = m.Hits * 0.6;

                if (damage < 10.0)
                    damage = 200.0;
                else if (damage >= 10.0)
                    damage = 500.0;

                this.DoHarmful(m);

                AOS.Damage(m, this, (int)damage, 100, 0, 0, 0, 0);

                if (m.Alive && m.Body.IsHuman && !m.Mounted)
                    m.Animate(20, 7, 1, true, false, 0); // take hit
            }
        }

        public override int GetAngerSound()
        {
            return Utility.Random(0x2CE, 2);
        }

        public override int GetIdleSound()
        {
            return 0x2D2;
        }

        public override int GetAttackSound()
        {
            return Utility.Random(0x2C7, 5);
        }

        public override int GetHurtSound()
        {
            return 0x2D1;
        }

        public override int GetDeathSound()
        {
            return 0x2CC;
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
