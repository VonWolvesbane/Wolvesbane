using Server.ContextMenus;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Multis
{

	public abstract partial class BaseBoat : BaseMulti, IMount
	{
		public IEnumerable<Mobile> GetMobilesOnBoard()
		{
			return GetEntitiesOnBoard().OfType<Mobile>();
		}

	}
}