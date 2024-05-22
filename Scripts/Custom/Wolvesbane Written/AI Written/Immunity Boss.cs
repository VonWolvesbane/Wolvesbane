using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a boss corpse")]
	public class RandomlyImmuneBoss : BaseCreature
	{
		private WeaponType _immuneWeaponType;

		public enum WeaponType
		{
			Swords,
			Fencing,
			Macing
		}

		[Constructable]
		public RandomlyImmuneBoss() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2)
		{
			Name = "Randomly Immune Boss";
			Body = 400; // Change to your boss's body ID
			BaseSoundID = 357; // Change to your boss's sound ID

			SetStr(800, 1000);
			SetDex(150, 200);
			SetInt(300, 400);

			SetHits(5000);
			SetDamage(20, 30);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 50, 60);
			SetResistance(ResistanceType.Fire, 40, 50);
			SetResistance(ResistanceType.Cold, 40, 50);
			SetResistance(ResistanceType.Poison, 30, 40);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.MagicResist, 100.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 100.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 60;

			ChooseRandomImmunity();
		}

		public void ChooseRandomImmunity()
		{
			Array values = Enum.GetValues(typeof(WeaponType));
			Random random = new Random();
			_immuneWeaponType = (WeaponType)values.GetValue(random.Next(values.Length));
			Say($"I am immune to {_immuneWeaponType} weapons!");
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			if (from != null && from.Weapon is BaseWeapon weapon && IsImmune(weapon))
			{
				from.SendMessage("Your weapon seems to have no effect!");
				return;
			}
			base.OnDamage(amount, from, willKill);
		}

		public bool IsImmune(BaseWeapon weapon)
		{
			if (weapon == null) return false;

			switch (_immuneWeaponType)
			{
				case WeaponType.Swords:
					return weapon is BaseSword;
				case WeaponType.Fencing:
					return weapon is BaseSpear; // Use BaseSpear for fencing weapons
				case WeaponType.Macing:
					return weapon is BaseBashing;
				default:
					return false;
			}
		}

		public RandomlyImmuneBoss(Serial serial) : base(serial) { }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
			writer.Write((int)_immuneWeaponType);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			_immuneWeaponType = (WeaponType)reader.ReadInt();
		}
	}
}

