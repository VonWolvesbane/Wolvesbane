using System;
using Server.Items;

namespace Server.Items
{
	public class CrusaderLegs : DragonLegs
	{
		public override int ArtifactRarity { get { return 777; } }
		public override bool IsArtifact { get { return true; } }

		public override int InitMinHits { get { return 255; } }
		public override int InitMaxHits { get { return 255; } }

		[Constructable]
		public CrusaderLegs() : base()
		{
			Name = "Crusader Legs of Wrath";
			Hue = 2658;
			ItemID = 0xC536;
			

			Attributes.BonusStr = 103;
			Attributes.BonusInt = 103;
			Attributes.BonusDex = 103;
			Attributes.NightSight = 1;

			Attributes.RegenHits = 3;
			Attributes.RegenMana = 3;
			Attributes.RegenStam = 3;
			Attributes.SpellDamage = 777;
			Attributes.ReflectPhysical = 333;
			Attributes.AttackChance = 333;
			Attributes.DefendChance = 333;
			Attributes.BonusHits = 333;
			Attributes.BonusMana = 333;
			Attributes.BonusStam = 333;
			Attributes.Luck = 777;
			ArmorAttributes.MageArmor = 1;

			PhysicalBonus = 10;
			SkillBonuses.SetValues(0, SkillName.Swords, 10);
			SkillBonuses.SetValues(1, SkillName.Chivalry, 10);
			SkillBonuses.SetValues(2, SkillName.Tactics, 10);

			Attributes.CastRecovery = 3;
			Attributes.CastSpeed = 3;
			Attributes.WeaponDamage = 333;

			ArmorAttributes.SelfRepair = 10;
			
		}

		public CrusaderLegs(Serial serial) : base(serial) { }

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
