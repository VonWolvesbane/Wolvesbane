using Server.Targeting;
using System;

namespace Server.Items
{
	public class Diamond : Item, IGem
	{
		[Constructable]
		public Diamond()
			: this(1)
		{
		}

		[Constructable]
		public Diamond(int amount)
			: base(0xF26)
		{
			this.Stackable = true;
			this.Amount = amount;
		}

		public Diamond(Serial serial)
			: base(serial)
		{
		}

		public override double DefaultWeight
		{
			get
			{
				return 0.1;
			}
		}
		public override void OnDoubleClick(Mobile from)
		{

			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}
			from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(OnTarget));
			from.SendMessage("Target item to add charges to");
		}
		private void OnTarget(Mobile from, object target)
		{
			PetLeash leash = target as PetLeash;
			if (leash == null)
			{
				from.SendMessage("You cant charge that!");
				return;
			}
			if (leash.Charges < leash.MaxCharges)
			{
				// Diamonds charge at 25 to 1 rate
				int ConversionRate = 25;
				int charges = Math.Min(leash.MaxCharges - leash.Charges, (int)(this.Amount / ConversionRate));
				if (charges > 0)
				{
					leash.Charges += charges;
					if (this.Amount > (charges * ConversionRate))
					{
						this.Amount -= (charges * ConversionRate);
					}
					else
					{
						this.Delete();
					}
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
