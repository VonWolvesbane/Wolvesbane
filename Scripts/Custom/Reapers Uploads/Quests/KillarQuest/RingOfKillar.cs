using System;
using Server;

namespace Server.Items
{
	public class RingOfKillar : GoldRing
	{

		public override int ArtifactRarity{ get{ return 20; } }

		[Constructable]
		public RingOfKillar()
		{
			Name = "Ring Of Killar";
			Hue = 1161;
			
			Attributes.RegenHits = 5;
			Attributes.BonusInt = 20;
			Attributes.RegenMana = 20;
			Attributes.BonusStr = 25;
			Attributes.CastRecovery = 2;
			Attributes.CastSpeed = 2;

			Resistances.Fire = 15;
		
		}

		public RingOfKillar( Serial serial ) : base( serial )
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