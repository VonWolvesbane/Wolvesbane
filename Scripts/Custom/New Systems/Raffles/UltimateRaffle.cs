#region Header
//   Vorspire    _,-'/-'/  UltimateRaffle.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2019  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#if ServUO58
#define ServUOX
#endif

#region References
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using Server;
using Server.Commands.Generic;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;

using VitaNex.Network;
#endregion

namespace VitaNex.Modules.UltimateRaffle
{
	public sealed class UltimateRaffle : Item
	{
		public static Dictionary<int, RaffleResults> History { get; private set; }

		public static List<UltimateRaffle> Instances { get; private set; }

		public static PollTimer InstanceTimer { get; private set; }

		static UltimateRaffle()
		{
			History = new Dictionary<int, RaffleResults>();

			Instances = new List<UltimateRaffle>();

			InstanceTimer = PollTimer.FromMinutes(1.0, PollInstances, () => Instances.Count > 0);
		}

		private static void PollInstances()
		{
			Instances.RemoveAll(o => o == null || o.Deleted);
			Instances.ForEachReverse(o => o.Slice());
		}

		public static void Configure()
		{
			CommandUtility.Register(
				"Raffles",
				AccessLevel.GameMaster,
				e =>
				{
					if (Instances.Count == 0)
					{
						return;
					}

					var cols = new[] {"Object"};
					var list = new ArrayList(Instances);

					e.Mobile.SendGump(new InterfaceGump(e.Mobile, cols, list, 0, null));
				});

			CommandUtility.Register("RafflesLog", AccessLevel.Administrator, e =>
			{
				if (History.Count == 0)
				{
					e.Mobile.SendMessage("There is no raffle history to export.");
					return;
				}

				using (var w = File.CreateText("ultimate_raffle.log"))
				{
					foreach (var h in History.Values)
					{
						h.Log(w);
					}
				}

				e.Mobile.SendMessage("Exported 'ultimate_raffle.log'");
			});

			CommandUtility.Register("RafflesLogClear", AccessLevel.Administrator, e =>
			{
				if (History.Count == 0)
				{
					e.Mobile.SendMessage("There is no raffle history to clear.");
					return;
				}

				History.Clear();

				e.Mobile.SendMessage("Cleared raffle history.");
			});

			EventSink.WorldLoad += () => Persistence.Deserialize("Saves/Raffle/History.bin", DeserializeHistory);
			EventSink.WorldSave += e => Persistence.Serialize("Saves/Raffle/History.bin", SerializeHistory);
		}

		private static void SerializeHistory(GenericWriter writer)
		{
			writer.SetVersion(0);

			writer.WriteBlockDictionary(
				History,
				(w, k, v) =>
				{
					w.Write(k);
					v.Serialize(w);
				});
		}

		private static void DeserializeHistory(GenericReader reader)
		{
			reader.GetVersion();

			History = reader.ReadBlockDictionary(
				r =>
				{
					var k = r.ReadInt();
					var v = new RaffleResults(r);

					return new KeyValuePair<int, RaffleResults>(k, v);
				},
				History);
		}

		private static Item DupeItem(Item item)
		{
			try
			{
				var t = item.GetType();

				Item o;

				try
				{
					o = Activator.CreateInstance(t, true) as Item;
				}
				catch
				{
					o = null;
				}

				if (o == null)
				{
					return null;
				}

				CopyProperties(item, o);

				o.Parent = null;

				item.OnAfterDuped(o);

				if (item is Container && o is Container)
				{
					DupeChildren((Container)item, (Container)o);
				}

				item.Delta(ItemDelta.Update);

				return o;
			}
			catch
			{
				return null;
			}
		}

