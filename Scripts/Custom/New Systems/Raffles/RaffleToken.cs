#region Header
//   Vorspire    _,-'/-'/  RaffleToken.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2019  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using Server;

using VitaNex.Items;
#endregion

namespace VitaNex.Modules.UltimateRaffle
{
	public sealed class RaffleToken : VendorToken
	{
		public override double DefaultWeight { get { return 0.01; } }

		public override string DefaultName { get { return "Raffle Token"; } }

		[Constructable]
		public RaffleToken()
			: this(1)
		{ }

		[Constructable]
		public RaffleToken(int amount)
			: base(amount)
		{
			Name = "Raffle Token";
			Hue = 68;
			Weight = 0.01;
		}

		public RaffleToken(Serial serial) 
			: base(serial)
		{ }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.SetVersion(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.GetVersion();
		}
	}
}