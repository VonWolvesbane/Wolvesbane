#region Header
//   Vorspire    _,-'/-'/  RaffleResults.cs
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
using System.IO;
using System.Linq;

using Server;
#endregion

namespace VitaNex.Modules.UltimateRaffle
{
	public sealed class RaffleResults : PropertyObject
	{
		private static int _Serial;

		public int Serial { get; private set; }

		public string Source { get; private set; }

		public string CurrencyType { get; private set; }
		public string CurrencyName { get; private set; }
		public int CurrencyTaken { get; private set; }

		public int TicketsSold { get; private set; }
		public int TicketCost { get; private set; }

		public DateTime Started { get; private set; }
		public DateTime Ended { get; private set; }

		public string[] Reward { get; private set; }

		public RaffleResult[] Entries { get; private set; }

		public RaffleResult Winner { get; private set; }

		public RaffleResults(UltimateRaffle raffle)
		{
			Serial = ++_Serial;

			Source = raffle.Serial.ToString();

			Reward = raffle.RewardItem.GetOPLStrings().ToArray();

			CurrencyType = raffle.CurrencyType.InternalType.Name;
			CurrencyName = raffle.CurrencyName;
			CurrencyTaken = raffle.CurrencyTaken;

			TicketsSold = raffle.TicketsSold;
			TicketCost = raffle.TicketCost;

			Started = raffle.StartDate;
			Ended = raffle.ExpireDate;

			var e = raffle.Entries.Where(o => o != null && !o.IsDisposed && !o.IsRefunded).ToArray();

			var r = new List<RaffleResult>();

			foreach (var o in e.ToLookup(o => o.Mobile))
			{
				var entry = new RaffleResult(o.Key, e.Percent(x => x.Mobile == o.Key), e.IndexOfAll(x => x.Mobile == o.Key));

				r.Add(entry);

				if (Winner == null && entry.IsWinner)
				{
					Winner = entry;
				}
			}

			Entries = r.FreeToArray(true);
		}

		public RaffleResults(GenericReader reader)
			: base(reader)
		{ }

		public override void Clear()
		{ }

		public override void Reset()
		{ }

		public void Log(StreamWriter w)
		{
			w.WriteLine();
			w.WriteLine("Raffle: #{0} ({1} - {2})", Serial, Started, Ended);
			w.WriteLine("Serial: {0}", Source);
			w.WriteLine("Ticket: {0:#,0} @ {1:#,0} {2}/ea", TicketsSold, TicketCost, CurrencyName);
			w.WriteLine("Profit: {0:#,0} {1}", CurrencyTaken, CurrencyName);
			w.WriteLine();
			w.WriteLine("Reward:");
			w.WriteLine(new string('=', 20));

			foreach (var line in Reward)
			{
				w.WriteLine(line);
			}

			w.WriteLine(new string('=', 20));
			w.WriteLine();

			if (Winner != null)
			{
				w.WriteLine("Winner:");
				w.WriteLine("{0} '{1}'\t\t{2}% ({3:#,0} {4})",
					Winner.Account, Winner.Name, Winner.Chance * 100, Winner.Count,
					Winner.Count != 1 ? "entries" : "entry");
				//w.WriteLine(string.Join(", ", Winner.Positions).WrapWords(20));
			}

			w.WriteLine();
			w.WriteLine("Entries: {0:#,0}", Entries.Length);
			w.WriteLine(new string('=', 20));

			foreach (var entry in Entries.OrderByDescending(o => o.Chance))
			{
				w.WriteLine("{0} '{1}'\t\t{2}% ({3:#,0} {4})",
					entry.Account, entry.Name, entry.Chance * 100, entry.Count,
					entry.Count != 1 ? "entries" : "entry");
				//w.WriteLine(string.Join(", ", entry.Positions).WrapWords(20));
			}

			w.WriteLine(new string('*', 20));
			w.WriteLine();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.Write(Serial);

			writer.Write(Source);

			writer.Write(CurrencyType);
			writer.Write(CurrencyName);
			writer.Write(CurrencyTaken);

			writer.Write(TicketsSold);
			writer.Write(TicketCost);

			writer.Write(Started);
			writer.Write(Ended);

			writer.WriteArray(Reward, (w, o) => w.Write(o));

			writer.WriteArray(Entries, (w, o) => o.Serialize(w));

			writer.Write(Entries.IndexOf(Winner));
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			Serial = reader.ReadInt();

			Source = reader.ReadString();

			CurrencyType = reader.ReadString();
			CurrencyName = reader.ReadString();
			CurrencyTaken = reader.ReadInt();

			TicketsSold = reader.ReadInt();
			TicketCost = reader.ReadInt();

			Started = reader.ReadDateTime();
			Ended = reader.ReadDateTime();

			Reward = reader.ReadArray(r => r.ReadString(), Reward);

			Entries = reader.ReadArray(r => new RaffleResult(r), Entries);

			var winner = reader.ReadInt();

			if (winner >= 0 && winner < Entries.Length)
			{
				Winner = Entries[winner];
			}

			if (Winner == null)
			{
				Winner = Entries.FirstOrDefault(o => o.IsWinner);
			}

			if (Serial > _Serial)
			{
				_Serial = Serial;
			}
		}
	}
}