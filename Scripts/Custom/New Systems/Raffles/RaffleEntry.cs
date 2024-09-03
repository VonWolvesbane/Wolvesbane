#region Header
//   Vorspire    _,-'/-'/  RaffleEntry.cs
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

using Server;
using Server.Items;
using Server.Mobiles;
#endregion

namespace VitaNex.Modules.UltimateRaffle
{
	[PropertyObject]
	public sealed class RaffleEntry : IDisposable
	{
		[CommandProperty(AccessLevel.GameMaster, true)]
		public bool IsDisposed { get; private set; }

		[CommandProperty(AccessLevel.GameMaster, true)]
		public bool IsRefunded { get; private set; }

		[CommandProperty(AccessLevel.GameMaster, true)]
		public Mobile Mobile { get; private set; }

		[CommandProperty(AccessLevel.GameMaster, true)]
		public DateTime Date { get; private set; }

		[CommandProperty(AccessLevel.GameMaster, true)]
		public int Cost { get; private set; }

		[CommandProperty(AccessLevel.GameMaster, true)]
		public Type Currency { get; private set; }

		public RaffleEntry(Mobile m, DateTime date, int cost, Type currency)
		{
			Mobile = m;
			Date = date;
			Cost = cost;
			Currency = currency;
		}

		public RaffleEntry(GenericReader reader)
		{
			Deserialize(reader);
		}

		~RaffleEntry()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

			IsDisposed = true;

			Mobile = null;
			Date = DateTime.MinValue;
		}

		public void Refund()
		{
			if (IsRefunded)
			{
				return;
			}

			if (Mobile != null && Cost > 0 && Currency != null)
			{
				if (!Currency.IsEqual<Gold>())
				{
					var amount = Cost;

					while (amount > 0)
					{
						var o = Currency.CreateInstanceSafe<Item>();

						if (o == null)
						{
							return;
						}

						if (o.Stackable)
						{
							o.Amount = Math.Min(amount, 60000);
						}

						amount -= o.Amount;

						Mobile.BankBox.DropItem(o);
					}
				}
				else if (!Banker.Deposit(Mobile, Cost))
				{
					return;
				}
			}

			IsRefunded = true;
		}

		public void Serialize(GenericWriter writer)
		{
			writer.SetVersion(0);

			writer.Write(Mobile);
			writer.Write(Date);
			writer.Write(Cost);
			writer.WriteType(Currency);
			writer.Write(IsRefunded);
		}

		public void Deserialize(GenericReader reader)
		{
			reader.GetVersion();

			Mobile = reader.ReadMobile();
			Date = reader.ReadDateTime();
			Cost = reader.ReadInt();
			Currency = reader.ReadType();
			IsRefunded = reader.ReadBool();
		}
	}
}