		private static void DupeChildren(Item src, Item dest)
		{
			foreach (var item in src.Items)
			{
				try
				{
					var t = item.GetType();

					Item o;

					try
					{
						o = Activator.CreateInstance(t, true) as Item;
					}
					catch
					{
						o = null;
					}

					if (o == null)
					{
						continue;
					}

					CopyProperties(item, o);

					o.Parent = null;

					item.OnAfterDuped(o);

					if (item is Container && o is Container)
					{
						DupeChildren((Container)item, (Container)o);
					}

					dest.AddItem(o);
					o.Location = item.Location;

					o.UpdateTotals();
					o.InvalidateProperties();
					o.Delta(ItemDelta.Update);

					item.Delta(ItemDelta.Update);
				}
				catch
				{ }
			}
		}

		private static void CopyProperties(object src, object dest)
		{
			var props = src.GetType().GetProperties();

			foreach (var p in props)
			{
				try
				{
					if (p.CanRead && p.CanWrite)
					{
						p.SetValue(dest, p.GetValue(src, null), null);
					}
				}
				catch
				{ }
			}
		}

		private long _NextPropsUpdate;

		private Type _CurrencyTypeCache;

		[CommandProperty(AccessLevel.GameMaster)]
		public List<RaffleEntry> Entries { get; private set; }

