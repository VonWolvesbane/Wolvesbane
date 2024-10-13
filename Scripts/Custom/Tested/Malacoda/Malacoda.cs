using System.Collections;
using Server.Targeting;
using Server.Network;
using Server.Items;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Server.Mobiles
{
	[CorpseName("corpse of Malacoda")]
	public class Malacoda : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return Utility.RandomBool() ? WeaponAbility.CrushingBlow : WeaponAbility.ConcussionBlow;
		}

		[Constructable]
		public Malacoda() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Malacoda";
			Title = "Leader of the Malabranche";
			Body = 400;
			Female = false;
			Hue = 33775;

			SetStr(8400);
			SetDex(8400);
			SetInt(8400);
			SetHits(850000);
			SetDamage(150, 200);
			SetDamageType(ResistanceType.Physical, 100);
			SetDamageType(ResistanceType.Cold, 100);
			SetDamageType(ResistanceType.Fire, 100);
			SetDamageType(ResistanceType.Energy, 100);
			SetDamageType(ResistanceType.Poison, 100);

			SetResistance(ResistanceType.Physical, 200);
			SetResistance(ResistanceType.Cold, 200);
			SetResistance(ResistanceType.Fire, 80);
			SetResistance(ResistanceType.Energy, 200);
			SetResistance(ResistanceType.Poison, 200);

			SetSkill(SkillName.EvalInt, 320.0);
			SetSkill(SkillName.Magery, 590.0);
			SetSkill(SkillName.Meditation, 640.0);
			SetSkill(SkillName.Poisoning, 480.0);
			SetSkill(SkillName.MagicResist, 590.0);
			SetSkill(SkillName.Tactics, 790.0);
			SetSkill(SkillName.Wrestling, 450.0);
			SetSkill(SkillName.Swords, 400.0);
			SetSkill(SkillName.Anatomy, 700.0);
			SetSkill(SkillName.Parry, 350.0);

			Fame = 15000;
			Karma = -15000;
			VirtualArmor = 85;

			PackGold(20000, 30000);

			MalabrancheChest Chest = new MalabrancheChest();
			Chest.Movable = false;
			AddItem(Chest);

			MalabrancheArms Arms = new MalabrancheArms();
			Arms.Movable = false;
			AddItem(Arms);

			MalabrancheLegs Legs = new MalabrancheLegs();
			Legs.Movable = false;
			AddItem(Legs);

			MalabrancheGloves Gloves = new MalabrancheGloves();
			Gloves.Movable = false;
			AddItem(Gloves);

			MalabrancheVest HalfApron = new MalabrancheVest();
			HalfApron.Movable = false;
			AddItem(HalfApron);

			MalabrancheHelm Helm = new MalabrancheHelm();
			Helm.Movable = false;
			AddItem(Helm);

			MalabrancheRobe Robe = new MalabrancheRobe();
			Robe.Movable = false;
			AddItem(Robe);
		}

		public override void GenerateLoot()
		{
			switch (Utility.Random(70))
			{
				case 0: PackItem(new MalabrancheRobe()); break;
				case 1: PackItem(new MalabrancheHelm()); break;
				case 2: PackItem(new MalabrancheLegs()); break;
				case 3: PackItem(new MalabrancheArms()); break;
				case 4: PackItem(new MalabrancheGloves()); break;
				case 5: PackItem(new MalabrancheChest()); break;
				case 6: PackItem(new MalabrancheVest()); break;
			}
			AddLoot(LootPack.SuperBoss, 3);
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);
			List<DamageStore> rights = GetLootingRights();

			foreach (Mobile m in rights.Select(x => x.m_Mobile).Distinct())
			{
				if (m is PlayerMobile)
				{
					int level;
					double random = Utility.RandomDouble();
					if (random <= 0.05)
						level = 150;
					else if (random <= 0.10)
						level = 145;
					else if (random <= 0.15)
						level = 140;
					else if (random <= 0.20)
						level = 135;
					else if (random <= 0.25)
						level = 130;
					else
						level = 125;

					int skillIndex = Utility.Random(32);
					switch (skillIndex)
					{
						case 0: m.AddToBackpack(new PowerScroll(SkillName.Swords, level)); break;
						case 1: m.AddToBackpack(new PowerScroll(SkillName.Fencing, level)); break;
						case 2: m.AddToBackpack(new PowerScroll(SkillName.Macing, level)); break;
						case 3: m.AddToBackpack(new PowerScroll(SkillName.Archery, level)); break;
						case 4: m.AddToBackpack(new PowerScroll(SkillName.Wrestling, level)); break;
						case 5: m.AddToBackpack(new PowerScroll(SkillName.Parry, level)); break;
						case 6: m.AddToBackpack(new PowerScroll(SkillName.Tactics, level)); break;
						case 7: m.AddToBackpack(new PowerScroll(SkillName.Anatomy, level)); break;
						case 8: m.AddToBackpack(new PowerScroll(SkillName.Healing, level)); break;
						case 9: m.AddToBackpack(new PowerScroll(SkillName.Magery, level)); break;
						case 10: m.AddToBackpack(new PowerScroll(SkillName.Meditation, level)); break;
						case 11: m.AddToBackpack(new PowerScroll(SkillName.EvalInt, level)); break;
						case 12: m.AddToBackpack(new PowerScroll(SkillName.MagicResist, level)); break;
						case 13: m.AddToBackpack(new PowerScroll(SkillName.AnimalTaming, level)); break;
						case 14: m.AddToBackpack(new PowerScroll(SkillName.AnimalLore, level)); break;
						case 15: m.AddToBackpack(new PowerScroll(SkillName.Veterinary, level)); break;
						case 16: m.AddToBackpack(new PowerScroll(SkillName.Musicianship, level)); break;
						case 17: m.AddToBackpack(new PowerScroll(SkillName.Provocation, level)); break;
						case 18: m.AddToBackpack(new PowerScroll(SkillName.Discordance, level)); break;
						case 19: m.AddToBackpack(new PowerScroll(SkillName.Peacemaking, level)); break;
						case 20: m.AddToBackpack(new PowerScroll(SkillName.Chivalry, level)); break;
						case 21: m.AddToBackpack(new PowerScroll(SkillName.Focus, level)); break;
						case 22: m.AddToBackpack(new PowerScroll(SkillName.Necromancy, level)); break;
						case 23: m.AddToBackpack(new PowerScroll(SkillName.Stealing, level)); break;
						case 24: m.AddToBackpack(new PowerScroll(SkillName.Stealth, level)); break;
						case 25: m.AddToBackpack(new PowerScroll(SkillName.SpiritSpeak, level)); break;
						case 26: m.AddToBackpack(new PowerScroll(SkillName.Spellweaving, level)); break;
						case 27: m.AddToBackpack(new PowerScroll(SkillName.Ninjitsu, level)); break;
						case 28: m.AddToBackpack(new PowerScroll(SkillName.Bushido, level)); break;
						case 29: m.AddToBackpack(new PowerScroll(SkillName.Imbuing, level)); break;
						case 30: m.AddToBackpack(new PowerScroll(SkillName.Throwing, level)); break;
						case 31: m.AddToBackpack(new PowerScroll(SkillName.Mysticism, level)); break;
					}
					m.SendMessage("You have recieved A Scroll of power for your efforts!");
				}
			}
		}
		public override bool AutoDispel { get { return true; } }
		public override bool BardImmune { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }
		public override bool AlwaysMurderer { get { return true; } }

		public override void AlterMeleeDamageFrom(Mobile from, ref int damage)
		{
			if (from is BaseCreature)
			{
				BaseCreature bc = (BaseCreature)from;

				if (bc.Controlled || bc.BardTarget == this)
					damage = 0;
			}
		}

		public Malacoda(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
