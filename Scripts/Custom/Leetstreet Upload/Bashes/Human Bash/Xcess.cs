using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server;
using Server.Spells;
using Server.Spells.Necromancy;
using VitaNex.FX;

namespace Server.Mobiles
{
    [CorpseName("corpse of Xcess")]
    public class Xcess : BaseCreature
    {
        public override WeaponAbility GetWeaponAbility()
        {
            return Utility.RandomBool() ? WeaponAbility.ArmorPierce : WeaponAbility.ArmorPierce;

        }

        [Constructable]
        public Xcess() : base(AIType.AI_Melee, FightMode.Closest, 30, 10, 0.001, 0.002)
        {
            Name = "Xcess";
            Title = "Devil's Advocate";
            Body = 400;
            Female = false;
            Hue = 0;

            SetStr(1500);
            SetDex(1500);
            SetInt(8400);
            SetHits(1000000);
            SetDamage(100, 100);

            SetDamageType(ResistanceType.Physical, 100);
            SetDamageType(ResistanceType.Cold, 100);
            SetDamageType(ResistanceType.Fire, 100);
            SetDamageType(ResistanceType.Energy, 100);
            SetDamageType(ResistanceType.Poison, 100);

            SetResistance(ResistanceType.Physical, 200);
            SetResistance(ResistanceType.Cold, 200);
            SetResistance(ResistanceType.Fire, 200);
            SetResistance(ResistanceType.Energy, 200);
            SetResistance(ResistanceType.Poison, 200);

            SetSkill(SkillName.SpiritSpeak, 320.0);
            SetSkill(SkillName.Necromancy, 320.0);
            SetSkill(SkillName.EvalInt, 320.0);
            SetSkill(SkillName.Magery, 590.0);
            SetSkill(SkillName.Meditation, 1000.0);
            SetSkill(SkillName.Focus, 1000.0);
            SetSkill(SkillName.Poisoning, 480.0);
            SetSkill(SkillName.MagicResist, 590.0);
            SetSkill(SkillName.Tactics, 400.0);
            SetSkill(SkillName.Archery, 800.0);
            SetSkill(SkillName.Swords, 400.0);
            SetSkill(SkillName.Anatomy, 700.0);
            SetSkill(SkillName.Parry, 200.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 85;

            PackGold(20000, 30000);

            Item hair = new Item(8263);
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            hair.Hue = 2922;
            AddItem(hair);

            Sandals sandals = new Sandals();
            sandals.Movable = false;
            sandals.Hue = 2922;
            AddItem(sandals);

            LeatherChest leatherChest = new LeatherChest();
            leatherChest.Movable = false;
            leatherChest.Hue = 2922;
            leatherChest.ArmorAttributes.SelfRepair = 300;
            AddItem(leatherChest);


            StuddedArms studdedArms = new StuddedArms();
            studdedArms.Movable = false;
            studdedArms.Hue = 2922;
            studdedArms.ArmorAttributes.SelfRepair = 300;
            AddItem(studdedArms);


            OrderShield shield = new OrderShield();
            shield.Movable = false;
            shield.Hue = 2922;
            shield.ArmorAttributes.SelfRepair = 300;
            AddItem(shield);

            StuddedLegs studdedLegs = new StuddedLegs();
            studdedLegs.Movable = false;
            studdedLegs.Hue = 2922;
            studdedLegs.ArmorAttributes.SelfRepair = 300;
            AddItem(studdedLegs);

            LeatherGloves gloves = new LeatherGloves();
            gloves.Movable = false;
            gloves.Hue = 2922;
            gloves.ArmorAttributes.SelfRepair = 300;
            AddItem(gloves);

            SwordBeltSkin swordbelt = new SwordBeltSkin();
            swordbelt.Movable = false;
            swordbelt.Hue = 2922;
            swordbelt.ArmorAttributes.SelfRepair = 300;
            AddItem(swordbelt);

            CompositeBow bow = new CompositeBow();
            bow.Movable = false;
            bow.Hue = 2922;
            bow.WeaponAttributes.SelfRepair = 300;
            bow.Layer = Layer.OneHanded;
            bow.WeaponAttributes.HitLeechMana = 200;
            EquipItem(bow);

            Arrow arrows = new Arrow(1000);
            arrows.Movable = false;
            AddToBackpack(arrows);

            Nightmare nightmare = new Nightmare();
            nightmare.Hue = 2922;
            nightmare.Name = "Fiery Nightmare";
                                               
            nightmare.SetStr(1250);
            nightmare.SetDex(3000);
            nightmare.SetInt(2000);
            nightmare.SetHits(1000000); 
            nightmare.SetDamage(200, 200); 

  
            nightmare.SetResistance(ResistanceType.Physical, 100);
            nightmare.SetResistance(ResistanceType.Fire, 100);
            nightmare.SetResistance(ResistanceType.Cold, 100);
            nightmare.SetResistance(ResistanceType.Poison, 100);
            nightmare.SetResistance(ResistanceType.Energy, 100);

            nightmare.SetSkill(SkillName.Wrestling, 1000);     
            nightmare.SetSkill(SkillName.Tactics, 1000);       
            nightmare.SetSkill(SkillName.MagicResist, 1000);   
            nightmare.SetSkill(SkillName.Anatomy, 1000);
            nightmare.SetSkill(SkillName.Magery, 1000);
            nightmare.Rider = this;
        }
        public override bool AutoDispel { get { return true; } }
        public override bool BardImmune { get { return true; } }
        public override bool Unprovokable { get { return true; } }
        public override bool Uncalmable { get { return true; } }
        public override bool AreaPeaceImmune { get { return true; } }
        public override bool BleedImmune { get { return true; } }
        public override bool FreezeOnCast { get { return false; } }
        public override bool ReduceSpeedWithDamage { get { return false; } }
        public override Poison HitPoison { get { return Poison.Lethal; } }
        public override bool AlwaysMurderer { get { return true; } }

        public override void GenerateLoot()
        {
            PackGold(120000, 120000);
        }

        private DateTime _nextWaveTime;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= _nextWaveTime)
            {
                _nextWaveTime = DateTime.UtcNow.AddSeconds(10);

                for (int i = 0; i < 8; i++)
                {
                    Timer.DelayCall(TimeSpan.FromMilliseconds(i * 100), () =>
                    {
                        var dir = (Direction)i;

                        var effect = WaveFX.Fire.CreateInstance(Location, Map, dir, 15, 1, TimeSpan.FromMilliseconds(150),
                            info =>
                            {
                                info.Hue = 2922; 
                            });

                        if (effect != null)
                        {
                            effect.Send();
                        }
                    });
                }
                Combatant.Damage(Utility.Random(2000, 2000), this);
            }
        }

        public Xcess(Serial serial) : base(serial)
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
