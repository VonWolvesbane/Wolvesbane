using System;
using Server;

namespace Xanthos.Evo
{
	public sealed class WolfEvoSpec : BaseEvoSpec
	{
		// This class implements a singleton pattern; meaning that no matter how many times the
		// Instance attribute is used, there will only ever be one of these created in the entire system.
		// Copy this template and give it a new name.  Assign all of the data members of the EvoSpec
		// base class in the constructor.  Your subclass must not be abstract.
		// Never call new on this class, use the Instance attribute to get the instance instead.

		WolfEvoSpec()
		{
			m_Tamable = true;
			m_MinTamingToHatch = 101.1;
			m_PercentFemaleChance = 0.05;	// Made small to limit access to eggs.
			m_GuardianEggOrDeedChance = .01;
			m_AlwaysHappy = false;
			m_ProducesYoung = true;
			m_PregnancyTerm = 0.01;
			m_AbsoluteStatValues = false;
			m_MaxEvoResistance = 100;
			m_MaxTrainingStage = 1;
			m_MountStage = 3;
			m_CanAttackPlayers = false;

			m_RandomHues = new int[] { 1157, 1175, 1172, 1170, 2703, 2473, 2643, 1156, 2704, 2734, 2669, 2621, 2859, 2716, 2791, 2927, 2974, 1161, 2717, 2652, 2821, 2818, 2730, 2670, 2678, 2630, 2641, 2644, 2592, 2543, 2526, 2338, 2339, 1793, 1980, 1983 };

			m_Skills = new SkillName[7] { SkillName.Chivalry, SkillName.Parry, SkillName.MagicResist,
										  SkillName.Tactics, SkillName.Wrestling, SkillName.Anatomy, SkillName.Healing };
			m_MinSkillValues = new int[7] { 50, 50, 50, 15, 19, 19, 8 };
			m_MaxSkillValues = new int[7] { 200, 200, 200, 200, 200, 200, 200 };

			m_Stages = new BaseEvoStage[] { new WolfEvoStageOne(), new WolfEvoStageTwo(),
											  new WolfEvoStageThree(), new WolfEvoStageFour(),
											  new WolfEvoStageFive() };
		}

		// These next 2 lines facilitate the singleton pattern.  In your subclass only change the
		// BaseEvoSpec class name to your subclass of BaseEvoSpec class name and uncomment both lines.
		public static WolfEvoSpec Instance { get { return Nested.instance; } }
		class Nested { static Nested() { } internal static readonly WolfEvoSpec instance = new WolfEvoSpec();}
	}	

	// Define a subclass of BaseEvoStage for each stage in your creature and place them in the
	// array in your subclass of BaseEvoSpec.  See the example classes for how to do this.
	// Your subclass must not be abstract.

	public class WolfEvoStageOne : BaseEvoStage
	{
		public WolfEvoStageOne()
		{
			EvolutionMessage = "has evolved";
			Title = "Puppy";
			NextEpThreshold = 500000; EpMinDivisor = 10; EpMaxDivisor = 5; DustMultiplier = 20;
			BaseSoundID = 0x85;
			BodyValue = 217; ControlSlots = 2; MinTameSkill = 101.1; VirtualArmor = 30;
			Hue = Evo.Flags.kRandomHueFlag;

			DamagesTypes = new ResistanceType[1] { ResistanceType.Physical };
			MinDamages = new int[1] { 100 };
			MaxDamages = new int[1] { 100 };

			ResistanceTypes = new ResistanceType[1] { ResistanceType.Physical };
			MinResistances = new int[1] { 15 };
			MaxResistances = new int[1] { 15 };

			DamageMin = 25; DamageMax = 30; HitsMin = 250; HitsMax = 350;
			StrMin = 1000; StrMax = 1010; DexMin = 156; DexMax = 175; IntMin = 376; IntMax = 396;
		}
	}

	public class WolfEvoStageTwo : BaseEvoStage
	{
		public WolfEvoStageTwo()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 3250000; EpMinDivisor = 20; EpMaxDivisor = 10; DustMultiplier = 20;
			BaseSoundID = 0xE5;
			BodyValue = 25; VirtualArmor = 40;
		
			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 100, 25, 25, 25, 25 };
			MaxDamages = new int[5] { 100, 25, 25, 25, 25 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 30, 30, 30, 30, 30 };
			MaxResistances = new int[5] { 30, 30, 30, 30, 30 };

			DamageMin = 50; DamageMax = 50; HitsMin= 500; HitsMax = 500;
			StrMin = 350; StrMax = 350; DexMin = 20; DexMax = 20; IntMin = 30; IntMax = 30;
		}
	}

	public class WolfEvoStageThree : BaseEvoStage
	{
		public WolfEvoStageThree()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 7500000; EpMinDivisor = 30; EpMaxDivisor = 20; DustMultiplier = 20;
			BaseSoundID = 0xE5;
			BodyValue = 739; VirtualArmor = 50;
		
			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 100, 50, 50, 50, 50 };
			MaxDamages = new int[5] { 100, 50, 50, 50, 50 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 60, 60, 60, 60, 60 };
			MaxResistances = new int[5] { 60, 60, 60, 60, 60 };

			DamageMin = 50; DamageMax = 55; HitsMin= 300; HitsMax = 300;
			StrMin = 350; StrMax = 350; DexMin = 10; DexMax = 10; IntMin = 20; IntMax = 20;
		}
	}
	public class WolfEvoStageFour : BaseEvoStage
	{
		public WolfEvoStageFour()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 15000000; EpMinDivisor = 540; EpMaxDivisor = 480; DustMultiplier = 50;
			BodyValue = 277; VirtualArmor = 170; ControlSlots = 7;
		
			DamagesTypes = null;
			MinDamages = null;
			MaxDamages = null;

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 98, 98, 98, 98, 98 };
			MaxResistances = new int[5] { 98, 98, 98, 98, 98 };	

			DamageMin = 45; DamageMax = 50; HitsMin= 1000; HitsMax = 1000;
			StrMin = 450; StrMax = 450; DexMin = 50; DexMax = 50; IntMin = 130; IntMax = 130;
		}
	}

	public class WolfEvoStageFive : BaseEvoStage
	{
		public WolfEvoStageFive()
		{
			Title = "The Elder Wolf";
			EvolutionMessage = "has evolved to its highest form and is now an Elder Wolf!";
			NextEpThreshold = 0; EpMinDivisor = 740; EpMaxDivisor = 660; DustMultiplier = 50;
			BaseSoundID = 1517;
			BodyValue = 719; ControlSlots = 8; VirtualArmor = 270;
			
		
			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													 ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 100, 100, 100, 100, 100 };
			MaxDamages = new int[5] { 100, 100, 100, 100, 100 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 100, 100, 100, 100, 100 };
			MaxResistances = new int[5] { 100, 100, 100, 100, 100 };		

			DamageMin = 275; DamageMax = 575; HitsMin= 2350; HitsMax = 4500;
			StrMin = 1700; StrMax = 2500; DexMin = 135; DexMax = 235; IntMin = 155; IntMax = 255;
		}
	}
}
