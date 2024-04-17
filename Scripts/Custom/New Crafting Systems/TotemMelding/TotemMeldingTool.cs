//Written By Von Wolvesbane Wolvesbane UO
using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.Craft;
using Server.Engines.XmlSpawner2;
using Server.Factions;
using Server.Network;
using Server.Mobiles;
using AMA = Server.Items.ArmorMeditationAllowance;
using AMT = Server.Items.ArmorMaterialType;
using ABT = Server.Items.ArmorBodyType;
using System.Linq;
using Server;
using Server.Items;


namespace Server.Items
{
	[FlipableAttribute( 0x1EBA )]
	public class TotemMeldingTool : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefTotemMelding.CraftSystem; } }

        
        

        [Constructable]
		public TotemMeldingTool() : base( 0x1EBA )
		{
            
            Weight = 2.0;
			Hue = 1670;
			Name = "Welding Tool";
            
    }



        

        public TotemMeldingTool( Serial serial ) : base( serial )
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
