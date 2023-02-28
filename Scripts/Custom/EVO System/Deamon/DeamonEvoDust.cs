using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Xanthos.Evo
{
	public class DeamonDust : BaseEvoDust
	{
		[Constructable]
		public DeamonDust() : this( 1 )
		{
		}

		[Constructable]
		public DeamonDust( int amount ) : base( amount )
		{
			Amount = amount;
			Name = "Deamon Dust";
			Hue = 1153;
		}

		public DeamonDust( Serial serial ) : base ( serial )
		{
		}

		public override BaseEvoDust NewDust()
		{
			return new DeamonDust();
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