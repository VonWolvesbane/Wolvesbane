#region Header
//   Vorspire    _,-'/-'/  Sycophant.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using Server.Items;
#endregion

namespace Server.Mobiles
{
	[CorpseName("rotting remains of Sycophant")]
	public class Sycophant : BaseCreature
	{
		public override Poison PoisonImmune { get { return Poison.Greater; } }

		[Constructable]
		public Sycophant()
			: base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Sycophant";
			Body = 315;
			Hue = 2967;

			SetStr(878, 993);
			SetDex(581, 683);
			SetInt(1200, 1300);

			SetHits(50000);
			SetStam(507, 669);
			SetMana(1200, 1300);

			SetDamage(21, 28);

			SetDamageType(ResistanceType.Physical, 40);
			SetDamageType(ResistanceType.Poison, 60);

			SetResistance(ResistanceType.Physical, 40, 55);
			SetResistance(ResistanceType.Fire, 50, 65);
			SetResistance(ResistanceType.Cold, 50, 65);
			SetResistance(ResistanceType.Poison, 65, 75);
			SetResistance(ResistanceType.Energy, 60, 75);

			SetSkill(SkillName.Wrestling, 120.0);
			SetSkill(SkillName.Tactics, 120.0);
			SetSkill(SkillName.MagicResist, 120.0);
			SetSkill(SkillName.Anatomy, 120.0);
			SetSkill(SkillName.Poisoning, 120.0);

			Fame = 32000;
			Karma = -32000;
		}

		public Sycophant(Serial serial)
			: base(serial)
		{ }

		public override void GenerateLoot()
		{
			AddLoot(LootPack.SuperBoss, 8);
		}

		public override WeaponAbility GetWeaponAbility()
		{
			return Utility.RandomBool() ? WeaponAbility.Dismount : WeaponAbility.ParalyzingBlow;
		}

		public override int GetAttackSound()
		{
			return 0x34C;
		}

		public override int GetHurtSound()
		{
			return 0x354;
		}

		public override int GetAngerSound()
		{
			return 0x34C;
		}

		public override int GetIdleSound()
		{
			return 0x34C;
		}

		public override int GetDeathSound()
		{
			return 0x354;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();
		}
	}
}
