using Server.Targeting;
using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Menus;
using Server.Menus.Questions;
using Server.Mobiles;
using System.Collections;

namespace Server.Items
{
	public class RemoveUnwieldlyDeed : Item
	{

		[Constructable]
		public RemoveUnwieldlyDeed() : base(0x14F0)
		{
			Weight = 1.0;
			Movable = true;
			Name = "Remove Unwieldly Deed";
		}

		public RemoveUnwieldlyDeed(Serial serial) : base(serial)
		{
		}
		public override void OnDoubleClick(Mobile from)
		{

			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else if (from.InRange(this.GetWorldLocation(), 1))
			{

				this.SendLocalizedMessageTo(from, 1010086);
				from.Target = new RemoveUnwieldlyTarget(this);

			}
			else
			{
				from.SendLocalizedMessage(500446); // That is too far away. 
			}

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


		private class RemoveUnwieldlyTarget : Target
		{
			private RemoveUnwieldlyDeed m_Deed;

			public RemoveUnwieldlyTarget(RemoveUnwieldlyDeed deed) : base(10, false, TargetFlags.None)
			{
				m_Deed = deed;
			}

			protected override void OnTarget(Mobile from, object target)
			{
				if (target is Item)
				{
					Item item = target as Item;
					if (item.IsChildOf(from.Backpack))
					{
						if (target is BaseJewel)
						{
							BaseJewel c = target as BaseJewel;
							if (c.NegativeAttributes.Unwieldly == 1)
							{
								c.RemoveUnwieldly();
								c.Weight = c.DefaultWeight;
								c.InvalidateProperties();
								m_Deed.Delete();								
								from.UpdateTotals();
								return;
							}
						}
						if (target is BaseWeapon)
						{
							BaseWeapon c = target as BaseWeapon;
							if (c.NegativeAttributes.Unwieldly == 1)
							{
								c.RemoveUnwieldly();
								c.UpdateTotals();
								c.Weight = c.DefaultWeight; 
								m_Deed.Delete();
								from.UpdateTotals();
								return;
							}
						}
					}
				}
				from.SendMessage("You cant do that.");
			}
		}
	}
}
