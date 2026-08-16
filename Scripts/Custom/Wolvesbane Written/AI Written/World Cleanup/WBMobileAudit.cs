using System;
using Server;
using Server.Commands;

namespace Wolvesbane.WorldCleanup
{
    public class WBMobileAudit
    {
        public static void Initialize()
        {
            CommandSystem.Register("WBMobileAudit", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
        }

        [Usage("WBMobileAudit [verbose]")]
        [Description("Read-only audit for plain Server.Mobile objects that may be orphan placeholders.")]
        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            bool verbose = e.Arguments.Length > 0 && e.Arguments[0].Equals("verbose", StringComparison.OrdinalIgnoreCase);

            int totalMobiles = World.Mobiles.Count;
            int exactBaseMobiles = 0;
            int internalBaseMobiles = 0;
            int strongCandidates = 0;
            int shown = 0;

            DateTime oldest = DateTime.MaxValue;
            DateTime newest = DateTime.MinValue;

            foreach (Mobile m in World.Mobiles.Values)
            {
                if (m == null || m.Deleted || m.GetType() != typeof(Mobile))
                    continue;

                exactBaseMobiles++;

                if (m.Map == Map.Internal)
                    internalBaseMobiles++;

                bool candidate =
                    m.Map == Map.Internal &&
                    m.Account == null &&
                    m.NetState == null &&
                    m.Items.Count == 0 &&
                    String.IsNullOrEmpty(m.Name);

                if (!candidate)
                    continue;

                strongCandidates++;

                if (m.CreationTime < oldest)
                    oldest = m.CreationTime;

                if (m.CreationTime > newest)
                    newest = m.CreationTime;

                if (verbose && shown < 20)
                {
                    from.SendMessage(0x59, "Candidate {0}: Serial={1}, Created={2:u}, Location={3}, Map={4}",
                        shown + 1, m.Serial, m.CreationTime, m.Location, m.Map);
                    shown++;
                }
            }

            from.SendMessage(0x35, "Wolvesbane Mobile Audit (READ ONLY)");
            from.SendMessage("World mobiles: {0:N0}", totalMobiles);
            from.SendMessage("Exact Server.Mobile objects: {0:N0}", exactBaseMobiles);
            from.SendMessage("Exact Server.Mobile objects on Internal: {0:N0}", internalBaseMobiles);
            from.SendMessage("Strong orphan-placeholder candidates: {0:N0}", strongCandidates);

            if (strongCandidates > 0)
            {
                from.SendMessage("Candidate creation range: {0:u} through {1:u}", oldest, newest);
                from.SendMessage(0x22, "Nothing was deleted. Use [WBMobileAudit verbose to show up to 20 sample serials.");
            }
            else
            {
                from.SendMessage(0x44, "No strong orphan-placeholder candidates matched the conservative filter.");
            }
        }
    }
}
