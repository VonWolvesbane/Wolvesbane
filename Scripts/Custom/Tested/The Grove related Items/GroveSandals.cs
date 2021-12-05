//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 2/22/2015 3:02:41 PM
//=================================================

using System;
using Server;

namespace Server.Items
{
	public class GroveSandals : Sandals
	{

		[Constructable]
		public GroveSandals()
		{
			Name = "Thank You For Playing Wolvesbane";
			Hue = 706;
			LootType = LootType.Blessed;
			Attributes.BonusMana = 15;
			//Attributes.BonusStam = 15;
			Attributes.BonusHits = 15;
			Attributes.Luck = 100;
			//Attributes.CastRecovery = 2;
			//Attributes.CastSpeed = 2;
		}

		public GroveSandals( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

	}
}
