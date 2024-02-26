using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class Hephastos : BaseCreature
	{
		[Constructable]
		public Hephastos() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Hephastos";
			Title = "The Evil Blacksmith";
			Body = 1575;
			Hue = 0;

			SetStr(6660);
			SetDex(6660);
			SetInt(6660);
			SetHits(666666);
			SetDamage(150, 666);
			SetDamageType(ResistanceType.Physical, 100);
			SetDamageType(ResistanceType.Cold, 100);
			SetDamageType(ResistanceType.Fire, 100);
			SetDamageType(ResistanceType.Energy, 100);
			SetDamageType(ResistanceType.Poison, 100);

			SetResistance(ResistanceType.Physical, 200);
			SetResistance(ResistanceType.Cold, 200);
			SetResistance(ResistanceType.Fire, 200);
			SetResistance(ResistanceType.Energy, 80);
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
			SetSkill(SkillName.Healing, 450.0);



		}
		public override void GenerateLoot()
		{
			switch (Utility.Random(100))
			{
				case 0: PackItem(new CrusaderBelt()); break;
				case 1: PackItem(new CrusaderArms()); break;
				case 2: PackItem(new CrusaderBoots()); break;
				case 3: PackItem(new CrusaderChest()); break;
				case 4: PackItem(new CrusaderGloves()); break;
				case 5: PackItem(new CrusaderHelm()); break;
				case 6: PackItem(new CrusaderLegs()); break;
				case 7: PackItem(new CrusaderSash()); break;
				case 8: PackItem(new CrusaderWings()); break;
				case 9: PackItem(new HolyAvengersWrath()); break;
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
					damage = 0; // Immune to pets and provoked creatures
			}
		}

		public Hephastos(Serial serial) : base(serial) { }

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

