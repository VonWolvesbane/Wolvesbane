using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Server;
using Server.Targeting;
using Server.Gumps;
using Server.ContextMenus;
using Solaris.ItemStore;							//for connection to item store data objects

namespace Server.Items
{
    //this is a special kind of storage key, that contains multiple store entries at once.  A Master Key is populated by adding other
    //keys that are derived from the BaseStoreKey class
    public class MasterItemStoreKey : Item, IItemStoreObject
    {
        //itemid for item storage keys
        const int ITEM_ID = 0x176B;     //0x176B - full keyring graphic

        //no more than this number can be stored in the master key
        public int MaxEntries { get { return 25; } }

        //the ItemStore list which is contained by the keys
        protected List<ItemStore> _Stores;

        //a list of the key types contained within this master keyring  This is used to synchronize the storage list
        //note: the entries in this needs to have a one-to-one correspondance with the ItemStore list.  This is kinda junky, as it defeats
        //the purpose of the separated key-store system.  But it is necessary if we want to maintain synchronization between scripts and
        //world saves for the stores added into the master ring
        protected List<Type> _KeyTypes;

        //TODO: define a static collection of stores that don't sync up with the user-defined keys - this could be another type of item, like
        //a resource chest

        //public accessor for stores
        public List<ItemStore> Stores
        {
            get
            {
                if (_Stores == null)
                {
                    _Stores = new List<ItemStore>();
                }
                return _Stores;
            }
        }

        public List<Type> KeyTypes
        {
            get
            {
                if (_KeyTypes == null)
                {
                    _KeyTypes = new List<Type>();
                }
                return _KeyTypes;
            }
        }

        //constructor
        [Constructable]
        public MasterItemStoreKey() : base(ITEM_ID)
        {
            Hue = 1153;
            Weight = 5;
            Name = "Master Keys";
        }

        //serial constructor
        public MasterItemStoreKey(Serial serial) : base(serial)
        {
        }

        public bool CanUse(Mobile from)
        {
            //definition for if a player can use these keys: needs to be in backpack.  Also, gamemasters+ have read-only access
            //Note: read-only access is maintained by the gump

            if (!IsChildOf(from.Backpack))
            {
                //TODO: look for cliloc equivalent?
                from.SendMessage(Name + " must be in your backpack to use.");
                return false;
            }

            return true;
        }

        //scans thru the contents for requested consumables and returns a list of all usable candidates.  The foundentries boolean
        //array holds recod of previously found resources from other IItemStoreObjects previously scanned, and thus are ignored
        public List<StoreEntry> FindConsumableEntries(Type[] types,int[] amounts,ref bool[] foundentries)
        {
            List<StoreEntry> stores = new List<StoreEntry>();

            for (int i = 0; i < types.Length; i++)
            {
                //ignore it if this has already been found in another storage
                if (foundentries[i])
                {
                    continue;
                }

                //find a match in this key's store
                foreach (ItemStore store in _Stores)
                {
                    int index = StoreEntry.IndexOfType(store.StoreEntries,types[i],true);

                    //check if there was a match, and there is a sufficient amount
                    if (index > -1 && store.StoreEntries[index].Amount >= amounts[i])
                    {
                        //add to the list to return
                        stores.Add(store.StoreEntries[index]);

                        //record the amount to consume, so if the operation is a success, the store entry will perform the consumption
                        store.StoreEntries[index].ToConsume = amounts[i];

                        //flag this entry as found
                        foundentries[i] = true;

                        //go on to the next type
                        break;
                    }
                }
            }

            return stores;
        }

        public StoreEntry FindConsumableEntry(Type[] types,int amount)
        {
            foreach (ItemStore store in _Stores)
            {
                //find a match in this key's store
                int index = StoreEntry.IndexOfType(store.StoreEntries,types,true);

                //check if there was a match, and there is a sufficient amount
                if (index > -1 && store.StoreEntries[index].Amount >= amount)
                {
                    //return a reference to this
                    return store.StoreEntries[index];
                }
            }

            //nothing suitable found, return null
            return null;
        }

        //look for the entry based on the entry type and the specified search parameters
        public StoreEntry FindEntryByEntryType(Type entrytype,int amount,object[] parameters)
        {
            foreach (ItemStore store in _Stores)
            {
                foreach (StoreEntry entry in store.StoreEntries)
                {

                    if (entry.GetType() == entrytype)
                    {
                        if (entry.Match(amount,parameters))
                        {
                            return entry;
                        }
                    }
                }
            }

            return null;
        }

        public void Add(Mobile from)
        {
            from.Target = new AddKeyTarget(this);
        }

        //these are accessed through the context menu use
        public void Fill(Mobile from)
        {
            FillEntriesFromBackpack(from);
        }

#if false
		//this triggers all fill from backpack methods in all entries contained within the master keys
		public void FillEntriesFromBackpack( Mobile from )
		{
			foreach( ItemStore store in _Stores )
			{
				//don't resend this gump if it's not up
				store.FillFromBackpack( from, false );
			}
		}
#endif

