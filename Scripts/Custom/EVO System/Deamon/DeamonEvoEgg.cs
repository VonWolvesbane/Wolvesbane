using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Xanthos.Interfaces;

namespace Xanthos.Evo
{
	public class DeamonEgg : BaseEvoEgg
	{
		public override IEvoCreature GetEvoCreature()
		{
			return new EvoDeamon( "a deamon baby" );
		}

		[Constructable]
		public DeamonEgg() : base()
		{
			Name = "a deamon egg";
			HatchDuration = 0.01;		// 15 minutes
		}

		public DeamonEgg( Serial serial ) : base ( serial )
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