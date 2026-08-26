using System;
using System.Collections.Generic;
using System.Reflection;
using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Targeting;

namespace Wolvesbane.Commands
{
    public static class WBBODDiagnostics
    {
        private static readonly string[] ExactTimerNames =
        {
            "m_NextSmithBulkOrder",
            "m_NextTailorBulkOrder",
            "NextSmithBulkOrder",
            "NextTailorBulkOrder"
        };

        public static void Initialize()
        {
            CommandSystem.Register("BODStatus", AccessLevel.GameMaster, new CommandEventHandler(BODStatus_OnCommand));
            CommandSystem.Register("BODReset", AccessLevel.Administrator, new CommandEventHandler(BODReset_OnCommand));
        }

        [Usage("BODStatus")]
        [Description("Target a player and inspect BOD/Bulk Order cooldown fields and properties.")]
        private static void BODStatus_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage(0x35, "Target the player whose BOD timers you want to inspect.");
            e.Mobile.Target = new BODTarget(false);
        }

        [Usage("BODReset")]
        [Description("Target a player and reset only recognized Smith/Tailor BOD cooldown timers.")]
        private static void BODReset_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage(0x35, "Target the player whose Smith/Tailor BOD cooldowns you want to reset.");
            e.Mobile.Target = new BODTarget(true);
        }

        private sealed class BODTarget : Target
        {
            private readonly bool _Reset;

            public BODTarget(bool reset)
                : base(12, false, TargetFlags.None)
            {
                _Reset = reset;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                PlayerMobile pm = targeted as PlayerMobile;

                if (pm == null)
                {
                    from.SendMessage(33, "You must target a player character.");
                    return;
                }

                if (_Reset)
                    ResetTimers(from, pm);
                else
                    ShowStatus(from, pm);
            }
        }

        private static bool IsExactTimerName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            for (int i = 0; i < ExactTimerNames.Length; ++i)
            {
                if (String.Equals(name, ExactTimerNames[i], StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private static bool LooksLikeBODTimerName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            string n = name.ToLowerInvariant();

            return
                n.Contains("bulkorder") ||
                n.Contains("bulk_order") ||
                (n.Contains("bod") && (n.Contains("next") || n.Contains("time") || n.Contains("date")));
        }

        private static string Describe(DateTime value)
        {
            DateTime now = DateTime.UtcNow;
            TimeSpan remaining = value - now;

            if (value == DateTime.MinValue)
                return "DateTime.MinValue (eligible)";
            if (value == DateTime.MaxValue)
                return "DateTime.MaxValue (BLOCKED indefinitely)";

            if (remaining <= TimeSpan.Zero)
                return String.Format("{0:u} (eligible; expired {1} ago)", value, FormatSpan(-remaining));

            return String.Format("{0:u} ({1} remaining)", value, FormatSpan(remaining));
        }

        private static string FormatSpan(TimeSpan ts)
        {
            if (ts.TotalDays >= 1.0)
                return String.Format("{0}d {1:00}:{2:00}:{3:00}", (int)ts.TotalDays, ts.Hours, ts.Minutes, ts.Seconds);

            return String.Format("{0:00}:{1:00}:{2:00}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
        }

        private static void ShowStatus(Mobile staff, PlayerMobile pm)
        {
            staff.SendMessage(0x35, "BOD timer audit for {0} ({1})", pm.Name, pm.Serial);
            staff.SendMessage("Server UTC now: {0:u}", DateTime.UtcNow);

            Type t = pm.GetType();
            int found = 0;

            // Walk inheritance chain so private PlayerMobile/base fields are visible.
            for (Type cur = t; cur != null; cur = cur.BaseType)
            {
                FieldInfo[] fields = cur.GetFields(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.DeclaredOnly);

                for (int i = 0; i < fields.Length; ++i)
                {
                    FieldInfo f = fields[i];

                    if (f.FieldType != typeof(DateTime) || !LooksLikeBODTimerName(f.Name))
                        continue;

                    try
                    {
                        DateTime value = (DateTime)f.GetValue(pm);
                        staff.SendMessage("FIELD {0}.{1} = {2}", cur.Name, f.Name, Describe(value));
                        ++found;
                    }
                    catch (Exception ex)
                    {
                        staff.SendMessage(33, "FIELD {0}.{1}: {2}", cur.Name, f.Name, ex.Message);
                    }
                }
            }

            PropertyInfo[] props = t.GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic);

            for (int i = 0; i < props.Length; ++i)
            {
                PropertyInfo p = props[i];

                if (p.PropertyType != typeof(DateTime) ||
                    p.GetIndexParameters().Length != 0 ||
                    !LooksLikeBODTimerName(p.Name) ||
                    !p.CanRead)
                    continue;

                try
                {
                    DateTime value = (DateTime)p.GetValue(pm, null);
                    staff.SendMessage("PROP {0} = {1}", p.Name, Describe(value));
                    ++found;
                }
                catch (Exception ex)
                {
                    staff.SendMessage(33, "PROP {0}: {1}", p.Name, ex.Message);
                }
            }

            if (found == 0)
            {
                staff.SendMessage(33, "No DateTime BOD/Bulk Order timers were found by reflection.");
                staff.SendMessage(33, "That would mean OWLTR stores its cooldown somewhere other than PlayerMobile.");
            }
            else
            {
                staff.SendMessage(0x35, "Found {0} candidate BOD/Bulk Order timer member(s).", found);
            }
        }

        private static void ResetTimers(Mobile staff, PlayerMobile pm)
        {
            DateTime resetTo = DateTime.UtcNow - TimeSpan.FromMinutes(1.0);
            Type t = pm.GetType();
            int reset = 0;

            // Deliberately reset ONLY the exact known Smith/Tailor Bulk Order names.
            // We do not mutate arbitrary reflected DateTime fields.
            for (Type cur = t; cur != null; cur = cur.BaseType)
            {
                FieldInfo[] fields = cur.GetFields(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.DeclaredOnly);

                for (int i = 0; i < fields.Length; ++i)
                {
                    FieldInfo f = fields[i];

                    if (f.FieldType != typeof(DateTime) || !IsExactTimerName(f.Name))
                        continue;

                    try
                    {
                        DateTime oldValue = (DateTime)f.GetValue(pm);
                        f.SetValue(pm, resetTo);
                        staff.SendMessage(0x59, "Reset FIELD {0}.{1}: {2} -> {3:u}",
                            cur.Name, f.Name, Describe(oldValue), resetTo);
                        ++reset;
                    }
                    catch (Exception ex)
                    {
                        staff.SendMessage(33, "Could not reset FIELD {0}.{1}: {2}", cur.Name, f.Name, ex.Message);
                    }
                }
            }

            PropertyInfo[] props = t.GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.NonPublic);

            for (int i = 0; i < props.Length; ++i)
            {
                PropertyInfo p = props[i];

                if (p.PropertyType != typeof(DateTime) ||
                    p.GetIndexParameters().Length != 0 ||
                    !IsExactTimerName(p.Name) ||
                    !p.CanWrite)
                    continue;

                try
                {
                    DateTime oldValue = p.CanRead ? (DateTime)p.GetValue(pm, null) : DateTime.MinValue;
                    p.SetValue(pm, resetTo, null);
                    staff.SendMessage(0x59, "Reset PROP {0}: {1} -> {2:u}",
                        p.Name, Describe(oldValue), resetTo);
                    ++reset;
                }
                catch (Exception ex)
                {
                    staff.SendMessage(33, "Could not reset PROP {0}: {1}", p.Name, ex.Message);
                }
            }

            if (reset == 0)
            {
                staff.SendMessage(33, "No recognized Smith/Tailor BOD timer members were found.");
                staff.SendMessage(33, "Run [BODStatus and send me its output; OWLTR may own the cooldown instead.");
            }
            else
            {
                staff.SendMessage(0x35, "Reset {0} recognized BOD cooldown timer member(s) for {1}.", reset, pm.Name);
                staff.SendMessage(0x35, "Now run [OWLTRBOD and then test a Blacksmith/Tailor NPC.");
            }
        }
    }
}