        /// Fix By datguy
        /*http://www.runuo.com/community/threads/runuo-2-0-svn-universal-storage-keys.87815/page-9#post-3814209*/
        //this triggers all fill from backpack methods in all entries contained within the master keys
        public void FillEntriesFromBackpack(Mobile from)
        {
            if (_Stores == null)
                return;

            foreach (ItemStore store in _Stores)
            {
                //don't resend this gump if it's not up
                store.FillFromBackpack(from,false);
            }
        }

        //context menu entries
        public override void GetContextMenuEntries(Mobile from,List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from,list);

            if (CanUse(from))
            {
                list.Add(new KeyOpenEntry(from,this,1));
                list.Add(new KeyAddEntry(from,this,2));
                list.Add(new KeyFillEntry(from,this,3));
            }
        }

        //events

        public override void OnDoubleClick(Mobile from)
        {
            if (!CanUse(from))
            {
                return;
            }

            //this gump shows a list of all the keyring types within the master ring
            from.SendGump(new MasterItemStoreKeyGump(from,this));
        }

        //add a specified key into the master key, if it can take it
        public bool AddKey(BaseStoreKey key)
        {
            //if this key type already exists within the master key
            if (KeyTypes.IndexOf(key.GetType()) > -1)
            {
                return false;
            }

            ItemStore newstore = ItemStore.Clone(key.Store);
            newstore.Owner = this;

            //save the loot type for this key into the store
            newstore.LootType = key.LootType;
            newstore.Insured = key.Insured;

            Stores.Add(newstore);
            KeyTypes.Add(key.GetType());

            //remove the keys from the world
            key.Delete();

            return true;
        }

        //this removes a key type from the master key and generates a new item
        public BaseStoreKey RemoveKey(int index)
        {
            if (index < 0 || index >= KeyTypes.Count)
            {
                return null;
            }

            //create new key object
            BaseStoreKey key = (BaseStoreKey)Activator.CreateInstance(KeyTypes[index]);

            key.SetStore(ItemStore.Clone(Stores[index]));

            //set up the key store to the right owner
            key.Store.Owner = key;

            key.LootType = Stores[index].LootType;
            key.Insured = Stores[index].Insured;

            //remove the entry from the KeyTypes and Stores lists
            Stores.RemoveAt(index);
            KeyTypes.RemoveAt(index);

            return key;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

            int storeCount = Stores.Count;
            int typeCount = KeyTypes.Count;

            if (storeCount != typeCount)
            {
                Console.WriteLine(
                    "WB MASTERKEY ERROR: Serial {0} has Stores={1} but KeyTypes={2}. Save may fail; no automatic repair was attempted.",
                    Serial, storeCount, typeCount);
            }

            writer.Write(storeCount);

            Stopwatch totalTimer = Stopwatch.StartNew();
            List<string> slowStores = new List<string>();

            for (int index = 0; index < storeCount; index++)
            {
                ItemStore store = Stores[index];

                if (store == null)
                {
                    Console.WriteLine(
                        "WB MASTERKEY ERROR: Serial {0} store index {1} is NULL. Save may fail; no automatic repair was attempted.",
                        Serial, index);

                    throw new InvalidOperationException(
                        String.Format("MasterItemStoreKey {0} has a null ItemStore at index {1}.", Serial, index));
                }

                Type keyType = index < typeCount ? KeyTypes[index] : null;

                if (keyType == null)
                {
                    Console.WriteLine(
                        "WB MASTERKEY ERROR: Serial {0} key type index {1} is NULL/missing. Save stopped before silently corrupting the master key.",
                        Serial, index);

                    throw new InvalidOperationException(
                        String.Format("MasterItemStoreKey {0} has a null/missing key type at index {1}.", Serial, index));
                }

                Stopwatch storeTimer = Stopwatch.StartNew();
                store.Serialize(writer);
                storeTimer.Stop();

                writer.Write(keyType.Name);

                if (storeTimer.Elapsed.TotalMilliseconds >= 1.0)
                {
                    slowStores.Add(String.Format(
                        "{0}={1:0.000}ms entries={2}",
                        keyType.Name,
                        storeTimer.Elapsed.TotalMilliseconds,
                        store.StoreEntries != null ? store.StoreEntries.Count : 0));
                }
            }

            totalTimer.Stop();

            // Keep console output useful: only report master keys that are materially slow.
            if (totalTimer.Elapsed.TotalMilliseconds >= 2.0)
            {
                string owner = GetDiagnosticOwner();
                Console.WriteLine(
                    "WB MASTERKEY PROFILE: Serial={0} Owner={1} Stores={2} Total={3:0.000}ms Parent={4} LootType={5} Insured={6}",
                    Serial, owner, storeCount, totalTimer.Elapsed.TotalMilliseconds,
                    Parent != null ? Parent.GetType().FullName : "(none)",
                    LootType, Insured);

                int shown = Math.Min(8, slowStores.Count);
                for (int i = 0; i < shown; i++)
                    Console.WriteLine("  WB MASTERKEY STORE: " + slowStores[i]);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                default:
                    {
                        int count = reader.ReadInt();

                        for (int i = 0; i < count; i++)
                        {
                            ItemStore store = new ItemStore(reader);
                            Stores.Add(store);

                            store.Owner = this;

                            string keyTypeName = reader.ReadString();
                            Type keytype = ScriptCompiler.FindTypeByName(keyTypeName);
                            KeyTypes.Add(keytype);

                            if (keytype == null)
                            {
                                Console.WriteLine(
                                    "WB MASTERKEY LOAD ERROR: Serial {0}, index {1}: key type '{2}' could not be resolved. The old code silently ignored this.",
                                    Serial, i, keyTypeName);
                                continue;
                            }

                            // Synchronize the stored entry definition with the current child-key script.
                            // Unlike the old code, failures are reported instead of silently swallowed.
                            BaseStoreKey synchkey = null;

                            try
                            {
                                synchkey = (BaseStoreKey)Activator.CreateInstance(keytype);

                                store.SynchronizeStore(synchkey.EntryStructure);
                                store.DisplayColumns = synchkey.DisplayColumns;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(
                                    "WB MASTERKEY LOAD ERROR: Serial {0}, index {1}, type {2}: {3}: {4}",
                                    Serial, i, keytype.FullName, ex.GetType().Name, ex.Message);
                            }
                            finally
                            {
                                if (synchkey != null && !synchkey.Deleted)
                                    synchkey.Delete();
                            }
                        }

                        ValidateDiagnosticIntegrity("Deserialize");
                        break;
                    }
            }
        }

