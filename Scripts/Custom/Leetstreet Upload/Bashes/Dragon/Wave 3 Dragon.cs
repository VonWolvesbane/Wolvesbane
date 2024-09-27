using System;

namespace Server.Mobiles
{
    [CorpseName("a white dragon corpse")]
    public class WhiteDragon : BaseCreature
    {
        [Constructable]
        public WhiteDragon()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.4)
        {
            this.Name = "a white dragon";
            this.Body = Utility.RandomList(12, 59);
            this.BaseSoundID = 362;
            this.Hue = 1153;

            this.SetStr(800);
            this.SetDex(800);
            this.SetInt(800);

            this.SetHits(120000);
            this.SetStam(120000);
            this.SetDamage(75, 75);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 60);
            this.SetResistance(ResistanceType.Fire, 60);
            this.SetResistance(ResistanceType.Cold, 60);
            this.SetResistance(ResistanceType.Poison, 60);
            this.SetResistance(ResistanceType.Energy, 60);


            this.SetSkill(SkillName.EvalInt, 200);
            this.SetSkill(SkillName.Magery, 200);
            this.SetSkill(SkillName.Meditation, 200);
            this.SetSkill(SkillName.MagicResist, 200);
            this.SetSkill(SkillName.Tactics, 200);
            this.SetSkill(SkillName.Wrestling, 400);
            this.SetSkill(SkillName.DetectHidden, 100.0);
            this.Fame = 15000;
            this.Karma = -15000;

            this.VirtualArmor = 60;

            this.Tamable = false;
            this.ControlSlots = 3;
            this.MinTameSkill = 93.9;
        }

        public WhiteDragon(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement
        {
            get
            {
                return !this.Controlled;
            }
        }
        public override bool HasBreath
        {
            get
            {
                return true;
            }
        }// fire breath enabled
        public override bool AutoDispel
        {
            get
            {
                return !this.Controlled;
            }
        }

        public override bool CanFly
        {
            get
            {
                return true;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.FilthyRich, 2);
            this.AddLoot(LootPack.Gems, 8);
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