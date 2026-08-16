using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;
using daat99;

namespace Server.Commands
{
    public class WBOWLTRAudit
    {
        public static void Initialize()
        {
            CommandSystem.Register("WBOWLTRAudit", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("WBOWLTRAudit [verbose]")]
        [Description("Read-only audit of Daat99 OWLTR holder serialization volume.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            bool verbose = e.Arguments != null && e.Arguments.Length > 0 && String.Equals(e.Arguments[0], "verbose", StringComparison.OrdinalIgnoreCase);

            Hashtable holders = Daat99OWLTR.StaticHolders;
            Hashtable temp = Daat99OWLTR.TempHolders;

            if (holders == null)
            {
                from.SendMessage(33, "Daat99OWLTR.StaticHolders is null.");
                return;
            }

            int holderCount = 0;
            int nullKeyCount = 0;
            int nullHolderCount = 0;
            int deletedMobileCount = 0;
            int playerMobileCount = 0;
            int nonPlayerMobileCount = 0;
            long recipeEntries = 0;
            long resourceEntries = 0;
            long recipeStringChars = 0;
            long recipeStringUtf8Bytes = 0;
            int minRecipes = Int32.MaxValue;
            int maxRecipes = 0;
            long zeroRecipeHolders = 0;
            Dictionary<string, int> recipeFrequency = new Dictionary<string, int>();
            List<HolderStat> largest = new List<HolderStat>();

            foreach (DictionaryEntry de in holders)
            {
                Mobile owner = de.Key as Mobile;
                NewDaat99Holder holder = de.Value as NewDaat99Holder;

                if (owner == null)
                {
                    nullKeyCount++;
                    continue;
                }

                if (holder == null)
                {
                    nullHolderCount++;
                    continue;
                }

                holderCount++;

                if (owner.Deleted)
                    deletedMobileCount++;

                if (owner is PlayerMobile)
                    playerMobileCount++;
                else
                    nonPlayerMobileCount++;

                ArrayList recipes = holder.ItemTypeList;
                ArrayList resources = holder.Resources;
                int rc = recipes == null ? 0 : recipes.Count;
                int resc = resources == null ? 0 : resources.Count;

                recipeEntries += rc;
                resourceEntries += resc;

                if (rc == 0)
                    zeroRecipeHolders++;
                if (rc < minRecipes)
                    minRecipes = rc;
                if (rc > maxRecipes)
                    maxRecipes = rc;

                if (recipes != null)
                {
                    for (int i = 0; i < recipes.Count; i++)
                    {
                        Type t = recipes[i] as Type;
                        string s = t == null ? "error - bad type" : t.ToString();
                        recipeStringChars += s.Length;
                        recipeStringUtf8Bytes += Encoding.UTF8.GetByteCount(s);

                        int freq;
                        if (recipeFrequency.TryGetValue(s, out freq))
                            recipeFrequency[s] = freq + 1;
                        else
                            recipeFrequency[s] = 1;
                    }
                }

                AddLargest(largest, new HolderStat(owner, rc, resc));
            }

            if (minRecipes == Int32.MaxValue)
                minRecipes = 0;

            int distinctRecipeStrings = recipeFrequency.Count;
            long theoreticalCatalogUtf8Bytes = 0;
            foreach (KeyValuePair<string, int> kvp in recipeFrequency)
                theoreticalCatalogUtf8Bytes += Encoding.UTF8.GetByteCount(kvp.Key);

            long repeatedUtf8Bytes = recipeStringUtf8Bytes - theoreticalCatalogUtf8Bytes;
            double avgRecipes = holderCount == 0 ? 0.0 : (double)recipeEntries / holderCount;
            double avgResources = holderCount == 0 ? 0.0 : (double)resourceEntries / holderCount;

            from.SendMessage(88, "Wolvesbane OWLTR Serialization Audit [READ ONLY]");
            from.SendMessage("Static holders: {0}; Temp/online holders: {1}", holders.Count, temp == null ? 0 : temp.Count);
            from.SendMessage("Serializable valid holders: {0}", holderCount);
            from.SendMessage("Player holders: {0}; Non-player holders: {1}", playerMobileCount, nonPlayerMobileCount);
            from.SendMessage("Deleted-mobile keys: {0}; Null keys: {1}; Null holders: {2}", deletedMobileCount, nullKeyCount, nullHolderCount);
            from.SendMessage("Recipe entries serialized: {0:N0}", recipeEntries);
            from.SendMessage("Resource entries serialized: {0:N0}", resourceEntries);
            from.SendMessage("Recipes/holder avg: {0:F1}; min: {1}; max: {2}; zero: {3}", avgRecipes, minRecipes, maxRecipes, zeroRecipeHolders);
            from.SendMessage("Resources/holder avg: {0:F1}", avgResources);
            from.SendMessage("Distinct recipe type strings: {0:N0}", distinctRecipeStrings);
            from.SendMessage("Recipe type-name text repeated: ~{0:N2} MiB UTF-8", recipeStringUtf8Bytes / 1048576.0);
            from.SendMessage("Same names once as a shared catalog: ~{0:N2} MiB UTF-8", theoreticalCatalogUtf8Bytes / 1048576.0);
            from.SendMessage("Approx repeated recipe-name payload: ~{0:N2} MiB", repeatedUtf8Bytes / 1048576.0);
            from.SendMessage(33, "Nothing was modified or deleted.");

            if (verbose)
            {
                from.SendMessage(88, "Largest OWLTR holders by recipe count:");
                for (int i = 0; i < largest.Count && i < 20; i++)
                {
                    HolderStat hs = largest[i];
                    string name = hs.Owner == null ? "(null)" : hs.Owner.Name;
                    if (String.IsNullOrEmpty(name))
                        name = "(unnamed)";
                    from.SendMessage("#{0}: {1}, Serial={2}, Recipes={3}, Resources={4}, Deleted={5}",
                        i + 1, name, hs.Owner == null ? Serial.MinusOne : hs.Owner.Serial, hs.Recipes, hs.Resources, hs.Owner != null && hs.Owner.Deleted);
                }

                List<KeyValuePair<string, int>> top = new List<KeyValuePair<string, int>>(recipeFrequency);
                top.Sort(delegate(KeyValuePair<string, int> a, KeyValuePair<string, int> b) { return b.Value.CompareTo(a.Value); });
                from.SendMessage(88, "Most repeated recipe type names:");
                for (int i = 0; i < top.Count && i < 15; i++)
                    from.SendMessage("#{0}: {1:N0} holders -> {2}", i + 1, top[i].Value, top[i].Key);
            }
        }

        private static void AddLargest(List<HolderStat> list, HolderStat stat)
        {
            list.Add(stat);
            list.Sort(delegate(HolderStat a, HolderStat b) { return b.Recipes.CompareTo(a.Recipes); });
            if (list.Count > 20)
                list.RemoveAt(list.Count - 1);
        }

        private class HolderStat
        {
            public Mobile Owner;
            public int Recipes;
            public int Resources;

            public HolderStat(Mobile owner, int recipes, int resources)
            {
                Owner = owner;
                Recipes = recipes;
                Resources = resources;
            }
        }
    }
}
