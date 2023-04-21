//Created By Milva
using System;
using Server.Items;
using Server.Mobiles; 
using Server.Misc;
using Server.Network;

namespace Server.Engines.Craft
{
	public class DefMechanicalTinkering : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Tinkering; }
		}

		public override string GumpTitleString
		{
			get{ return "MechanicalTinkering"; } // <CENTER>Mechanical Tinkering MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefMechanicalTinkering();

				return m_CraftSystem;
			}
		}

		public override CraftECA ECA{ get{ return CraftECA.ChanceMinusSixtyToFourtyFive; } }

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
		}

		private DefMechanicalTinkering() : base( 1, 1, 1.25 )// base( 1, 1, 3.0 )
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
			index = AddCraft( typeof( MechanicalTinkeringTool ), "Tools", "Mechanical Tinkering Tool", 99.9, 100.0, typeof( IronIngot ), "Iron Ingot", 200 );
			AddRes( index, typeof( Board ), "Boards", 50 );

			//Start Mechanical Pet
			index = AddCraft( typeof( MechanicalPetStatue ), "Mechanical Statue Pet", "Mechanical Pet", 110.1, 115.0, typeof( Gears ), "Gears", 4 ); 
			AddRes( index, typeof( IronIngot ), "Iron Ingots", 200 );
                                                AddRes(index, typeof(ClockworkAssembly), "Clockwork Assembly", 1);
                                               AddRes(index, typeof(PowerCrystal), "Power Crystal", 1);

                                               index = AddCraft(typeof(MechanicalBoneDemonPetStatue), "Mechanical BoneDemon Statue Pet", "Mechanical BoneDemon Pet", 110.1, 115.0, typeof(Bone), "Bone", 50);
                                               AddRes(index, typeof(IronIngot), "Iron Ingots", 300);
                                               AddRes(index, typeof(ClockworkAssembly), "Clockwork Assembly", 2);
                                                AddRes(index, typeof(PowerCrystal), "Power Crystal", 1);

                                                index = AddCraft(typeof(MechanicalBlackWidowPetStatue), "Mechanical BlackWidow Statue Pet", "Mechanical BlackWidow Pet", 110.1, 115.0, typeof(SpidersSilk), "Spiders Silk", 50);
                                               AddRes(index, typeof(IronIngot), "Iron Ingots", 300);
                                               AddRes(index, typeof(ClockworkAssembly), "Clockwork Assembly", 2);
                                                AddRes(index, typeof(PowerCrystal), "Power Crystal", 1);

                                                //index = AddCraft(typeof(MechanicalFrenziedOstardPetStatue), "Mechanical FrenziedOstard Statue Pet", "Mechanical FrenziedOstard Pet", 110.1, 115.0, typeof(Feather), "Feather", 60);
                                              //AddRes(index, typeof(IronIngot), "Iron Ingots", 300);
                                               //AddRes(index, typeof(ClockworkAssembly), "Clockwork Assembly", 2);
                                                //AddRes(index, typeof(PowerCrystal), "Power Crystal", 1);

                                                index = AddCraft(typeof(MechanicalHellSteedPetStatue), "Mechanical HellSteed Statue Pet", "Mechanical HellSteed Pet", 110.1, 115.0, typeof(HornedLeather), "Horned Leather", 20);
                                               AddRes(index, typeof(IronIngot), "Iron Ingots", 300);
                                               AddRes(index, typeof(ClockworkAssembly), "Clockwork Assembly", 2);
                                                AddRes(index, typeof(PowerCrystal), "Power Crystal", 1);
                                                
                                                
                                               

			

			

                      
			

	
                       

                       
                          
                          

		}
	}
}
