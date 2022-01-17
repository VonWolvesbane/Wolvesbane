using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a balron corpse")]
    public class Balron : BaseCreature
    {
		[Constructable]
		public Balron()
			: base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			this.Name = NameList.RandomName("balron");
			this.Body = 40;
			this.BaseSoundID = 357;

			SetStr(3986, 4185);
			SetDex(1177, 2255);
			SetInt(1151, 2250);

			SetHits(532500, 742000);
			SetMana(10045, 15000);

			SetDamage(90, 250);

			SetDamageType(ResistanceType.Physical, 97, 110);
			SetResistance(ResistanceType.Fire, 160, 180);
			SetResistance(ResistanceType.Cold, 10, 20);
			SetResistance(ResistanceType.Poison, 60);
			SetResistance(ResistanceType.Energy, 50, 70);

			SetSkill(SkillName.MagicResist, 125.1, 135.0);
			SetSkill(SkillName.Magery, 125.1, 135.0);
			SetSkill(SkillName.Necromancy, 125.1, 135.0);
			SetSkill(SkillName.EvalInt, 125.1, 135.0);
			SetSkill(SkillName.SpiritSpeak, 125.1, 135.0);
			SetSkill(SkillName.Anatomy, 25.1, 50.0);
			SetSkill(SkillName.EvalInt, 90.1, 100.0);
			SetSkill(SkillName.Magery, 95.5, 100.0);
			SetSkill(SkillName.Meditation, 125.1, 150.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 100.0);

			Fame = 34000;
			Karma = -34000;

			VirtualArmor = 190;

			PackItem(new Halberd());
			if (Utility.RandomDouble() < 0.05)
			{
				switch (Utility.Random(2))
				{
					case 0:
						{
							HalberdOfEvolution loot = new HalberdOfEvolution();
							loot.Slayer = SlayerGroup.RandomSuperSlayerAOS();
							PackItem(loot);
						}
						break;
					case 1:
						{
							// Make a GargishWarHammer similar to the Halby
							GargishWarHammerOfEvolution loot = new GargishWarHammerOfEvolution();
							loot.Slayer = SlayerGroup.RandomSuperSlayerAOS();
							PackItem(loot);
						}
						break;
				}
			}
			else if (Utility.RandomDouble() < 0.4)
			{
				switch (Utility.Random(4))
				{
					case 0:
						{
							BarbedWhip loot = new BarbedWhip();
							PackItem(loot);
						}
						break;
					case 1:
						{
							BladedWhip loot = new BladedWhip();
							PackItem(loot);
						}
						break;
					case 2:
						{
							SpikedWhip loot = new SpikedWhip();
							PackItem(loot);
						}
						break;
					case 3:
						{
							PowderOfTemperament loot = new PowderOfTemperament();
							PackItem(loot);
						}
						break;
				}
			}

			SetSpecialAbility(SpecialAbility.Inferno);
		}
		

        public Balron(Serial serial)
            : base(serial)
        {
        }

        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Deadly;
            }
        }
        public override int TreasureMapLevel
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
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.FilthyRich, 2);
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.MedScrolls, 2);

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