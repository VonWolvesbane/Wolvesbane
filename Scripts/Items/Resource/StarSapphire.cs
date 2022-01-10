using System;
using Server.Targeting;

namespace Server.Items
{
    public class StarSapphire : Item, IGem
    {
        [Constructable]
        public StarSapphire()
            : this(1)
        {
        }

        [Constructable]
        public StarSapphire(int amount)
            : base(0x0F0F)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public StarSapphire(Serial serial)
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
				// Star Saphire charge at 50 to 1 rate
				int ConversionRate = 50;
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

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                ItemID = 0x0F0F;
        }
    }
}