using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Commands;
using Server.Items;

namespace Wolvesbane.WorldCleanup
{
    public class WBTokenAudit
    {
        private const string TokenTypeName = "Daat99Tokens";

        public static void Initialize()
        {
            CommandSystem.Register("WBTokenAudit", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("WBTokenAudit [verbose]")]
        [Description("Read-only audit of Daat99 token world objects and the OWLTR control object.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            bool verbose = e.Arguments.Length > 0 && e.Arguments[0].Equals("verbose", StringComparison.OrdinalIgnoreCase);

            int instances = 0;
            long totalAmount = 0;
            int amountOne = 0;
            int amountGreaterOne = 0;
            int nonStackable = 0;
            int deleted = 0;
            int inBank = 0;
            int inBackpack = 0;
            int inOtherContainer = 0;
            int onMobileOther = 0;
            int worldPlaced = 0;
            int internalPlaced = 0;
            int noParent = 0;

            Dictionary<Serial, int> parentCounts = new Dictionary<Serial, int>();
            Dictionary<Serial, Item> parentItems = new Dictionary<Serial, Item>();
            Dictionary<Serial, Mobile> parentMobiles = new Dictionary<Serial, Mobile>();

            Item owltr = null;

            foreach (Item item in World.Items.Values)
            {
                if (item == null)
                    continue;

                string typeName = item.GetType().Name;

                if (typeName == "Daat99OWLTR")
                    owltr = item;

                if (typeName != TokenTypeName)
                    continue;

                instances++;

                if (item.Deleted)
                {
                    deleted++;
                    continue;
                }

                int amount = item.Amount;
                if (amount < 0)
                    amount = 0;

                totalAmount += amount;

                if (item.Amount == 1)
                    amountOne++;
                else if (item.Amount > 1)
                    amountGreaterOne++;

                if (!item.Stackable)
                    nonStackable++;

                object parent = item.Parent;

                if (parent == null)
                {
                    noParent++;

                    if (item.Map == Map.Internal)
                        internalPlaced++;
                    else
                        worldPlaced++;
                }
                else if (parent is Item)
                {
                    Item parentItem = (Item)parent;
                    AddParent(parentCounts, parentItem.Serial);

                    if (!parentItems.ContainsKey(parentItem.Serial))
                        parentItems.Add(parentItem.Serial, parentItem);

                    if (IsInsideBank(item))
                        inBank++;
                    else if (IsInsideBackpack(item))
                        inBackpack++;
                    else
                        inOtherContainer++;
                }
                else if (parent is Mobile)
                {
                    Mobile parentMobile = (Mobile)parent;
                    AddParent(parentCounts, parentMobile.Serial);

                    if (!parentMobiles.ContainsKey(parentMobile.Serial))
                        parentMobiles.Add(parentMobile.Serial, parentMobile);

                    onMobileOther++;
                }
            }

            int multiTokenParents = 0;
            long tokenObjectsInMultiParents = 0;
            int largestParentCount = 0;
            Serial largestParentSerial = Serial.MinusOne;

            foreach (KeyValuePair<Serial, int> kvp in parentCounts)
            {
                if (kvp.Value > 1)
                {
                    multiTokenParents++;
                    tokenObjectsInMultiParents += kvp.Value;
                }

                if (kvp.Value > largestParentCount)
                {
                    largestParentCount = kvp.Value;
                    largestParentSerial = kvp.Key;
                }
            }

            from.SendMessage(0x35, "Wolvesbane Token Audit [READ ONLY]");
            from.SendMessage("Daat99Tokens item objects: {0:N0}", instances);
            from.SendMessage("Total token Amount represented: {0:N0}", totalAmount);
            from.SendMessage("Amount=1 objects: {0:N0}; Amount>1 objects: {1:N0}", amountOne, amountGreaterOne);
            from.SendMessage("Non-stackable token objects: {0:N0}", nonStackable);
            from.SendMessage("Bank: {0:N0}; Backpack: {1:N0}; Other containers: {2:N0}", inBank, inBackpack, inOtherContainer);
            from.SendMessage("Directly on mobiles: {0:N0}; World placed: {1:N0}; Internal/no parent: {2:N0}", onMobileOther, worldPlaced, internalPlaced);
            from.SendMessage("Immediate parents containing >1 token object: {0:N0}", multiTokenParents);
            from.SendMessage("Token objects in those multi-token parents: {0:N0}", tokenObjectsInMultiParents);

            if (largestParentCount > 0)
                from.SendMessage("Largest immediate parent: {0} token objects (Serial {1})", largestParentCount, largestParentSerial);

            if (deleted > 0)
                from.SendMessage(0x22, "Deleted token objects unexpectedly present in World.Items: {0:N0}", deleted);

            if (owltr != null)
            {
                from.SendMessage(0x44, "Daat99OWLTR found: Serial {0}, Map={1}, Location={2}", owltr.Serial, owltr.Map, owltr.Location);
                ReportLargeCollections(from, owltr, verbose);
            }
            else
            {
                from.SendMessage(0x22, "Daat99OWLTR control object was not found in World.Items.");
            }

            if (verbose)
                ReportLargestParents(from, parentCounts, parentItems, parentMobiles, 15);

            from.SendMessage(0x22, "Nothing was modified or deleted.");
        }

        private static void AddParent(Dictionary<Serial, int> counts, Serial serial)
        {
            int count;

            if (counts.TryGetValue(serial, out count))
                counts[serial] = count + 1;
            else
                counts.Add(serial, 1);
        }

        private static bool IsInsideBank(Item item)
        {
            object p = item.Parent;
            int safety = 0;

            while (p != null && safety++ < 100)
            {
                if (p is BankBox)
                    return true;

                Item pi = p as Item;
                if (pi != null)
                {
                    p = pi.Parent;
                    continue;
                }

                break;
            }

            return false;
        }

        private static bool IsInsideBackpack(Item item)
        {
            object p = item.Parent;
            int safety = 0;

            while (p != null && safety++ < 100)
            {
                if (p is Backpack)
                    return true;

                Item pi = p as Item;
                if (pi != null)
                {
                    p = pi.Parent;
                    continue;
                }

                break;
            }

            return false;
        }

        private static void ReportLargestParents(Mobile from, Dictionary<Serial, int> counts, Dictionary<Serial, Item> items, Dictionary<Serial, Mobile> mobiles, int max)
        {
            List<KeyValuePair<Serial, int>> list = new List<KeyValuePair<Serial, int>>(counts);
            list.Sort(delegate(KeyValuePair<Serial, int> a, KeyValuePair<Serial, int> b)
            {
                return b.Value.CompareTo(a.Value);
            });

            from.SendMessage(0x35, "Largest immediate token parents:");

            int limit = Math.Min(max, list.Count);
            for (int i = 0; i < limit; i++)
            {
                KeyValuePair<Serial, int> kvp = list[i];
                Item pi;
                Mobile pm;

                if (items.TryGetValue(kvp.Key, out pi))
                {
                    from.SendMessage("#{0}: {1:N0} objects in {2} Serial={3}", i + 1, kvp.Value, pi.GetType().Name, pi.Serial);
                }
                else if (mobiles.TryGetValue(kvp.Key, out pm))
                {
                    from.SendMessage("#{0}: {1:N0} objects on {2} Serial={3} Name={4}", i + 1, kvp.Value, pm.GetType().Name, pm.Serial, pm.Name == null ? "(null)" : pm.Name);
                }
                else
                {
                    from.SendMessage("#{0}: {1:N0} objects, parent Serial={2}", i + 1, kvp.Value, kvp.Key);
                }
            }
        }

        private static void ReportLargeCollections(Mobile from, object target, bool verbose)
        {
            Type type = target.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            List<string> reports = new List<string>();

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                object value = null;

                try
                {
                    value = field.GetValue(target);
                }
                catch
                {
                    continue;
                }

                if (value == null || value is string)
                    continue;

                int count = GetCollectionCount(value);
                if (count < 0)
                    continue;

                if (count >= 100 || verbose)
                    reports.Add(field.Name + " (" + field.FieldType.Name + ") = " + count.ToString("N0") + " entries");
            }

            reports.Sort();

            if (reports.Count == 0)
            {
                from.SendMessage("OWLTR: no instance collection fields with 100+ entries were visible via reflection.");
                return;
            }

            from.SendMessage(0x35, "OWLTR collection fields{0}:", verbose ? "" : " (100+ entries)");

            int max = verbose ? 30 : 20;
            for (int i = 0; i < reports.Count && i < max; i++)
                from.SendMessage(reports[i]);

            if (reports.Count > max)
                from.SendMessage("...and {0:N0} additional collection fields.", reports.Count - max);
        }

        private static int GetCollectionCount(object value)
        {
            ICollection collection = value as ICollection;
            if (collection != null)
                return collection.Count;

            PropertyInfo countProperty = value.GetType().GetProperty("Count", BindingFlags.Instance | BindingFlags.Public);
            if (countProperty != null && countProperty.PropertyType == typeof(int) && countProperty.GetIndexParameters().Length == 0)
            {
                try
                {
                    return (int)countProperty.GetValue(value, null);
                }
                catch
                {
                }
            }

            return -1;
        }
    }
}