        private string GetDiagnosticOwner()
        {
            object root = RootParent;

            Mobile mobile = root as Mobile;
            if (mobile != null)
                return String.Format("{0}({1})", mobile.Name, mobile.Serial);

            return root != null ? root.GetType().FullName : "(none)";
        }

        public int ValidateDiagnosticIntegrity(string source)
        {
            int problems = 0;

            int storeCount = _Stores != null ? _Stores.Count : 0;
            int typeCount = _KeyTypes != null ? _KeyTypes.Count : 0;

            if (storeCount != typeCount)
            {
                problems++;
                Console.WriteLine(
                    "WB MASTERKEY INTEGRITY: Serial {0} [{1}] Stores={2}, KeyTypes={3} (MISMATCH)",
                    Serial, source, storeCount, typeCount);
            }

            int max = Math.Max(storeCount, typeCount);

            for (int i = 0; i < max; i++)
            {
                ItemStore store = i < storeCount ? _Stores[i] : null;
                Type keyType = i < typeCount ? _KeyTypes[i] : null;

                if (store == null)
                {
                    problems++;
                    Console.WriteLine(
                        "WB MASTERKEY INTEGRITY: Serial {0} [{1}] store index {2} is NULL.",
                        Serial, source, i);
                }
                else if (store.Owner != this)
                {
                    problems++;
                    Console.WriteLine(
                        "WB MASTERKEY INTEGRITY: Serial {0} [{1}] store index {2} Owner is {3}, expected this master key.",
                        Serial, source, i,
                        store.Owner != null ? store.Owner.GetType().FullName : "NULL");
                }

                if (keyType == null)
                {
                    problems++;
                    Console.WriteLine(
                        "WB MASTERKEY INTEGRITY: Serial {0} [{1}] key type index {2} is NULL/missing.",
                        Serial, source, i);
                }
            }

            return problems;
        }

        //targeting to select a keyring to add to master keys
        class AddKeyTarget : Target
        {
            MasterItemStoreKey _MasterItemStoreKey;

            public AddKeyTarget(MasterItemStoreKey key) : base(1,false,TargetFlags.None)
            {
                _MasterItemStoreKey = key;
            }

            protected override void OnTarget(Mobile from,object targeted)
            {
                if (!(targeted is BaseStoreKey))
                {
                    from.SendMessage("You cannot add that.");
                    return;
                }

                BaseStoreKey key = (BaseStoreKey)targeted;

                if (!key.IsChildOf(from.Backpack))
                {
                    from.SendMessage("That must be in your backpack to add");
                    return;
                }

                //cannot add keys that cannot be use from backpack
                if (!key.CanUseFromPack)
                {
                    from.SendMessage("That type cannot be added to master keys.  Try locking it down in your house instead.");
                    return;
                }

                if (_MasterItemStoreKey.KeyTypes.Count >= _MasterItemStoreKey.MaxEntries)
                {
                    from.SendMessage("You cannot add any more to that");
                    return;
                }

                _MasterItemStoreKey.AddKey(key);

                from.CloseGump(typeof(ItemStoreGump));
                from.CloseGump(typeof(ListEntryGump));

                from.SendGump(new MasterItemStoreKeyGump(from,_MasterItemStoreKey));
            }
        }//addkeytarget class
    }//masterkey class
}