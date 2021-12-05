using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Accounting;
using Server.ContextMenus;
using Server.Items;

namespace Server.Multis
{
	public abstract partial class BaseHouse : BaseMulti
	{
		public virtual Guildstone FindGuildstone()
		{
			Map map = Map;

			if (map == null)
				return null;

			MultiComponentList mcl = Components;
			IPooledEnumerable eable = map.GetItemsInBounds(new Rectangle2D(X + mcl.Min.X, Y + mcl.Min.Y, mcl.Width, mcl.Height));

			foreach (Item item in eable)
			{
				if (item is Guildstone && Contains(item))
				{
					eable.Free();
					return (Guildstone)item;
				}
			}

			eable.Free();
			return null;
		}
	}
}