using System;


namespace Server.Items

{
	public class ArchersIdol : BaseTalisman
	{
		[Constructable]
		public ArchersIdol()
			  : base(0x9E29)
		{
			Name = "Archer's Idol";
			this.SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
			this.SkillBonuses.SetValues(1, SkillName.Fletching, 15.0);
			Attributes.LowerAmmoCost = 100;
			Attributes.WeaponSpeed = 100;
			Attributes.BonusDex = 200;

		}




		public ArchersIdol(Serial serial)
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