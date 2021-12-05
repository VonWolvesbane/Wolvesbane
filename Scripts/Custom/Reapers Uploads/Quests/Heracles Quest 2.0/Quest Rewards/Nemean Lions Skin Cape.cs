using System; 
using Server.Network; 
using Server.Targeting; 
using Server.Items; 

namespace Server.Items 
	{ 
	[Flipable( 0x230A, 0x2309 )]
	public class NemeanSkin : BaseCloak

	{

		[Constructable]
		public NemeanSkin() : base( 0x230A )
		{
			Weight = 4.0;
			Hue= 0x497;
			Attributes.BonusStr = 15;
			Attributes.BonusDex = 15;
			Attributes.BonusInt = 15;
			Attributes.Luck = 150;
			Attributes.NightSight = 1;
			Attributes.CastRecovery = 3;
			Attributes.CastSpeed = 1;
			Attributes.WeaponSpeed = 20;
			Resistances.Cold = 15;
		}

		public NemeanSkin( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}