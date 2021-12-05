using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Xanthos.Evo
{
	public class WolfDust : BaseEvoDust
	{
		[Constructable]
		public WolfDust() : this( 1 )
		{
		}

		[Constructable]
		public WolfDust( int amount ) : base( amount )
		{
			Amount = amount;
			Name = "Wolf Dust";
			Hue = 1157;
		}

		public WolfDust( Serial serial ) : base ( serial )
		{
		}

		public override BaseEvoDust NewDust()
		{
			return new WolfDust();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}