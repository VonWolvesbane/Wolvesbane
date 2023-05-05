using System;


namespace Server.Items

{
	public class TamersIdol : BaseTalisman
	{
		[Constructable]
		public TamersIdol()
			  : base(0xC4F5)
		{
			Name = "Tamer's Idol";
			this.SkillBonuses.SetValues(0, SkillName.Veterinary, 25.0);
			this.SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);
			this.SkillBonuses.SetValues(2, SkillName.AnimalTaming, 10.0);
			
		}
		private bool GiveFollowers(Mobile from)
		{
			from.FollowersMax += 1;
			return true;
		}

		private void RemoveFollowers(Mobile from)
		{
			from.FollowersMax -= 1;
		}

		public override bool OnEquip(Mobile from)
		{

			IEntity mfrom = new Entity(Serial.Zero, new Point3D(from.X, from.Y, from.Z - 10), from.Map);
			IEntity mto = new Entity(Serial.Zero, new Point3D(from.X, from.Y, from.Z + 50), from.Map);
			Effects.SendMovingParticles(mfrom, mto, 0x2255, 1, 0, false, false, 13, 3, 9501, 1, 0, EffectLayer.Head, 0x100);
			if (GiveFollowers(from))
				return true;

			return base.OnEquip(from);
		}

		public override void OnRemoved(object parent)
		{
			base.OnRemoved(parent);

			if (parent is Mobile)
			{
				RemoveFollowers((Mobile)parent);
			}

			
		}



		public TamersIdol(Serial serial)
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