using System;
using Server;

namespace Server.Items
{
	public class BackpackOfSpellbooks : Backpack, Engines.Craft.ICraftable
	{
		public override string DefaultName { get { return "Backpack of Spellbooks"; } }

		[Constructable]
		public BackpackOfSpellbooks() : base()
		{
			LootType = LootType.Blessed;
		}
		public BackpackOfSpellbooks(Serial serial) : base(serial) { }
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
		#region ICraftable Members
		public int OnCraft(int quality, bool makersMark, Mobile from, Engines.Craft.CraftSystem craftSystem, Type typeRes, ITool tool, Engines.Craft.CraftItem craftItem, int resHue)
		{
			if (quality >= 2)
				Exceptional = true;

			if (makersMark)
				Crafter = from;

			Type resourceType = typeRes;
			if (resourceType == null)
				resourceType = craftItem.Resources.GetAt(0).ItemType;

			Resource = CraftResources.GetFromType(resourceType);

			Engines.Craft.CraftContext context = craftSystem.GetContext(from);

			if (context != null && context.DoNotColor)
				Hue = 0;

			return quality;
		}

		private CraftResource m_Resource;
		[CommandProperty(AccessLevel.GameMaster)]
		public CraftResource Resource { get { return m_Resource; } set { m_Resource = value; Hue = CraftResources.GetHue(value); InvalidateProperties(); } }

		internal bool m_Exceptional;
		[CommandProperty(AccessLevel.GameMaster)]
		public bool Exceptional { get { return m_Exceptional; } set { m_Exceptional = value; InvalidateProperties(); } }


		internal Mobile m_Crafter;
		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Crafter
		{
			get
			{
				return m_Crafter;
			}
			set
			{
				m_Crafter = value;
				InvalidateProperties();
			}
		}
		#endregion

	}
}