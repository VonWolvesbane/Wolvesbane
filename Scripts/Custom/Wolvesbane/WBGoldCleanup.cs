using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Items;

namespace Server.Commands
{
    public class WBGoldCleanup
    {
        private class PreviewInfo
        {
            public DateTime Time;
            public int Count;
            public long Amount;

            public PreviewInfo(DateTime time, int count, long amount)
            {
                Time = time;
                Count = count;
                Amount = amount;
            }
        }

        private static readonly Dictionary<Serial, PreviewInfo> m_Previews = new Dictionary<Serial, PreviewInfo>();
        private static readonly TimeSpan PreviewLifetime = TimeSpan.FromMinutes(10.0);

        public static void Initialize()
        {
            CommandSystem.Register("WBGoldCleanup", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("WBGoldCleanup preview|confirm")]
        [Description("Guarded cleanup for the historical OWLTR/MasterStorage orphan Gold leak.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (e.Arguments == null || e.Arguments.Length != 1)
            {
                from.SendMessage("Usage: [WBGoldCleanup preview");
                from.SendMessage("       [WBGoldCleanup confirm");
                return;
            }

            string action = e.Arguments[0].ToLower();

            if (action == "preview")
            {
                DoPreview(from);
                return;
            }

            if (action == "confirm")
            {
                DoConfirm(from);
                return;
            }

            from.SendMessage("Usage: [WBGoldCleanup preview|confirm");
        }

        private static void DoPreview(Mobile from)
        {
            int count;
            long amount;
            CountCandidates(out count, out amount);

            m_Previews[from.Serial] = new PreviewInfo(DateTime.UtcNow, count, amount);

            from.SendMessage(88, "Wolvesbane OWLTR Gold Cleanup PREVIEW [NO DELETION]");
            from.SendMessage("Strict orphan candidates: {0:N0}", count);
            from.SendMessage("Gold represented by candidates: {0:N0}", amount);
            from.SendMessage("Fingerprint: exact Server.Items.Gold; Parent=null;");
            from.SendMessage("Map.Internal; Location=(0,0,0); Stackable; Amount>0;");
            from.SendMessage("Hue=0; Name=null; LootType=Regular; Movable; Visible.");
            from.SendMessage(33, "Nothing was deleted.");
            from.SendMessage(68, "If these numbers match the Gold audit, use [WBGoldCleanup confirm within 10 minutes.");
        }

        private static void DoConfirm(Mobile from)
        {
            PreviewInfo preview;

            if (!m_Previews.TryGetValue(from.Serial, out preview))
            {
                from.SendMessage(33, "Cleanup refused: run [WBGoldCleanup preview first.");
                return;
            }

            if ((DateTime.UtcNow - preview.Time) > PreviewLifetime)
            {
                m_Previews.Remove(from.Serial);
                from.SendMessage(33, "Cleanup refused: your preview is older than 10 minutes. Preview again.");
                return;
            }

            int currentCount;
            long currentAmount;
            CountCandidates(out currentCount, out currentAmount);

            if (currentCount != preview.Count || currentAmount != preview.Amount)
            {
                m_Previews.Remove(from.Serial);
                from.SendMessage(33, "Cleanup refused: the candidate population changed since preview.");
                from.SendMessage("Preview: {0:N0} objects / {1:N0} gold", preview.Count, preview.Amount);
                from.SendMessage("Current: {0:N0} objects / {1:N0} gold", currentCount, currentAmount);
                from.SendMessage("Run [WBGoldCleanup preview again.");
                return;
            }

            List<Gold> snapshot = new List<Gold>();

            foreach (Item item in World.Items.Values)
            {
                Gold gold = item as Gold;

                if (IsCandidate(gold))
                    snapshot.Add(gold);
            }

            int deleted = 0;
            int skipped = 0;
            long deletedAmount = 0;

            for (int i = 0; i < snapshot.Count; i++)
            {
                Gold gold = snapshot[i];

                // Re-evaluate immediately before deletion.
                if (!IsCandidate(gold))
                {
                    skipped++;
                    continue;
                }

                int amount = gold.Amount;
                gold.Delete();

                if (gold.Deleted)
                {
                    deleted++;
                    deletedAmount += amount;
                }
                else
                {
                    skipped++;
                }
            }

            m_Previews.Remove(from.Serial);

            int remainingCount;
            long remainingAmount;
            CountCandidates(out remainingCount, out remainingAmount);

            from.SendMessage(88, "Wolvesbane OWLTR Gold Cleanup COMPLETE");
            from.SendMessage("Deleted: {0:N0} Gold objects representing {1:N0} gold", deleted, deletedAmount);
            from.SendMessage("Skipped/rejected: {0:N0}", skipped);
            from.SendMessage("Remaining strict candidates: {0:N0} representing {1:N0} gold", remainingCount, remainingAmount);
            from.SendMessage(68, "No automatic world save was performed. Run [WBGoldAudit, then manually save and measure.");
        }

        private static void CountCandidates(out int count, out long amount)
        {
            count = 0;
            amount = 0;

            foreach (Item item in World.Items.Values)
            {
                Gold gold = item as Gold;

                if (IsCandidate(gold))
                {
                    count++;
                    amount += gold.Amount;
                }
            }
        }

        private static bool IsCandidate(Gold gold)
        {
            if (gold == null || gold.Deleted)
                return false;

            // Exact runtime type only. Do not touch custom Gold subclasses.
            if (gold.GetType() != typeof(Gold))
                return false;

            if (gold.Parent != null)
                return false;

            if (gold.Map != Map.Internal)
                return false;

            if (gold.X != 0 || gold.Y != 0 || gold.Z != 0)
                return false;

            if (!gold.Stackable || gold.Amount <= 0)
                return false;

            // Historical MasterStorage leak samples all have stock Gold state.
            // These extra checks deliberately narrow the deletion population.
            if (gold.Hue != 0)
                return false;

            if (gold.Name != null)
                return false;

            if (gold.LootType != LootType.Regular)
                return false;

            if (!gold.Movable || !gold.Visible)
                return false;

            return true;
        }
    }
}
