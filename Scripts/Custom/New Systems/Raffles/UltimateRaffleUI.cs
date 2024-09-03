#region Header
//   Vorspire    _,-'/-'/  UltimateRaffleUI.cs
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
using System.Drawing;
using System.Linq;
using System.Text;

using Server;
using Server.Gumps;

using VitaNex.SuperGumps;
using VitaNex.SuperGumps.UI;
#endregion

namespace VitaNex.Modules.UltimateRaffle
{
	public class UltimateRaffleUI : InputDialogGump
	{
		public UltimateRaffle Raffle { get; private set; }

		public int Tickets { get; private set; }
		public double Chance { get; private set; }

		public UltimateRaffleUI(UltimateRaffle raffle, Mobile user, Gump parent)
			: base(user, parent, null, null, "Ultimate Raffle")
		{
			Raffle = raffle;

			Icon = 7012;

			Width = 600;
			Height = 400;

			InputText = "1";
			Limit = 3;

			ForceRecompile = true;

			AutoRefreshRate = TimeSpan.FromSeconds(30.0);
			AutoRefresh = true;
		}

		protected override void Compile()
		{
			if (Raffle != null)
			{
				var count = 0;
				var total = 0;

				foreach (var o in Raffle.Entries.Where(o => o != null && !o.IsDisposed && !o.IsRefunded))
				{
					++total;

					if (o.Mobile == User)
					{
						++count;
					}
				}

				Tickets = count;

				if (total > 0)
				{
					Chance = Tickets / (double)total;
				}
				else if (Tickets > 0)
				{
					Chance = 1.0;
				}
				else
				{
					Chance = 0.0;
				}

				var html = new StringBuilder();

				var c1 = Tickets > 0 ? Color.LawnGreen : Color.OrangeRed;
				var c2 = Color.OrangeRed.Interpolate(Color.LawnGreen, Chance);

				html.AppendLine(
					"You have {0} tickets in this raffle with a {1} chance to win!".WrapUOHtmlColor(Color.Gold, HtmlColor),
					Tickets.ToString("#,0").WrapUOHtmlColor(c1, Color.Gold),
					Chance.ToString("0.#%").WrapUOHtmlColor(c2, Color.Gold));

				html.AppendLine(String.Empty.WrapUOHtmlColor(Color.White, false));

				var opl = Raffle.GetOPLStrings(User);

				foreach (var o in opl.Not(o => o.ContainsAny(true, "[Inactive]", "Use:")))
				{
					html.AppendLine(o.ToUpperWords());
				}

				Html = html.ToString().WrapUOHtmlCenter();
			}

			base.Compile();
		}

		protected override void CompileLayout(SuperGumpLayout layout)
		{
			base.CompileLayout(layout);

			layout.Replace(
				"textentry/body/input",
				() =>
				{
					var text = "Buy Tickets: ";

					text = text.WrapUOHtmlBig();
					text = text.WrapUOHtmlRight();
					text = text.WrapUOHtmlColor(Color.Green, false);

					var w = (Width - 130) / 2;

					AddHtml(25, Height - 43, w, 40, text, false, false);

					if (Limited)
					{
						AddTextEntryLimited(30 + w, Height - 45, w - 5, 20, TextHue, InputText, Limit, ParseInput);
					}
					else
					{
						AddTextEntry(30 + w, Height - 45, w - 5, 20, TextHue, InputText, ParseInput);
					}
				});
		}

		protected override void OnAccept(GumpButton button)
		{
			base.OnAccept(button);

			if (AcceptHandler == null)
			{
				Purchase();
			}
		}

		protected override void ParseInput(string text)
		{
			int count;

			if (!Int32.TryParse(text, out count))
			{
				count = 1;
			}

			count = Math.Max(1, Math.Min(100, count));

			base.ParseInput(count.ToString());
		}

		private void Purchase()
		{
			var purchased = 0;

			if (!String.IsNullOrWhiteSpace(InputText))
			{
				int count;

				Int32.TryParse(InputText, out count);

				count = Math.Max(1, Math.Min(100, count));

				while (--count >= 0)
				{
					if (Raffle.Purchase(User))
					{
						++purchased;
					}
					else
					{
						break;
					}
				}
			}

			if (purchased > 0)
			{
				var cost = Raffle.TicketCost * purchased;

				User.SendMessage(
					"You purchase {0:#,0} ticket{1} for {2:#,0} {3}",
					purchased,
					purchased != 1 ? "s" : String.Empty,
					cost,
					Raffle.CurrencyName);
			}

			Refresh(true);
		}

		protected override void OnDispose()
		{
			Raffle = null;

			base.OnDispose();
		}
	}
}