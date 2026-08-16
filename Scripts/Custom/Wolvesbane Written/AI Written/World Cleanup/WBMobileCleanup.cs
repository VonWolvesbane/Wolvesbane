using System;
using System.Collections.Generic;
using Server;
using Server.Commands;

namespace Wolvesbane.WorldCleanup
{
    public class WBMobileCleanup
    {
        private class PreviewRecord
        {
            public int Count;
            public DateTime CreatedUtc;

            public PreviewRecord(int count)
            {
                Count = count;
                CreatedUtc = DateTime.UtcNow;
            }
        }

        private static readonly Dictionary<Serial, PreviewRecord> m_Previews = new Dictionary<Serial, PreviewRecord>();
        private static readonly TimeSpan PreviewLifetime = TimeSpan.FromMinutes(10.0);

        public static void Initialize()
        {
            CommandSystem.Register("WBMobileCleanup", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("WBMobileCleanup preview | confirm")]
        [Description("Safely previews or removes confirmed orphan plain Server.Mobile placeholders.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (e.Arguments.Length != 1)
            {
                SendUsage(from);
                return;
            }

            string action = e.Arguments[0];

            if (action.Equals("preview", StringComparison.OrdinalIgnoreCase))
            {
                DoPreview(from);
                return;
            }

            if (action.Equals("confirm", StringComparison.OrdinalIgnoreCase))
            {
                DoConfirm(from);
                return;
            }

            SendUsage(from);
        }

        private static void SendUsage(Mobile from)
        {
            from.SendMessage(0x35, "Wolvesbane Mobile Cleanup");
            from.SendMessage("Use [WBMobileCleanup preview first.");
            from.SendMessage("Then use [WBMobileCleanup confirm within 10 minutes if the preview is correct.");
            from.SendMessage(0x22, "This command targets only the strict FS-ATS orphan-placeholder fingerprint.");
        }

        private static void DoPreview(Mobile from)
        {
            int count;
            DateTime oldest;
            DateTime newest;

            CountCandidates(out count, out oldest, out newest);
            m_Previews[from.Serial] = new PreviewRecord(count);

            from.SendMessage(0x35, "Wolvesbane Mobile Cleanup PREVIEW (NO DELETION)");
            from.SendMessage("Strict cleanup candidates: {0:N0}", count);

            if (count > 0)
            {
                from.SendMessage("Candidate creation range: {0:u} through {1:u}", oldest, newest);
                from.SendMessage("Required fingerprint:");
                from.SendMessage("  Exact type Server.Mobile; Map.Internal; Location (0,0,0)");
                from.SendMessage("  No account; no NetState; no items; empty name");
                from.SendMessage(0x22, "Nothing was deleted.");
                from.SendMessage(0x44, "If this is expected, use [WBMobileCleanup confirm within 10 minutes.");
            }
            else
            {
                from.SendMessage(0x44, "No strict orphan-placeholder candidates were found.");
            }
        }

        private static void DoConfirm(Mobile from)
        {
            PreviewRecord preview;

            if (!m_Previews.TryGetValue(from.Serial, out preview))
            {
                from.SendMessage(0x22, "Cleanup refused: you must run [WBMobileCleanup preview first.");
                return;
            }

            if ((DateTime.UtcNow - preview.CreatedUtc) > PreviewLifetime)
            {
                m_Previews.Remove(from.Serial);
                from.SendMessage(0x22, "Cleanup refused: your preview is older than 10 minutes. Run preview again.");
                return;
            }

            int currentCount;
            DateTime oldest;
            DateTime newest;

            CountCandidates(out currentCount, out oldest, out newest);

            if (currentCount != preview.Count)
            {
                m_Previews.Remove(from.Serial);
                from.SendMessage(0x22, "Cleanup refused: candidate count changed from {0:N0} to {1:N0}.", preview.Count, currentCount);
                from.SendMessage("Run [WBMobileCleanup preview again and review the new count.");
                return;
            }

            if (currentCount <= 0)
            {
                m_Previews.Remove(from.Serial);
                from.SendMessage(0x44, "There are no candidates to delete.");
                return;
            }

            List<Mobile> candidates = GetCandidates();

            if (candidates.Count != currentCount)
            {
                m_Previews.Remove(from.Serial);
                from.SendMessage(0x22, "Cleanup refused: world state changed while building the deletion list.");
                from.SendMessage("Run [WBMobileCleanup preview again.");
                return;
            }

            DateTime started = DateTime.UtcNow;
            int deleted = 0;
            int skipped = 0;

            for (int i = 0; i < candidates.Count; i++)
            {
                Mobile m = candidates[i];

                // Re-check every object immediately before deletion. Do not trust the earlier snapshot alone.
                if (!IsStrictCandidate(m))
                {
                    skipped++;
                    continue;
                }

                m.Delete();

                if (m.Deleted)
                    deleted++;
                else
                    skipped++;
            }

            m_Previews.Remove(from.Serial);

            int remaining;
            DateTime remainingOldest;
            DateTime remainingNewest;
            CountCandidates(out remaining, out remainingOldest, out remainingNewest);

            TimeSpan elapsed = DateTime.UtcNow - started;

            from.SendMessage(0x35, "Wolvesbane Mobile Cleanup COMPLETE");
            from.SendMessage(0x44, "Deleted: {0:N0}", deleted);
            from.SendMessage("Skipped/rejected: {0:N0}", skipped);
            from.SendMessage("Remaining strict candidates: {0:N0}", remaining);
            from.SendMessage("Cleanup time: {0:F2} seconds", elapsed.TotalSeconds);
            from.SendMessage(0x22, "The world was NOT automatically saved. Manually save now and record the save time.");
        }

        private static List<Mobile> GetCandidates()
        {
            List<Mobile> list = new List<Mobile>();

            foreach (Mobile m in World.Mobiles.Values)
            {
                if (IsStrictCandidate(m))
                    list.Add(m);
            }

            return list;
        }

        private static void CountCandidates(out int count, out DateTime oldest, out DateTime newest)
        {
            count = 0;
            oldest = DateTime.MaxValue;
            newest = DateTime.MinValue;

            foreach (Mobile m in World.Mobiles.Values)
            {
                if (!IsStrictCandidate(m))
                    continue;

                count++;

                if (m.CreationTime < oldest)
                    oldest = m.CreationTime;

                if (m.CreationTime > newest)
                    newest = m.CreationTime;
            }
        }

        private static bool IsStrictCandidate(Mobile m)
        {
            return m != null &&
                   !m.Deleted &&
                   m.GetType() == typeof(Mobile) &&
                   m.Map == Map.Internal &&
                   m.X == 0 &&
                   m.Y == 0 &&
                   m.Z == 0 &&
                   m.Account == null &&
                   m.NetState == null &&
                   m.Items.Count == 0 &&
                   String.IsNullOrEmpty(m.Name);
        }
    }
}
