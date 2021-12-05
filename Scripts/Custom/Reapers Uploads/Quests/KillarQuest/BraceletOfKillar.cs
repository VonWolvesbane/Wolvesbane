using System;
using Server;

namespace Server.Items
{
	public class BraceletOfKillar : GoldBracelet
	{

		public override int ArtifactRarity{ get{ return 20; } }

		[Constructable]
		public BraceletOfKillar()
		{
			Name = "Bracelet Of Killar";
			Hue = 1161;
			
		
			Attributes.WeaponDamage = 15;
			Attributes.BonusInt = 25;
			Attributes.RegenMana = 5;
			Attributes.WeaponSpeed = 15;
			Attributes.CastRecovery = 2;
			Attributes.CastSpeed = 2;

			Resistances.Energy = 25;
		
		}

		public BraceletOfKillar( Serial serial ) : base( serial )
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