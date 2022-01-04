using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Accounting;
using System.Linq;

namespace Server.Items
{
	public class PetBag : SmallBagofHolding
	{
		public override string DefaultName { get { return "Pet GoodieBag"; } }     /// Default Name for the Bag / Safe
		public override double DefaultWeight { get { return 1; } }            /// Default weight of the Bag / Safe itself
		public override bool DisplayWeight { get { return true; } }            /// Setting to say if it should display the Weight
		public override bool DisplaysContent { get { return true; } }         /// Setting to say if it should display the Item count
        public override int ContainerMaxItems { get { return 5; } }
		
		private int maxHeldAmount = -1;                                        /// anything below 0 sets it back to the default, you shouldnt edit this
		public static int defaultHeldMaxAmount = 5;                       

		[CommandProperty(AccessLevel.GameMaster)]                              
		public int MaxHeldAmount { get { return maxHeldAmount >= 0 ? maxHeldAmount : defaultHeldMaxAmount; } set { maxHeldAmount = value; } }

		[Constructable]
		public PetBag()
		{
				Name = "Dimensional Bag of animals";
				Hue = 1153;
				LootType = LootType.Blessed;
		}

		public PetBag(Serial serial) : base(serial)
		{ }

		/*public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add(1114057, "A Magical bag to store pets." ); // ~1_val~
		}*/

		  public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
			writer.Write(maxHeldAmount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
			maxHeldAmount = reader.ReadInt();
        }

		public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
		{
			if (m.IsStaff())
				return true;


			int petCount = 0;
			int leashCount = 0;
			foreach (var i in Items)
			{
				if (i is PetLeash || i is UnlimitedPetLeash)
					leashCount += 1;
				if (i is ShrinkItem)
					petCount += 1;
			}

			if (item is ShrinkItem)
				if (petCount < MaxItems)
				{
					return true;
				}
				else
				{
					if (message)
						m.SendMessage("The maximum number of pets are already in this " + DefaultName);
					return false;
				}

			// Note: The 1 pet leash is not counted as an item in the container Max
			if (item is PetLeash || item is UnlimitedPetLeash)
			{
				if (leashCount < 1)
				{
					return true;
				}
				else
				{
					if (message)
						m.SendMessage("Only 1 leash can be put in this " + DefaultName);
					return false;
				}
			}

			if (message)
				m.SendMessage("Only Pets or a leash may be put into this " + DefaultName);
			return false;
		}

		public static T SplitItem<T>(T i, int amount) where T : Item
		{
			T item = Activator.CreateInstance<T>();

			if (item != null)
			{
				BounceInfo bi = i.GetBounce();
				if (bi.m_Parent is Container)
				{
					((Container)bi.m_Parent).AddItem(item);
				}

				item.Location = bi.m_Location;
				item.Map = bi.m_Map;
				item.Amount = i.Amount - amount;
				i.Amount = amount;
			}
			return item;
		}
	}
}