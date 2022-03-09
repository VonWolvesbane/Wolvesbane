using System;
using Server;

namespace Server.Items
{
	public class BackpackOfSpellbooks : Backpack
	{
		public override string DefaultName { get { return "Backpack of Spellbooks"; } }

		[Constructable]
		public BackpackOfSpellbooks() : base()
		{
			LootType = LootType.Blessed;
		}
		public BackpackOfSpellbooks(Serial serial) : base(serial) { }
		public override bool CanCastFrom { get { return true; } }

		public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
		{
			bool result = false;
			if (item is ACC.CSS.CSpellbook)
				result = true;
			if (item is Spellbook)
				result = true;
			if (item is Runebook)
				result = true;

			if (result == true)
				return base.OnDragDropInto(from, item, p);

			return false;
		}
		public override bool TryDropItem(Mobile from, Item dropped, bool sendFullMessage)
		{
			bool result = false;
			if (dropped is ACC.CSS.CSpellbook)
				result = true;
			if (dropped is Spellbook)
				result = true;
			if (dropped is Runebook)
				result = true;

			if (result == true)
				return base.TryDropItem(from, dropped, sendFullMessage);

			return false;
		}

	}
}