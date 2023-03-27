#region Header
//   Vorspire    _,-'/-'/  SewerThing.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2017  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using Server;
using Server.Mobiles;
#endregion

namespace VitaNex.Dungeons
{
	[CorpseName("stinking remains of Sewer Thing")]
	public class SewerThing : BaseCreature
	{
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return Poison.Lethal; } }

#if !ServUO
		public override bool HasAura => true;
		public override int AuraPoisonDamage => 100;
#endif

		[Constructable]
		public SewerThing()
			: base(AIType.AI_Mage, FightMode.Strongest, 10, 2, 0.02, 0.04)
		{
			Name = "Sewer Thing";
			Body = 780;
			Hue = 2967;

			SetStr(1232, 1400);
			SetDex(76, 82);
			SetInt(76, 85);

			SetHits(50000);

			SetDamage(27, 31);

			SetDamageType(ResistanceType.Physical, 80);
			SetDamageType(ResistanceType.Poison, 20);

			SetResistance(ResistanceType.Physical, 75, 85);
			SetResistance(ResistanceType.Fire, 40, 50);
			SetResistance(ResistanceType.Cold, 50, 60);
			SetResistance(ResistanceType.Poison, 55, 65);
			SetResistance(ResistanceType.Energy, 50, 60);

			SetSkill(SkillName.Wrestling, 90.0);
			SetSkill(SkillName.Tactics, 90.0);
			SetSkill(SkillName.MagicResist, 110.0);
			SetSkill(SkillName.Poisoning, 120.0);
			SetSkill(SkillName.Magery, 110.0);
			SetSkill(SkillName.EvalInt, 110.0);
			SetSkill(SkillName.Meditation, 110.0);
			SetSkill(SkillName.Spellweaving, 120.0);

			Fame = 25000;
			Karma = -25000;
		}

		public SewerThing(Serial serial)
			: base(serial)
		{ }

		public override void GenerateLoot()
		{
			AddLoot(LootPack.SuperBoss, 8);
			AddLoot(LootPack.LowScrolls, 4);
			AddLoot(LootPack.MedScrolls, 4);
			AddLoot(LootPack.HighScrolls, 4);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();
		}
	}
}
