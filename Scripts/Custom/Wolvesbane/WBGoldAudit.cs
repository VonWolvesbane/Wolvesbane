using System;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Commands;
using Server.Items;

namespace Server.Commands
{
    public class WBGoldAudit
    {
        public static void Initialize()
        {
            CommandSystem.Register("WBGoldAudit", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("WBGoldAudit [verbose]")]
        [Description("Read-only Wolvesbane Gold population and orphan-pattern audit.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            bool verbose = e.Arguments != null && e.Arguments.Length > 0 &&
                e.Arguments[0].Equals("verbose", StringComparison.OrdinalIgnoreCase);

            int total = 0;
            long totalAmount = 0;

            int contained = 0;
            int worldPlaced = 0;
            int parentless = 0;
            int internalParentless = 0;
            int internalZero = 0;

            long internalZeroAmount = 0;
            int internalZeroAmountOne = 0;
            int internalZeroAmountGtOne = 0;

            int band1 = 0;
            int band2to9 = 0;
            int band10to99 = 0;
            int band100to999 = 0;
            int band1000to9999 = 0;
            int band10000plus = 0;

            int minAmount = Int32.MaxValue;
            int maxAmount = Int32.MinValue;

            Dictionary<int, int> amountFrequency = new Dictionary<int, int>();
            Dictionary<int, int> hueFrequency = new Dictionary<int, int>();
            Dictionary<string, int> nameFrequency = new Dictionary<string, int>();
            Dictionary<string, int> lootFrequency = new Dictionary<string, int>();

            DateTime? oldest = null;
            DateTime? newest = null;
            int withCreatedDate = 0;

            List<string> samples = new List<string>();

            foreach (Item item in World.Items.Values)
            {
                Gold gold = item as Gold;
                if (gold == null)
                    continue;

                total++;
                int amount = gold.Amount;
                totalAmount += amount;

                if (gold.Parent != null)
                {
                    contained++;
                }
                else
                {
                    parentless++;

                    if (gold.Map == Map.Internal)
                    {
                        internalParentless++;

                        if (gold.X == 0 && gold.Y == 0 && gold.Z == 0)
                        {
                            internalZero++;
                            internalZeroAmount += amount;

                            if (amount == 1)
                                internalZeroAmountOne++;
                            else if (amount > 1)
                                internalZeroAmountGtOne++;

                            if (amount < minAmount)
                                minAmount = amount;
                            if (amount > maxAmount)
                                maxAmount = amount;

                            if (amount <= 1)
                                band1++;
                            else if (amount <= 9)
                                band2to9++;
                            else if (amount <= 99)
                                band10to99++;
                            else if (amount <= 999)
                                band100to999++;
                            else if (amount <= 9999)
                                band1000to9999++;
                            else
                                band10000plus++;

                            Increment(amountFrequency, amount);
                            Increment(hueFrequency, gold.Hue);
                            Increment(nameFrequency, gold.Name == null ? "(default/null)" : gold.Name);

                            string loot = gold.LootType.ToString();
                            Increment(lootFrequency, loot);

                            DateTime? created = GetDateTime(gold, "Created");
                            if (created == null)
                                created = GetDateTime(gold, "CreationTime");

                            if (created != null)
                            {
                                withCreatedDate++;

                                if (oldest == null || created.Value < oldest.Value)
                                    oldest = created;
                                if (newest == null || created.Value > newest.Value)
                                    newest = created;
                            }

                            if (verbose && samples.Count < 25)
                            {
                                samples.Add(String.Format(
                                    "Gold {0}: Amount={1:N0}, Hue={2}, Name={3}, Loot={4}, Movable={5}, Visible={6}, Created={7}",
                                    gold.Serial,
                                    amount,
                                    gold.Hue,
                                    gold.Name == null ? "(default)" : gold.Name,
                                    gold.LootType,
                                    gold.Movable ? "yes" : "no",
                                    gold.Visible ? "yes" : "no",
                                    created != null ? created.Value.ToString("u") : "?"));
                            }
                        }
                    }
                    else
                    {
                        worldPlaced++;
                    }
                }
            }

            from.SendMessage(88, "Wolvesbane Gold Forensics Audit [READ ONLY]");
            from.SendMessage("Gold objects: {0:N0}; total represented gold: {1:N0}", total, totalAmount);
            from.SendMessage("Contained: {0:N0}; world-placed: {1:N0}; parentless: {2:N0}", contained, worldPlaced, parentless);
            from.SendMessage("Parentless Map.Internal: {0:N0}; Internal at (0,0,0): {1:N0}", internalParentless, internalZero);
            from.SendMessage(53, "Internal (0,0,0) represented gold: {0:N0}", internalZeroAmount);
            from.SendMessage("Amount=1 objects: {0:N0}; Amount>1 objects: {1:N0}", internalZeroAmountOne, internalZeroAmountGtOne);

            if (internalZero > 0)
            {
                from.SendMessage("Internal-zero stack min: {0:N0}; max: {1:N0}; average: {2:N1}",
                    minAmount == Int32.MaxValue ? 0 : minAmount,
                    maxAmount == Int32.MinValue ? 0 : maxAmount,
                    (double)internalZeroAmount / (double)internalZero);
            }

            from.SendMessage(88, "Internal (0,0,0) amount bands:");
            from.SendMessage("1: {0:N0}; 2-9: {1:N0}; 10-99: {2:N0}", band1, band2to9, band10to99);
            from.SendMessage("100-999: {0:N0}; 1,000-9,999: {1:N0}; 10,000+: {2:N0}",
                band100to999, band1000to9999, band10000plus);

            if (withCreatedDate > 0 && oldest != null && newest != null)
            {
                from.SendMessage("Creation dates visible: {0:N0}/{1:N0}; range {2:u} through {3:u}",
                    withCreatedDate, internalZero, oldest.Value, newest.Value);
            }
            else
            {
                from.SendMessage("Creation dates were not exposed by this Gold/Item implementation.");
            }

            ShowTopInt(from, "Most common Internal-zero stack amounts:", amountFrequency, 15);
            ShowTopInt(from, "Most common hues:", hueFrequency, 8);
            ShowTopString(from, "Names:", nameFrequency, 8);
            ShowTopString(from, "Loot types:", lootFrequency, 8);

            if (verbose)
            {
                from.SendMessage(88, "Sample Internal (0,0,0) Gold:");
                for (int i = 0; i < samples.Count; i++)
                    from.SendMessage(samples[i]);
            }
            else
            {
                from.SendMessage(88, "Run [WBGoldAudit verbose] for sample serials/state.");
            }

            from.SendMessage(33, "Nothing was modified or deleted.");
        }

        private static void Increment(Dictionary<int, int> dict, int key)
        {
            int value;
            if (!dict.TryGetValue(key, out value))
                value = 0;

            dict[key] = value + 1;
        }

        private static void Increment(Dictionary<string, int> dict, string key)
        {
            int value;
            if (!dict.TryGetValue(key, out value))
                value = 0;

            dict[key] = value + 1;
        }

        private static void ShowTopInt(Mobile from, string title, Dictionary<int, int> dict, int limit)
        {
            List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>(dict);
            list.Sort(delegate(KeyValuePair<int, int> a, KeyValuePair<int, int> b)
            {
                int c = b.Value.CompareTo(a.Value);
                if (c != 0)
                    return c;

                return a.Key.CompareTo(b.Key);
            });

            from.SendMessage(88, title);

            for (int i = 0; i < list.Count && i < limit; i++)
                from.SendMessage("#{0}: Amount {1:N0} -> {2:N0} objects", i + 1, list[i].Key, list[i].Value);
        }

        private static void ShowTopString(Mobile from, string title, Dictionary<string, int> dict, int limit)
        {
            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>(dict);
            list.Sort(delegate(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
            {
                return b.Value.CompareTo(a.Value);
            });

            from.SendMessage(88, title);

            for (int i = 0; i < list.Count && i < limit; i++)
                from.SendMessage("#{0}: {1} -> {2:N0}", i + 1, list[i].Key, list[i].Value);
        }

        private static object GetMemberValue(object obj, string name)
        {
            if (obj == null)
                return null;

            Type t = obj.GetType();

            while (t != null)
            {
                try
                {
                    PropertyInfo p = t.GetProperty(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

                    if (p != null && p.GetIndexParameters().Length == 0)
                        return p.GetValue(obj, null);
                }
                catch
                {
                }

                try
                {
                    FieldInfo f = t.GetField(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

                    if (f != null)
                        return f.GetValue(obj);

                    f = t.GetField("m_" + name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);

                    if (f != null)
                        return f.GetValue(obj);
                }
                catch
                {
                }

                t = t.BaseType;
            }

            return null;
        }

        private static DateTime? GetDateTime(object obj, string name)
        {
            object value = GetMemberValue(obj, name);
            if (value is DateTime)
                return (DateTime)value;

            return null;
        }
    }
}
