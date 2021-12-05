using System;
using Server;

namespace Server.Items
{
	public class BraceletElements : GoldBracelet
	{
		public override int LabelNumber{ get{ return 1061103; } } // Bracelet Elements
		public override int ArtifactRarity{ get{ return 11; } }

		[Constructable]
		public BraceletElements()
		{
			Hue = 1167;
			Attributes.Luck = 200;
			Attributes.CastSpeed = 2;
			Attributes.SpellDamage = 55;
			Attributes.CastRecovery = 2;
			Resistances.Fire = 10;
			Resistances.Cold = 10;
			Resistances.Poison =10;
			Resistances.Energy = 10;
                        LootType = LootType.Blessed;
                        Name = "Bracelet of Elements";
		}

		public BraceletElements( Serial serial ) : base( serial )
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