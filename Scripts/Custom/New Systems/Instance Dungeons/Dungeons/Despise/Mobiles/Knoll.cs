using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a knoll corpse")]
	public class Knoll : BaseCreature
	{
		[Constructable]
		public Knoll() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.4)
		{
			Name = "Knoll";
			Body = 1581;
			//BaseSoundID = 357; // You can change this to any appropriate sound ID.

			SetStr(5000);
			SetDex(5000);
			SetInt(5000);

			SetHits(50000);
			SetMana(10000);

			SetDamage(15, 75);

			SetDamageType(ResistanceType.Physical, 90);

			SetResistance(ResistanceType.Physical, 70, 90);
			SetResistance(ResistanceType.Fire, 70, 90);
			SetResistance(ResistanceType.Cold, 70, 90);
			SetResistance(ResistanceType.Poison, 70, 90);
			SetResistance(ResistanceType.Energy, 70, 90);

			SetSkill(SkillName.Wrestling, 200.0);
			SetSkill(SkillName.Tactics, 200.0);
			SetSkill(SkillName.MagicResist, 200.0);
			SetSkill(SkillName.Anatomy, 200.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 100;

			PackGold(10000, 20000);
			PackItem(new Item(Utility.RandomList(0x1EBC, 0x1EBD))); // Randomly pack a rare item, example
		}

		public override void GenerateLoot()
		
		{
			AddLoot(LootPack.FilthyRich, 3);
			switch (Utility.Random(150))
			{
				case 0: PackItem(new DaggerBeltSkin()); break;
				case 1: PackItem(new MaceBeltSkin()); break;
				case 2: PackItem(new SwordBeltSkin()); break;
						


			}
		}

		public override bool AutoDispel { get { return true; } }
		//public override bool BardImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 19; } }
		public override int Hides { get { return 30; } }
		public override HideType HideType { get { return HideType.Barbed; } }

		public Knoll(Serial serial) : base(serial)
		{
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