		private bool _Active;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Active
		{
			get { return _Active; }
			set
			{
				if (!_Active && value)
				{
					StartRaffle();
				}
				else if (_Active && !value)
				{
					EndRaffle();
				}

				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool AutoRestart { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool AutoDelete { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool RewardDupe { get; set; }

		[CommandProperty(AccessLevel.GameMaster, true)]
		public Mobile Winner { get; private set; }

		[CommandProperty(AccessLevel.GameMaster, true)]
		public int WinnerTickets { get; private set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public Item RewardItem { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int TicketCost { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public int TicketsReserve { get; set; }

		[CommandProperty(AccessLevel.GameMaster, true)]
		public int TicketsSold { get; private set; }

		[CommandProperty(AccessLevel.GameMaster, true)]
		public long TicketsSoldTotal { get; private set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public ItemTypeSelectProperty CurrencyType { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public string CurrencyName { get; set; }

		[CommandProperty(AccessLevel.GameMaster, true)]
		public int CurrencyTaken { get; private set; }

		[CommandProperty(AccessLevel.GameMaster, true)]
		public long CurrencyTakenTotal { get; private set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime StartDate { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan Duration { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime ExpireDate
		{
			get
			{
				if (StartDate == DateTime.MinValue)
				{
					return DateTime.MaxValue;
				}

				return StartDate + Duration;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan TimeRemaining { get { return ExpireDate - DateTime.UtcNow; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public bool IsValidReward { get { return RewardItem != null && !RewardItem.Deleted && RewardItem != this; } }

		public override bool DisplayWeight { get { return false; } }
		public override bool DisplayLootType { get { return false; } }

		[Constructable]
		public UltimateRaffle()
			: base(0xED6)
		{
			Entries = new List<RaffleEntry>();

			CurrencyType = typeof(RaffleToken);
			CurrencyName = "Raffle Token";

			TicketCost = 1;

			Duration = TimeSpan.FromDays(3);
			StartDate = DateTime.MinValue;

			_CurrencyTypeCache = CurrencyType.InternalType;

			Name = "Ultimate Raffle";
			Active = false;
			Hue = 1150;
			Movable = false;

			Instances.Add(this);
		}

		public UltimateRaffle(Serial serial)
			: base(serial)
		{
			Instances.Add(this);
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			string text;

			var opl = new ExtendedOPL(list);

			if (!Active || !IsValidReward)
			{
				text = "[Inactive]";
				text = text.WrapUOHtmlColor(Color.OrangeRed);

				opl.Add(text);

				if (Winner != null && !Winner.Deleted)
				{
					text = Winner.GetFullName(true);
					text = text.WrapUOHtmlColor(Color.LawnGreen);

					opl.Add("Previous Winner: {0}", text);
				}

				opl.Apply();
				return;
			}

			text = "Use: Purchase A Raffle Ticket!";
			text = text.WrapUOHtmlColor(Color.LawnGreen);

			opl.Add(text);

			text = String.Format("Ticket Price: {0:#,0} {1}", TicketCost, CurrencyName);
			text = text.WrapUOHtmlColor(Color.SkyBlue);

			opl.Add(text);
			opl.Add(String.Empty);

			text = RewardItem.ResolveName();

			opl.Add("Current Prize: {0}", text);

			text = TimeRemaining.ToSimpleString(@"!<d\d ><h\h >m\m");
			text = text.WrapUOHtmlColor(Color.OrangeRed);

			opl.Add("Time Remaining: {0}", text);

			Color color;

			if (TicketsReserve > 0)
			{
				color = Color.OrangeRed.Interpolate(Color.LawnGreen, TicketsSold / (double)TicketsReserve);
				text = String.Format("{0:#,0} / {1:#,0}", TicketsSold, TicketsReserve);
			}
			else if (TicketsSold > 0)
			{
				color = Color.LawnGreen;
				text = TicketsSold.ToString("#,0");
			}
			else
			{
				color = Color.OrangeRed;
				text = TicketsSold.ToString("#,0");
			}

			text = text.WrapUOHtmlColor(color);

			opl.Add("Tickets Sold: {0}", text);

			opl.Add("{0} Taken: {1:#,0}", CurrencyName, CurrencyTaken);

			if (Winner != null && !Winner.Deleted)
			{
				text = Winner.RawName;
				text = text.WrapUOHtmlColor(Winner.GetNotorietyColor());

				opl.Add("Previous Winner: {0}", text);
			}

			opl.Apply();
		}

		public override void OnDoubleClick(Mobile m)
		{
			if (Active && IsValidReward && this.CheckDoubleClick(m, true, false, 5))
			{
				new UltimateRaffleUI(this, m, null).Send();
			}
			else if (m != null && m.AccessLevel >= AccessLevel.GameMaster)
			{
				m.SendGump(new PropertiesGump(m, this));
			}
		}

		private void Slice()
		{
			if (!World.Loaded || World.Loading)
			{
				return;
			}

			if (_CurrencyTypeCache != CurrencyType.InternalType)
			{
				if (CurrencyType.InternalType != null)
				{
					CurrencyName = CurrencyType.InternalType.Name.SpaceWords();
				}

				Entries.ForEachReverse(
					o =>
					{
						if (o != null && o.Currency == _CurrencyTypeCache)
						{
							o.Refund();
						}
					});

				CurrencyTaken = 0;
				CurrencyTakenTotal = 0;

				_CurrencyTypeCache = CurrencyType.InternalType;
			}

			Entries.RemoveAll(o => o == null || o.Mobile == null || o.Mobile.Deleted || o.IsDisposed || o.IsRefunded);

			if (!Active)
			{
				if (Entries.Count > 0)
				{
					World.Broadcast(33, false, "A raffle has ended, the prize is no longer available. All tickets have been refunded.");

					Destroy(true);
				}

				return;
			}

			if (!IsValidReward)
			{
				World.Broadcast(33, false, "A raffle has ended, the prize is no longer available. All tickets have been refunded.");

				Destroy(true);

				_Active = false;
				RewardItem = null;

				return;
			}

			if (TimeRemaining <= TimeSpan.Zero)
			{
				EndRaffle();
			}

			if (Core.TickCount >= _NextPropsUpdate)
			{
				InvalidateProperties();

				_NextPropsUpdate = Core.TickCount + 10000;
			}
		}

		public bool Purchase(Mobile m)
		{
			if (!Active || m == null || CurrencyType == null || CurrencyType.InternalType == null)
			{
				return false;
			}

			var isGold = CurrencyType.InternalType.IsEqual<Gold>();

			var bought = false;

			if (isGold)
			{
				bought = Banker.Withdraw(m, TicketCost);
			}

			if (!bought)
			{
				bought = m.Backpack.ConsumeTotal(CurrencyType.InternalType, TicketCost);
			}

			if (!bought)
			{
				m.SendMessage("You do not have sufficient funds to purchase a ticket.");
				return false;
			}

			++TicketsSold;

			CurrencyTaken += TicketCost;

			Entries.Add(new RaffleEntry(m, DateTime.UtcNow, TicketCost, CurrencyType.InternalType));

			return true;
		}

		private void RefundAll()
		{
			Entries.ForEachReverse(o => o.Refund());

			InvalidateProperties();
		}

		private void DisposeAll()
		{
			Entries.ForEachReverse(o => o.Dispose());
			Entries.Clear();

			InvalidateProperties();
		}

		private void RecordStats()
		{
			try
			{
				var o = new RaffleResults(this);

				History[o.Serial] = o;
			}
			catch
			{ }

			CurrencyTakenTotal += CurrencyTaken;
			TicketsSoldTotal += TicketsSold;

			InvalidateProperties();
		}

		private void ResetStats()
		{
			CurrencyTaken = 0;
			TicketsSold = 0;

			InvalidateProperties();
		}

		private void Destroy(bool refund)
		{
			if (refund)
			{
				RefundAll();
			}

			DisposeAll();
			ResetStats();

			InvalidateProperties();
		}

		public void StartRaffle()
		{
			if (Active || !IsValidReward)
			{
				return;
			}

			StartDate = DateTime.UtcNow;

			_Active = true;

			InvalidateProperties();
		}

		private void EndRaffle()
		{
			if (!Active)
			{
				return;
			}

			_Active = false;

			if (Entries.Count == 0)
			{
				World.Broadcast(33, false, "The raffle for {0} has ended, no tickets were purchased.", RewardItem.ResolveName());

				Destroy(false);

				if (AutoDelete)
				{
					Delete();
				}
				else if (AutoRestart)
				{
					StartDate = DateTime.UtcNow;
					_Active = true;
				}

				return;
			}

			if (TicketsReserve > 0 && Entries.Count < TicketsReserve)
			{
				World.Broadcast(
					33,
					false,
					"The raffle for {0} has ended, the reserve amount of tickets has not been met. All tickets have been refunded.",
					RewardItem.ResolveName());

				Destroy(true);

				if (AutoDelete)
				{
					Delete();
				}
				else if (AutoRestart)
				{
					StartDate = DateTime.UtcNow;
					_Active = true;
				}

				return;
			}

			RecordStats();
			ResetStats();

			var rand = Utility.RandomMinMax(1, 5);

			while (--rand >= 0)
			{
				Entries.Shuffle();
			}

			RaffleEntry winner = null;
			Item reward = null;

			if (IsValidReward)
			{
				reward = RewardItem;

				BaseHouse house = null;

				if (reward is HouseSign)
				{
					house = ((HouseSign)reward).Owner;
				}
				else if (reward is BaseHouse)
				{
					house = (BaseHouse)reward;
				}

				if (house != null)
				{
#if !ServUOX
					if (RewardDupe)
					{
						reward = house.GetDeed();
					}
#endif
				}
				else if (RewardDupe)
				{
					reward = DupeItem(RewardItem);

					if (reward != null)
					{
						reward.Internalize();
					}
					else
					{
						reward = RewardItem;
					}
				}

				if (reward != null)
				{
					var index = Entries.Count;

					while (--index >= 0)
					{
						if (!Entries.InBounds(index))
						{
							continue;
						}

						winner = Entries[index];

						if (winner == null)
						{
							Entries.RemoveAt(index);
							continue;
						}

						if (reward == house)
						{
							house.Owner = winner.Mobile;

							if (house.Owner == winner.Mobile)
							{
								break;
							}
						}
						else
						{
							winner.Mobile.BankBox.DropItem(reward);

							if (reward.RootParent == winner.Mobile)
							{
								reward.Movable = true;
								break;
							}
						}

						foreach (var o in Entries.RemoveAllFind(o => o == null || o.Mobile == winner.Mobile))
						{
							o.Dispose();
						}

						winner = null;
					}
				}
			}

			if (winner != null)
			{
				Winner = winner.Mobile;
				WinnerTickets = Entries.Count(o => o != winner && o.Mobile == winner.Mobile);

				World.Broadcast(
					33,
					false,
					"The raffle for {0} has ended, the winner is {1}!",
					reward.ResolveName(),
					Winner.GetFullName(false));

				DisposeAll();

				if (!RewardDupe || RewardItem == reward)
				{
					RewardItem = null;
				}

				if (AutoDelete)
				{
					Delete();
				}
				else if (RewardItem != null && AutoRestart)
				{
					StartDate = DateTime.UtcNow;
					_Active = true;
				}
				else
				{
					StartDate = DateTime.MinValue;
					_Active = false;
				}
			}
			else
			{
				Winner = null;
				WinnerTickets = 0;

				bool delete;

				if (AutoRestart)
				{
					World.Broadcast(
						33,
						false,
						"The raffle for {0} has ended, there was no eligible winner! The raffle has been extended!",
						reward.ResolveName());

					StartDate = DateTime.UtcNow;
					_Active = true;

					delete = false;
				}
				else
				{
					World.Broadcast(
						33,
						false,
						"The raffle for {0} has ended, there was no eligible winner! All tickets have been refunded!",
						reward.ResolveName());

					Destroy(true);

					delete = AutoDelete;
				}

				if (RewardDupe && reward != null && reward != RewardItem)
				{
					reward.Delete();
				}

				if (delete)
				{
					Delete();
				}
				else
				{
					InvalidateProperties();
				}
			}
		}

		public override void OnDelete()
		{
			if (Active)
			{
				Destroy(true);

				RewardItem = null;

				StartDate = DateTime.MinValue;
				_Active = false;
			}

			base.OnDelete();

			Instances.Remove(this);
		}

		public override void OnAfterDelete()
		{
			if (Active)
			{
				Destroy(true);

				RewardItem = null;

				StartDate = DateTime.MinValue;
				_Active = false;
			}

			base.OnAfterDelete();

			Instances.Remove(this);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);

			writer.Write(_Active);

			writer.Write(AutoRestart);
			writer.Write(AutoDelete);

			writer.Write(RewardItem);
			writer.Write(RewardDupe);

			writer.Write(Winner);
			writer.Write(WinnerTickets);

			writer.Write(TicketCost);
			writer.Write(TicketsReserve);
			writer.Write(TicketsSold);
			writer.Write(TicketsSoldTotal);

			CurrencyType.Serialize(writer);

			writer.Write(CurrencyName);
			writer.Write(CurrencyTaken);
			writer.Write(CurrencyTakenTotal);

			writer.Write(StartDate);
			writer.Write(Duration);

			writer.WriteList(Entries, (w, e) => e.Serialize(w));
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();

			_Active = reader.ReadBool();

			AutoRestart = reader.ReadBool();
			AutoDelete = reader.ReadBool();

			RewardItem = reader.ReadItem();
			RewardDupe = reader.ReadBool();

			Winner = reader.ReadMobile();
			WinnerTickets = reader.ReadInt();

			TicketCost = reader.ReadInt();
			TicketsReserve = reader.ReadInt();
			TicketsSold = reader.ReadInt();
			TicketsSoldTotal = reader.ReadLong();

			CurrencyType = new ItemTypeSelectProperty(reader);

			CurrencyName = reader.ReadString();
			CurrencyTaken = reader.ReadInt();
			CurrencyTakenTotal = reader.ReadLong();

			StartDate = reader.ReadDateTime();
			Duration = reader.ReadTimeSpan();

			Entries = reader.ReadList(r => new RaffleEntry(r), Entries);

			_CurrencyTypeCache = CurrencyType.InternalType;
		}
	}
}
