using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Eighth;

namespace LadyJ
{
    public class LadyJ : BaseCreature
    {
        private readonly TimeSpan _waveInterval = TimeSpan.FromSeconds(2);
        private readonly int _waveDamage = 200000;
        private readonly int _waveCount = 8;
        private readonly int _waveDelay = 2000;

        private DateTime _nextWaveTime;

        [Constructable]
        public LadyJ() : base(AIType.AI_Mage, FightMode.Closest, 15, 1, 0.01, 0.02)
        {
            Name = "J";
            Title = "the Paralyzer";
            Body = 606;
            this.Female = true;
            Hue = 1037;

            SetStr(800);
            SetDex(1500);
            SetInt(30000);

            SetHits(1000000);

            SetDamage(50, 50);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            SetSkill(SkillName.Magery, 500.0);
            SetSkill(SkillName.Macing, 500.0);
            SetSkill(SkillName.EvalInt, 400.0);
            SetSkill(SkillName.Meditation, 10000.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.Macing, 100.0);
            SetSkill(SkillName.Focus, 10000.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 50;

            FloppyHat hat = new FloppyHat();
            hat.Movable = false;
            hat.Hue = 2087;
            AddItem(hat);

            PlainDress dress = new PlainDress();
            dress.Movable = false;
            dress.Hue = 2087;
            AddItem(dress);

            Boots chaosBoots = new Boots();
            chaosBoots.ItemID = 9903;
            chaosBoots.Movable = false;
            chaosBoots.Hue = 2087;
            AddItem(chaosBoots);

            Shirt steveShirt = new Shirt();
            steveShirt.ItemID = 7933;
            steveShirt.Movable = false;
            steveShirt.Hue = 2087;
            AddItem(steveShirt);

            WildStaff wildStaff = new WildStaff();
            wildStaff.ItemID = 11557;
            wildStaff.Movable = false;
            wildStaff.Hue = 2087;
            wildStaff.Attributes.SpellDamage = 5000;
            AddItem(wildStaff);

            Item hair = new Item(41397);
            hair.Movable = false;
            hair.Hue = 1153;
            hair.Layer = Layer.Hair;
            AddItem(hair);

            Item ring = new Item(4234);
            ring.Movable = false;
            ring.Hue = 0;
            ring.Layer = Layer.Ring;
            AddItem(ring);

            Item bracelet = new Item(4230);
            bracelet.Movable = false;
            bracelet.Hue = 2087;
            bracelet.Layer = Layer.Bracelet;
            AddItem(bracelet);

            Item earrings = new Item(7943);
            earrings.Movable = false;
            earrings.Hue = 1154;
            earrings.Layer = Layer.Earrings;
            AddItem(earrings);

            //Halberd halberd = new Halberd();
            //halberd.Movable = false;
            //EquipItem(halberd);

            LightSource light = new LightSource();
            light.Light = LightType.Circle300;
            light.Hue = 0x1F; // Blue color
            AddItem(light);

            _nextWaveTime = DateTime.UtcNow + _waveInterval;

            PackGold(1000, 2000);
            AddLoot(LootPack.FilthyRich, 2);
        }

        public override bool AutoDispel { get { return true; } }
        public override bool BardImmune { get { return true; } }
        public override bool Unprovokable { get { return true; } }
        public override bool Uncalmable { get { return true; } }
        public override bool AreaPeaceImmune { get { return true; } }
        public override bool BleedImmune { get { return true; } }
        public override bool ShowSpellMantra { get { return true; } }
        public override bool FreezeOnCast { get { return false; } }

        public override bool ReduceSpeedWithDamage { get { return false; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }
        public override bool AlwaysMurderer { get { return true; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
        }

        public override void OnThink()
        {
            if (DateTime.UtcNow >= _nextWaveTime)
            {
                CastParalysisWave();
                _nextWaveTime = DateTime.UtcNow + _waveInterval;
            }

            base.OnThink();
        }

        private void CastParalysisWave()
        {
            Map map = Map;
            if (map == null)
                return;

            for (int i = 0; i < _waveCount; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * _waveDelay), index =>
                {
                    double angle = Utility.RandomDouble() * Math.PI * 2;
                    Point3D startLoc = new Point3D(X + (int)(Math.Cos(angle) * 2), Y + (int)(Math.Sin(angle) * 2), Z);
                    ParalysisWave wave = new ParalysisWave(this, _waveDamage, startLoc, angle, map);
                    wave.MoveToWorld(startLoc, map);
                }, i);
            }
        }

        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != Map || !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            if (Utility.RandomDouble() < 0.85)
            {
                DoHarmful(combatant);
                Spell spell = GetRandomSpell();
                if (spell != null)
                    spell.Cast();
            }
            else
            {
                DoHarmful(combatant);
                Halberd weapon = FindItemOnLayer(Layer.TwoHanded) as Halberd;
                if (weapon != null)
                    weapon.OnSwing(this, combatant);
            }

            base.OnActionCombat();
        }

