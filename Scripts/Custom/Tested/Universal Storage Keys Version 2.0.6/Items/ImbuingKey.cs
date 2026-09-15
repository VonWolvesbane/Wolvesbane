using System;
using System.Collections.Generic;
using Server;
using Solaris.ItemStore;

namespace Server.Items
{
    /// <summary>
    /// Wolvesbane Imbuing Storage Key
    ///
    /// Built for Universal Storage Keys Version 2.0.6.
    /// Stores the core imbuing resources, rare gems and special imbuing
    /// ingredients that exist on the shard.
    ///
    /// Ingredient types are resolved by name at runtime so this key remains
    /// compile-safe if a particular expansion ingredient is absent from the shard.
    /// </summary>
    public class ImbuingKey : BaseStoreKey
    {
        // Two columns keeps the gump compact without making long ingredient lists
        // excessively tall.
        public override int DisplayColumns
        {
            get { return 2; }
        }

        public override List<StoreEntry> EntryStructure
        {
            get
            {
                List<StoreEntry> entries = base.EntryStructure;

                // Core imbuing resources
                AddResource(entries, "MagicalResidue", "Magical Residue");
                AddResource(entries, "EnchantedEssence", "Enchanted Essence");
                AddResource(entries, "RelicFragment", "Relic Fragment");

                // Rare / special gems commonly used by imbuing recipes.
                // Standard gems are intentionally left to the existing Gem Key
                // to avoid duplicate entries when both keys are inside a Master Key.
                AddResource(entries, "BlueDiamond", "Blue Diamond");
                AddResource(entries, "BrilliantAmber", "Brilliant Amber");
                AddResource(entries, "DarkSapphire", "Dark Sapphire");
                AddResource(entries, "EcruCitrine", "Ecru Citrine");
                AddResource(entries, "FireRuby", "Fire Ruby");
                AddResource(entries, "PerfectEmerald", "Perfect Emerald");
                AddResource(entries, "Turquoise", "Turquoise");
                AddResource(entries, "WhitePearl", "White Pearl");

                // Special imbuing ingredients
                AddResource(entries, "AbyssalCloth", "Abyssal Cloth");
                AddResource(entries, "BouraPelt", "Boura Pelt");
                AddResource(entries, "ChagaMushroom", "Chaga Mushroom");
                AddResource(entries, "CrystallineBlackrock", "Crystalline Blackrock");
                AddResource(entries, "DaemonClaw", "Daemon Claw");
                AddResource(entries, "DelicateScales", "Delicate Scales");
                AddResource(entries, "ElvenFletching", "Elven Fletching");

                // Wolvesbane additions requested 2026-09-14.
                // Runtime lookup keeps these safe across ServUO/custom naming variants.
                AddResource(entries, "SilverSerpentVenom", "Silver Serpent Venom");
                AddResource(entries, "ToxicVenomSac", "Toxic Venom Sac");
                AddResource(entries, "SlithEye", "Slith's Eye");
                AddResource(entries, "SlithsEye", "Slith's Eye");
                AddResource(entries, "BottleIchor", "Bottle of Ichor");
                AddResource(entries, "BottleOfIchor", "Bottle of Ichor");

                AddResource(entries, "EssenceOfAchievement", "Essence of Achievement");
                AddResource(entries, "EssenceOfBalance", "Essence of Balance");
                AddResource(entries, "EssenceOfControl", "Essence of Control");
                AddResource(entries, "EssenceOfDiligence", "Essence of Diligence");
                AddResource(entries, "EssenceOfDirection", "Essence of Direction");
                AddResource(entries, "EssenceOfFeeling", "Essence of Feeling");
                AddResource(entries, "EssenceOfOrder", "Essence of Order");
                AddResource(entries, "EssenceOfPassion", "Essence of Passion");
                AddResource(entries, "EssenceOfPersistence", "Essence of Persistence");
                AddResource(entries, "EssenceOfPrecision", "Essence of Precision");
                AddResource(entries, "EssenceOfSingularity", "Essence of Singularity");
                AddResource(entries, "EssenceOfSpirituality", "Essence of Spirituality");

                AddResource(entries, "FaeryDust", "Faery Dust");
                AddResource(entries, "FeyWings", "Fey Wings");
                AddResource(entries, "GoblinBlood", "Goblin Blood");
                AddResource(entries, "LavaSerpentCrust", "Lava Serpent Crust");
                AddResource(entries, "LuminescentFungi", "Luminescent Fungi");
                AddResource(entries, "ParasiticPlant", "Parasitic Plant");
                AddResource(entries, "PowderedIron", "Powdered Iron");
                AddResource(entries, "RaptorTeeth", "Raptor Teeth");
                AddResource(entries, "ReflectiveWolfEye", "Reflective Wolf Eye");
                AddResource(entries, "SeedOfRenewal", "Seed of Renewal");
                AddResource(entries, "SilverSnakeSkin", "Silver Snake Skin");
                AddResource(entries, "SlithTongue", "Slith Tongue");
                AddResource(entries, "SpiderCarapace", "Spider Carapace");
                AddResource(entries, "UndyingFlesh", "Undying Flesh");
                AddResource(entries, "VialOfVitriol", "Vial of Vitriol");
                AddResource(entries, "VoidCore", "Void Core");
                AddResource(entries, "VoidOrb", "Void Orb");

                // Additional names seen on some ServUO/custom distributions.
                // These are harmless if they do not exist; AddResource simply skips them.
                AddResource(entries, "CrystalShards", "Crystal Shards");
                AddResource(entries, "CrushedGlass", "Crushed Glass");
                AddResource(entries, "CrystalDust", "Crystal Dust");
                AddResource(entries, "ArcanicRuneStone", "Arcanic Rune Stone");
                AddResource(entries, "ArachnidCarapace", "Arachnid Carapace");

                return entries;
            }
        }

        /// <summary>
        /// Runtime type lookup prevents compile failures when Wolvesbane does not
        /// contain one of the optional expansion/custom ingredient classes.
        /// </summary>
        private static void AddResource(List<StoreEntry> entries, string typeName, string displayName)
        {
            if (entries == null || String.IsNullOrEmpty(typeName))
                return;

            Type type = null;

            try
            {
                type = ScriptCompiler.FindTypeByName(typeName);
            }
            catch
            {
                type = null;
            }

            if (type == null || !typeof(Item).IsAssignableFrom(type))
                return;

            // Avoid accidental duplicate entries if two aliases resolve to
            // the same underlying item class.
            for (int i = 0; i < entries.Count; ++i)
            {
                StoreEntry existing = entries[i];

                if (existing != null && existing.Type == type)
                    return;
            }

            entries.Add(new ResourceEntry(type, displayName));
        }

        [Constructable]
        public ImbuingKey()
            : base(0x48D) // deep arcane blue/purple
        {
            // Key-ring artwork. The Universal Storage Key gump supplies the real UI.
            ItemID = 0x2FEA; // display case graphic; larger and more visible than the key-ring art
            Name = "Imbuing Storage Key";
        }

        protected override ItemStore GenerateItemStore()
        {
            ItemStore store = base.GenerateItemStore();

            store.Label = "Imbuing Ingredient Storage";
            store.Dynamic = false;

            // These are crafting ingredients rather than bulk commodity resources.
            // Keep withdrawals as normal items instead of commodity deeds.
            store.OfferDeeds = false;

            return store;
        }

        public ImbuingKey(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
