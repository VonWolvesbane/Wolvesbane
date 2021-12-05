//Created By Milva

using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	[FlipableAttribute( 0x1EB8, 0x1EB9 )]
	public class MechanicalTinkeringTool : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefMechanicalTinkering.CraftSystem; } }

		[Constructable]
		public MechanicalTinkeringTool() : base( 0x1EB8 )
		{
			Weight = 2.0;
			Hue = 1154;
			Name = "Mechanical Tinkering Tool";
		}

		[Constructable]
		public  MechanicalTinkeringTool( int uses ) : base( uses, 0x1EB8 )
		{
			Weight = 2.0;
		}

		public MechanicalTinkeringTool( Serial serial ) : base( serial )
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