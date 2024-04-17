//Created By Milva
using System;
using Server.Items;
using Server.Mobiles; 
using Server.Misc;
using Server.Network;

namespace Server.Engines.Craft
{
	public class DefTotemMelding : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Tinkering; }
		}
       
        public override string GumpTitleString
		{
			get{ return "Totem Melding"; } // <CENTER>Totem Melding MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefTotemMelding();

				return m_CraftSystem;
			}
		}

		public override CraftECA ECA{ get{ return CraftECA.ChanceMinusSixtyToFourtyFive; } }

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
		}

		private DefTotemMelding() : base( 1, 1, 1.25 )// base( 1, 1, 3.0 )
		{
		}

        public override int CanCraft(Mobile from, ITool tool, Type itemType)
        {
            if (tool.Deleted || tool.UsesRemaining < 0)
                return 1044038; // You have worn out your tool!

            int num = 0;

            if (!tool.CheckAccessible(from, ref num))
                return num;

            return 0;
        }

        public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x241 );
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

			if ( failed )
			{
				if ( lostMaterial )
					return 1044043; // You failed to create the item, and some of your materials are lost.
				else
					return 1044157; // You failed to create the item, but no materials were lost.
			}
			else
			{	
				return 1044154; // You create the item.
			}
		}

		public override void InitCraftList()
		{
			int index = -1;

			//Start Tools
			index = AddCraft( typeof( TotemMeldingTool ), "Tools", "Welding Tool", 120.0, 150.0, typeof( GargoyleCraftingTool ), "Gargoyle Crafting Tool", 2 );
			AddSkill(index, SkillName.Blacksmith, 120.0, 150.0);
			AddSkill(index, SkillName.Imbuing, 120.0, 150.0);
			//AddRes(index, typeof(ElvenCraftingTool), "Elven Crafting Tool", 2 );
			AddRes(index, typeof(TinkerTools), "Tinker Tools", 2); 
			AddRes( index, typeof( WolvesbanianIngot ), "Wolvesbanian Ingot", 5 );

            




















        }
	}
}
