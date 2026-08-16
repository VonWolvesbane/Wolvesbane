using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Items;

namespace Server.Commands
{
    public class WBMasterKeyProtection
    {
        private class PreviewInfo
        {
            public DateTime Time;
            public List<Serial> Serials;

            public PreviewInfo(DateTime time, List<Serial> serials)
            {
                Time = time;
                Serials = serials;
            }
        }

        private static readonly Dictionary<Serial, PreviewInfo> m_Previews = new Dictionary<Serial, PreviewInfo>();
        private static readonly TimeSpan PreviewLifetime = TimeSpan.FromMinutes(10.0);

        public static void Initialize()
        {
            CommandSystem.Register("WBMasterKeyProtection", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("WBMasterKeyProtection preview|confirm")]
        [Description("Guarded conversion of existing MasterItemStoreKey objects to Blessed.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (e.Arguments == null || e.Arguments.Length != 1)
            {
                from.SendMessage("Usage: [WBMasterKeyProtection preview|confirm");
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

            from.SendMessage("Usage: [WBMasterKeyProtection preview|confirm");
        }

        private static void DoPreview(Mobile from)
        {
            List<MasterItemStoreKey> candidates = GetCandidates();
            List<Serial> serials = new List<Serial>();

            from.SendMessage(88, "Wolvesbane Master Key Protection PREVIEW [NO CHANGES]");
            from.SendMessage("Non-Blessed MasterItemStoreKeys: {0:N0}", candidates.Count);

            for (int i = 0; i < candidates.Count; i++)
            {
                MasterItemStoreKey key = candidates[i];
                serials.Add(key.Serial);

                Mobile owner = key.RootParent as Mobile;

                from.SendMessage(
                    "#{0}: Serial={1} Owner={2} Loot={3} Insured={4} Parent={5}",
                    i + 1,
                    key.Serial,
                    owner != null ? owner.Name : "(no mobile root)",
                    key.LootType,
                    key.Insured ? "yes" : "no",
                    key.Parent != null ? key.Parent.GetType().FullName : "(none)");
            }

            m_Previews[from.Serial] = new PreviewInfo(DateTime.UtcNow, serials);

            from.SendMessage(33, "Nothing was modified.");
            from.SendMessage(68, "If this list is correct, run [WBMasterKeyProtection confirm within 10 minutes.");
        }

        private static void DoConfirm(Mobile from)
        {
            PreviewInfo preview;

            if (!m_Previews.TryGetValue(from.Serial, out preview))
            {
                from.SendMessage(33, "Protection update refused: run preview first.");
                return;
            }

            if ((DateTime.UtcNow - preview.Time) > PreviewLifetime)
            {
                m_Previews.Remove(from.Serial);
                from.SendMessage(33, "Protection update refused: preview expired. Run preview again.");
                return;
            }

            List<MasterItemStoreKey> current = GetCandidates();

            if (current.Count != preview.Serials.Count)
            {
                m_Previews.Remove(from.Serial);
                from.SendMessage(33, "Protection update refused: candidate count changed. Preview again.");
                return;
            }

            HashSet<Serial> expected = new HashSet<Serial>(preview.Serials);

            for (int i = 0; i < current.Count; i++)
            {
                if (!expected.Contains(current[i].Serial))
                {
                    m_Previews.Remove(from.Serial);
                    from.SendMessage(33, "Protection update refused: candidate serials changed. Preview again.");
                    return;
                }
            }

            int changed = 0;
            int skipped = 0;

            for (int i = 0; i < current.Count; i++)
            {
                MasterItemStoreKey key = current[i];

                if (key == null || key.Deleted || key.GetType() != typeof(MasterItemStoreKey))
                {
                    skipped++;
                    continue;
                }

                if (key.LootType == LootType.Blessed)
                {
                    skipped++;
                    continue;
                }

                key.LootType = LootType.Blessed;
                changed++;
            }

            m_Previews.Remove(from.Serial);

            from.SendMessage(88, "Wolvesbane Master Key Protection COMPLETE");
            from.SendMessage("Changed to Blessed: {0:N0}", changed);
            from.SendMessage("Skipped: {0:N0}", skipped);
            from.SendMessage("Remaining non-Blessed Master Keys: {0:N0}", GetCandidates().Count);
            from.SendMessage(68, "No automatic world save was performed.");
        }

        private static List<MasterItemStoreKey> GetCandidates()
        {
            List<MasterItemStoreKey> list = new List<MasterItemStoreKey>();

            foreach (Item item in World.Items.Values)
            {
                MasterItemStoreKey key = item as MasterItemStoreKey;

                if (key == null || key.Deleted)
                    continue;

                if (key.GetType() != typeof(MasterItemStoreKey))
                    continue;

                if (key.LootType != LootType.Blessed)
                    list.Add(key);
            }

            list.Sort(delegate(MasterItemStoreKey a, MasterItemStoreKey b)
            {
                return a.Serial.Value.CompareTo(b.Serial.Value);
            });

            return list;
        }
    }
}
