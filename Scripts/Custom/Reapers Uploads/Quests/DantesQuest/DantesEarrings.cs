using System;
using Server;

namespace Server.Items
{
	public class DantesEarrings : GoldEarrings
	{

		public override int ArtifactRarity{ get{ return 21; } }

		[Constructable]
		public DantesEarrings()
		{
			Name = "Dantes Earrings";
			Hue = 468;
			
		
			Attributes.LowerManaCost = 10;
			Attributes.DefendChance = 10;
                        Attributes.Luck = 150;
			Attributes.BonusMana = 15;
			Attributes.BonusHits = 15;
			Attributes.BonusStam = 15;
			Attributes.RegenHits = 5;
			Attributes.RegenMana = 5;
			Attributes.RegenStam = 5;
			Attributes.CastRecovery = 2;
			Attributes.CastSpeed = 2;
			Resistances.Energy = 5;
                        Resistances.Fire = 5;
			Resistances.Cold = 5;
			Resistances.Poison = 5;
                        Resistances.Physical = 5;
                        SkillBonuses.SetValues( 0, SkillName.MagicResist, 25.0 );
		
		}

		public DantesEarrings( Serial serial ) : base( serial )
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