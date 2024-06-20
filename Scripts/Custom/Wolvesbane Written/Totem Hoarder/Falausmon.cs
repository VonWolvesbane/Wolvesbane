// Created by Tom Sibilsky aka Neptune

using System;
using System.Collections.Generic;
//using daat99;
using Server.Items;

namespace Server.Mobiles



{
	[CorpseName(" corpse of Falausmon")]
	public class Falausmon : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return Utility.RandomBool() ? WeaponAbility.BleedAttack : WeaponAbility.TalonStrike;

		}
		

		[Constructable]
		public Falausmon() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "Falausmon";
			Title = "The Totem Hoarder";
			Body = 188;
			
			

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
			SetResistance(ResistanceType.Cold, 80);
			SetResistance(ResistanceType.Fire, 200);
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
			SetSkill(SkillName.Healing, 499.0);


			/*m_Timer = new TeleportTimer( this );
			m_Timer.Start();
			*/


			Fame = 15000;
			Karma = -15000;
			VirtualArmor = 85;

			PackGold(20000, 30000);

			
		}
		public override void GenerateLoot()
		{
			switch (Utility.Random(15))
			{
				case 0: PackItem(new RandomTalisman()); break;
				case 1: PackItem(new ArchersIdol()); break;
				case 2: PackItem(new TinkersIdol()); break;
				case 3: PackItem(new TamersIdol()); break;

				case 4: PackItem(new RandomTalisman()); break;
				case 5: PackItem(new RandomTalisman()); break;
				case 6: PackItem(new RandomTalisman()); break;
				case 7: PackItem(new SwordsmansIdol()); break;


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


		public Falausmon(Serial serial) : base(serial)
		{
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
