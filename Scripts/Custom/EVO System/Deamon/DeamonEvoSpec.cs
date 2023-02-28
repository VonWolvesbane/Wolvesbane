using System;
using Server;

namespace Xanthos.Evo
{
	public sealed class DeamonSpec : BaseEvoSpec
	{
		// This class implements a singleton pattern; meaning that no matter how many times the
		// Instance attribute is used, there will only ever be one of these created in the entire system.
		// Copy this template and give it a new name.  Assign all of the data members of the EvoSpec
		// base class in the constructor.  Your subclass must not be abstract.
		// Never call new on this class, use the Instance attribute to get the instance instead.

		DeamonSpec()
		{
			m_Tamable = true;
			m_MinTamingToHatch = 89.9;
			m_PercentFemaleChance = 0.02;	// Made small to limit access to eggs.
			m_GuardianEggOrDeedChance = .01;
			m_AlwaysHappy = true;
			m_ProducesYoung = true;
			m_PregnancyTerm = 0.10;
			m_AbsoluteStatValues = false;
			m_MaxEvoResistance = 100;
			m_MaxTrainingStage = 3;
			m_CanAttackPlayers = false;

			m_RandomHues = new int[] { 1157, 1175, 1172, 1170, 2703, 2473, 2643, 1156, 2704, 2734, 2669, 2621, 2859, 2716, 2791, 2927, 2974, 1161, 2717, 2652, 2821, 2818, 2730, 2670, 2678, 2630, 2641, 2644, 2592, 2543, 2526, 2338, 2339, 1793, 1980, 1983 };

			m_Skills = new SkillName[7] { SkillName.Magery, SkillName.EvalInt, SkillName.Meditation, SkillName.MagicResist,
										  SkillName.Tactics, SkillName.Wrestling, SkillName.Anatomy };
			m_MinSkillValues = new int[7] { 50, 50, 50, 15, 19, 19, 19 };
			m_MaxSkillValues = new int[7] { 150, 150, 150, 150, 150, 150, 150 };

			m_Stages = new BaseEvoStage[] { new DeamonStageOne(), new DeamonStageTwo(),
											  new DeamonStageThree(), new DeamonStageFour(),
											  new DeamonStageFive(), new DeamonStageSix(),
											  new DeamonStageSeven() };
		}

		// These next 2 lines facilitate the singleton pattern.  In your subclass only change the
		// BaseEvoSpec class name to your subclass of BaseEvoSpec class name and uncomment both lines.
		public static DeamonSpec Instance { get { return Nested.instance; } }
		class Nested { static Nested() { } internal static readonly DeamonSpec instance = new DeamonSpec();}
	}	

	// Define a subclass of BaseEvoStage for each stage in your creature and place them in the
	// array in your subclass of BaseEvoSpec.  See the example classes for how to do this.
	// Your subclass must not be abstract.

	public class DeamonStageOne : BaseEvoStage
	{
		public DeamonStageOne()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 25000; EpMinDivisor = 10; EpMaxDivisor = 5; DustMultiplier = 20;
			BaseSoundID = 0xDB;
			BodyValue = 0x13D; ControlSlots = 2; MinTameSkill = 99.9; VirtualArmor = 30;
			Hue = Evo.Flags.kRandomHueFlag;

			DamagesTypes = new ResistanceType[1] { ResistanceType.Physical };
			MinDamages = new int[1] { 100 };
			MaxDamages = new int[1] { 100 };

			ResistanceTypes = new ResistanceType[1] { ResistanceType.Physical };
			MinResistances = new int[1] { 15 };
			MaxResistances = new int[1] { 15 };

