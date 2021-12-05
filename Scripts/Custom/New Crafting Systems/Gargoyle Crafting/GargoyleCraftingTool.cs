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
	public class GargoyleCraftingTool : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefGargoyleCrafting.CraftSystem; } }

        
        

        [Constructable]
		public GargoyleCraftingTool() : base( 0x1EBA )
		{
            
            Weight = 2.0;
			Hue = 1670;
			Name = "Gargoyle Crafting Tool";
            
    }



        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {

                if (from.Race == Race.Gargoyle == true)


                {
                    from.SendGump(new CraftGump(from, DefGargoyleCrafting.CraftSystem, this, null));
                    

                }

                else
                {
                    from.SendMessage("You are not a Gargoyle, and thus have no clue how to use Gargoyle Crafting Tools!");
                    
                }

            }
        }

        public GargoyleCraftingTool( Serial serial ) : base( serial )
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
