using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Items;

namespace Wolvesbane.WorldCleanup
{
    public class WBTokenCleanup
    {
        private class PreviewRecord
        {
            public int Count;
            public ulong Amount;
            public DateTime CreatedUtc;

            public PreviewRecord(int count, ulong amount)
            {
                Count = count;
                Amount = amount;
                CreatedUtc = DateTime.UtcNow;
            }
        }

        private static readonly Dictionary<Serial, PreviewRecord> m_Previews = new Dictionary<Serial, PreviewRecord>();
        private static readonly TimeSpan PreviewLifetime = TimeSpan.FromMinutes(10.0);

        public static void Initialize()
        {
            CommandSystem.Register("WBTokenCleanup", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("WBTokenCleanup preview | confirm")]
        [Description("Safely previews or removes strict orphan Daat99Tokens leaked by OWLTR MasterStorage.")]
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
            from.SendMessage(0x35, "Wolvesbane OWLTR Token Cleanup");
            from.SendMessage("Use [WBTokenCleanup preview first.");
            from.SendMessage("Then use [WBTokenCleanup confirm within 10 minutes if the preview is correct.");
            from.SendMessage(0x22, "Targets ONLY exact Daat99Tokens with no parent at (0,0,0) on Map.Internal.");
        }

        private static void DoPreview(Mobile from)
        {
            int count;
            ulong amount;
            int amountOne;
            int amountGreaterOne;

            CountCandidates(out count, out amount, out amountOne, out amountGreaterOne);
            m_Previews[from.Serial] = new PreviewRecord(count, amount);

            from.SendMessage(0x35, "Wolvesbane OWLTR Token Cleanup PREVIEW [NO DELETION]");
            from.SendMessage("Strict orphan candidates: {0:N0}", count);
            from.SendMessage("Token Amount represented by candidates: {0:N0}", amount);
            from.SendMessage("Amount=1 candidates: {0:N0}", amountOne);
            from.SendMessage("Amount>1 candidates: {0:N0}", amountGreaterOne);
            from.SendMessage("Fingerprint: exact Daat99Tokens; Parent=null; Map.Internal; Location=(0,0,0)");
            from.SendMessage(0x22, "Nothing was deleted.");

            if (count > 0)
                from.SendMessage(0x44, "If these numbers match the audit, use [WBTokenCleanup confirm within 10 minutes.");
        }

        private static void DoConfirm(Mobile from)
        {
            PreviewRecord preview;

            if (!m_Previews.TryGetValue(from.Serial, out preview))
            {
                from.SendMessage(0x22, "Cleanup refused: run [WBTokenCleanup preview first.");
                return;
            }

            if ((DateTime.UtcNow - preview.CreatedUtc) > PreviewLifetime)
            {
                m_Previews.Remove(from.Serial);
                from.SendMessage(0x22, "Cleanup refused: preview is older than 10 minutes. Run preview again.");
                return;
            }

            int currentCount;
            ulong currentAmount;
            int amountOne;
            int amountGreaterOne;

            CountCandidates(out currentCount, out currentAmount, out amountOne, out amountGreaterOne);

            if (currentCount != preview.Count || currentAmount != preview.Amount)
            {
                m_Previews.Remove(from.Serial);
                from.SendMessage(0x22, "Cleanup refused: world state changed since preview.");
                from.SendMessage("Preview: {0:N0} objects / {1:N0} tokens", preview.Count, preview.Amount);
                from.SendMessage("Current: {0:N0} objects / {1:N0} tokens", currentCount, currentAmount);
                from.SendMessage("Run [WBTokenCleanup preview again.");
                return;
            }

            if (currentCount <= 0)
            {
                m_Previews.Remove(from.Serial);
                from.SendMessage(0x44, "There are no strict orphan token candidates to delete.");
                return;
            }

            // Snapshot first so World.Items is never modified while it is being enumerated.
            List<Daat99Tokens> candidates = GetCandidates();

            if (candidates.Count != currentCount)
            {
                m_Previews.Remove(from.Serial);
                from.SendMessage(0x22, "Cleanup refused: world state changed while building the deletion list.");
                from.SendMessage("Run [WBTokenCleanup preview again.");
                return;
            }

            DateTime started = DateTime.UtcNow;
            int deleted = 0;
            int skipped = 0;
            ulong deletedAmount = 0;

            from.SendMessage(0x44, "Deleting {0:N0} confirmed orphan token objects. The shard may pause briefly...", candidates.Count);

            for (int i = 0; i < candidates.Count; i++)
            {
                Daat99Tokens token = candidates[i];

                // Re-check every object immediately before deletion.
                if (!IsStrictCandidate(token))
                {
                    skipped++;
                    continue;
                }

                int tokenAmount = token.Amount;
                token.Delete();

                if (token.Deleted)
                {
                    deleted++;
                    if (tokenAmount > 0)
                        deletedAmount += (ulong)tokenAmount;
                }
                else
                    skipped++;
            }

            m_Previews.Remove(from.Serial);

            int remaining;
            ulong remainingAmount;
            int remainingOne;
            int remainingGreaterOne;
            CountCandidates(out remaining, out remainingAmount, out remainingOne, out remainingGreaterOne);

            TimeSpan elapsed = DateTime.UtcNow - started;

            from.SendMessage(0x35, "Wolvesbane OWLTR Token Cleanup COMPLETE");
            from.SendMessage(0x44, "Deleted objects: {0:N0}", deleted);
            from.SendMessage("Deleted orphan token Amount: {0:N0}", deletedAmount);
            from.SendMessage("Skipped/rejected: {0:N0}", skipped);
            from.SendMessage("Remaining strict candidates: {0:N0}", remaining);
            from.SendMessage("Cleanup time: {0:F2} seconds", elapsed.TotalSeconds);
            from.SendMessage(0x22, "World was NOT automatically saved. Run [WBTokenAudit, then manually save and record the save time.");
        }

        private static List<Daat99Tokens> GetCandidates()
        {
            List<Daat99Tokens> list = new List<Daat99Tokens>();

            foreach (Item item in World.Items.Values)
            {
                Daat99Tokens token = item as Daat99Tokens;

                if (IsStrictCandidate(token))
                    list.Add(token);
            }

            return list;
        }

        private static void CountCandidates(out int count, out ulong totalAmount, out int amountOne, out int amountGreaterOne)
        {
            count = 0;
            totalAmount = 0;
            amountOne = 0;
            amountGreaterOne = 0;

            foreach (Item item in World.Items.Values)
            {
                Daat99Tokens token = item as Daat99Tokens;

                if (!IsStrictCandidate(token))
                    continue;

                count++;

                if (token.Amount > 0)
                    totalAmount += (ulong)token.Amount;

                if (token.Amount == 1)
                    amountOne++;
                else if (token.Amount > 1)
                    amountGreaterOne++;
            }
        }

        private static bool IsStrictCandidate(Daat99Tokens token)
        {
            return token != null &&
                   !token.Deleted &&
                   token.GetType() == typeof(Daat99Tokens) &&
                   token.Parent == null &&
                   token.Map == Map.Internal &&
                   token.X == 0 &&
                   token.Y == 0 &&
                   token.Z == 0 &&
                   token.Stackable &&
                   token.Amount > 0;
        }
    }
}
