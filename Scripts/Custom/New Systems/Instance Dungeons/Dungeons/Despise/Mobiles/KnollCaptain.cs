using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a knoll corpse")]
	public class KnollCaptain : BaseCreature
	{
		[Constructable]
		public KnollCaptain() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.4)
		{
			Name = "Knoll Captain";
			Body = 1580;
			//BaseSoundID = 357; // You can change this to any appropriate sound ID.

			SetStr(10000);
			SetDex(10000);
			SetInt(10000);

			SetHits(500000);
			SetMana(10000);

			SetDamage(200, 250);

			SetDamageType(ResistanceType.Physical, 95);

			SetResistance(ResistanceType.Physical, 80, 90);
			SetResistance(ResistanceType.Fire, 80, 90);
			SetResistance(ResistanceType.Cold, 80, 90);
			SetResistance(ResistanceType.Poison, 80, 90);
			SetResistance(ResistanceType.Energy, 80, 90);

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
		}

		public override bool AutoDispel { get { return true; } }
		public override bool BardImmune { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override int Meat { get { return 19; } }
		public override int Hides { get { return 30; } }
		public override HideType HideType { get { return HideType.Barbed; } }

		public KnollCaptain(Serial serial) : base(serial)
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
