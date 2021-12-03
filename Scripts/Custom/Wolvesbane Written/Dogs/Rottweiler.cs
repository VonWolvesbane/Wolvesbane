using System;

namespace Server.Mobiles
{
    [CorpseName("a Rottweiler corpse")]
    public class Rottweiler : BaseMount
    {
        [Constructable]
        public Rottweiler()
            : this("a Rottweiler")
        {
        }

        [Constructable]
        public Rottweiler(string name)
            : base(name, 1552, 0x3ED9, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {


            SetStr(2500, 2555);
            SetDex(285, 325);
            SetInt(285, 365);

            SetHits(2450, 2575);

            SetDamage(270, 430);

            SetDamageType(ResistanceType.Physical, 120);
            SetDamageType(ResistanceType.Poison, 200);

            SetResistance(ResistanceType.Physical, 155, 175);
            SetResistance(ResistanceType.Fire, 120, 140);
            SetResistance(ResistanceType.Cold, 155, 165);
            SetResistance(ResistanceType.Poison, 175, 190);
            SetResistance(ResistanceType.Energy, 125, 145);

            SetSkill(SkillName.Anatomy, 100, 120);
            SetSkill(SkillName.MagicResist, 191.4, 201.4);
            SetSkill(SkillName.Tactics, 200.1, 210.0);
            SetSkill(SkillName.Wrestling, 197.3, 205.2);
            SetSkill(SkillName.Poisoning, 295.0, 320.0);
            SetSkill(SkillName.Parry, 195.0, 205.0);

            Fame = 14000;
            Karma = -14000;

            VirtualArmor = 60;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 145;
        }

        public Rottweiler(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 1;
            }
        }
        public override int Hides
        {
            get
            {
                return 12;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.FruitsAndVegies | FoodType.GrainsAndHay;
            }
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
