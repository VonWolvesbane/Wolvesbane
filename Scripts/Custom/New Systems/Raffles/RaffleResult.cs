#region Header
//   Vorspire    _,-'/-'/  Raffleresult.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2019  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Linq;

using Server;
#endregion

namespace VitaNex.Modules.UltimateRaffle
{
	public sealed class RaffleResult : PropertyObject
	{
		public string Account { get; private set; }
		public string Name { get; private set; }

		public double Chance { get; private set; }

		public int[] Positions { get; private set; }

		public int Count { get { return Positions.Length; } }

		public bool IsWinner { get { return Positions.Contains(0); } }

		public RaffleResult(Mobile m, double chance, IEnumerable<int> positions)
		{
			Account = m.Account.Username;
			Name = m.RawName;

			Chance = chance;
			Positions = positions.ToArray();
		}

		public RaffleResult(GenericReader reader)
			: base(reader)
		{ }

		public override void Clear()
		{ }

		public override void Reset()
		{ }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.Write(Account);
			writer.Write(Name);

			writer.Write(Chance);

			writer.WriteArray(Positions, (w, o) => w.Write(o));
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			Account = reader.ReadString();
			Name = reader.ReadString();

			Chance = reader.ReadDouble();

			Positions = reader.ReadArray(r => r.ReadInt(), Positions);
		}
	}
}