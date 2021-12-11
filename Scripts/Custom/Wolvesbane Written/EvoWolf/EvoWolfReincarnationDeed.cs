// By SHAMBAMPOW
using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class EvoWolfReincarnationTarget : Target
	{
		private EvoWolfReincarnationDeed m_Deed;

		public EvoWolfReincarnationTarget(EvoWolfReincarnationDeed deed) : base(1, false, TargetFlags.None)
		{
			m_Deed = deed;
		}

		protected override void OnTarget(Mobile from, object target)
		{
			if (target is Xanthos.Evo.WolfEvo)
			{
				Xanthos.Evo.WolfEvo item = (Xanthos.Evo.WolfEvo)target;

				if (item.Stage < 4)
				{
					from.SendMessage("That wolf is not fully leveled!");
					return;
				}
				else
				{
					if (item.ControlMaster != from) // Make sure it is their pet
					{
						from.SendMessage("You can only reincarnate your own wolf!");
						return;
					}
					else
					{

						from.SendMessage("Your wolf is reincarnated as a wolf pup...");

						item.Delete();
						m_Deed.Delete();
						var egg = new Xanthos.Evo.WolfEgg();
						from.AddToBackpack(egg);
						
					}
				}
			}
			else
			{
				from.SendMessage("You cannot reincarnate that");
			}
		}
	}

	public class EvoWolfReincarnationDeed : Item // Create the item class which is derived from the base item class
	{
		[Constructable]
		public EvoWolfReincarnationDeed() : base(0x14F0)
		{
			Weight = 1.0;
			Name = "a reincarnation item deed";
			LootType = LootType.Blessed;
			Hue = 171;
		}

		public EvoWolfReincarnationDeed(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			LootType = LootType.Blessed;

			int version = reader.ReadInt();
		}

		public override bool DisplayLootType { get { return false; } }

		public override void OnDoubleClick(Mobile from) // Override double click of the deed to call our target
		{
			if (!IsChildOf(from.Backpack)) // Make sure its in their pack
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendMessage("What wolf would you like to reincarnate?");
				from.Target = new EvoWolfReincarnationTarget(this); // Call our target
			}
		}
	}
}


