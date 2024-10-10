using Server.Items;

using VitaNex.FX;

namespace Server.Mobiles
{
    [CorpseName("Corpse Of The Shard Owner")]
    public class VonDreadsbane : BaseCreature
    {
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

        [Constructable]
        public VonDreadsbane() : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.002, 0.004)
        {
            Name = "Von Dreadsbane";
            Title = "The Evil Shard Owner";
            Body = 400;
            Female = false;
            Hue = 0;

            BaseSoundID = 1072;

            HairItemID = 8265;

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 40;

            SetStr(300, 500);
            SetDex(500, 500);
            SetInt(200, 250);

            SetHits(1000000);

            SetDamage(100, 100);

            SetDamageType(ResistanceType.Physical, 170);
            SetDamageType(ResistanceType.Cold, 170);
            SetDamageType(ResistanceType.Fire, 170);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Energy, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Fire, 100);

            SetSkill(SkillName.EvalInt, 1000);
            SetSkill(SkillName.Macing, 500);
            SetSkill(SkillName.Magery, 1000);
            SetSkill(SkillName.Anatomy, 200);
            SetSkill(SkillName.MagicResist, 1000);
            SetSkill(SkillName.Necromancy, 1000);
            SetSkill(SkillName.Tactics, 200);
            SetSkill(SkillName.Parry, 200);
            SetSkill(SkillName.Focus, 1000);
            SetSkill(SkillName.SpiritSpeak, 300);
            SetSkill(SkillName.DetectHidden, 300);

            WildStaff wildStaff = new WildStaff();
            wildStaff.ItemID = 0x905;
            wildStaff.Movable = false;
            wildStaff.Hue = 1174;
            wildStaff.Attributes.SpellDamage = 5000;
            AddItem(wildStaff);

            TunicofExpertAnimalTaming Chest = new TunicofExpertAnimalTaming();
            Chest.Movable = false;
            Chest.Hue = 1174;
            AddItem(Chest);

            GorgetofExpertAnimalTaming Neck = new GorgetofExpertAnimalTaming();
            Neck.Movable = false;
            Neck.Hue = 1174;
            AddItem(Neck);

            ArmsofExpertAnimalTaming Arms = new ArmsofExpertAnimalTaming();
            Arms.Movable = false;
            Arms.Hue = 1174;
            AddItem(Arms);

            LegsofExpertAnimalTaming Legs = new LegsofExpertAnimalTaming();
            Legs.Movable = false;
            Legs.Hue = 1174;
            AddItem(Legs);

            GlovesofExpertAnimalTaming Gloves = new GlovesofExpertAnimalTaming();
            Gloves.Movable = false;
            Gloves.Hue = 1174;
            AddItem(Gloves);

            CapofExpertAnimalTaming Helm = new CapofExpertAnimalTaming();
            Helm.Movable = false;
            Helm.Hue = 1174;
            AddItem(Helm);

            Boots Boots = new Boots();
            AddItem(Boots);

            Robe OuterTorso = new Robe();
            OuterTorso.Movable = false;
            OuterTorso.ItemID = 30742;
            OuterTorso.Hue = 1174;
            AddItem(OuterTorso);

            Nightmare nightmare = new Nightmare();
            nightmare.Rider = this;
            nightmare.Hue = 1147;

            PackGold(400, 600);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (c?.Deleted != false)
            {
                return;
            }

            switch (Utility.Random(30))
            {
                case 0: c.DropItem(new Necroacidus()); break;
                case 1: c.DropItem(new NecroloricatusBoots()); break;
                case 2: c.DropItem(new NecroloricatusCap()); break;
                case 3: c.DropItem(new NecroloricatusGloves()); break;
                case 4: c.DropItem(new NecroloricatusNecklace()); break;
                case 5: c.DropItem(new NecroloricatusPants()); break;
                case 6: c.DropItem(new NecroloricatusShirt()); break;
            }
        }

        public override void OnDamagedBySpell(Mobile from)
        {
            base.OnDamagedBySpell(from);

            DoSpecialAbility(from);
        }

        public override void OnGotMeleeAttack(Mobile from)
        {
            base.OnGotMeleeAttack(from);

            DoSpecialAbility(from);
        }

        public void DoSpecialAbility(Mobile target)
        {
            if (Utility.RandomDouble() <= 0.25)
            {
                switch (Utility.Random(15))
                {
                    case 0: new FireExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
                    case 1: new EnergyExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
                    case 2: new SmokeExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
                    case 3: new WaterRippleEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
                    case 4: new EarthExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
                    case 5: new AirExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
                    case 6: new PoisonExplodeEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
                    case 7: new FirePentagramEffect(target, target.Map, 5, effectHandler: ExplosionDamage).Send(); break;
                    case 8: new WaterWaveEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
                    case 9: new FireWaveEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
                    case 10: new EarthWaveEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
                    case 11: new AirWaveEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
                    case 12: new EnergyWaveEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
                    case 13: new PoisonWaveEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
                    case 14: new TornadoEffect(target, target.Map, GetDirectionTo(target), 5, effectHandler: ExplosionDamage).Send(); break;
                }
            }
        }

        public void ExplosionDamage(EffectInfo info)
        {
            // don't do more than one round of damage per tile for fx with multiple effects per tile
            if (info.ProcessIndex > 0)
            {
                return;
            }

            Effects.PlaySound(info.Source, info.Map, 777);

            foreach (var m in info.Source.FindPlayersInRange(info.Map, 0))
            {
                if (CanBeHarmful(m, false, true))
                {
                    DoHarmful(m);

                    _ = m.Damage(Utility.RandomMinMax(3000, 3000), this);
                }
            }
        }

        public VonDreadsbane(Serial serial) : base(serial)
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

            _ = reader.ReadInt();
        }
    }
}
