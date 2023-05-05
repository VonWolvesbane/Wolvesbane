using System;


namespace Server.Items

{
	public class TinkersIdol : BaseTalisman
	{
		[Constructable]
		public TinkersIdol()
			  : base(0xC4FD)
		{
			Name = "Tinker's Idol";
			this.SkillBonuses.SetValues(0, SkillName.Tinkering, 25.0);
			this.SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
			

		}
		



		public TinkersIdol(Serial serial)
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