			DamageMin = 15; DamageMax = 20; HitsMin = 200; HitsMax = 250;
			StrMin = 321; StrMax = 334; DexMin = 56; DexMax = 75; IntMin = 76; IntMax = 96;
		}
	}

	public class DeamonStageTwo : BaseEvoStage
	{
		public DeamonStageTwo()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 75000; EpMinDivisor = 20; EpMaxDivisor = 10; DustMultiplier = 20;
			BaseSoundID = 219;
			BodyValue = 39; VirtualArmor = 40;
		
			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 100, 25, 25, 25, 25 };
			MaxDamages = new int[5] { 100, 25, 25, 25, 25 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 20, 20, 20, 20, 20 };
			MaxResistances = new int[5] { 20, 20, 20, 20, 20 };

			DamageMin = 5; DamageMax = 10; HitsMin= 500; HitsMax = 500;
			StrMin = 200; StrMax = 200; DexMin = 20; DexMax = 20; IntMin = 30; IntMax = 30;
		}
	}

	public class DeamonStageThree : BaseEvoStage
	{
		public DeamonStageThree()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 175000; EpMinDivisor = 30; EpMaxDivisor = 20; DustMultiplier = 20;
			BaseSoundID = 0x5A;
			BodyValue = 779; VirtualArmor = 50;
		
			DamagesTypes = null;
			MinDamages = null;
			MaxDamages = null;

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 40, 40, 40, 40, 40 };
			MaxResistances = new int[5] { 40, 40, 40, 40, 40 };

			DamageMin = 5; DamageMax = 10; HitsMin= 100; HitsMax = 100;
			StrMin = 100; StrMax = 100; DexMin = 10; DexMax = 10; IntMin = 20; IntMax = 20;
		}
	}

	public class DeamonStageFour : BaseEvoStage
	{
		public DeamonStageFour()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 3750000; EpMinDivisor = 50; EpMaxDivisor = 40; DustMultiplier = 20;
			BaseSoundID = 362;
			BodyValue = 784; ControlSlots = 3; MinTameSkill = 119.9; VirtualArmor = 60;
		
			DamagesTypes = null;
			MinDamages = null;
			MaxDamages = null;

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 60, 60, 60, 60, 60 };
			MaxResistances = new int[5] { 60, 60, 60, 60, 60 };	

			DamageMin = 5; DamageMax = 10; HitsMin= 100; HitsMax = 100;
			StrMin = 100; StrMax = 100; DexMin = 10; DexMax = 10; IntMin = 120; IntMax = 120;
		}
	}

	public class DeamonStageFive : BaseEvoStage
	{
		public DeamonStageFive()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 7750000; EpMinDivisor = 160; EpMaxDivisor = 40; DustMultiplier = 20;
			BodyValue = 792; VirtualArmor = 70;
		
			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													 ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 100, 50, 50, 50, 50 };
			MaxDamages = new int[5] { 100, 50, 50, 50, 50 };

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 80, 80, 80, 80, 80 };
			MaxResistances = new int[5] { 80, 80, 80, 80, 80 };	

			DamageMin = 25; DamageMax = 25; HitsMin= 100; HitsMax = 100;
			StrMin = 200; StrMax = 200; DexMin = 20; DexMax = 20; IntMin = 120; IntMax = 120;
		}
	}

	public class DeamonStageSix : BaseEvoStage
	{
		public DeamonStageSix()
		{
			EvolutionMessage = "has evolved";
			NextEpThreshold = 15000000; EpMinDivisor = 540; EpMaxDivisor = 480; DustMultiplier = 20;
			BodyValue = 9; VirtualArmor = 170;
		
			DamagesTypes = null;
			MinDamages = null;
			MaxDamages = null;

			ResistanceTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
														ResistanceType.Poison, ResistanceType.Energy };
			MinResistances = new int[5] { 98, 98, 98, 98, 98 };
			MaxResistances = new int[5] { 98, 98, 98, 98, 98 };	

			DamageMin = 25; DamageMax = 25; HitsMin= 800; HitsMax = 800;
			StrMin = 500; StrMax = 500; DexMin = 220; DexMax = 220; IntMin = 420; IntMax = 420;
		}
	}

	public class DeamonStageSeven : BaseEvoStage
	{
		public DeamonStageSeven()
		{
			Title = "The Black Gate";
			EvolutionMessage = "has evolved to its highest form and is now a Black Gate";
			NextEpThreshold = 0; EpMinDivisor = 740; EpMaxDivisor = 660; DustMultiplier = 20;
			BaseSoundID = 362;
			BodyValue = 38; ControlSlots = 4; VirtualArmor = 270;
		
			DamagesTypes = new ResistanceType[5] { ResistanceType.Physical, ResistanceType.Fire, ResistanceType.Cold,
													 ResistanceType.Poison, ResistanceType.Energy };
			MinDamages = new int[5] { 100, 75, 75, 75, 75 };
			MaxDamages = new int[5] { 100, 75, 75, 75, 75 };

			ResistanceTypes = null;
			MinResistances = null;
			MaxResistances = null;	

			DamageMin = 50; DamageMax = 50; HitsMin= 1350; HitsMax = 1400;
			StrMin = 1250; StrMax = 1250; DexMin = 125; DexMax = 35; IntMin = 125; IntMax = 125;
		}
	}
}