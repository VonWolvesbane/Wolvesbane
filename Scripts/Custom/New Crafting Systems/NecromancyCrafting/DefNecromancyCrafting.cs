using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Craft
{
	public class DefNecromancyCrafting : CraftSystem
	{
		public override SkillName MainSkill
		{
			get{ return SkillName.Necromancy; }
		}

		public override int GumpTitleNumber
		{
			get { return 1044009; } // <CENTER>INSCRIPTION MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefNecromancyCrafting();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.0; // 0%
		}

		private DefNecromancyCrafting() : base( 1, 1, 1.25 )// base( 1, 2, 1.7 )
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
			from.PlaySound( 0x1F5 ); // magic

			//if ( from.Body.Type == BodyType.Human && !from.Mounted )
			//	from.Animate( 9, 5, 1, true, false, 0 );

			//new InternalTimer( from ).Start();
		}

		// Delay to synchronize the sound with the hit on the anvil
		private class InternalTimer : Timer
		{
			private Mobile m_From;

			public InternalTimer( Mobile from ) : base( TimeSpan.FromSeconds( 0.7 ) )
			{
				m_From = from;
			}

			protected override void OnTick()
			{
				m_From.PlaySound( 0x2A );
			}
		}

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item )
		{
			if ( toolBroken )
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool

			if ( failed )
			{
				from.PlaySound( 65 ); // rune breaking
				if ( lostMaterial )
					return 1044043; // You failed to create the item, and some of your materials are lost.
				else
					return 1044157; // You failed to create the item, but no materials were lost.
			}
			else
			{
				from.PlaySound( 65 ); // rune breaking
				if ( quality == 0 )
					return 502785; // You were barely able to make this item.  It's quality is below average.
				else if ( makersMark && quality == 2 )
					return 1044156; // You create an exceptional quality item and affix your maker's mark.
				else if ( quality == 2 )
					return 1044155; // You create an exceptional quality item.
				else				
					return 1044154; // You create the item.
			}
		}

		public override void InitCraftList()
		{
			int index = AddCraft( typeof( VileCrystal ), "Crystals", "Vile Crystal", 20.0, 120.0, 
				typeof( CorruptCrystal ), "Corrupt Crystal", 1,"You do not have a corrupt crystal." );
				AddRes( index, typeof ( AmberPowder ), "amber powder", 1, "You do not have enough amber powder to make that."  );

			index = AddCraft( typeof( deceptiveCrystal ), "Crystals", "Deceptive Crystal", 30.0, 120.0, 
				typeof( CorruptCrystal ), "Corrupt Crystal", 1,"You do not have a corrupt crystal." );
				AddRes( index, typeof ( WhitePowder ), "white powder", 1, "You do not have enough white powder to make that."  );

			index = AddCraft( typeof( treacherousCrystal ), "Crystals", "Treacherous Crystal", 40.0, 120.0, 
				typeof( CorruptCrystal ), "Corrupt Crystal", 1,"You do not have a corrupt crystal." );
				AddRes( index, typeof ( RedPowder ), "red powder", 1, "You do not have enough red powder to make that."  );

			index = AddCraft( typeof( wickedCrystal ), "Crystals", "Wicked Crystal", 50.0, 120.0, 
				typeof( CorruptCrystal ), "Corrupt Crystal", 1,"You do not have a corrupt crystal." );
				AddRes( index, typeof ( OrangePowder ), "orange powder", 1, "You do not have enough orange powder to make that."  );

			index = AddCraft( typeof( taintedCrystal ), "Crystals", "Tainted Crystal", 20.0, 120.0, 
				typeof( CorruptCrystal ), "Corrupt Crystal", 1,"You do not have a corrupt crystal." );
				AddRes( index, typeof ( PurplePowder ), "purple powder", 1, "You do not have enough purple powder to make that."  );

			index = AddCraft( typeof( SorrowCrystal ), "Crystals", "Sorrow Crystal", 70.0, 120.0, 
				typeof( CorruptCrystal ), "Corrupt Crystal", 1,"You do not have a corrupt crystal." );
				AddRes( index, typeof ( AzulPowder ), "blue powder", 1, "You do not have enough blue powder to make that."  );

			index = AddCraft( typeof( perilousCrystal ), "Crystals", "Perilous Crystal", 80.0, 120.0, 
				typeof( CorruptCrystal ), "Corrupt Crystal", 1,"You do not have a corrupt crystal." );
				AddRes( index, typeof ( TourmalinePowder ), "tourmaline powder", 1, "You do not have enough tourmaline powder to make that."  );

			index = AddCraft( typeof( ominousCrystal ), "Crystals", "Ominous Crystal", 90.0, 120.0, 
				typeof( CorruptCrystal ), "Corrupt Crystal", 1,"You do not have a corrupt crystal." );
				AddRes( index, typeof ( ClearPowder ), "clear powder", 1, "You do not have enough clear powder to make that."  );

			index = AddCraft( typeof( MaliceCrystal ), "Crystals", "Malice Crystal", 100.0, 120.0, 
				typeof( CorruptCrystal ), "Corrupt Crystal", 1,"You do not have a corrupt crystal." );
				AddRes( index, typeof ( GreenPowder ), "green powder", 1, "You do not have enough green powder to make that."  );



			//Skeleton Crafts


			index = AddCraft( typeof( SkelLegs ), "Skeletal", "Skeleton legs", 20.0, 120.0,
				typeof( Bones ), "Bones", 20,"You do not have enough bones for the structure." );
				AddRes( index, typeof ( AnimateDeadScroll ), "Animate Dead Scroll", 1, "You do not have the scroll needed to summon the soul."  );
				//AddRes( index, typeof ( Spine ), "Spine", 1, "You do not have a spine for the structure."  );
				AddSkill( index, SkillName.Anatomy, 20.0, 50.0 );


			index = AddCraft( typeof( SkelBod ), "Skeletal", "Skeleton Torso", 20.0, 120.0,
				typeof( Bones ), "Bones", 30,"You do not have enough bones for the structure." );
				AddRes( index, typeof ( Skull ), "Skull", 1, "You do not have a skull for the structure."  );
				AddRes( index, typeof ( RibCage ), "Rib Cage", 1, "You do not have a rib cage for the structure."  );
				AddRes( index, typeof ( Spine ), "Spine", 1, "You do not have a spine for the structure."  );
				AddSkill( index, SkillName.Anatomy, 20.0, 50.0 );

			index = AddCraft( typeof( SkelMageBod ), "Skeletal", "Compleate Skeletal Torso", 30.0, 130.0,
				typeof( SkelBod ), "Skeleton Torso", 1,"You do not have the skeletons torso needed for the structure." );
				AddRes( index, typeof ( Jawbone ), "Jawbone", 1, "You do not have a jawbone for the structure."  );
				//AddRes( index, typeof ( Spine ), "Spine", 1, "You do not have a spine for the structure."  );
				AddSkill( index, SkillName.Anatomy, 30.0, 60.0 );

			
			// Rotting Crafts



			index = AddCraft( typeof( RottingBod ), "Rotting", "rotting torso", 20.0, 120.0,
				typeof( Head ), "Head", 1,"You do not have a head for the structure." );
				AddRes( index, typeof ( Torso ), "Torso", 1, "You do not have a torso for the structure."  );
				AddRes( index, typeof ( RightArm ), "right arm", 1, "You do not have the right arm for the structure."  );
				AddRes( index, typeof ( LeftArm ), "left arm", 1, "You do not have the left arm for the structure."  );
				AddSkill( index, SkillName.Anatomy, 25.0, 50.0 );

			index = AddCraft( typeof( RottingLegs ), "Rotting", "rotting legs", 20.0, 120.0,
				typeof( LeftLeg ), "Left Leg", 1,"You do not have a left leg for the structure." );
				AddRes( index, typeof ( RightLeg ), "Right Leg", 1, "You do not have a right leg for the structure."  );
				AddRes( index, typeof ( AnimateDeadScroll ), "Animate Dead Scroll", 1, "You do not have the scroll needed to summon the soul."  );
				//AddRes( index, typeof ( LeftArm ), "left arm", 1, "You do not have the left arm for the structure."  );
				AddSkill( index, SkillName.Anatomy, 25.0, 50.0 );
			
			index = AddCraft( typeof( ToxicBod ), "Rotting", "poisioned rotting torso", 20.0, 120.0,
				typeof( RottingBod ), "rotting torso", 1,"You do not have a rotting torsoto poision." );
				AddRes( index, typeof ( GreaterPoisonPotion ), "Greater Poison Potion", 10, "You do not have the potions needed to poision the corpse."  );
				AddRes( index, typeof ( CorpseSkinScroll ), "Corpse Skin Scroll", 1, "You do not have the scroll needed to bind the flesh of the creature."  );
				//AddRes( index, typeof ( LeftArm ), "left arm", 1, "You do not have the left arm for the structure."  );
				AddSkill( index, SkillName.Anatomy, 25.0, 50.0 );

			//Mummy Crafts


			index = AddCraft( typeof( WrappedLegs ), "Wrapped", "Mummified Legs", 40.0, 130.0,
				typeof( SkelLegs ), "Skeleton legs", 1,"You do not have the skeletons legs for the structure." );
				AddRes( index, typeof ( Bandage ), "Bandages", 100, "You do not have the bandages for the wrapping."  );
				AddRes( index, typeof ( EvilOmenScroll ), "Evil Omen Scroll", 1, "You do not have the scroll needed to curse the body."  );
				AddSkill( index, SkillName.Anatomy, 40.0, 80.0 );


			index = AddCraft( typeof( WrappedBod ), "Wrapped", "Mummified Torso", 40.0, 130.0,
				typeof( SkelBod ), "Skeleton Torso", 1,"You do not have the skeletons torso needed for the structure." );
				AddRes( index, typeof ( Bandage ), "Bandages", 100, "You do not have the bandages for the wrapping."  );
				//AddRes( index, typeof ( Spine ), "Spine", 1, "You do not have a spine for the structure."  );
				AddSkill( index, SkillName.Anatomy, 40.0, 80.0 );

			index = AddCraft( typeof( WrappedMageBod ), "Wrapped", "Inscribed Mummified Torso", 50.0, 140.0,
				typeof( SkelMageBod ), "Compleate Skeletal Torso", 1,"You do not have the compleate skeletal torso needed for the structure." );
				AddRes( index, typeof ( Bandage ), "Bandages", 100, "You do not have the bandages for the wrapping."  );
				AddRes( index, typeof ( BlankScroll ), "Blank Scroll", 10, "You do not have the blank scrolls for the scripture."  );
				AddSkill( index, SkillName.Anatomy, 50.0, 80.0 );


			// Phylacery

			index = AddCraft( typeof( Phylacery ), "Phylacery", "Phylacery", 100.0, 130.0,
				typeof( Soul ), "Soul", 1,"You do not have the soul needed to bind in the phylacery." );
				AddRes( index, typeof ( ArcaneGem ), "Arcane Gem", 6, "You do not have the arcane gem to entrap the soul."  );
				AddRes( index, typeof ( ExorcismScroll ), "Exorcism Scroll", 1, "You do not have the scroll needed to expel the soul into a body."  );
				AddRes( index, typeof ( WoodenBox ), "Wooden Box", 1, "You do not have the box needed to craft the phylacery."  );
				AddSkill( index, SkillName.Inscribe, 100.0, 120.0 );
				AddSkill( index, SkillName.Alchemy, 100.0, 120.0 );
				AddSkill( index, SkillName.Anatomy, 100.0, 120.0 );




		}
	}
}
