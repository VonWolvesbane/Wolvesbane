using System;
using Server;

namespace Server.Items
{
	public class CaptainJackSparrowsCutlass : Cutlass
	{
		public override int LabelNumber{ get{ return 1063474; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }
		public override float Speed{ get{ return 2.50f; } }

		[Constructable]
		public CaptainJackSparrowsCutlass()
		{
			Hue = 0x5C;
			Attributes.BonusDex = 25;
			Attributes.AttackChance = 50;
			Attributes.WeaponSpeed = 50;
			Attributes.WeaponDamage = 50;
			WeaponAttributes.UseBestSkill = 1;
		}

		public CaptainJackSparrowsCutlass( Serial serial ) : base( serial )
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

			if( Attributes.AttackChance == 50 )
				Attributes.AttackChance = 10;
		}
	}
}