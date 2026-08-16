using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Items;
using Solaris.ItemStore;

namespace Server.Commands
{
    public class WBMasterKeyAudit
    {
        public static void Initialize()
        {
            CommandSystem.Register("WBMasterKeyAudit", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("WBMasterKeyAudit [verbose]")]
        [Description("Read-only integrity and location audit for MasterItemStoreKey objects.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            bool verbose = e.Arguments != null && e.Arguments.Length > 0 &&
                e.Arguments[0].Equals("verbose", StringComparison.OrdinalIgnoreCase);

            int total = 0;
            int inBackpack = 0;
            int inBank = 0;
            int otherContainer = 0;
            int worldPlaced = 0;
            int internalParentless = 0;
            int otherParentless = 0;

            int insured = 0;
            int blessed = 0;
            int regular = 0;

            int integrityProblems = 0;
            int mismatchedLists = 0;
            int nullStores = 0;
            int nullTypes = 0;
            int wrongOwners = 0;
            int duplicateTypes = 0;

            int totalStores = 0;
            long totalEntries = 0;
            long totalStoredAmount = 0;

            List<string> details = new List<string>();

            foreach (Item item in World.Items.Values)
            {
                MasterItemStoreKey key = item as MasterItemStoreKey;
                if (key == null || key.Deleted)
                    continue;

                total++;

                if (key.Insured)
                    insured++;

                if (key.LootType == LootType.Blessed || key.LootType == LootType.Newbied)
                    blessed++;
                else if (key.LootType == LootType.Regular)
                    regular++;

                Mobile rootMobile = key.RootParent as Mobile;

                if (rootMobile != null && rootMobile.Backpack != null && key.IsChildOf(rootMobile.Backpack))
                {
                    inBackpack++;
                }
                else if (rootMobile != null && rootMobile.BankBox != null && key.IsChildOf(rootMobile.BankBox))
                {
                    inBank++;
                }
                else if (key.Parent != null)
                {
                    otherContainer++;
                }
                else if (key.Map == Map.Internal)
                {
                    internalParentless++;
                }
                else if (key.Map != null)
                {
                    worldPlaced++;
                }
                else
                {
                    otherParentless++;
                }

                int stores = key.Stores.Count;
                int types = key.KeyTypes.Count;
                totalStores += stores;

                if (stores != types)
                {
                    mismatchedLists++;
                    integrityProblems++;
                }

                HashSet<Type> seen = new HashSet<Type>();
                int max = Math.Max(stores, types);

                for (int i = 0; i < max; i++)
                {
                    ItemStore store = i < stores ? key.Stores[i] : null;
                    Type type = i < types ? key.KeyTypes[i] : null;

                    if (store == null)
                    {
                        nullStores++;
                        integrityProblems++;
                    }
                    else
                    {
                        if (store.Owner != key)
                        {
                            wrongOwners++;
                            integrityProblems++;
                        }

                        if (store.StoreEntries != null)
                        {
                            totalEntries += store.StoreEntries.Count;

                            for (int j = 0; j < store.StoreEntries.Count; j++)
                            {
                                StoreEntry entry = store.StoreEntries[j];
                                if (entry != null)
                                    totalStoredAmount += entry.Amount;
                            }
                        }
                    }

                    if (type == null)
                    {
                        nullTypes++;
                        integrityProblems++;
                    }
                    else if (!seen.Add(type))
                    {
                        duplicateTypes++;
                        integrityProblems++;
                    }
                }

                if (verbose || stores != types)
                {
                    string owner = rootMobile != null
                        ? String.Format("{0}({1})", rootMobile.Name, rootMobile.Serial)
                        : "(no mobile root)";

                    details.Add(String.Format(
                        "Serial={0} Owner={1} Stores={2} Types={3} Parent={4} Map={5} Loc={6} Loot={7} Insured={8}",
                        key.Serial,
                        owner,
                        stores,
                        types,
                        key.Parent != null ? key.Parent.GetType().FullName : "(none)",
                        key.Map != null ? key.Map.Name : "(null)",
                        key.Location,
                        key.LootType,
                        key.Insured ? "yes" : "no"));
                }
            }

            from.SendMessage(88, "Wolvesbane Master Item Store Key Audit [READ ONLY]");
            from.SendMessage("MasterItemStoreKeys: {0:N0}; total child stores: {1:N0}", total, totalStores);
            from.SendMessage("Total StoreEntries: {0:N0}; summed entry Amount values: {1:N0}", totalEntries, totalStoredAmount);
            from.SendMessage("Location: backpack={0:N0}, bank={1:N0}, other container={2:N0}, world={3:N0}, Internal/no-parent={4:N0}, other parentless={5:N0}",
                inBackpack, inBank, otherContainer, worldPlaced, internalParentless, otherParentless);
            from.SendMessage("Protection: Insured={0:N0}; Blessed/Newbied={1:N0}; Regular loot={2:N0}", insured, blessed, regular);

            from.SendMessage(53, "Integrity problems: {0:N0}", integrityProblems);
            from.SendMessage("List mismatches={0:N0}; null stores={1:N0}; null key types={2:N0}; wrong store owners={3:N0}; duplicate key types={4:N0}",
                mismatchedLists, nullStores, nullTypes, wrongOwners, duplicateTypes);

            if (regular > 0)
                from.SendMessage(33, "NOTE: Regular-loot master keys are not intrinsically death-safe. This audit does not change LootType.");

            if (verbose)
            {
                from.SendMessage(88, "Master key details:");
                int shown = Math.Min(details.Count, 60);

                for (int i = 0; i < shown; i++)
                    from.SendMessage(details[i]);

                if (details.Count > shown)
                    from.SendMessage("... {0:N0} more not shown.", details.Count - shown);
            }
            else
            {
                from.SendMessage(88, "Run [WBMasterKeyAudit verbose] for serial/owner/location details.");
            }

            from.SendMessage(33, "Nothing was modified, repaired, moved, or deleted.");
        }
    }
}
