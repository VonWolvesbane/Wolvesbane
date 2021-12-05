using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a Fairy corpse")]
    public class FairyTame : BaseCreature
    {
        [Constructable]
        public FairyTame()
            : base(AIType.AI_Mage, FightMode.Evil, 10, 1, 0.1, 0.1)
        {
            Name = "A Fairy";
            Body = 128;
            BaseSoundID = 0x467;
			NameHue = 1153;

            SetStr(321, 330);
            SetDex(401, 420);
            SetInt(5201, 5250);

            SetHits(500, 750);
			SetMana(10000, 15000);

            SetDamage(120, 150);

            SetDamageType(ResistanceType.Energy, 125);

            SetResistance(ResistanceType.Physical, 90, 95);
            SetResistance(ResistanceType.Fire, 80, 95);
            SetResistance(ResistanceType.Cold, 80, 95);
            SetResistance(ResistanceType.Poison, 90, 95);
            SetResistance(ResistanceType.Energy, 80, 100);

            SetSkill(SkillName.EvalInt, 110.1, 120.0);
            SetSkill(SkillName.Magery, 110.1, 120.0);
            SetSkill(SkillName.Meditation, 110.1, 120.0);
            SetSkill(SkillName.MagicResist, 110.5, 150.0);
            SetSkill(SkillName.Tactics, 110.1, 120.0);
            SetSkill(SkillName.Wrestling, 110.1, 120.5);
			
			Skills[SkillName.EvalInt].Cap = 200;
			Skills[SkillName.Magery].Cap = 200;
			Skills[SkillName.Meditation].Cap = 200;
			Skills[SkillName.MagicResist].Cap = 200;
			Skills[SkillName.Wrestling].Cap = 130;
			Skills[SkillName.Tactics].Cap = 130;
			Skills[SkillName.Anatomy].Cap = 130;

            Fame = 7000;
            Karma = 7000;
			
			Female = true;
			
			Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 90.1;

            VirtualArmor = 100;
            if (0.02 > Utility.RandomDouble())
                PackStatue();
        }

        public FairyTame(Serial serial)
            : base(serial)
        {
        }

        public override bool InitialInnocent
        {
            get
            {
                return true;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Spined;
            }
        }
        public override int Hides
        {
            get
            {
                return 5;
            }
        }
        public override int Meat
        {
            get
            {
                return 1;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Deadly;
            }
        }
        public override Poison HitPoison
        {
            get
            {
                return (0.4 >= Utility.RandomDouble() ? Poison.Deadly : Poison.Deadly);
            }
        }
        public override TribeType Tribe { get { return TribeType.Fey; } }

        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.LowScrolls);
            AddLoot(LootPack.Gems, 2);
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
