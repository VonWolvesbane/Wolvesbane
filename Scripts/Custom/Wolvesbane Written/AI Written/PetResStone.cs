using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
	public class PetResurrectionStone : Item
	{
		[Constructable]
		public PetResurrectionStone() : base(0xED4)
		{
			Name = "Pet Resurrection Stone";
			Hue = 1153; // Set the hue to whatever you prefer
			Weight = 1.0;
		}

		public PetResurrectionStone(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			from.SendMessage("Target your dead pet to resurrect it.");
			from.Target = new PetResurrectionTarget(this);
		}

		private class PetResurrectionTarget : Target
		{
			private PetResurrectionStone m_Stone;

			public PetResurrectionTarget(PetResurrectionStone stone) : base(15, false, TargetFlags.None)
			{
				m_Stone = stone;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is BaseCreature pet && pet.ControlMaster == from && pet.IsDeadBondedPet)
				{
					// Resurrect the targeted pet
					pet.ResurrectPet();

					// Remove the resurrection stone from the world
					//m_Stone.Delete();

					from.SendMessage("You use the Pet Resurrection Stone to resurrect your pet.");
				}
				else
				{
					from.SendMessage("You must target your dead pet to resurrect it.");
				}
			}
		}

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
