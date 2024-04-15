using System;


namespace Server.Items

{
	public class SwordsmansIdol : BaseTalisman
	{
		[Constructable]
		public SwordsmansIdol()
			  : base(0xC56D)
		{
			Name = "Swordsman's Idol";
			this.SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
			this.SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
			Attributes.WeaponSpeed = 100;
			Attributes.BonusDex = 200;

		}




		public SwordsmansIdol(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

	}
}