using Server.Engines.Craft;
using Server.Engines.PartySystem;
using Server.Mobiles;
using Server.SkillHandlers;
using Server.Spells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Items
{
    public enum TreasureLevel
    {
        Stash,
        Supply,
        Cache,
        Hoard,
        Trove
    }

    public enum TreasurePackage
    {
        Artisan,
        Assassin,
        Mage,
        Ranger,
        Warrior
    }

    public enum TreasureFacet
    {
        Trammel,
        Felucca,
        Ilshenar,
        Malas,
        Tokuno,
        TerMur,
        Eodon,
        NewWolvesbane
    }

    public enum ChestQuality
    {
        None,
        Rusty,
        Standard,
        Gold
    }

    public static class TreasureMapInfo
    {
        public static bool NewSystem => true;

        /// <summary>
        /// This is called from BaseCreature. Instead of editing EVERY creature that drops a map, we'll simply convert it here.
        /// </summary>
        /// <param name="level"></param>
        public static int ConvertLevel(int level)
        {
            if (!NewSystem || level == -1)
                return level;

            switch (level)
            {
                default: return (int)TreasureLevel.Stash;
                case 2:
                case 3: return (int)TreasureLevel.Supply;
                case 4:
                case 5: return (int)TreasureLevel.Cache;
                case 6: return (int)TreasureLevel.Hoard;
                case 7: return (int)TreasureLevel.Trove;
            }
        }

        /// <summary>
        /// Creates a treasure map from a legacy pre-revamp level (0-7).
        /// Use this for old creature/NPC/container systems that still express
        /// treasure difficulty using the classic numeric levels.
        ///
        /// Do NOT use this for the new system's own Stash/Supply/Cache/Hoard/Trove
        /// upgrades, because those values are already canonical 0-4 tiers.
        /// </summary>
        public static TreasureMap CreateLegacyMap(int legacyLevel, Map map)
        {
            return CreateLegacyMap(legacyLevel, map, false);
        }

        public static TreasureMap CreateLegacyMap(int legacyLevel, Map map, bool eodon)
        {
            return new TreasureMap(ConvertLevel(legacyLevel), map, eodon);
        }

        public static TreasureFacet GetFacet(IEntity e)
        {
            return GetFacet(e.Location, e.Map);
        }

        public static int PackageLocalization(TreasurePackage package)
        {
            switch (package)
            {
                case TreasurePackage.Artisan: return 1158989;
                case TreasurePackage.Assassin: return 1158987;
                case TreasurePackage.Mage: return 1158986;
                case TreasurePackage.Ranger: return 1158990;
                case TreasurePackage.Warrior: return 1158988;
            }

            return 0;
        }

        public static TreasureFacet GetFacet(IPoint2D p, Map map)
        {
            if (map == Map.NewWolvesbane)
            {
                return TreasureFacet.NewWolvesbane;
            }

            if (map == Map.TerMur)
            {
                if (SpellHelper.IsEodon(map, new Point3D(p.X, p.Y, 0)))
                {
                    return TreasureFacet.Eodon;
                }

                return TreasureFacet.TerMur;
            }

            if (map == Map.Felucca)
            {
                return TreasureFacet.Felucca;
            }

            if (map == Map.Malas)
            {
                return TreasureFacet.Malas;
            }

            if (map == Map.Ilshenar)
            {
                return TreasureFacet.Ilshenar;
            }

            if (map == Map.Tokuno)
            {
                return TreasureFacet.Tokuno;
            }

            return TreasureFacet.Trammel;
        }

        public static IEnumerable<Type> GetRandomEquipment(TreasureLevel level, TreasurePackage package, TreasureFacet facet, int amount)
        {
            Type[] weapons = GetWeaponList(level, package, facet);
            Type[] armor = GetArmorList(level, package, facet);
            Type[] jewels = GetJewelList(level, package, facet);
            Type[] list;

            for (int i = 0; i < amount; i++)
            {
                // Wolvesbane Phase 7G:
                // Keep jewelry desirable without letting it dominate treasure chests.
                // 30% weapons / 50% armor / 20% jewelry.
                switch (Utility.Random(10))
                {
                    default:
                    case 0:
                    case 1:
                    case 2: list = weapons; break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7: list = armor; break;
                    case 8:
                    case 9: list = jewels; break;
                }

                yield return list[Utility.Random(list.Length)];
            }
        }

        public static Type[] GetWeaponList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            Type[] list = null;

            switch (facet)
            {
                case TreasureFacet.Trammel:
                case TreasureFacet.Felucca:
                case TreasureFacet.NewWolvesbane: list = _WeaponTable[(int)package][0]; break;
                case TreasureFacet.Ilshenar: list = _WeaponTable[(int)package][1]; break;
                case TreasureFacet.Malas: list = _WeaponTable[(int)package][2]; break;
                case TreasureFacet.Tokuno: list = _WeaponTable[(int)package][3]; break;
                case TreasureFacet.TerMur: list = _WeaponTable[(int)package][4]; break;
                case TreasureFacet.Eodon: list = _WeaponTable[(int)package][5]; break;
            }

            // tram/fel lists are always default
            if (list == null || list.Length == 0)
            {
                list = _WeaponTable[(int)package][0];
            }

            return list;
        }

        public static Type[] GetArmorList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            Type[] list = null;

            switch (facet)
            {
                case TreasureFacet.Trammel:
                case TreasureFacet.Felucca:
                case TreasureFacet.NewWolvesbane: list = _ArmorTable[(int)package][0]; break;
                case TreasureFacet.Ilshenar: list = _ArmorTable[(int)package][1]; break;
                case TreasureFacet.Malas: list = _ArmorTable[(int)package][2]; break;
                case TreasureFacet.Tokuno: list = _ArmorTable[(int)package][3]; break;
                case TreasureFacet.TerMur: list = _ArmorTable[(int)package][4]; break;
                case TreasureFacet.Eodon: list = _ArmorTable[(int)package][5]; break;
            }

            // tram/fel lists are always default
            if (list == null || list.Length == 0)
            {
                list = _ArmorTable[(int)package][0];
            }

            return list;
        }

        public static Type[] GetJewelList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            if (facet == TreasureFacet.TerMur)
            {
                return _JewelTable[1];
            }
            else
            {
                return _JewelTable[0];
            }
        }

        public static SkillName[] GetTranscendenceList(TreasureLevel level, TreasurePackage package)
        {
            if (level == TreasureLevel.Supply || level == TreasureLevel.Cache)
            {
                return null;
            }

            return _TranscendenceTable[(int)package];
        }

        public static SkillName[] GetAlacrityList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            if (level == TreasureLevel.Stash || (facet == TreasureFacet.Felucca && level == TreasureLevel.Cache))
            {
                return null;
            }

            return _AlacrityTable[(int)package];
        }

        public static SkillName[] GetPowerScrollList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            if (facet != TreasureFacet.Felucca)
                return null;

            if (level >= TreasureLevel.Cache)
            {
                return _PowerscrollTable[(int)package];
            }

            return null;
        }

        public static Type[] GetCraftingMaterials(TreasureLevel level, TreasurePackage package, ChestQuality quality)
        {
            if (package == TreasurePackage.Artisan && level <= TreasureLevel.Supply && quality != ChestQuality.None)
            {
                return _MaterialTable[(int)quality - 1];
            }

            return null;
        }

        public static Type[] GetSpecialMaterials(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            if (package == TreasurePackage.Artisan && level == TreasureLevel.Supply)
            {
                // NewWolvesbane uses the classic Trammel material profile. Do not
                // index the stock seven-entry table with the custom enum value.
                int index = facet == TreasureFacet.NewWolvesbane ? 0 : (int)facet;

                if (_SpecialMaterialTable != null &&
                    index >= 0 &&
                    index < _SpecialMaterialTable.Length)
                {
                    return _SpecialMaterialTable[index];
                }
            }

            return null;
        }

        public static Type[] GetDecorativeList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            Type[] list = null;

            if (level >= TreasureLevel.Cache)
            {
                // The stock decorative package table is intentionally disabled on
                // Wolvesbane, but Malas still has its facet-specific CoffinPiece.
                // The previous code called list.Concat(...) while list was null,
                // which could throw during Cache/Hoard/Trove chest generation.
                if (facet == TreasureFacet.Malas)
                {
                    list = new Type[] { typeof(CoffinPiece) };
                }
            }
            else if (level == TreasureLevel.Supply)
            {
                list = _DecorativeMinorArtifacts;
            }

            return list;
        }

        public static Type[] GetReagentList(TreasureLevel level, TreasurePackage package, TreasureFacet facet)
        {
            if (level != TreasureLevel.Stash || package != TreasurePackage.Mage)
                return null;

            switch (facet)
            {
                case TreasureFacet.Felucca:
                case TreasureFacet.Trammel:
                case TreasureFacet.NewWolvesbane: return Loot.RegTypes;
                case TreasureFacet.Malas: return Loot.NecroRegTypes;
                case TreasureFacet.TerMur: return Loot.MysticRegTypes;
            }

            return null;
        }

        public static Recipe[] GetRecipeList(TreasureLevel level, TreasurePackage package)
        {
            if (package == TreasurePackage.Artisan && level == TreasureLevel.Supply)
            {
                return Recipe.Recipes.Values.ToArray();
            }

            return null;
        }

        public static Type[] GetSpecialLootList(TreasureLevel level, TreasurePackage package)
        {
            if (level == TreasureLevel.Stash)
                return null;

            Type[] list = null;

            if (level == TreasureLevel.Supply)
            {
                int index = (int)package;

                if (_SpecialSupplyLoot != null &&
                    index >= 0 &&
                    index < _SpecialSupplyLoot.Length)
                {
                    list = _SpecialSupplyLoot[index];
                }
            }
            else
            {
                list = _SpecialCacheHordeAndTrove;
            }

            Type[] professionPool = GetProfessionArtifactPool(package);

            if (professionPool != null && professionPool.Length > 0)
            {
                if (list == null || list.Length == 0)
                    list = professionPool;
                else
                    list = list.Concat(professionPool).ToArray();
            }

            return list != null && list.Length > 0 ? list : null;
        }

        /// <summary>
        /// Wolvesbane Phase 7G profession identity.
        /// Uses only artifact classes already referenced by this script so this
        /// balance pass does not introduce compile-time dependencies on unknown types.
        /// </summary>
        public static Type[] GetProfessionArtifactPool(TreasurePackage package)
        {
            switch (package)
            {
                case TreasurePackage.Artisan:
                    return _DecorativeMinorArtifacts;

                case TreasurePackage.Assassin:
                    return new Type[]
                    {
                        typeof(BurglarsBandana), typeof(NightsKiss), typeof(ColdBlood),
                        typeof(CaptainQuacklebushsCutlass), typeof(DreadPirateHat)
                    };

                case TreasurePackage.Mage:
                    return new Type[]
                    {
                        typeof(AlchemistsBauble), typeof(GwennosHarp), typeof(IolosLute),
                        typeof(EnchantedTitanLegBone)
                    };

                case TreasurePackage.Ranger:
                    return new Type[]
                    {
                        typeof(NoxRangersHeavyCrossbow), typeof(PolarBearMask),
                        typeof(ArcticDeathDealer), typeof(BlazeOfDeath)
                    };

                case TreasurePackage.Warrior:
                    return new Type[]
                    {
                        typeof(LunaLance), typeof(HeartOfTheLion), typeof(VioletCourage),
                        typeof(ShieldOfInvulnerability), typeof(CavortingClub)
                    };
            }

            return null;
        }

        /// <summary>
        /// Independent jackpot roll for rare/custom-feeling equipment.
        /// This is deliberately tiny and is NOT affected by the normal special-loot roll.
        /// Cache 0.25%, Hoard 0.75%, Trove 1.50%.
        /// Supply/Stash never roll this jackpot.
        /// </summary>
        public static double GetCustomGearChance(TreasureLevel level)
        {
            switch (level)
            {
                case TreasureLevel.Cache: return 0.0025;
                case TreasureLevel.Hoard: return 0.0075;
                case TreasureLevel.Trove: return 0.0150;
            }

            return 0.0;
        }

        public static Type GetRandomCustomGear(TreasurePackage package)
        {
            Type[] pool = GetProfessionArtifactPool(package);

            if (pool == null || pool.Length == 0)
                return null;

            return pool[Utility.Random(pool.Length)];
        }

        // Wolvesbane Phase 7H ultra-rare custom treasure rewards.
        // Each reward rolls independently. WDollar is Trove-only at exactly 1 in 750.
        private static readonly Type[] _EvoWeaponJackpot = new Type[]
        {
                typeof(AssassinSpikeOfEvolution),
                typeof(AxeOfEvolution),
                typeof(BardicheOfEvolution),
                typeof(BattleAxeOfEvolution),
                typeof(BladedStaffOfEvolution),
                typeof(BloodBladeOfEvolution),
                typeof(BokutoOfEvolution),
                typeof(BoneHarvesterOfEvolution),
                typeof(BoomerangOfEvolution),
                typeof(BowOfEvolution),
                typeof(BroadswordOfEvolution),
                typeof(ButcherKnifeOfEvolution),
                typeof(CleaverOfEvolution),
                typeof(ClubOfEvolution),
                typeof(CompositeBowOfEvolution),
                typeof(CrossbowOfEvolution),
                typeof(CutlassOfEvolution),
                typeof(CycloneOfEvolution),
                typeof(DaggerOfEvolution),
                typeof(DaishoOfEvolution),
                typeof(DiamondMaceOfEvolution),
                typeof(DiscMaceOfEvolution),
                typeof(DoubleAxeOfEvolution),
                typeof(DoubleBladedStaffOfEvolution),
                typeof(DreadSwordOfEvolution),
                typeof(DualPointedSpearOfEvolution),
                typeof(DualShortAxesOfEvolution),
                typeof(ElvenCompositeLongbowOfEvolution),
                typeof(ElvenMacheteOfEvolution),
                typeof(ElvenSpellbladeOfEvolution),
                typeof(ExecutionersAxeOfEvolution),
                typeof(GargishAxeOfEvolution),
                typeof(GargishBardicheOfEvolution),
                typeof(GargishBattleAxeOfEvolution),
                typeof(GargishBoneHarvesterOfEvolution),
                typeof(GargishButcherKnifeOfEvolution),
                typeof(GargishCleaverOfEvolution),
                typeof(GargishDaggerOfEvolution),
                typeof(GargishDaishoOfEvolution),
                typeof(GargishGnarledStaffOfEvolution),
                typeof(GargishKatanaOfEvolution),
                typeof(GargishKryssOfEvolution),
                typeof(GargishLanceOfEvolution),
                typeof(GargishMaulOfEvolution),
                typeof(GargishPikeOfEvolution),
                typeof(GargishScytheOfEvolution),
                typeof(GargishTalwarOfEvolution),
                typeof(GargishTekagiOfEvolution),
                typeof(GargishTessenOfEvolution),
                typeof(GargishWarForkOfEvolution),
                typeof(GargishWarHammerOfEvolution),
                typeof(GlassStaffOfEvolution),
                typeof(GlassSwordOfEvolution),
                typeof(GnarledStaffOfEvolution),
                typeof(HalberdOfEvolution),
                typeof(HammerPickOfEvolution),
                typeof(HatchetOfEvolution),
                typeof(HeavyCrossbowOfEvolution),
                typeof(KamaOfEvolution),
                typeof(KatanaOfEvolution),
                typeof(KryssOfEvolution),
                typeof(LajatangOfEvolution),
                typeof(LanceOfEvolution),
                typeof(LargeBattleAxeOfEvolution),
                typeof(LeafbladeOfEvolution),
                typeof(LongswordOfEvolution),
                typeof(MaceOfEvolution),
                typeof(MaulOfEvolution),
                typeof(NoDachiOfEvolution),
                typeof(NunchakuOfEvolution),
                typeof(OrnateAxeOfEvolution),
                typeof(PaladinSwordOfEvolution),
                typeof(PickaxeOfEvolution),
                typeof(PikeOfEvolution),
                typeof(PitchforkOfEvolution),
                typeof(QuarterStaffOfEvolution),
                typeof(RadiantScimitarOfEvolution),
                typeof(RepeatingCrossbowOfEvolution),
                typeof(SaiOfEvolution),
                typeof(ScepterOfEvolution),
                typeof(ScimitarOfEvolution),
                typeof(ScytheOfEvolution),
                typeof(ShepherdsCrookOfEvolution),
                typeof(ShortSpearOfEvolution),
                typeof(ShortbladeOfEvolution),
                typeof(SkinningKnifeOfEvolution),
                typeof(SoulGlaiveOfEvolution),
                typeof(SpearOfEvolution),
                typeof(TekagiOfEvolution),
                typeof(TessenOfEvolution),
                typeof(TetsuboOfEvolution),
                typeof(TwoHandedAxeOfEvolution),
                typeof(VikingSwordOfEvolution),
                typeof(WakizashiOfEvolution),
                typeof(WarAxeOfEvolution),
                typeof(WarCleaverOfEvolution),
                typeof(WarForkOfEvolution),
                typeof(WarHammerOfEvolution),
                typeof(WarMaceOfEvolution),
                typeof(WildStaffOfEvolution),
                typeof(YumiOfEvolution)
        };

        private static double GetEvoWeaponChance(TreasureLevel level)
        {
            switch (level)
            {
                case TreasureLevel.Cache: return 1.0 / 1000.0;
                case TreasureLevel.Hoard: return 1.0 / 500.0;
                case TreasureLevel.Trove: return 1.0 / 250.0;
            }
            return 0.0;
        }

        private static double GetBankHiveChance(TreasureLevel level)
        {
            switch (level)
            {
                case TreasureLevel.Cache: return 1.0 / 2000.0;
                case TreasureLevel.Hoard: return 1.0 / 1000.0;
                case TreasureLevel.Trove: return 1.0 / 500.0;
            }
            return 0.0;
        }

        private static double GetMobileForgeChance(TreasureLevel level)
        {
            switch (level)
            {
                case TreasureLevel.Cache: return 1.0 / 1500.0;
                case TreasureLevel.Hoard: return 1.0 / 750.0;
                case TreasureLevel.Trove: return 1.0 / 375.0;
            }
            return 0.0;
        }

        private static double GetCellarDeedChance(TreasureLevel level)
        {
            switch (level)
            {
                case TreasureLevel.Cache: return 1.0 / 2000.0;
                case TreasureLevel.Hoard: return 1.0 / 1000.0;
                case TreasureLevel.Trove: return 1.0 / 500.0;
            }
            return 0.0;
        }

        private static void DropUltraRareCustomRewards(TreasureMapChest chest, TreasureLevel level)
        {
            if (chest == null)
                return;

            double chance = GetEvoWeaponChance(level);
            if (chance > 0.0 && Utility.RandomDouble() < chance && _EvoWeaponJackpot.Length > 0)
            {
                Item evo = Loot.Construct(_EvoWeaponJackpot[Utility.Random(_EvoWeaponJackpot.Length)]);
                if (evo != null)
                    chest.DropItem(evo);
            }

            chance = GetBankHiveChance(level);
            if (chance > 0.0 && Utility.RandomDouble() < chance)
                chest.DropItem(new BankHive());

            chance = GetMobileForgeChance(level);
            if (chance > 0.0 && Utility.RandomDouble() < chance)
                chest.DropItem(new MobileForge());

            chance = GetCellarDeedChance(level);
            if (chance > 0.0 && Utility.RandomDouble() < chance)
                chest.DropItem(new CellarDeed());

            // Requested special case: Wolvesbane Dollar only comes from Troves, 1 in 750.
            if (level == TreasureLevel.Trove && Utility.RandomDouble() < (1.0 / 750.0))
                chest.DropItem(new WDollar());
        }

        public static void DropArtisanHighTierBonus(TreasureMapChest chest, TreasureLevel level, ChestQuality quality)
        {
            if (chest == null || level < TreasureLevel.Cache)
                return;

            // One compact material reward rather than dumping every resource type.
            Type[] materials = _MaterialTable[Math.Max(0, Math.Min(_MaterialTable.Length - 1, (int)quality - 1))];

            if (materials == null || materials.Length == 0)
                return;

            Item resource = Loot.Construct(materials[Utility.Random(materials.Length)]);

            if (resource == null)
                return;

            switch (level)
            {
                case TreasureLevel.Cache: resource.Amount = 50; break;
                case TreasureLevel.Hoard: resource.Amount = 75; break;
                case TreasureLevel.Trove: resource.Amount = 100; break;
            }

            chest.DropItem(resource);
        }

        public static int GetGemCount(ChestQuality quality, TreasureLevel level)
        {
            int baseAmount = 0;

            switch (quality)
            {
                case ChestQuality.Rusty: baseAmount = 7; break;
                case ChestQuality.Standard: baseAmount = Utility.RandomBool() ? 7 : 9; break;
                case ChestQuality.Gold: baseAmount = Utility.RandomList(7, 9, 11); break;
            }

            return baseAmount + ((int)level * 5);
        }

        public static int GetGoldCount(TreasureLevel level)
        {
            switch (level)
            {
                default:
                case TreasureLevel.Stash: return Utility.RandomMinMax(10000, 40000);
                case TreasureLevel.Supply: return Utility.RandomMinMax(20000, 50000);
                case TreasureLevel.Cache: return Utility.RandomMinMax(30000, 60000);
                case TreasureLevel.Hoard: return Utility.RandomMinMax(40000, 70000);
                case TreasureLevel.Trove: return Utility.RandomMinMax(60000, 90000);
            }
        }

        public static int GetRefinementRolls(ChestQuality quality)
        {
            switch (quality)
            {
                default:
                case ChestQuality.Rusty: return 2;
                case ChestQuality.Standard: return 4;
                case ChestQuality.Gold: return 6;
            }
        }

        public static int GetResourceAmount(TreasureLevel level)
        {
            switch (level)
            {
                case TreasureLevel.Stash: return 50;
                case TreasureLevel.Supply: return 100;
            }

            return 0;
        }

        public static int GetRegAmount(ChestQuality quality)
        {
            switch (quality)
            {
                default:
                case ChestQuality.Rusty: return 20;
                case ChestQuality.Standard: return 40;
                case ChestQuality.Gold: return 60;
            }
        }

        public static int GetSpecialResourceAmount(ChestQuality quality)
        {
            switch (quality)
            {
                default:
                case ChestQuality.Rusty: return 1;
                case ChestQuality.Standard: return 2;
                case ChestQuality.Gold: return 3;
            }
        }

        public static int GetEquipmentAmount(Mobile from, TreasureLevel level, TreasurePackage package)
        {
            int amount = 0;

            switch (level)
            {
                default:
                case TreasureLevel.Stash: amount = 6; break;
                case TreasureLevel.Supply: amount = 8; break;
                case TreasureLevel.Cache:
                    amount = package == TreasurePackage.Assassin ? 18 : 12;
                    break;

                // Wolvesbane Phase 7G:
                // High tiers improve quality instead of flooding the chest.
                // Artisan maps intentionally trade some combat gear volume for
                // crafting/decorative reward identity.
                case TreasureLevel.Hoard:
                    amount = package == TreasurePackage.Artisan ? 10 : 14;
                    break;
                case TreasureLevel.Trove:
                    amount = package == TreasurePackage.Artisan ? 12 : 18;
                    break;
            }

            Party p = Party.Get(from);

            if (p != null && p.Count > 1)
            {
                for (int i = 0; i < p.Count - 1; i++)
                {
                    if (Utility.RandomBool())
                    {
                        amount++;
                    }
                }
            }

            return amount;
        }

        public static void GetMinMaxBudget(TreasureLevel level, Item item, out int min, out int max)
        {
            int preArtifact = item != null ? Imbuing.GetMaxWeight(item) + 100 : 250;
            min = max = 0;

            switch (level)
            {
                default:
                case TreasureLevel.Stash:
                case TreasureLevel.Supply:
                    min = 250;
                    max = preArtifact;
                    break;

                case TreasureLevel.Cache:
                    min = 500;
                    max = 900;
                    break;

                case TreasureLevel.Hoard:
                    min = 650;
                    max = 1150;
                    break;

                case TreasureLevel.Trove:
                    min = 800;
                    max = 1300;
                    break;
            }
        }

        private static readonly Type[][][] _WeaponTable = new Type[][][]
        {
            new Type[][] // Artisan
                {
                    new Type[] { typeof(HammerPick), typeof(SledgeHammerWeapon), typeof(SmithyHammer), typeof(WarAxe), typeof(WarHammer), typeof(Axe), typeof(BattleAxe), typeof(DoubleAxe), typeof(ExecutionersAxe), typeof(Hatchet), typeof(LargeBattleAxe), typeof(OrnateAxe), typeof(TwoHandedAxe), typeof(Pickaxe) }, // Trammel, Felucca
                    null, // Ilshenar
                    null, // Malas
                    null, // Tokuno
                    new Type[] { typeof(HammerPick), typeof(SledgeHammerWeapon), typeof(SmithyHammer), typeof(WarAxe), typeof(WarHammer), typeof(Axe), typeof(BattleAxe), typeof(DoubleAxe), typeof(ExecutionersAxe), typeof(Hatchet), typeof(LargeBattleAxe), typeof(OrnateAxe), typeof(TwoHandedAxe), typeof(Pickaxe), typeof(DualShortAxes) },  // TerMur
                    new Type[] {  }  // Eodon
                },
            new Type[][] // Assassin
                {
                    new Type[] { typeof(Dagger), typeof(Kryss), typeof(Cleaver), typeof(Cutlass), typeof(ElvenMachete) },
                    null,
                    null,
                    null,
                    new Type[] { typeof(Dagger), typeof(Kryss), typeof(Cleaver), typeof(Cutlass) },
                    new Type[] { typeof(Dagger), typeof(Kryss), typeof(Cleaver), typeof(Cutlass), typeof(BladedWhip), typeof(BarbedWhip), typeof(SpikedWhip) },
                },
            new Type[][] // Mage
                {
                    new Type[] { typeof(BlackStaff), typeof(ShepherdsCrook), typeof(GnarledStaff), typeof(QuarterStaff) },
                    null,
                    null,
                    null,
                    null,
                    null,
                },
            new Type[][] // Ranger
                {
                    new Type[] { typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow), typeof(CompositeBow), typeof(ButcherKnife), typeof(SkinningKnife) },
                    new Type[] { typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow), typeof(CompositeBow), typeof(ButcherKnife), typeof(SkinningKnife), typeof(SoulGlaive) },
                    new Type[] { typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow), typeof(CompositeBow), typeof(ButcherKnife), typeof(SkinningKnife), typeof(ElvenCompositeLongbow) },
                    null,
                    new Type[] { typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow), typeof(CompositeBow), typeof(ButcherKnife), typeof(SkinningKnife), typeof(GargishButcherKnife), typeof(Cyclone), typeof(SoulGlaive) },
                    null,
                },
            new Type[][] // Warrior
                {
                    new Type[] { typeof(Lance), typeof(Pike), typeof(Pitchfork), typeof(ShortSpear), typeof(WarFork), typeof(Club), typeof(Mace), typeof(Maul), typeof(WarAxe), typeof(Bardiche), typeof(Broadsword), typeof(CrescentBlade), typeof(Halberd), typeof(Longsword), typeof(Scimitar), typeof(VikingSword) },
                    null,
                    null,
                    new Type[] { typeof(Lance), typeof(Pike), typeof(Pitchfork), typeof(ShortSpear), typeof(WarFork), typeof(Club), typeof(Mace), typeof(Maul), typeof(WarAxe), typeof(Bardiche), typeof(Broadsword), typeof(CrescentBlade), typeof(Halberd), typeof(Longsword), typeof(Scimitar), typeof(VikingSword), typeof(Bokuto), typeof(Daisho) },
                    null,
                    null,
                },
        };

        private static readonly Type[][][] _ArmorTable = new Type[][][]
        {
            new Type[][] // Artisan
                {
                    new Type[] { typeof(Bonnet), typeof(Cap), typeof(Circlet), typeof(ElvenGlasses), typeof(FeatheredHat), typeof(FlowerGarland), typeof(JesterHat), typeof(SkullCap), typeof(StrawHat), typeof(TallStrawHat), typeof(WideBrimHat) }, // Trammel/Fel
                    null, // Ilshenar
                    null, // Malas
                    null, // Tokuno
                    null, // TerMur
                    new Type[] { typeof(Bonnet), typeof(Cap), typeof(Circlet), typeof(ElvenGlasses), typeof(FeatheredHat), typeof(FlowerGarland), typeof(JesterHat), typeof(SkullCap), typeof(StrawHat), typeof(TallStrawHat), typeof(WideBrimHat), typeof(ChefsToque) }, // Eodon
                },
            new Type[][] // Assassin
                {
                    new Type[] { typeof(ChainLegs), typeof(ChainCoif), typeof(ChainChest), typeof(RingmailLegs), typeof(RingmailGloves), typeof(RingmailChest), typeof(RingmailArms), typeof(Bandana) }, // Trammel/Fel
                    null, // Ilshenar
                    null, // Malas
                    new Type[] { typeof(ChainLegs), typeof(ChainCoif), typeof(ChainChest), typeof(RingmailLegs), typeof(RingmailGloves), typeof(RingmailArms), typeof(RingmailArms), typeof(Bandana), typeof(LeatherSuneate), typeof(LeatherMempo), typeof(LeatherJingasa), typeof(LeatherHiroSode), typeof(LeatherHaidate), typeof(LeatherDo) }, // Tokuno
                    null, // TerMur
                    null, // Eodon
                },
            new Type[][] // Mage
                {
                    new Type[] { typeof(LeafGloves), typeof(LeafLegs), typeof(LeafTonlet), typeof(LeafGorget), typeof(LeafArms),typeof(LeafChest), typeof(LeatherArms), typeof(LeatherChest), typeof(LeatherLegs), typeof(LeatherGloves), typeof(LeatherGorget), typeof(WizardsHat) }, // Trammel/Fel
                    null, // Ilshenar
                    new Type[] { typeof(LeafGloves), typeof(LeafLegs), typeof(LeafTonlet), typeof(LeafGorget), typeof(LeafArms),typeof(LeafChest), typeof(LeatherArms), typeof(LeatherChest), typeof(LeatherLegs), typeof(LeatherGloves), typeof(LeatherGorget), typeof(WizardsHat), typeof(BoneLegs), typeof(BoneHelm), typeof(BoneGloves), typeof(BoneChest), typeof(BoneArms) }, // Malas
                    null, // Tokuno
                    new Type[] { typeof(LeatherArms), typeof(LeatherChest), typeof(LeatherLegs), typeof(LeatherGloves), typeof(LeatherGorget), typeof(WizardsHat) }, // TerMur
                    new Type[] { typeof(LeatherArms), typeof(LeatherChest), typeof(LeatherLegs), typeof(LeatherGloves), typeof(LeatherGorget), typeof(WizardsHat) }, // Eodon
                },
            new Type[][] // Ranger
                {
                    new Type[] { typeof(HidePants), typeof(HidePauldrons), typeof(HideGorget), typeof(HideFemaleChest), typeof(HideChest), typeof(HideGloves), typeof(StuddedLegs), typeof(StuddedGorget), typeof(StuddedGloves), typeof(StuddedChest), typeof(StuddedBustierArms), typeof(StuddedArms), typeof(RavenHelm), typeof(VultureHelm), typeof(WingedHelm) }, // Trammel/Fel
                    null, // Ilshenar
                    null, // Malas
                    new Type[] { typeof(StuddedLegs), typeof(StuddedGorget), typeof(StuddedGloves), typeof(StuddedChest), typeof(StuddedBustierArms), typeof(StuddedArms) }, // Tokuno
                    new Type[] { typeof(HidePants), typeof(HidePauldrons), typeof(HideGorget), typeof(HideFemaleChest), typeof(HideChest), typeof(HideGloves), typeof(StuddedLegs), typeof(StuddedGorget), typeof(StuddedGloves), typeof(StuddedChest), typeof(StuddedBustierArms), typeof(StuddedArms), typeof(GargishLeatherKilt), typeof(GargishLeatherLegs), typeof(GargishLeatherArms), typeof(GargishLeatherChest) }, // TerMur
                    new Type[] { typeof(StuddedLegs), typeof(StuddedGorget), typeof(StuddedGloves), typeof(StuddedChest), typeof(StuddedBustierArms), typeof(StuddedArms), typeof(TigerPeltSkirt), typeof(TigerPeltShorts), typeof(TigerPeltLegs), typeof(TigerPeltLongSkirt), typeof(TigerPeltHelm), typeof(TigerPeltChest), typeof(TigerPeltCollar), typeof(TigerPeltBustier), typeof(VultureHelm), typeof(TribalMask) }, // Eodon
                },
            new Type[][] // Warrior
                {
                    new Type[] { typeof(PlateLegs), typeof(PlateHelm), typeof(PlateGorget), typeof(PlateGloves), typeof(PlateChest), typeof(PlateArms), typeof(Bascinet), typeof(CloseHelm), typeof(Helmet), typeof(LeatherCap), typeof(NorseHelm), typeof(TricorneHat), typeof(BronzeShield), typeof(Buckler), typeof(ChaosShield), typeof(HeaterShield), typeof(MetalKiteShield), typeof(MetalShield), typeof(OrderShield), typeof(WoodenKiteShield) }, // Trammel/Fel
                    null, // Ilshenar
                    new Type[] { typeof(PlateLegs), typeof(PlateHelm), typeof(PlateGorget), typeof(PlateGloves), typeof(PlateChest), typeof(PlateArms), typeof(Bascinet), typeof(CloseHelm), typeof(Helmet), typeof(LeatherCap), typeof(NorseHelm), typeof(TricorneHat), typeof(BronzeShield), typeof(Buckler), typeof(ChaosShield), typeof(HeaterShield), typeof(MetalKiteShield), typeof(MetalShield), typeof(OrderShield), typeof(WoodenKiteShield), typeof(DragonHelm), typeof(DragonGloves), typeof(DragonChest), typeof(DragonArms), typeof(DragonLegs) }, // Malas
                    new Type[] { typeof(PlateLegs), typeof(PlateHelm), typeof(PlateGorget), typeof(PlateGloves), typeof(PlateChest), typeof(PlateArms), typeof(Bascinet), typeof(CloseHelm), typeof(Helmet), typeof(LeatherCap), typeof(NorseHelm), typeof(TricorneHat), typeof(BronzeShield), typeof(Buckler), typeof(ChaosShield), typeof(HeaterShield), typeof(MetalKiteShield), typeof(MetalShield), typeof(OrderShield), typeof(WoodenKiteShield), typeof(PlateSuneate), typeof(PlateMempo), typeof(PlateHiroSode), typeof(PlateHatsuburi), typeof(PlateHaidate), typeof(PlateDo), typeof(PlateBattleKabuto), typeof(DecorativePlateKabuto), typeof(LightPlateJingasa), typeof(SmallPlateJingasa)  }, // Tokuno
                    new Type[] { typeof(PlateLegs), typeof(PlateHelm), typeof(PlateGorget), typeof(PlateGloves), typeof(PlateChest), typeof(PlateArms), typeof(Bascinet), typeof(CloseHelm), typeof(Helmet), typeof(LeatherCap), typeof(NorseHelm), typeof(TricorneHat), typeof(BronzeShield), typeof(Buckler), typeof(ChaosShield), typeof(HeaterShield), typeof(MetalKiteShield), typeof(MetalShield), typeof(OrderShield), typeof(WoodenKiteShield), typeof(GargishPlateArms), typeof(GargishPlateChest), typeof(GargishPlateKilt), typeof(GargishPlateLegs), typeof(GargishStoneKilt), typeof(GargishStoneLegs), typeof(GargishStoneArms), typeof(GargishStoneChest) }, // TerMur
                    new Type[] { typeof(PlateLegs), typeof(PlateHelm), typeof(PlateGorget), typeof(PlateGloves), typeof(PlateChest), typeof(PlateArms), typeof(Bascinet), typeof(CloseHelm), typeof(Helmet), typeof(LeatherCap), typeof(NorseHelm), typeof(TricorneHat), typeof(BronzeShield), typeof(Buckler), typeof(ChaosShield), typeof(HeaterShield), typeof(MetalKiteShield), typeof(MetalShield), typeof(OrderShield), typeof(WoodenKiteShield), typeof(DragonTurtleHideHelm), typeof(DragonTurtleHideLegs), typeof(DragonTurtleHideChest), typeof(DragonTurtleHideBustier), typeof(DragonTurtleHideArms) }, // Eodon
                }
        };

        public static Type[][] _MaterialTable = new Type[][]
        {
            new Type[] { typeof(SpinedLeather), typeof(OakBoard), typeof(AshBoard), typeof(DullCopperIngot), typeof(ShadowIronIngot), typeof(CopperIngot) },
            new Type[] { typeof(HornedLeather), typeof(YewBoard), typeof(HeartwoodBoard), typeof(BronzeIngot), typeof(GoldIngot), typeof(AgapiteIngot) },
            new Type[] { typeof(BarbedLeather), typeof(BloodwoodBoard), typeof(FrostwoodBoard), typeof(ValoriteIngot), typeof(VeriteIngot) }
        };

        public static Type[][] _JewelTable = new Type[][]
            {
                new Type[] { typeof(GoldRing), typeof(GoldBracelet), typeof(SilverRing), typeof(SilverBracelet) }, // standard
                new Type[] { typeof(GoldRing), typeof(GoldBracelet), typeof(SilverRing), typeof(SilverBracelet), typeof(GargishBracelet) }, // Ranger/TerMur
            };

        public static Type[][] _DecorativeTable = new Type[][]
            {/*
                new Type[] { typeof(SkullTiledFloorAddonDeed) },
                new Type[] { typeof(AncientWeapon3) },
                new Type[] { typeof(DecorativeHourglass) },
                new Type[] { typeof(AncientWeapon1), typeof(CreepingVine) },
                new Type[] { typeof(AncientWeapon2) },
           */ };
		
        public static Type[][] _SpecialMaterialTable = new Type[][]
            {
                null, // tram
                null, // fel
                null, // ilsh
                new Type[] { typeof(LuminescentFungi), typeof(BarkFragment), typeof(Blight), typeof(Corruption), typeof(Muculent),/* typeof(Putrefaction),*/ typeof(Scourge), typeof(Taint)  }, // malas
                null, // tokuno
                TreasureMapChest.ImbuingIngreds, // ter
                null, // eodon
            };

        public static Type[][] _SpecialSupplyLoot = new Type[][]
            {/*
                new Type[] { typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding), typeof(ShieldEngravingTool), null },
                new Type[] { typeof(ForgedPardon), typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding), typeof(Skeletonkey), typeof(MasterSkeletonKey), typeof(SurgeShield) },
                new Type[] { typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding) },
                new Type[] { typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding), typeof(TastyTreat) },
                new Type[] { typeof(LegendaryMapmakersGlasses), typeof(ManaPhasingOrb), typeof(RunedSashOfWarding) },
            */};

        public static Type[] _SpecialCacheHordeAndTrove = new Type[]
            {
                //typeof(OctopusNecklace), typeof(SkullGnarledStaff), typeof(SkullLongsword)
            };
	   
        public static Type[] _DecorativeMinorArtifacts = new Type[]
            {
                typeof(CandelabraOfSouls), typeof(GoldBricks), typeof(PhillipsWoodenSteed), typeof(AncientShipModelOfTheHMSCape), typeof(AdmiralsHeartyRum)
            };

        public static Type[] _FunctionalMinorArtifacts = new Type[]
            {
                typeof(ArcticDeathDealer), typeof(BlazeOfDeath), typeof(BurglarsBandana),
                typeof(CavortingClub), typeof(DreadPirateHat),
                typeof(EnchantedTitanLegBone), typeof(GwennosHarp), typeof(IolosLute),
                typeof(LunaLance), typeof(NightsKiss), typeof(NoxRangersHeavyCrossbow),
                typeof(PolarBearMask), typeof(VioletCourage), typeof(HeartOfTheLion),
                typeof(ColdBlood), typeof(AlchemistsBauble), typeof(CaptainQuacklebushsCutlass),
                typeof(ShieldOfInvulnerability),
            };

        public static SkillName[][] _TranscendenceTable = new SkillName[][]
            {
                new SkillName[] { SkillName.ArmsLore, SkillName.Blacksmith, SkillName.Carpentry, SkillName.Cartography, SkillName.Cooking, SkillName.Cooking, SkillName.Fletching, SkillName.Mining, SkillName.Tailoring },
                new SkillName[] { SkillName.Anatomy, SkillName.DetectHidden, SkillName.Fencing, SkillName.Poisoning, SkillName.RemoveTrap, SkillName.Snooping, SkillName.Stealth },
                new SkillName[] { SkillName.Magery, SkillName.Meditation, SkillName.MagicResist, SkillName.Spellweaving },
                new SkillName[] { SkillName.Alchemy, SkillName.AnimalLore, SkillName.AnimalTaming, SkillName.Archery, },
                new SkillName[] { SkillName.Chivalry, SkillName.Focus, SkillName.Parry, SkillName.Swords, SkillName.Tactics, SkillName.Wrestling },
            };

        public static SkillName[][] _AlacrityTable = new SkillName[][]
           {
                new SkillName[] { SkillName.ArmsLore, SkillName.Blacksmith, SkillName.Carpentry, SkillName.Cartography, SkillName.Cooking, SkillName.Cooking, SkillName.Fletching, SkillName.Mining, SkillName.Tailoring, SkillName.Lumberjacking },
                new SkillName[] { SkillName.DetectHidden, SkillName.Fencing, SkillName.Hiding, SkillName.Lockpicking, SkillName.Poisoning, SkillName.RemoveTrap, SkillName.Snooping, SkillName.Stealing, SkillName.Stealth },
                new SkillName[] { SkillName.Alchemy, SkillName.EvalInt, SkillName.Inscribe, SkillName.Magery, SkillName.Meditation, SkillName.Spellweaving, SkillName.SpiritSpeak },
                new SkillName[] { SkillName.AnimalLore, SkillName.AnimalTaming, SkillName.Archery, SkillName.Musicianship, SkillName.Peacemaking, SkillName.Provocation, SkillName.Tinkering, SkillName.Tracking, SkillName.Veterinary },
                new SkillName[] { SkillName.Chivalry, SkillName.Focus, SkillName.Macing, SkillName.Parry, SkillName.Swords, SkillName.Wrestling },
           };

        public static SkillName[][] _PowerscrollTable = new SkillName[][]
            {
                null,
                new SkillName[] { SkillName.Ninjitsu },
                new SkillName[] { SkillName.Magery, SkillName.Meditation, SkillName.Mysticism, SkillName.Spellweaving, SkillName.SpiritSpeak },
                new SkillName[] { SkillName.AnimalTaming, SkillName.Discordance, SkillName.Provocation, SkillName.Veterinary },
                new SkillName[] { SkillName.Bushido, SkillName.Chivalry, SkillName.Focus, SkillName.Healing, SkillName.Parry, SkillName.Swords, SkillName.Tactics },
            };

        public static void Fill(Mobile from, TreasureMapChest chest, TreasureMap tMap)
        {
            TreasureLevel level = tMap.TreasureLevel;
            TreasurePackage package = tMap.Package;
            TreasureFacet facet = tMap.TreasureFacet;
            ChestQuality quality = chest.ChestQuality;

            chest.Movable = false;
            chest.Locked = true;

            chest.TrapType = TrapType.ExplosionTrap;

            switch ((int)level)
            {
                default:
                case 0:
                    chest.RequiredSkill = 5;
                    chest.TrapPower = 25;
                    chest.TrapLevel = 1;
                    break;
                case 1:
                    chest.RequiredSkill = 45;
                    chest.TrapPower = 75;
                    chest.TrapLevel = 3;
                    break;
                case 2:
                    chest.RequiredSkill = 75;
                    chest.TrapPower = 125;
                    chest.TrapLevel = 5;
                    break;
                case 3:
                    chest.RequiredSkill = 80;
                    chest.TrapPower = 150;
                    chest.TrapLevel = 6;
                    break;
                case 4:
                    chest.RequiredSkill = 80;
                    chest.TrapPower = 170;
                    chest.TrapLevel = 7;
                    break;
            }

            chest.LockLevel = chest.RequiredSkill - 10;
            chest.MaxLockLevel = chest.RequiredSkill + 40;

            /*if (Engines.JollyRoger.JollyRogerEvent.Instance.Running && 0.10 > Utility.RandomDouble())
            {
                chest.DropItem(new MysteriousFragment());
            }
			*/
            #region Refinements
            if (level == TreasureLevel.Stash)
            {
                RefinementComponent.Roll(chest, GetRefinementRolls(quality), 0.9);
            }
            #endregion

            #region TMaps
            bool dropMap = false;
            if (level < TreasureLevel.Trove && 0.1 > Utility.RandomDouble())
            {
                chest.DropItem(new TreasureMap(tMap.Level + 1, chest.Map));
                dropMap = true;
            }
            #endregion

            Type[] list = null;
            int amount = 0;
            double dropChance = 0.0;

            #region Gold
            int goldAmount = GetGoldCount(level);
            Bag lootBag = new BagOfGold();

            while (goldAmount > 0)
            {
                if (goldAmount <= 20000)
                {
                    lootBag.DropItem(new Gold(goldAmount));
                    goldAmount = 0;
                }
                else
                {
                    lootBag.DropItem(new Gold(20000));
                    goldAmount -= 20000;
                }

                chest.DropItem(lootBag);
            }
            #endregion

            #region Regs
            list = GetReagentList(level, package, facet);

            if (list != null)
            {
                amount = GetRegAmount(quality);
                lootBag = new BagOfRegs();

                for (int i = 0; i < amount; i++)
                {
                    lootBag.DropItemStacked(Loot.Construct(list));
                }

                chest.DropItem(lootBag);
                list = null;
            }
            #endregion

            #region Gems
            amount = GetGemCount(quality, level);

            if (amount > 0)
            {
                lootBag = new BagOfGems();

                foreach (Type gemType in Loot.GemTypes)
                {
                    Item gem = Loot.Construct(gemType);
                    gem.Amount = amount;

                    lootBag.DropItem(gem);

                }

                chest.DropItem(lootBag);
            }
            #endregion

            #region Crafting Resources
            // TODO: DO each drop, or do only 1 drop?
            list = GetCraftingMaterials(level, package, quality);

            if (list != null)
            {
                amount = GetResourceAmount(level);

                foreach (Type type in list)
                {
                    Item craft = Loot.Construct(type);
                    craft.Amount = amount;

                    chest.DropItem(craft);
                }

                list = null;
            }
            #endregion

            #region Special Resources
            // TODO: DO each drop, or do only 1 drop?
            list = GetSpecialMaterials(level, package, facet);

            if (list != null)
            {
                amount = GetSpecialResourceAmount(quality);

                foreach (Type type in list)
                {
                    Item specialCraft = Loot.Construct(type);
                    specialCraft.Amount = amount;

                    chest.DropItem(specialCraft);
                }

                list = null;
            }
            #endregion

            #region Special Scrolls
            amount = (int)level + 1;

            if (dropMap)
            {
                amount--;
            }

            if (amount > 0)
            {
                SkillName[] transList = GetTranscendenceList(level, package);
                SkillName[] alacList = GetAlacrityList(level, package, facet);
                SkillName[] pscrollList = GetPowerScrollList(level, package, facet);

                List<Tuple<int, SkillName>> scrollList = new List<Tuple<int, SkillName>>();

                if (transList != null)
                {
                    foreach (SkillName sk in transList)
                    {
                        scrollList.Add(new Tuple<int, SkillName>(1, sk));
                    }
                }

                if (alacList != null)
                {
                    foreach (SkillName sk in alacList)
                    {
                        scrollList.Add(new Tuple<int, SkillName>(2, sk));
                    }
                }

                if (pscrollList != null)
                {
                    foreach (SkillName sk in pscrollList)
                    {
                        scrollList.Add(new Tuple<int, SkillName>(3, sk));
                    }
                }

                if (scrollList.Count > 0)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        Tuple<int, SkillName> random = scrollList[Utility.Random(scrollList.Count)];

                        switch (random.Item1)
                        {
                            case 1: chest.DropItem(new ScrollOfTranscendence(random.Item2, Utility.RandomMinMax(1.0, chest.Map == Map.Felucca ? 7.0 : 5.0) / 10)); break;
                            case 2: chest.DropItem(new ScrollOfAlacrity(random.Item2)); break;
                            case 3: chest.DropItem(new PowerScroll(random.Item2, 110.0)); break;
                        }
                    }
                }
            }
            #endregion

            #region Decorations
            switch (level)
            {
                case TreasureLevel.Stash: dropChance = 0.00; break;
                case TreasureLevel.Supply: dropChance = 0.10; break;
                case TreasureLevel.Cache: dropChance = 0.20; break;
                case TreasureLevel.Hoard: dropChance = 0.40; break;
                case TreasureLevel.Trove: dropChance = 0.50; break;
            }

            if (Utility.RandomDouble() < dropChance)
            {
                list = GetDecorativeList(level, package, facet);

                if (list != null)
                {
                    if (list.Length > 0)
                    {
                        Item deco = Loot.Construct(list[Utility.Random(list.Length)]);

                        if (_DecorativeMinorArtifacts.Any(t => t == deco.GetType()))
                        {
                            Container pack = new Backpack
                            {
                                Hue = 1278
                            };

                            pack.DropItem(deco);
                            chest.DropItem(pack);
                        }
                        else
                        {
                            chest.DropItem(deco);
                        }
                    }

                    list = null;
                }
            }

            switch (level)
            {
                case TreasureLevel.Stash: dropChance = 0.00; break;
                case TreasureLevel.Supply: dropChance = 0.10; break;
                case TreasureLevel.Cache: dropChance = 0.20; break;
                case TreasureLevel.Hoard: dropChance = 0.50; break;
                case TreasureLevel.Trove: dropChance = 0.75; break;
            }

            if (Utility.RandomDouble() < dropChance)
            {
                list = GetSpecialLootList(level, package);

                if (list != null)
                {
                    if (list.Length > 0)
                    {
                        // The original Publish-era reward block was commented out
                        // because several of its newer item types were not present in
                        // this older/custom Wolvesbane tree. Restore the reward roll
                        // using ONLY types already active in Wolvesbane's compiled
                        // artifact arrays.
                        Type type = list[Utility.Random(list.Length)];
                        Item reward = type != null ? Loot.Construct(type) : null;

                        if (reward != null)
                        {
                            bool artifactBag =
                                (_FunctionalMinorArtifacts != null &&
                                 _FunctionalMinorArtifacts.Any(t => t == type)) ||
                                (_DecorativeMinorArtifacts != null &&
                                 _DecorativeMinorArtifacts.Any(t => t == type));

                            if (artifactBag)
                            {
                                Container pack = new Backpack
                                {
                                    Hue = 1278
                                };

                                pack.DropItem(reward);
                                chest.DropItem(pack);
                            }
                            else
                            {
                                chest.DropItem(reward);
                            }
                        }
                    }

                    list = null;
                }
            }
            #endregion

            #region Wolvesbane Artisan High-Tier Bonus
            if (package == TreasurePackage.Artisan)
            {
                DropArtisanHighTierBonus(chest, level, quality);
            }
            #endregion

            #region Wolvesbane Rare Custom Gear Jackpot
            double customGearChance = GetCustomGearChance(level);

            if (customGearChance > 0.0 && Utility.RandomDouble() < customGearChance)
            {
                Type customGearType = GetRandomCustomGear(package);
                Item customGear = customGearType != null ? Loot.Construct(customGearType) : null;

                if (customGear != null)
                {
                    // Put jackpot gear in the same distinctive artifact backpack used
                    // by the existing treasure-map special rewards.
                    Container jackpotPack = new Backpack
                    {
                        Hue = 1278
                    };

                    jackpotPack.DropItem(customGear);
                    chest.DropItem(jackpotPack);
                }
            }
            #endregion

            #region Wolvesbane Ultra-Rare Custom Rewards
            DropUltraRareCustomRewards(chest, level);
            #endregion

            #region Magic Equipment
            amount = GetEquipmentAmount(from, level, package);

            foreach (Type type in GetRandomEquipment(level, package, facet, amount))
            {
                Item item = Loot.Construct(type);
                int min, max;
                GetMinMaxBudget(level, item, out min, out max);

                if (item != null)
                {
                    RunicReforging.GenerateRandomItem(item, from is PlayerMobile ? ((PlayerMobile)from).RealLuck : from.Luck, min, max, chest.Map);
                    chest.DropItem(item);
                }
            }

            list = null;
            #endregion
        }

        /*private static Type MutateType(Type type, TreasureFacet facet)
        {
            if (type == typeof(SkullGnarledStaff))
            {
                type = typeof(GargishSkullGnarledStaff);
            }
            else if (type == typeof(SkullLongsword))
            {
                type = typeof(GargishSkullLongsword);
            }

            return type;
        }*/
    }
}

