using System;
using Server;

namespace Server.Items
{
	public class ArrianasEarrings : SilverEarrings
	{

		public override int ArtifactRarity{ get{ return 15; } }

		[Constructable]
		public ArrianasEarrings()
		{
			Name = "Arriana's Earrings";
			Hue = 1154;
			
		
			Attributes.NightSight = 1;
            Attributes.Luck = 2000;
			Attributes.BonusStr = 25;
			Attributes.BonusDex = 25;
			Attributes.RegenStam = 5;
			Attributes.BonusStam = 50;
			Attributes.RegenHits = 5;
			Attributes.BonusHits = 30;
			Resistances.Energy = 5;
            Resistances.Fire = 5;
			Resistances.Cold = 5;
			Resistances.Poison = 5;
            Resistances.Physical = 5;
		
		}

		public ArrianasEarrings( Serial serial ) : base( serial )
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