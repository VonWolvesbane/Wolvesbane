using System;
using Server;

namespace Server
{
	/* !!!READ HERE!!! before editing this file! A few words of advice. First you can disable breeding and keep leveling 
	system however you cannot disable leveling and keep breeding. Breeding system depends on the leveling system. Please 
	read all notes i have left thur out this file for further help.*/

	public class FSATS
	{
		/***********************************************
		************************************************
		***  Custom Creature Settings. Here you can  ***
		***  adjust what pets do not require tame-   ***
		***  ing skill, Pack Animals, and Creatres   ***
		***  that you do not wish to level/breed     ***
		************************************************
		***********************************************/	

		//Add all pets that do not require any tameing to control/ride
		public static string[] NoTamingNeeded = new string[]
		{
			"Horse",
			"RidableLlama",
			"ForestOstard",
			"DesertOstard",
			"SwampDragon",
			"Ridgeback",
			"PackHorse",
			"PackLlama",
			"Beetle"
		};

		//Add all pack animals for your server here.
		public static string[] PackAnimals = new string[]
		{
			"PackHorse",
			"PackLlama",
			"Beetle"
		};

		//Add all creatures you do not wish to level / breed
		public static string[] NoLevelCreatures = new string[]
		{
			"Golem",
			"CoMWarHorse",
			"MinaxWarHorse",
			"SLWarHorse",
			"TBWarHorse",
			"FactionWarHorse",
			"BBC",
			"DaemonEvo",
			"GolemEvo",
			"LionEvo",
			"MareEvo",
			"RatEvo",
			"ChiefEvo",
			"WispEvo",
			"BeetleEvo",
			"ChargerOfTheFallenEvo",
			"CuSidheEvo",
			"DesertOstardEvo",
			"ForestOstardEvo",
			"FrenziedOstardEvo",
			"HellSteedEvo",
			"KirinEvo",
			"LlamaEvo",
			"PolarBearEvo",
			"RidgebackEvo",
			"SavageRidgebackEvo",
			"ScaledSwampDragonEvo",
			"SteedEvo",
			"SwampDragonEvo",
			"UnicornEvo"
		};

		//Creatures you always want as Female
		public static string[] AlwaysFemale = new string[]
		{
			"Cow",
			"Hind"
		};

		//Creatures You always want as Male
		public static string[] AlwaysMale = new string[]
		{
			"Bull",
			"GreatHart"
		};

		/***********************************************
		************************************************
		***  Here you can turn on or off features    ***
		***  of the FSATS system. Such as Bio system ***
		***  Breeding, Leveling, and Taming BODS you ***
		***  can pick and choice system you want     ***
		************************************************
		***********************************************/

		// Turns on/off Shrink System
		public static readonly bool EnableShrinkSystem = true;

		// Turns on/off Taming Craft
		public static readonly bool EnableTamingCraft = true;

		// Turns on/off Bio Engineering
		public static readonly bool EnableBioEngineer = true;

		// Turns on/off Pet Breeding
		public static readonly bool EnablePetBreeding = true;

		// Turns on/off Pet Leveling (NOTE: If you disable this disable breeding as well.)
		public static readonly bool EnablePetLeveling = true;

		// Turns on/off Taming BODs
		public static readonly bool EnableTamingBODs = true;

		// Enable Bio Shrink
		public static readonly bool EnableBioShrink = false;

		/***********************************************
		************************************************
		***  Below you can set the caps for normal   ***
		***  creatures stats. This keeps them from   ***
		***  being uber with the pet leveling system ***
		***  they cannot pass these caps below.      ***
		************************************************
		***********************************************/

		//Normal Creatures STR Cap
		public static readonly int NormalSTR = 5000;

		//Normal Creatures DEX Cap
		public static readonly int NormalDEX = 3000;

		//Normal Creatures INT Cap
		public static readonly int NormalINT = 5000;

		//Normal Creatures HITS Cap
		public static readonly int NormalHITS = 7000;

		//Normal Creatures STAM Cap
		public static readonly int NormalSTAM = 7000;

		//Normal Creatures MANA Cap
		public static readonly int NormalMANA = 15000;

		//Normal Creatures Min Damage Cap
		public static readonly int NormalMinDam = 45;

		//Normal Creatures Max Damage Cap
		public static readonly int NormalMaxDam = 55;

		//Normal Creatures PhysResist Cap
		public static readonly int NormalPhys = 90;

		//Normal Creatures PhysFire Cap
		public static readonly int NormalFire = 90;

		//Normal Creatures PhysCold Cap
		public static readonly int NormalCold = 90;

		//Normal Creatures PhysEnergy Cap
		public static readonly int NormalEnergy = 90;

		//Normal Creatures PhysPoison Cap
		public static readonly int NormalPoison = 90;

		//Normal Creatures V Armor Cap
		public static readonly int NormalVArmor = 75;

		/***********************************************
		************************************************
		***  Below you can set the caps for bio      ***
		***  creatures stats. This keeps them from   ***
		***  being uber with the pet leveling system ***
		***  they cannot pass these caps below.      ***
		************************************************
		***********************************************/

		//Bio Creatures STR Cap
		public static readonly int BioSTR = 7000;

		//Bio Creatures DEX Cap
		public static readonly int BioDEX = 5000;

		//Bio Creatures INT Cap
		public static readonly int BioINT = 7000;

		//Bio Creatures HITS Cap
		public static readonly int BioHITS = 15000;

		//Bio Creatures STAM Cap
		public static readonly int BioSTAM = 15000;

		//Bio Creatures MANA Cap
		public static readonly int BioMANA = 25000;

		//Bio Creatures Min Damage Cap
		public static readonly int BioMinDam = 145;

		//Bio Creatures Max Damage Cap
		public static readonly int BioMaxDam = 355;

		//Bio Creatures PhysResist Cap
		public static readonly int BioPhys = 90;

		//Bio Creatures PhysFire Cap
		public static readonly int BioFire = 90;

		//Bio Creatures PhysCold Cap
		public static readonly int BioCold = 90;

		//Bio Creatures PhysEnergy Cap
		public static readonly int BioEnergy = 90;

		//Bio Creatures PhysPoison Cap
		public static readonly int BioPoison = 95;

		//Bio Creatures V Armor Cap
		public static readonly int BioVArmor = 75;
	}
}