        private Spell GetRandomSpell()
        {
            int maxCircle = (int)(Skills[SkillName.Magery].Value / 10);
            switch (Utility.Random(maxCircle))
            {
                case 0:
                    return new MagicArrowSpell(this, null);
                case 1:
                    return new HarmSpell(this, null);
                case 2:
                    return new FireballSpell(this, null);
                case 3:
                    return new LightningSpell(this, null);
                case 4:
                    return new MindBlastSpell(this, null);
                case 5:
                    return new ParalyzeSpell(this, null);
                case 6:
                    return new EnergyBoltSpell(this, null);
                case 7:
                    return new ExplosionSpell(this, null);
                default:
                    return null;
            }
        }

        public LadyJ(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class LightSource : Item
    {
        [Constructable]
        public LightSource() : base(0x1647)
        {
            Movable = false;
            Layer = Layer.Waist;
        }

        public LightSource(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class ParalysisWave : Item
    {
        private readonly Mobile _caster;
        private readonly int _damage;
        private readonly double _initialAngle;
        private readonly Map _map;
        private double _currentAngle;

        public ParalysisWave(Mobile caster, int damage, Point3D loc, double angle, Map map)
            : base(0x3967)
        {
            Movable = false;
            _caster = caster;
            _damage = damage;
            _initialAngle = angle;
            _currentAngle = angle;
            _map = map;

            Timer.DelayCall(TimeSpan.FromMilliseconds(100), MoveWave);
            Timer.DelayCall(TimeSpan.FromSeconds(2), Delete);

            ItemID = 0x3967;
            Hue = 2087; // Blue color
        }

        private void MoveWave()
        {
            if (_map == null || _map == Map.Internal)
                return;

            // Calculate the new location based on the current angle
            int newX = X + (int)(Math.Cos(_currentAngle) * 2);
            int newY = Y + (int)(Math.Sin(_currentAngle) * 2);

            // Randomly adjust the angle to make the wave jump side to side
            if (Utility.RandomDouble() < 0.4)
            {
                _currentAngle = _initialAngle + Utility.RandomDouble() * Math.PI / 2 - Math.PI / 4;
            }

            Point3D newLoc = new Point3D(newX, newY, Z);

            IPooledEnumerable eable = _map.GetMobilesInRange(newLoc, 0);
            foreach (Mobile m in eable)
            {
                if (m is PlayerMobile && _caster.CanBeHarmful(m))
                {
                    m.SendMessage("You are hit by a paralysis wave!");
                    AOS.Damage(m, _caster, _damage, 0, 0, 0, 0, 100);
                    m.Paralyze(TimeSpan.FromSeconds(3));
                    Effects.PlaySound(m.Location, m.Map, 0xB39);
                }
            }
            eable.Free();

            if (!Utility.InRange(_caster.Location, newLoc, 10))
            {
                Delete();
            }
            else
            {
                MoveToWorld(newLoc, _map);
                Timer.DelayCall(TimeSpan.FromMilliseconds(200), MoveWave);
            }
        }

        public ParalysisWave(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}