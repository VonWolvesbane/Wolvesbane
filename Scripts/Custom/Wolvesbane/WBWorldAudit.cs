using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;

namespace Server.Commands
{
    public class WBWorldAudit
    {
        public static void Initialize()
        {
            CommandSystem.Register("WBWorldAudit", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("WBWorldAudit [items|mobiles|suspicious] [count]")]
        [Description("Read-only Wolvesbane world object population audit.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            string mode = e.Arguments != null && e.Arguments.Length > 0 ? e.Arguments[0].ToLower() : "summary";
            int limit = 25;

            if (e.Arguments != null && e.Arguments.Length > 1)
            {
                int parsed;
                if (Int32.TryParse(e.Arguments[1], out parsed))
                    limit = Math.Max(5, Math.Min(100, parsed));
            }

            Dictionary<Type, TypeStat> itemStats = new Dictionary<Type, TypeStat>();
            Dictionary<Type, TypeStat> mobileStats = new Dictionary<Type, TypeStat>();

            long totalItemAmount = 0;
            int parentlessItems = 0;
            int internalParentlessItems = 0;
            int internalZeroParentlessItems = 0;
            int worldPlacedItems = 0;
            int containedItems = 0;
            int deletedItems = 0;

            foreach (Item item in World.Items.Values)
            {
                if (item == null)
                    continue;

                Type t = item.GetType();
                TypeStat stat;
                if (!itemStats.TryGetValue(t, out stat))
                {
                    stat = new TypeStat(t);
                    itemStats.Add(t, stat);
                }

                stat.Count++;

                if (item.Deleted)
                {
                    deletedItems++;
                    stat.Deleted++;
                }

                if (item.Stackable && item.Amount > 0)
                {
                    stat.StackAmount += item.Amount;
                    totalItemAmount += item.Amount;
                }

                if (item.Parent == null)
                {
                    parentlessItems++;
                    stat.Parentless++;

                    if (item.Map == Map.Internal)
                    {
                        internalParentlessItems++;
                        stat.InternalParentless++;

                        if (item.X == 0 && item.Y == 0 && item.Z == 0)
                        {
                            internalZeroParentlessItems++;
                            stat.InternalZeroParentless++;
                        }
                    }
                    else
                    {
                        worldPlacedItems++;
                        stat.WorldPlaced++;
                    }
                }
                else
                {
                    containedItems++;
                    stat.Contained++;
                }
            }

            int deletedMobiles = 0;
            int internalMobiles = 0;
            int internalZeroMobiles = 0;
            int playerMobiles = 0;

            foreach (Mobile mob in World.Mobiles.Values)
            {
                if (mob == null)
                    continue;

                Type t = mob.GetType();
                TypeStat stat;
                if (!mobileStats.TryGetValue(t, out stat))
                {
                    stat = new TypeStat(t);
                    mobileStats.Add(t, stat);
                }

                stat.Count++;

                if (mob.Deleted)
                {
                    deletedMobiles++;
                    stat.Deleted++;
                }

                if (mob is PlayerMobile)
                {
                    playerMobiles++;
                    stat.Players++;
                }

                if (mob.Map == Map.Internal)
                {
                    internalMobiles++;
                    stat.Internal++;

                    if (mob.X == 0 && mob.Y == 0 && mob.Z == 0)
                    {
                        internalZeroMobiles++;
                        stat.InternalZero++;
                    }
                }
            }

            from.SendMessage(88, "Wolvesbane World Population Audit [READ ONLY]");
            from.SendMessage("World.Items: {0:N0}; distinct item types: {1:N0}", World.Items.Count, itemStats.Count);
            from.SendMessage("Contained: {0:N0}; world-placed: {1:N0}; parentless total: {2:N0}", containedItems, worldPlacedItems, parentlessItems);
            from.SendMessage("Parentless Map.Internal: {0:N0}; Internal at (0,0,0): {1:N0}", internalParentlessItems, internalZeroParentlessItems);
            from.SendMessage("Deleted item records still visible: {0:N0}; stack Amount sum: {1:N0}", deletedItems, totalItemAmount);
            from.SendMessage("World.Mobiles: {0:N0}; distinct mobile types: {1:N0}; players: {2:N0}", World.Mobiles.Count, mobileStats.Count, playerMobiles);
            from.SendMessage("Internal mobiles: {0:N0}; Internal at (0,0,0): {1:N0}; deleted: {2:N0}", internalMobiles, internalZeroMobiles, deletedMobiles);

            if (mode == "items")
            {
                ShowTop(from, itemStats, limit, false);
            }
            else if (mode == "mobiles")
            {
                ShowTop(from, mobileStats, limit, true);
            }
            else if (mode == "suspicious")
            {
                ShowSuspicious(from, itemStats, mobileStats, limit);
            }
            else
            {
                from.SendMessage(88, "Next commands:");
                from.SendMessage("[WBWorldAudit items 30");
                from.SendMessage("[WBWorldAudit mobiles 30");
                from.SendMessage("[WBWorldAudit suspicious 30");
            }

            from.SendMessage(33, "Nothing was modified or deleted.");
        }

        private static void ShowTop(Mobile from, Dictionary<Type, TypeStat> stats, int limit, bool mobiles)
        {
            List<TypeStat> list = new List<TypeStat>(stats.Values);
            list.Sort(delegate(TypeStat a, TypeStat b) { return b.Count.CompareTo(a.Count); });

            from.SendMessage(88, "Top {0} {1} types by live object count:", Math.Min(limit, list.Count), mobiles ? "mobile" : "item");

            for (int i = 0; i < list.Count && i < limit; i++)
            {
                TypeStat s = list[i];

                if (mobiles)
                {
                    from.SendMessage("#{0}: {1:N0} {2} | Internal={3:N0}, (0,0,0)={4:N0}, Players={5:N0}",
                        i + 1, s.Count, s.Type.FullName, s.Internal, s.InternalZero, s.Players);
                }
                else
                {
                    from.SendMessage("#{0}: {1:N0} {2} | Contained={3:N0}, World={4:N0}, Internal/no-parent={5:N0}, (0,0,0)={6:N0}, AmountSum={7:N0}",
                        i + 1, s.Count, s.Type.FullName, s.Contained, s.WorldPlaced, s.InternalParentless, s.InternalZeroParentless, s.StackAmount);
                }
            }
        }

        private static void ShowSuspicious(Mobile from, Dictionary<Type, TypeStat> itemStats, Dictionary<Type, TypeStat> mobileStats, int limit)
        {
            List<TypeStat> items = new List<TypeStat>();
            foreach (TypeStat s in itemStats.Values)
            {
                if (s.InternalZeroParentless > 0)
                    items.Add(s);
            }

            items.Sort(delegate(TypeStat a, TypeStat b)
            {
                int c = b.InternalZeroParentless.CompareTo(a.InternalZeroParentless);
                return c != 0 ? c : b.Count.CompareTo(a.Count);
            });

            from.SendMessage(88, "Top item types parentless on Map.Internal at (0,0,0):");
            for (int i = 0; i < items.Count && i < limit; i++)
            {
                TypeStat s = items[i];
                from.SendMessage("#{0}: {1:N0} / {2:N0} -> {3}", i + 1, s.InternalZeroParentless, s.Count, s.Type.FullName);
            }

            List<TypeStat> mobs = new List<TypeStat>();
            foreach (TypeStat s in mobileStats.Values)
            {
                if (s.InternalZero > 0)
                    mobs.Add(s);
            }

            mobs.Sort(delegate(TypeStat a, TypeStat b)
            {
                int c = b.InternalZero.CompareTo(a.InternalZero);
                return c != 0 ? c : b.Count.CompareTo(a.Count);
            });

            from.SendMessage(88, "Top mobile types on Map.Internal at (0,0,0):");
            for (int i = 0; i < mobs.Count && i < limit; i++)
            {
                TypeStat s = mobs[i];
                from.SendMessage("#{0}: {1:N0} / {2:N0} -> {3}", i + 1, s.InternalZero, s.Count, s.Type.FullName);
            }
        }

        private class TypeStat
        {
            public Type Type;
            public int Count;
            public int Deleted;
            public long StackAmount;

            public int Parentless;
            public int Contained;
            public int WorldPlaced;
            public int InternalParentless;
            public int InternalZeroParentless;

            public int Internal;
            public int InternalZero;
            public int Players;

            public TypeStat(Type type)
            {
                Type = type;
            }
        }
    }
}
