//Created By Milva
using System;
using Server.Items;
using Server.Mobiles; 
using Server.Misc;
using Server.Network;

namespace Server.Engines.Craft
{
	public class DefGargoyleCrafting : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Tinkering; }
		}
       
        public override string GumpTitleString
		{
			get{ return "Gargoyle Crafting"; } // <CENTER>Gargoyle Crafting MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefGargoyleCrafting();

				return m_CraftSystem;
			}
		}

		public override CraftECA ECA{ get{ return CraftECA.ChanceMinusSixtyToFourtyFive; } }

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
		}

		private DefGargoyleCrafting() : base( 1, 1, 1.25 )// base( 1, 1, 3.0 )
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
			index = AddCraft( typeof( GargoyleCraftingTool ), "Tools", "Gargoyle Crafting Tool", 50.1, 85.0, typeof( ShadowIronIngot ), "ShadowIron Ingot", 200 );
			AddRes( index, typeof( Board ), "Boards", 50 );

            index = AddCraft(typeof(GargoylesPickaxe), "Tools", "Gargoyle's Pickaxe", 99.9, 100.0, typeof(ShadowIronIngot), "ShadowIron Ingot", 50);
            

            index = AddCraft(typeof(GargoylesAxe), "Tools", "Gargoyle's Axe", 99.9, 100.0, typeof(ShadowIronIngot), "ShadowIron Ingot", 50);
            

            index = AddCraft(typeof(GargoylesKnife), "Tools", "Gargoyle's Knife", 99.9, 100.0, typeof(ShadowIronIngot), "ShadowIron Ingot", 50);

            index = AddCraft(typeof(GargoyleFirePick), "Tools", "Gargoyle's Fire Pickaxe", 99.9, 100.0, typeof(ShadowIronIngot), "ShadowIron Ingot", 50);
            AddRes(index, typeof(BlazeIngot), "Blaze Ingot", 50);

            //Start Gargoyle Minions
            index = AddCraft( typeof( GargoyleMinionStatue ), "Gargoyle Minion Statue", "Gargoyle Minion", 90.1, 105.0, typeof( Gears ), "Gears", 4 ); 
			AddRes( index, typeof( Granite ), "Granite", 400 );
                                                AddRes(index, typeof(ClockworkAssembly), "Clockwork Assembly", 1);
                                               AddRes(index, typeof(PowerCrystal), "Power Crystal", 1);

            index = AddCraft(typeof(IceGargoyleMinionStatue), "Gargoyle Minion Statue", "Ice Gargoyle Minion", 95.1, 105.0, typeof(Gears), "Gears", 4);
            AddRes(index, typeof(IceGranite), "Ice Granite", 400);
            AddRes(index, typeof(ClockworkAssembly), "Clockwork Assembly", 1);
            AddRes(index, typeof(PowerCrystal), "Power Crystal", 1);

            index = AddCraft(typeof(BlazeGargoyleMinionStatue), "Gargoyle Minion Statue", "Blaze Gargoyle Minion", 100.1, 110.0, typeof(Gears), "Gears", 4);
            AddRes(index, typeof(BlazeGranite), "Blaze Granite", 400);
            AddRes(index, typeof(ClockworkAssembly), "Clockwork Assembly", 1);
            AddRes(index, typeof(PowerCrystal), "Power Crystal", 1);

            index = AddCraft(typeof(ToxicGargoyleMinionStatue), "Gargoyle Minion Statue", "Toxic Gargoyle Minion", 110.1, 115.0, typeof(Gears), "Gears", 4);
            AddRes(index, typeof(ToxicGranite), "Toxic Granite", 400);
            AddRes(index, typeof(ClockworkAssembly), "Clockwork Assembly", 1);
            AddRes(index, typeof(PowerCrystal), "Power Crystal", 1);

            index = AddCraft(typeof(ElectrumGargoyleMinionStatue), "Gargoyle Minion Statue", "Electrum Gargoyle Minion", 115.1, 120.0, typeof(Gears), "Gears", 4);
            AddRes(index, typeof(ElectrumGranite), "Electrum Granite", 400);
            AddRes(index, typeof(ClockworkAssembly), "Clockwork Assembly", 1);
            AddRes(index, typeof(PowerCrystal), "Power Crystal", 1);

            index = AddCraft(typeof(PlatinumGargoyleMinionStatue), "Gargoyle Minion Statue", "Platinum Gargoyle Minion", 120.1, 140.0, typeof(Gears), "Gears", 4);
            AddRes(index, typeof(PlatinumGranite), "Platinum Granite", 400);
            AddRes(index, typeof(ClockworkAssembly), "Clockwork Assembly", 1);
            AddRes(index, typeof(PowerCrystal), "Power Crystal", 1);




















        }
	}
}
