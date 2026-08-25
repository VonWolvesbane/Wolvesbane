using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.BossDrops
{
    public class BossDropDefinition
    {
        public string Key;
        public string BossName;
        public string SetName;
        public string Category;
        public BossDropDisplayRace Race;
        public bool Female;
        public Type[] ItemTypes;
        public bool MaxEvolution;

        public BossDropDefinition(string key, string bossName, string setName, string category, BossDropDisplayRace race, bool female, params Type[] itemTypes)
            : this(key, bossName, setName, category, race, female, false, itemTypes)
        {
        }

        public BossDropDefinition(string key, string bossName, string setName, string category, BossDropDisplayRace race, bool female, bool maxEvolution, params Type[] itemTypes)
        {
            Key = key;
            BossName = bossName;
            SetName = setName;
            Category = category;
            Race = race;
            Female = female;
            MaxEvolution = maxEvolution;
            ItemTypes = itemTypes;
        }

        public string Label
        {
            get
            {
                string race = Race == BossDropDisplayRace.Human ? "" : " - " + Race.ToString() + " Only";
                return BossName + " - " + SetName + race;
            }
        }
    }

    public static class BossDropRegistry
    {
        private static readonly List<BossDropDefinition> m_Definitions = new List<BossDropDefinition>();

        public static IList<BossDropDefinition> Definitions { get { return m_Definitions.AsReadOnly(); } }

        public static void Initialize()
        {
            if (m_Definitions.Count > 0)
                return;

            // Confirmed from Custom/Tested/Idium/Idium.cs
            // Keep the original key as the unleveled display so existing placed mannequins continue to refresh correctly.
            Register(new BossDropDefinition(
                "idium-evolution", "Idium", "Evolution Set - Unleveled", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(DragonArmsOfEvolution), typeof(DragonChestOfEvolution), typeof(DragonGlovesOfEvolution),
                typeof(DragonLegsOfEvolution), typeof(DragonHelmOfEvolution), typeof(DragonGorgetOfEvolution),
                typeof(OrderShieldOfEvolution)));

            Register(new BossDropDefinition(
                "idium-evolution-max", "Idium", "Evolution Set - Fully Leveled (1001)", "Boss Sets", BossDropDisplayRace.Human, false, true,
                typeof(DragonArmsOfEvolution), typeof(DragonChestOfEvolution), typeof(DragonGlovesOfEvolution),
                typeof(DragonLegsOfEvolution), typeof(DragonHelmOfEvolution), typeof(DragonGorgetOfEvolution),
                typeof(OrderShieldOfEvolution)));

            // Confirmed from Custom/Tested/Goliath/Goliath.cs
            Register(new BossDropDefinition(
                "goliath-gargish-evolution", "Goliath", "Gargish Evolution Set - Unleveled", "Boss Sets", BossDropDisplayRace.Gargoyle, false,
                typeof(GargishArmsOfEvolution), typeof(GargishChestOfEvolution), typeof(GargishWingArmorOfEvolution),
                typeof(GargishLegsOfEvolution), typeof(GargishKiltOfEvolution), typeof(GargishShieldOfEvolution)));

            Register(new BossDropDefinition(
                "goliath-gargish-evolution-max", "Goliath", "Gargish Evolution Set - Fully Leveled (1001)", "Boss Sets", BossDropDisplayRace.Gargoyle, false, true,
                typeof(GargishArmsOfEvolution), typeof(GargishChestOfEvolution), typeof(GargishWingArmorOfEvolution),
                typeof(GargishLegsOfEvolution), typeof(GargishKiltOfEvolution), typeof(GargishShieldOfEvolution)));

            Register(new BossDropDefinition(
                "hephastos-crusader", "Hephastos", "Crusader Set", "Wolvesbane Sets", BossDropDisplayRace.Human, false,
                typeof(CrusaderArms), typeof(CrusaderBoots), typeof(CrusaderChest), typeof(CrusaderGloves),
                typeof(CrusaderHelm), typeof(CrusaderLegs), typeof(CrusaderSash), typeof(CrusaderWings),
                typeof(HolyAvengersWrath)));

            Register(new BossDropDefinition(
                "swampqueen-nox", "Swamp Queen", "Nox Set", "Boss Sets", BossDropDisplayRace.Human, true,
                typeof(NoxGorget), typeof(NoxHelm), typeof(NoxTunic), typeof(NoxArms), typeof(NoxLegs),
                typeof(NoxGloves), typeof(NoxKatana), typeof(NoxShield), typeof(NoxRobe)));

            Register(new BossDropDefinition(
                "destrobo-robo", "Destructabo Robo", "Robo Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(RoboArms), typeof(RoboLegs), typeof(RoboChest), typeof(RoboGorget), typeof(RoboGloves), typeof(RoboHelm),
                typeof(DRoboBlaster), typeof(NDRoboBlaster)));

            Register(new BossDropDefinition(
                "thoroar-oblivion", "Thoroar", "Oblivion Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(OblivionCap), typeof(OblivionChest), typeof(OblivionArms), typeof(OblivionGloves), typeof(OblivionLegs),
                typeof(OblivionBlade), typeof(OblivionShield), typeof(OblivionGorget)));

            Register(new BossDropDefinition(
                "kashmir-mining", "Kashmir", "Expert Mining Set", "Wolvesbane Sets", BossDropDisplayRace.Human, false,
                typeof(TunicofExpertMining), typeof(GorgetofExpertMining), typeof(ArmsofExpertMining), typeof(LegsofExpertMining),
                typeof(GlovesofExpertMining), typeof(CapofExpertMining), typeof(MinersPickaxe)));

            Register(new BossDropDefinition(
                "vonwolvesbane-taming", "Von Wolvesbane", "Expert Animal Taming Set", "Wolvesbane Sets", BossDropDisplayRace.Human, false,
                typeof(TunicofExpertAnimalTaming), typeof(GorgetofExpertAnimalTaming), typeof(ArmsofExpertAnimalTaming),
                typeof(LegsofExpertAnimalTaming), typeof(GlovesofExpertAnimalTaming), typeof(CapofExpertAnimalTaming),
                typeof(EarringsofExpertAnimalTaming), typeof(RingofExpertAnimalTaming)));

            Register(new BossDropDefinition(
                "daneelec-maniac-tailor", "Dane Elec", "Maniac Tailor Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(ManiacTailorChest), typeof(ManiacTailorGorget), typeof(ManiacTailorArms),
                typeof(ManiacTailorLegs), typeof(ManiacTailorGloves), typeof(ManiacTailorHelm), typeof(ManiacTailorKnife)));

            Register(new BossDropDefinition(
                "peccatus-sin", "Peccatus", "Sin Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(SinShield), typeof(SinChest), typeof(SinLegs), typeof(SinArms), typeof(SinGloves), typeof(SinHelm), typeof(SinBlade)));

            Register(new BossDropDefinition(
                "cratylus", "Cratylus", "Cratylus Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(CratylusChest), typeof(CratylusArms), typeof(CratylusLegs), typeof(CratylusGloves), typeof(CratylusGorget)));

            Register(new BossDropDefinition(
                "demonlord", "Demon Lord", "Demon Lord Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(DemonLordHelm), typeof(DemonLordChest), typeof(DemonLordArms), typeof(DemonLordGloves), typeof(DemonLordLegs)));

            Register(new BossDropDefinition(
                "malacoda-malabranche", "Malacoda", "Malabranche Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(MalabrancheChest), typeof(MalabrancheArms), typeof(MalabrancheLegs), typeof(MalabrancheGloves),
                typeof(MalabrancheVest), typeof(MalabrancheHelm), typeof(MalabrancheRobe)));

            Register(new BossDropDefinition(
                "necroloricatus", "Necroloricatus", "Necroloricatus Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(Necroacidus), typeof(NecroloricatusBoots), typeof(NecroloricatusCap), typeof(NecroloricatusGloves),
                typeof(NecroloricatusNecklace), typeof(NecroloricatusPants), typeof(NecroloricatusShirt)));

            Register(new BossDropDefinition(
                "revelation", "Revelation", "Revelation Set", "Boss Sets", BossDropDisplayRace.Human, true,
                typeof(RevelationShirt), typeof(RevelationSkirt), typeof(RevelationSash), typeof(RevelationRobe),
                typeof(RevelationBoots), typeof(RevelationCloak), typeof(RevelationApron)));

            Register(new BossDropDefinition(
                "tmminion-twisted", "TM Minion", "Twisted Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(TwistedShield), typeof(TwistedChest), typeof(TwistedLegs), typeof(TwistedArms),
                typeof(TwistedGloves), typeof(TwistedHelm), typeof(TwistedScythe)));

            Register(new BossDropDefinition(
                "alien-perfect", "Alien", "Perfected Armor", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(PerfectedArms), typeof(PerfectedCap), typeof(PerfectedChest), typeof(PerfectedGorget),
                typeof(PerfectedGloves), typeof(PerfectedLegs)));

            Register(new BossDropDefinition(
                "arachnis", "Arachnis", "Arachnis Set", "Boss Sets", BossDropDisplayRace.Human, true,
                typeof(ShirtofArachnis), typeof(SkirtofArachnis), typeof(CloakofArachnis), typeof(BootsofArachnis),
                typeof(SurcoatofArachnis), typeof(ShroudofArachnis), typeof(SoulofArachnis)));

            Register(new BossDropDefinition(
                "sataness", "Sataness", "Sataness Set", "Boss Sets", BossDropDisplayRace.Human, true,
                typeof(SSShirt), typeof(SSSkirt), typeof(SSSash), typeof(SSStaff)));

            Register(new BossDropDefinition(
                "orochimaru", "Orochimaru", "Orochimaru Set", "Quest/Boss Sets", BossDropDisplayRace.Human, false,
                typeof(OrochimaruShirt), typeof(OrochimaruPants), typeof(OrochimaruBoots), typeof(OrochimaruUnderShirt)));

            Register(new BossDropDefinition(
                "wolverine-xmen", "Wolverine", "X-Men Uniform", "X-Men", BossDropDisplayRace.Human, false,
                typeof(TunicofXMenUniform), typeof(LegsofXMenUniform), typeof(GlovesofXMenUniform),
                typeof(BootsofXMenUniform), typeof(MaskofWolverine), typeof(ClawsOfWolverine)));

            Register(new BossDropDefinition(
                "cyclops-xmen", "Cyclops", "X-Men Uniform", "X-Men", BossDropDisplayRace.Human, false,
                typeof(TunicofXMenUniform), typeof(LegsofXMenUniform), typeof(GlovesofXMenUniform),
                typeof(BootsofXMenUniform), typeof(ArmsofXMenUniform), typeof(CyclopsVisor)));

            Register(new BossDropDefinition(
                "storm-xmen", "Storm", "X-Men Uniform", "X-Men", BossDropDisplayRace.Human, true,
                typeof(TunicofXMenUniform), typeof(LegsofXMenUniform), typeof(GlovesofXMenUniform),
                typeof(BootsofXMenUniform), typeof(ArmsofXMenUniform), typeof(StormsCloak)));

            Register(new BossDropDefinition(
                "gambit-xmen", "Gambit", "X-Men Uniform", "X-Men", BossDropDisplayRace.Human, false,
                typeof(TunicofXMenUniform), typeof(LegsofXMenUniform), typeof(GlovesofXMenUniform),
                typeof(BootsofXMenUniform), typeof(ArmsofXMenUniform), typeof(GorgetofXMenUniform), typeof(StaffofGambit)));


            // Additional complete/custom sets recovered in the v2 full-folder scan.
            Register(new BossDropDefinition(
                "seth-chaos", "Seth", "Chaos Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(ChaosGloves), typeof(ChaosCloak), typeof(ChaosRobe), typeof(ChaosBoots), typeof(SlothChaosShield)));

            Register(new BossDropDefinition(
                "adalbrecht-daminoc", "Adalbrecht", "Daminoc Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(DaminocShield), typeof(DaminocHelm), typeof(DaminocBlade), typeof(DaminocLegs), typeof(DaminocChest)));

            Register(new BossDropDefinition(
                "quardanic-ancient-dragon", "Quardanic", "Ancient Dragon Lord Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(AncientDragonLordArms), typeof(AncientDragonLordGloves), typeof(AncientDragonLordChest),
                typeof(AncientDragonLordLegs), typeof(AncientDragonLordHelm)));

            Register(new BossDropDefinition(
                "void-knight", "Void Knight", "Void Knight Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(VoidKnightGloves), typeof(VoidKnightChest), typeof(VoidKnightsWarHelm), typeof(VoidKnightLegs),
                typeof(VoidKnightArms), typeof(VoidBlade), typeof(AbysalBow)));

            Register(new BossDropDefinition(
                "leprechaun", "Leprechaun", "Leprechaun Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(GorgetOfTheLeprechaun), typeof(ArmsOfTheLeprechaun), typeof(HelmOfTheLeprechaun),
                typeof(HoodedRobeOfTheLeprechaun), typeof(GlovesOfTheLeprechaun), typeof(ChestOfTheLeprechaun),
                typeof(LegsOfTheLeprechaun), typeof(ShillelaghOfTheLeprechaun)));

            Register(new BossDropDefinition(
                "suicide", "Suicide", "Suicide Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(SuicideGorget), typeof(SuicideChest), typeof(SuicideLegs), typeof(SuicideHelm),
                typeof(SuicideGloves), typeof(SuicideArms), typeof(SuicideBoots)));

            Register(new BossDropDefinition(
                "might", "Might", "Armor of Might", "Quest/Boss Sets", BossDropDisplayRace.Human, false,
                typeof(ArmsofMight), typeof(GlovesofMight), typeof(ChestofMight), typeof(HelmofMight),
                typeof(LegsofMight), typeof(BowofMight)));

            Register(new BossDropDefinition(
                "zeus-heavens", "Zeus", "Armor of the Heavens", "Quest/Boss Sets", BossDropDisplayRace.Human, false,
                typeof(ArmsoftheHeavens), typeof(HelmoftheHeavens), typeof(GlovesoftheHeavens),
                typeof(ChestoftheHeavens), typeof(LegsoftheHeavens)));

            Register(new BossDropDefinition(
                "poseidon-sea", "Poseidon", "Armor of the Sea", "Quest/Boss Sets", BossDropDisplayRace.Human, false,
                typeof(GlovesoftheSea), typeof(LegsoftheSea), typeof(ArmsoftheSea), typeof(ChestoftheSea), typeof(HelmoftheSea),
                typeof(TridentoftheSea)));

            Register(new BossDropDefinition(
                "hades-underworld", "Hades", "Armor of the Underworld", "Quest/Boss Sets", BossDropDisplayRace.Human, false,
                typeof(HelmoftheUnderworld), typeof(StaffoftheUnderworld), typeof(LegsoftheUnderworld),
                typeof(GlovesoftheUnderworld), typeof(ArmsoftheUnderworld), typeof(ChestoftheUnderworld)));

            Register(new BossDropDefinition(
                "crockett-demon-pact", "Crockett Scarr", "Demon Pact Set", "Quest/Boss Sets", BossDropDisplayRace.Human, false,
                typeof(DemonPactLegs), typeof(DemonPactShield), typeof(DemonPactChest), typeof(DemonPactGloves), typeof(DemonPactHelm),
                typeof(DemonPactFork)));

            Register(new BossDropDefinition(
                "expert-tinkering", "Custom Gear", "Expert Tinkering Set", "Crafting Sets", BossDropDisplayRace.Human, false,
                typeof(GlovesofExpertTinkering), typeof(TunicofExpertTinkering), typeof(LegsofExpertTinkering),
                typeof(GorgetofExpertTinkering), typeof(ArmsofExpertTinkering), typeof(CapofExpertTinkering)));

            Register(new BossDropDefinition(
                "expert-tailoring", "Custom Gear", "Expert Tailoring Set", "Crafting Sets", BossDropDisplayRace.Human, false,
                typeof(ArmsofExpertTailoring), typeof(LegsofExpertTailoring), typeof(GorgetofExpertTailoring),
                typeof(GlovesofExpertTailoring), typeof(TunicofExpertTailoring), typeof(CapofExpertTailoring)));

            Register(new BossDropDefinition(
                "expert-fletching", "Custom Gear", "Expert Fletching Set", "Crafting Sets", BossDropDisplayRace.Human, false,
                typeof(GorgetofExpertFletching), typeof(CapofExpertFletching), typeof(LegsofExpertFletching),
                typeof(TunicofExpertFletching), typeof(ArmsofExpertFletching), typeof(GlovesofExpertFletching)));

            Register(new BossDropDefinition(
                "expert-smithy", "Custom Gear", "Expert Smithy Set", "Crafting Sets", BossDropDisplayRace.Human, false,
                typeof(TunicofExpertSmithy), typeof(LegsofExpertSmithy), typeof(CapofExpertSmithy),
                typeof(ArmsofExpertSmithy), typeof(GorgetofExpertSmithy), typeof(GlovesofExpertSmithy)));

            Register(new BossDropDefinition(
                "white-wizard", "Custom Gear", "White Wizard Set", "Custom Sets", BossDropDisplayRace.Human, false,
                typeof(WhiteWizardsSash), typeof(WhiteWizardsCloak), typeof(WhiteWizardsShirt),
                typeof(WhiteWizardsBoots), typeof(WhiteWizardsKilt)));

            Register(new BossDropDefinition(
                "kage-maru", "Kage-Maru", "Kage-Maru Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(KageMaruChest), typeof(KageMaruMask), typeof(KageMaruGloves), typeof(KageMaruPants),
                typeof(KageMaruHood), typeof(KageMaruShoes)));

            Register(new BossDropDefinition(
                "skeletal-serpent", "Skeletal Serpent", "Skeletal Serpent Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(SkeletalSerpentChest), typeof(SkeletalSerpentArms), typeof(SkeletalSerpentLegs), typeof(SkeletalSerpentGloves)));

            Register(new BossDropDefinition(
                "tergus", "Tergus", "Tergus Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(TergusGloves), typeof(TergusChest), typeof(TergusLegs), typeof(TergusGorget), typeof(TergusArms)));

            Register(new BossDropDefinition(
                "pimp", "Pimp", "Pimp Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(PimpsHands), typeof(PimpRobe), typeof(PimpHat), typeof(ParachutePants), typeof(PimpsFace),
                typeof(PimpStick), typeof(PimpCane)));

            // Confirmed from Custom/Tested/Caveman/Caveman.cs
            Register(new BossDropDefinition(
                "alley-oop-caveman", "Alley Oop", "Caveman Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(CavemanShirt), typeof(CavemanNeck), typeof(CavemanLoincloth), typeof(CavemanClub), typeof(Flyswatter)));

            Register(new BossDropDefinition(
                "pillager", "Pillager", "Pillager Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(PillagerTunic), typeof(PillagerPants), typeof(PillagerCap)));

            Register(new BossDropDefinition(
                "death-angel", "Death Angel", "Death Angel Set", "Boss Sets", BossDropDisplayRace.Human, true,
                typeof(DAChest), typeof(DASkirt), typeof(DACirclet), typeof(DASandals), typeof(DASickle)));

            Register(new BossDropDefinition(
                "sexi-vampire", "Sexi Vampire", "Vampire Set", "Boss Sets", BossDropDisplayRace.Human, true,
                typeof(SexiSkirt), typeof(BloodyKatana), typeof(SexiChest), typeof(SexiEyes)));

            Register(new BossDropDefinition(
                "king-kamuu-atlantis", "King Kamuu", "Atlantis Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(AtlantisSword), typeof(AtlantisCloak), typeof(AtlantisRobe)));

            Register(new BossDropDefinition(
                "dragon-quest", "Dragon Quest", "Dragon Set", "Quest/Boss Sets", BossDropDisplayRace.Human, false,
                typeof(HelmetofDragon), typeof(DragonShield), typeof(DragonBlade), typeof(LeggingsofDragon),
                typeof(TunicofDragon), typeof(GlovesofDragon), typeof(DragonNeck), typeof(ArmsofDragon)));

            Register(new BossDropDefinition(
                "santa", "Santa", "Santa Set", "Seasonal Sets", BossDropDisplayRace.Human, false,
                typeof(SantaBoots), typeof(SantaLegs), typeof(SantaHelm), typeof(SantaTunic),
                typeof(SantaGorget), typeof(SantaArms), typeof(SantaGloves)));

            Register(new BossDropDefinition(
                "hell-male", "Custom Gear", "Hell Set (Male)", "Custom Sets", BossDropDisplayRace.Human, false,
                typeof(BootsOfHell), typeof(HellsBow), typeof(BeltOfHell), typeof(LegsOfHell), typeof(CapOfHell),
                typeof(ChestOfHell), typeof(GorgetOfHell), typeof(ArmsOfHell), typeof(GlovesOfHell)));

            Register(new BossDropDefinition(
                "hell-female", "Custom Gear", "Hell Set (Female)", "Custom Sets", BossDropDisplayRace.Human, true,
                typeof(BootsOfHell), typeof(HellsBow), typeof(BeltOfHell), typeof(SkirtOfHell), typeof(CapOfHell),
                typeof(FemaleChestOfHell), typeof(GorgetOfHell), typeof(ArmsOfHell), typeof(GlovesOfHell)));

            Register(new BossDropDefinition(
                "witches", "Kindred", "Witches Set", "Boss Sets", BossDropDisplayRace.Human, true,
                typeof(WitchesArms), typeof(WitchesLegs), typeof(WitchesChest), typeof(WitchesHat),
                typeof(WitchesGloves), typeof(WitchesGorget), typeof(WitchesSkirt)));

            Register(new BossDropDefinition(
                "fisher", "Fisher Quest", "Special Fishing Set", "Quest/Boss Sets", BossDropDisplayRace.Human, false,
                typeof(SpecialFishingBoots), typeof(SpecialFishingSash), typeof(SpecialFishingPants),
                typeof(SpecialFishingShirt), typeof(SpecialFishingGloves)));

            // Additional Custom/Tested audit (v2.10)
            Register(new BossDropDefinition(
                "akyndah", "Lady Akyndah", "Akyndah Drop", "Boss Sets", BossDropDisplayRace.Human, true,
                typeof(AkyndahEarrings)));

            Register(new BossDropDefinition(
                "kronik", "Kronik", "Kronik Set", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(KronikTome2), typeof(KronikAxe), typeof(KronikTome)));

            Register(new BossDropDefinition(
                "love-angel-female", "Phileon", "Angel of Love Set (Female)", "Boss Sets", BossDropDisplayRace.Human, true,
                typeof(FLoveChest), typeof(LoveSkirt), typeof(NDLoveSpear), typeof(LoveLight), typeof(DLoveSpear)));

            Register(new BossDropDefinition(
                "love-angel-male", "Phileon", "Angel of Love Set (Male)", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(LoveChest), typeof(LoveLegs), typeof(NDLoveSpear), typeof(LoveLight), typeof(DLoveSpear)));

            Register(new BossDropDefinition(
                "megami", "Megami Tensei", "Reborn Goddess Set", "Boss Sets", BossDropDisplayRace.Human, true,
                typeof(NeptuneShirt), typeof(MegamiChest), typeof(MegamiLegs), typeof(MegamiScepter)));

            Register(new BossDropDefinition(
                "father-time", "Father Time", "Timepiece Collection", "Seasonal Sets", BossDropDisplayRace.Human, false,
                typeof(FTWatch), typeof(FTSWatch), typeof(FTGWatch), typeof(FTRWatch)));

            Register(new BossDropDefinition(
                "ignis-malum", "Ignis Malum", "Fire Demon Drop", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(FireDemonStaff)));

            Register(new BossDropDefinition(
                "magi-armor", "Magi Bosses", "Magi Armor Collection", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(Chestofthemagi), typeof(Legsofthemagi), typeof(Gorgetofthemagi), typeof(Armsofthemagi),
                typeof(Glovesofthemagi), typeof(StaffOfTheMagi), typeof(HatOfTheMagi), typeof(PendantOfTheMagi),
                typeof(EmperorsEarringsofFavor), typeof(SwampBoots), typeof(BeltOfLostSouls), typeof(AdmiralsHat),
                typeof(TacticalMask), typeof(ChestoftheFemaleMagi), typeof(SkirtoftheMagi)));

            Register(new BossDropDefinition(
                "scarab-ancient", "Scarab", "Ancient Robe", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(AncientRobe)));

            Register(new BossDropDefinition(
                "garuda-stolen-jewels", "Garuda", "Stolen Jewelry", "Boss Sets", BossDropDisplayRace.Human, false,
                typeof(StolenBracelet), typeof(StolenEarrings), typeof(StolenNecklace)));

            Register(new BossDropDefinition(
                "shame-blacksmithing", "Custom Gear", "Shame Blacksmithing Armor", "Crafting Sets", BossDropDisplayRace.Human, false,
                typeof(ShameArmorofBlacksmithingChest), typeof(ShameArmorofBlacksmithingArms),
                typeof(ShameArmorofBlacksmithingGloves), typeof(ShameArmorofBlacksmithingLegs)));

            Register(new BossDropDefinition(
                "legolas-fletching", "Custom Gear", "Legolas Bowcraft & Fletching Armor", "Crafting Sets", BossDropDisplayRace.Human, false,
                typeof(BowcraftandFletchingArmorofLegolasChest), typeof(BowcraftandFletchingArmorofLegolasArms),
                typeof(BowcraftandFletchingArmorofLegolasGloves), typeof(BowcraftandFletchingArmorofLegolasLegs)));

            Register(new BossDropDefinition(
                "tamer-clothing", "Custom Gear", "Tamer Clothing Set", "Custom Sets", BossDropDisplayRace.Human, false,
                typeof(TamingShroud), typeof(SandalsoftheTamer), typeof(SashoftheTamer), typeof(TamersApron)));

            Register(new BossDropDefinition(
                "grove-newbie-human", "The Grove", "Newbie Set (Human)", "Custom Sets", BossDropDisplayRace.Human, false,
                typeof(NewbieHat), typeof(NewbieChest), typeof(NewbieArms), typeof(NewbieGloves), typeof(NewbieLegs),
                typeof(NewbieGorget), typeof(GroveSandals)));

            Register(new BossDropDefinition(
                "grove-newbie-gargoyle", "The Grove", "Newbie Set (Gargoyle)", "Gargoyle Sets", BossDropDisplayRace.Gargoyle, false,
                typeof(NewbieGargChest), typeof(NewbieGargArms), typeof(NewbieGargLegs), typeof(NewbieGargKilt), typeof(NewbieGargWings)));

            Register(new BossDropDefinition(
                "gargish-firerock-male", "Custom Gear", "Gargish FireRock Set (Male)", "Gargoyle Sets", BossDropDisplayRace.Gargoyle, false,
                typeof(MGargishFireRockArms), typeof(MGargishFireRockChest), typeof(MGargishFireRockLegs),
                typeof(MGargishFireRockKilt), typeof(GargishFlameShield), typeof(SoaringFlames)));

            Register(new BossDropDefinition(
                "gargish-firerock-female", "Custom Gear", "Gargish FireRock Set (Female)", "Gargoyle Sets", BossDropDisplayRace.Gargoyle, true,
                typeof(FGargishFireRockArms), typeof(FGargishFireRockChest), typeof(FGargishFireRockLegs),
                typeof(FGargishFireRockKilt), typeof(GargishFlameShield), typeof(FireWind)));

            Register(new BossDropDefinition(
                "gargish-crystaline-male", "Custom Gear", "Gargish CrystalineFire Set (Male)", "Gargoyle Sets", BossDropDisplayRace.Gargoyle, false,
                typeof(MGargishCrystalineFireArms), typeof(MGargishCrystalineFireChest), typeof(MGargishCrystalineFireLegs),
                typeof(MGargishCrystalineFireKilt), typeof(GargishShieldOfCrystalineFire), typeof(FlameTongue)));

            Register(new BossDropDefinition(
                "gargish-crystaline-female", "Custom Gear", "Gargish CrystalineFire Set (Female)", "Gargoyle Sets", BossDropDisplayRace.Gargoyle, true,
                typeof(FGargishCrystalineFireArms), typeof(FGargishCrystalineFireChest), typeof(FGargishCrystalineFireLegs),
                typeof(FGargishCrystalineFireKilt), typeof(GargishShieldOfCrystalineFire), typeof(ForkedFire)));
        }

        private static void Register(BossDropDefinition def)
        {
            m_Definitions.Add(def);
        }

        public static BossDropDefinition Find(string key)
        {
            for (int i = 0; i < m_Definitions.Count; i++)
            {
                if (String.Equals(m_Definitions[i].Key, key, StringComparison.OrdinalIgnoreCase))
                    return m_Definitions[i];
            }

            return null;
        }

        private static BossDropDisplayCase GetOrCreateAlternateCase(BossDropDefinition def, BossDropMannequin mannequin)
        {
            BossDropDisplayCase displayCase = mannequin.DisplayCase;

            if (displayCase == null || displayCase.Deleted)
            {
                displayCase = new BossDropDisplayCase(def.Key, def.BossName);
                displayCase.Mannequin = mannequin;
                mannequin.AddDisplayCase(displayCase);
            }

            return displayCase;
        }

        private static void AddAlternateItem(BossDropDefinition def, BossDropMannequin mannequin, Item item)
        {
            BossDropDisplayCase displayCase = GetOrCreateAlternateCase(def, mannequin);
            displayCase.AddDisplayItem(item);
        }

        private static void MaxEvolutionDisplayItem(Item item, Mobile staff)
        {
            if (item == null)
                return;

            try
            {
                // The Idium/Goliath Evolution scripts expose EvolutionPoints and ApplyGain().
                // Setting the display copy to 1000, then calling its own ApplyGain() once,
                // produces the genuine capped 1001-point attributes without modifying the source items.
                System.Reflection.PropertyInfo points = item.GetType().GetProperty("EvolutionPoints");
                System.Reflection.MethodInfo applyGain = item.GetType().GetMethod("ApplyGain", Type.EmptyTypes);

                if (points != null && points.CanWrite && applyGain != null)
                {
                    points.SetValue(item, 1000, null);
                    applyGain.Invoke(item, null);
                    item.InvalidateProperties();
                }
                else if (staff != null)
                {
                    staff.SendMessage(33, "Could not fully level display item {0}: EvolutionPoints/ApplyGain was not found.", item.GetType().Name);
                }
            }
            catch (Exception ex)
            {
                if (staff != null)
                    staff.SendMessage(33, "Could not fully level display item {0}: {1}", item.GetType().Name, ex.Message);
            }
        }

        public static BossDropMannequin Create(BossDropDefinition def, Mobile staff)
        {
            if (def == null)
                return null;

            BossDropMannequin mannequin = new BossDropMannequin(def.Key, def.BossName + " - " + def.SetName, def.Race, def.Female);
            int equipped = 0;
            int alternates = 0;
            bool weaponEquipped = false;

            for (int i = 0; i < def.ItemTypes.Length; i++)
            {
                Type type = def.ItemTypes[i];
                Item item = null;

                try
                {
                    item = Activator.CreateInstance(type) as Item;
                }
                catch (Exception ex)
                {
                    if (staff != null)
                        staff.SendMessage(33, "Could not create {0}: {1}", type.Name, ex.Message);
                }

                if (item == null)
                    continue;

                if (def.MaxEvolution)
                    MaxEvolutionDisplayItem(item, staff);

                // Display-only correction. The actual player drop is never changed.
                if (item is ParachutePants)
                    item.Layer = Layer.Pants;

                // Only ONE weapon is ever worn by a mannequin. This applies even when
                // multiple weapons use different hand layers. Any additional BaseWeapon
                // is shown in its own adjacent glass display case.
                bool isWeapon = item is BaseWeapon;

                if (isWeapon && weaponEquipped)
                {
                    alternates++;
                    AddAlternateItem(def, mannequin, item);
                    continue;
                }

                // For non-weapons (and the first weapon), the first item registered for
                // an equipment layer stays on the mannequin. Later layer conflicts become
                // alternate displays.
                Item existing = null;

                if (item.Layer != Layer.Invalid)
                    existing = mannequin.FindItemOnLayer(item.Layer);

                if (existing != null)
                {
                    alternates++;
                    AddAlternateItem(def, mannequin, item);
                    continue;
                }

                if (mannequin.AddDisplayItem(item))
                {
                    equipped++;

                    if (isWeapon)
                        weaponEquipped = true;
                }
                else
                {
                    // Custom equip restrictions can also prevent an item from being worn.
                    // Keep each such item in its own inspectable case.
                    if (!item.Deleted)
                    {
                        alternates++;
                        AddAlternateItem(def, mannequin, item);
                    }
                }
            }

            if (staff != null)
                staff.SendMessage(68, "Created {0}: {1} equipped, {2} alternate item(s) in the shared display case.", def.Label, equipped, alternates);

            return mannequin;
        }

        public static void PlaceDisplay(BossDropMannequin mannequin, Point3D loc, Map map)
        {
            if (mannequin == null || map == null)
                return;

            // Boss Drop showroom standard: mannequins always face East.
            mannequin.Direction = Direction.East;
            mannequin.MoveToWorld(loc, map);

            // A small information plaque is placed directly north of the mannequin.
            // It intentionally contains no boss-location information.
            BossDropInfoPlaque plaque = mannequin.InfoPlaque;
            if (plaque == null || plaque.Deleted)
            {
                BossDropDefinition plaqueDef = Find(mannequin.DisplayKey);
                plaque = new BossDropInfoPlaque(mannequin.DisplayKey, plaqueDef != null ? plaqueDef.BossName : mannequin.Name);
                plaque.Mannequin = mannequin;
                mannequin.InfoPlaque = plaque;
            }
            plaque.MoveToWorld(new Point3D(loc.X, loc.Y - 1, loc.Z), map);

            // v2.13: boss statues were removed from the showroom layout because some boss bodies
            // are too large for predictable placement.  Keep backward compatibility with v2.12
            // saves by deleting any legacy linked statue during placement/refresh.
            BossDropBossStatue legacyStatue = mannequin.BossStatue;
            if (legacyStatue != null && !legacyStatue.Deleted)
                legacyStatue.Delete();

            mannequin.BossStatue = null;

            // All alternate/conflicting items for this mannequin share one glass case.
            BossDropDisplayCase displayCase = mannequin.DisplayCase;

            if (displayCase != null && !displayCase.Deleted)
            {
                if (displayCase.Items.Count == 0)
                {
                    displayCase.Delete();
                    mannequin.DisplayCases.Clear();
                }
                else
                {
                    // Use the turned vendor-style glass display case art and place it directly south
                    // of the mannequin. This keeps the showroom layout aligned with east-facing mannequins.
                    displayCase.ItemID = 0x2FEB;
                    Point3D caseLoc = new Point3D(loc.X, loc.Y + 1, loc.Z);
                    displayCase.MoveToWorld(caseLoc, map);
                }
            }
        }
    }
}
