using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Commands;
using Server.Items;
using Server.Mobiles;

namespace Server.Commands
{
    public class WBMountAudit
    {
        private const string HorseTypeName = "Server.Mobiles.Horse";
        private const string MountItemTypeName = "Server.Mobiles.MountItem";

        public static void Initialize()
        {
            CommandSystem.Register("WBMountAudit", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("WBMountAudit [verbose]")]
        [Description("Read-only Wolvesbane horse/mount-item relationship audit.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            bool verbose = e.Arguments != null && e.Arguments.Length > 0 &&
                e.Arguments[0].Equals("verbose", StringComparison.OrdinalIgnoreCase);

            List<Mobile> horses = new List<Mobile>();
            List<Item> mountItems = new List<Item>();

            foreach (Mobile mob in World.Mobiles.Values)
            {
                if (mob != null && mob.GetType().FullName == HorseTypeName)
                    horses.Add(mob);
            }

            foreach (Item item in World.Items.Values)
            {
                if (item != null && item.GetType().FullName == MountItemTypeName)
                    mountItems.Add(item);
            }

            HashSet<Mobile> horsesReferencedByMountItems = new HashSet<Mobile>();
            int mountItemsWithMountRef = 0;
            int mountItemsWithHorseRef = 0;
            int mountItemsWithNonHorseRef = 0;
            int mountItemsNoMountRef = 0;
            int mountItemsParented = 0;
            int mountItemsInternalParentless = 0;
            int mountItemsInternalZero = 0;

            List<string> mountSamples = new List<string>();

            for (int i = 0; i < mountItems.Count; i++)
            {
                Item item = mountItems[i];

                if (item.Parent != null)
                    mountItemsParented++;

                if (item.Parent == null && item.Map == Map.Internal)
                {
                    mountItemsInternalParentless++;

                    if (item.X == 0 && item.Y == 0 && item.Z == 0)
                        mountItemsInternalZero++;
                }

                object mountObj = GetMemberValue(item, "Mount");
                if (mountObj != null)
                {
                    mountItemsWithMountRef++;

                    Mobile mountMobile = mountObj as Mobile;
                    if (mountMobile != null)
                    {
                        if (mountMobile.GetType().FullName == HorseTypeName)
                        {
                            mountItemsWithHorseRef++;
                            horsesReferencedByMountItems.Add(mountMobile);
                        }
                        else
                        {
                            mountItemsWithNonHorseRef++;
                        }
                    }
                }
                else
                {
                    mountItemsNoMountRef++;
                }

                if (verbose && mountSamples.Count < 15 && item.Parent == null && item.Map == Map.Internal && item.X == 0 && item.Y == 0 && item.Z == 0)
                {
                    string mountDesc = DescribeObject(mountObj);
                    mountSamples.Add(String.Format("MountItem {0}: Mount={1}, Parent=null, Internal (0,0,0)", item.Serial, mountDesc));
                }
            }

            int internalHorses = 0;
            int internalZeroHorses = 0;
            int controlled = 0;
            int uncontrolled = 0;
            int hasControlMaster = 0;
            int noControlMaster = 0;
            int stabled = 0;
            int notStabled = 0;
            int stabledUnknown = 0;
            int summoned = 0;
            int notSummoned = 0;
            int hasRider = 0;
            int noRider = 0;
            int hasBackpackItems = 0;
            int noBackpackItems = 0;
            int referencedByMountItem = 0;
            int internalZeroReferenced = 0;
            int internalZeroUnreferenced = 0;

            int suspiciousStrong = 0;
            int suspiciousNeedsReview = 0;

            DateTime? oldest = null;
            DateTime? newest = null;

            List<string> horseSamples = new List<string>();

            for (int i = 0; i < horses.Count; i++)
            {
                Mobile horse = horses[i];

                bool isInternal = horse.Map == Map.Internal;
                bool isZero = isInternal && horse.X == 0 && horse.Y == 0 && horse.Z == 0;

                if (isInternal)
                    internalHorses++;
                if (isZero)
                    internalZeroHorses++;

                bool? isControlled = GetBool(horse, "Controlled");
                Mobile controlMaster = GetMemberValue(horse, "ControlMaster") as Mobile;

                if (isControlled == true)
                    controlled++;
                else
                    uncontrolled++;

                if (controlMaster != null)
                    hasControlMaster++;
                else
                    noControlMaster++;

                bool? isStabled = GetBool(horse, "IsStabled");
                if (isStabled == true)
                    stabled++;
                else if (isStabled == false)
                    notStabled++;
                else
                    stabledUnknown++;

                bool? isSummoned = GetBool(horse, "Summoned");
                if (isSummoned == true)
                    summoned++;
                else
                    notSummoned++;

                Mobile rider = GetMemberValue(horse, "Rider") as Mobile;
                if (rider != null)
                    hasRider++;
                else
                    noRider++;

                Container pack = horse.Backpack;
                int packCount = 0;
                if (pack != null && pack.Items != null)
                    packCount = pack.Items.Count;

                if (packCount > 0)
                    hasBackpackItems++;
                else
                    noBackpackItems++;

                bool referenced = horsesReferencedByMountItems.Contains(horse);
                if (referenced)
                    referencedByMountItem++;

                if (isZero)
                {
                    if (referenced)
                        internalZeroReferenced++;
                    else
                        internalZeroUnreferenced++;

                    // Strong candidate signature: internal zero, no rider, no control master,
                    // not controlled, not stabled, not summoned, empty/no backpack, and no MountItem reference.
                    if (rider == null &&
                        controlMaster == null &&
                        isControlled != true &&
                        isStabled != true &&
                        isSummoned != true &&
                        packCount == 0 &&
                        !referenced)
                    {
                        suspiciousStrong++;
                    }
                    else
                    {
                        suspiciousNeedsReview++;
                    }
                }

                DateTime? created = GetDateTime(horse, "Created");
                if (created == null)
                    created = GetDateTime(horse, "CreationTime");

                if (created != null)
                {
                    if (oldest == null || created.Value < oldest.Value)
                        oldest = created;
                    if (newest == null || created.Value > newest.Value)
                        newest = created;
                }

                if (verbose && horseSamples.Count < 20 && isZero)
                {
                    horseSamples.Add(String.Format(
                        "Horse {0}: Controlled={1}, Master={2}, Stabled={3}, Summoned={4}, Rider={5}, PackItems={6}, MountItemRef={7}",
                        horse.Serial,
                        BoolText(isControlled),
                        controlMaster != null ? controlMaster.Serial.ToString() : "null",
                        BoolText(isStabled),
                        BoolText(isSummoned),
                        rider != null ? rider.Serial.ToString() : "null",
                        packCount,
                        referenced ? "yes" : "no"));
                }
            }

            from.SendMessage(88, "Wolvesbane Mount Forensics Audit [READ ONLY]");
            from.SendMessage("Horses: {0:N0}; Internal: {1:N0}; Internal (0,0,0): {2:N0}", horses.Count, internalHorses, internalZeroHorses);
            from.SendMessage("Controlled: {0:N0}; Uncontrolled/unknown: {1:N0}; ControlMaster present: {2:N0}; absent: {3:N0}",
                controlled, uncontrolled, hasControlMaster, noControlMaster);
            from.SendMessage("Stabled true: {0:N0}; false: {1:N0}; unknown: {2:N0}", stabled, notStabled, stabledUnknown);
            from.SendMessage("Summoned: {0:N0}; not summoned/unknown: {1:N0}; Rider present: {2:N0}; absent: {3:N0}",
                summoned, notSummoned, hasRider, noRider);
            from.SendMessage("Horse backpacks with items: {0:N0}; empty/no backpack: {1:N0}", hasBackpackItems, noBackpackItems);
            from.SendMessage("Horses referenced by MountItem.Mount: {0:N0}", referencedByMountItem);
            from.SendMessage("Internal-zero horses referenced by MountItem: {0:N0}; unreferenced: {1:N0}",
                internalZeroReferenced, internalZeroUnreferenced);
            from.SendMessage(53, "Strong orphan-like horse signature: {0:N0}; Internal-zero needing review: {1:N0}",
                suspiciousStrong, suspiciousNeedsReview);

            if (oldest != null && newest != null)
                from.SendMessage("Horse creation range visible via reflection: {0:u} through {1:u}", oldest.Value, newest.Value);

            from.SendMessage(88, "MountItems: {0:N0}; parented: {1:N0}; Internal/no-parent: {2:N0}; Internal (0,0,0): {3:N0}",
                mountItems.Count, mountItemsParented, mountItemsInternalParentless, mountItemsInternalZero);
            from.SendMessage("MountItem Mount reference present: {0:N0}; absent: {1:N0}; Horse refs: {2:N0}; non-Horse refs: {3:N0}",
                mountItemsWithMountRef, mountItemsNoMountRef, mountItemsWithHorseRef, mountItemsWithNonHorseRef);

            if (verbose)
            {
                from.SendMessage(88, "Sample Internal (0,0,0) horses:");
                for (int i = 0; i < horseSamples.Count; i++)
                    from.SendMessage(horseSamples[i]);

                from.SendMessage(88, "Sample Internal (0,0,0) MountItems:");
                for (int i = 0; i < mountSamples.Count; i++)
                    from.SendMessage(mountSamples[i]);
            }
            else
            {
                from.SendMessage(88, "Run [WBMountAudit verbose] for sample object state.");
            }

            from.SendMessage(33, "Nothing was modified or deleted.");
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

        private static bool? GetBool(object obj, string name)
        {
            object v = GetMemberValue(obj, name);
            if (v is bool)
                return (bool)v;

            return null;
        }

        private static DateTime? GetDateTime(object obj, string name)
        {
            object v = GetMemberValue(obj, name);
            if (v is DateTime)
                return (DateTime)v;

            return null;
        }

        private static string BoolText(bool? value)
        {
            if (value == null)
                return "?";

            return value.Value ? "yes" : "no";
        }

        private static string DescribeObject(object obj)
        {
            if (obj == null)
                return "null";

            Mobile m = obj as Mobile;
            if (m != null)
                return m.GetType().FullName + " " + m.Serial;

            Item i = obj as Item;
            if (i != null)
                return i.GetType().FullName + " " + i.Serial;

            return obj.GetType().FullName;
        }
    }
}
