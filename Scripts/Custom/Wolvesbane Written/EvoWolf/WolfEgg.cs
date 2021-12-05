using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Xanthos.Interfaces;

namespace Xanthos.Evo
{
	public class WolfEgg : BaseEvoEgg
	{
		public override IEvoCreature GetEvoCreature()
		{
			return new WolfEvo( "A Wolf" );
		}

		[Constructable]
		public WolfEgg() : base()
		{
			Name = "A Wolf";
			Hue = 0;
			ItemID = 9608;
			HatchDuration = 0.01;		// 15 minutes
		}

		public WolfEgg( Serial serial ) : base ( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}