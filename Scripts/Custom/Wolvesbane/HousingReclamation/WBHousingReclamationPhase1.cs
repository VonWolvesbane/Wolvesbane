using System;
using System.Collections.Generic;
using Server;
using Server.Accounting;
using Server.Commands;
using Server.Gumps;
using Server.Mobiles;
using Server.Multis;
using Server.Network;

namespace Wolvesbane.HousingReclamation
{
    public enum WBHouseReviewState
    {
        None,
        Deferred,
        Exempt
    }

    public class WBHouseReviewEntry
    {
        public Serial HouseSerial;
        public WBHouseReviewState State;
        public DateTime DeferUntilUtc;
        public string StaffName;
        public DateTime ChangedUtc;

        public WBHouseReviewEntry()
        {
        }

        public WBHouseReviewEntry(GenericReader reader)
        {
            int version = reader.ReadInt();
            HouseSerial = (Serial)reader.ReadInt();
            State = (WBHouseReviewState)reader.ReadInt();
            DeferUntilUtc = reader.ReadDateTime();
            StaffName = reader.ReadString();
            ChangedUtc = reader.ReadDateTime();
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(0);
            writer.Write((int)HouseSerial);
            writer.Write((int)State);
            writer.Write(DeferUntilUtc);
            writer.Write(StaffName);
            writer.Write(ChangedUtc);
        }
    }

    public class WBHouseCandidate
    {
        public BaseHouse House;
        public Mobile Owner;
        public Account Account;
        public DateTime LastLoginUtc;
        public double InactiveDays;

        public string AccountName
        {
            get
            {
                return Account != null ? Account.Username : "(no account)";
            }
        }
    }

    // Hidden world item used only to persist staff review state.
    public class WBHousingReclamationController : Item
    {
        private static WBHousingReclamationController m_Instance;
        private Dictionary<Serial, WBHouseReviewEntry> m_Review =
            new Dictionary<Serial, WBHouseReviewEntry>();

        public static WBHousingReclamationController Instance
        {
            get { return m_Instance; }
        }

        public Dictionary<Serial, WBHouseReviewEntry> Review
        {
            get { return m_Review; }
        }

        [Constructable]
        public WBHousingReclamationController() : base(0x1)
        {
            Name = "Wolvesbane Housing Reclamation Controller";
            Movable = false;
            Visible = false;
            MoveToWorld(Point3D.Zero, Map.Internal);
            m_Instance = this;
        }

        public WBHousingReclamationController(Serial serial) : base(serial)
        {
            m_Instance = this;
        }

        public override void OnDelete()
        {
            base.OnDelete();

            if (m_Instance == this)
                m_Instance = null;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);

            writer.Write(m_Review.Count);

            foreach (KeyValuePair<Serial, WBHouseReviewEntry> kvp in m_Review)
            {
                writer.Write((int)kvp.Key);
                kvp.Value.Serialize(writer);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            int count = reader.ReadInt();

            m_Review = new Dictionary<Serial, WBHouseReviewEntry>();

            for (int i = 0; i < count; ++i)
            {
                Serial key = (Serial)reader.ReadInt();
                WBHouseReviewEntry entry = new WBHouseReviewEntry(reader);

                if (!m_Review.ContainsKey(key))
                    m_Review.Add(key, entry);
            }

            Visible = false;
            Movable = false;
            m_Instance = this;
        }
    }

    public static class WBHousingReclamationSystem
    {
        public const int InactiveDaysRequired = 30;
        public const int DefaultDeferDays = 7;
        private const AccessLevel StaffAccess = AccessLevel.GameMaster;

        public static void Initialize()
        {
            EnsureController();

            CommandSystem.Register(
                "WBHousingReview",
                StaffAccess,
                new CommandEventHandler(OnReviewCommand));

            CommandSystem.Register(
                "WBHousingAudit",
                StaffAccess,
                new CommandEventHandler(OnAuditCommand));

            EventSink.Login += new LoginEventHandler(OnLogin);
        }

        private static void EnsureController()
        {
            if (WBHousingReclamationController.Instance != null)
                return;

            Timer.DelayCall(
                TimeSpan.FromSeconds(2.0),
                delegate
                {
                    if (WBHousingReclamationController.Instance == null)
                        new WBHousingReclamationController();
                });
        }

        private static void OnLogin(LoginEventArgs e)
        {
            Mobile m = e.Mobile;

            if (m == null || m.AccessLevel < StaffAccess)
                return;

            Timer.DelayCall(
                TimeSpan.FromSeconds(3.0),
                delegate
                {
                    if (m == null || m.Deleted || m.NetState == null)
                        return;

                    List<WBHouseCandidate> candidates = GetCandidates();

                    if (candidates.Count > 0)
                    {
                        m.SendMessage(
                            68,
                            "Housing Reclamation: {0} inactive-account house{1} await staff review. Use [WBHousingReview.",
                            candidates.Count,
                            candidates.Count == 1 ? "" : "s");
                    }
                });
        }

        private static void OnReviewCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            from.CloseGump(typeof(WBHousingReviewGump));
            from.SendGump(new WBHousingReviewGump(from, GetCandidates(), 0));
        }

        private static void OnAuditCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            List<WBHouseCandidate> candidates = GetCandidates();

            int deferred = 0;
            int exempt = 0;
            WBHousingReclamationController controller = WBHousingReclamationController.Instance;

            if (controller != null)
            {
                foreach (WBHouseReviewEntry entry in controller.Review.Values)
                {
                    if (entry.State == WBHouseReviewState.Deferred &&
                        entry.DeferUntilUtc > DateTime.UtcNow)
                        deferred++;
                    else if (entry.State == WBHouseReviewState.Exempt)
                        exempt++;
                }
            }

            from.SendMessage(88, "Wolvesbane Housing Reclamation Audit");
            from.SendMessage("Eligibility threshold: more than {0} days since ACCOUNT last login.", InactiveDaysRequired);
            from.SendMessage("Current review candidates: {0}", candidates.Count);
            from.SendMessage("Active deferred records: {0}", deferred);
            from.SendMessage("Exempt records: {0}", exempt);
            from.SendMessage(68, "GameMaster+: review/defer/exempt. Administrator: approve/reclaim and batch processing.");
        }

        public static List<WBHouseCandidate> GetCandidates()
        {
            EnsureController();

            List<WBHouseCandidate> list = new List<WBHouseCandidate>();
            List<BaseHouse> houses = BaseHouse.AllHouses;
            DateTime now = DateTime.UtcNow;

            for (int i = 0; i < houses.Count; ++i)
            {
                BaseHouse house = houses[i];

                if (house == null || house.Deleted)
                    continue;

                Mobile owner = house.Owner;

                if (owner == null || owner.Deleted)
                    continue;

                Account account = owner.Account as Account;

                if (account == null)
                    continue;

                // Staff-owned accounts/houses are never candidates in Phase 1.
                if (account.AccessLevel >= StaffAccess || owner.AccessLevel >= StaffAccess)
                    continue;

                bool staffCharacter = false;

                for (int c = 0; c < account.Length; ++c)
                {
                    Mobile character = account[c];

                    if (character != null && character.AccessLevel >= StaffAccess)
                    {
                        staffCharacter = true;
                        break;
                    }
                }

                if (staffCharacter)
                    continue;

                DateTime lastLogin = account.LastLogin;

                // A zero/uninitialized date is too ambiguous for destructive automation.
                if (lastLogin == DateTime.MinValue)
                    continue;

                TimeSpan inactive = now - lastLogin;

                if (inactive.TotalDays <= InactiveDaysRequired)
                    continue;

                WBHouseReviewEntry review = GetReviewEntry(house.Serial);

                if (review != null)
                {
                    if (review.State == WBHouseReviewState.Exempt)
                        continue;

                    if (review.State == WBHouseReviewState.Deferred &&
                        review.DeferUntilUtc > now)
                        continue;
                }

                WBHouseCandidate candidate = new WBHouseCandidate();
                candidate.House = house;
                candidate.Owner = owner;
                candidate.Account = account;
                candidate.LastLoginUtc = lastLogin;
                candidate.InactiveDays = inactive.TotalDays;

                list.Add(candidate);
            }

            list.Sort(
                delegate(WBHouseCandidate a, WBHouseCandidate b)
                {
                    int result = b.InactiveDays.CompareTo(a.InactiveDays);

                    if (result != 0)
                        return result;

                    return a.House.Serial.Value.CompareTo(b.House.Serial.Value);
                });

            return list;
        }

        public static WBHouseReviewEntry GetReviewEntry(Serial houseSerial)
        {
            WBHousingReclamationController controller =
                WBHousingReclamationController.Instance;

            if (controller == null)
                return null;

            WBHouseReviewEntry entry;

            if (controller.Review.TryGetValue(houseSerial, out entry))
                return entry;

            return null;
        }

        public static void Defer(Mobile staff, BaseHouse house)
        {
            if (staff == null || house == null || house.Deleted)
                return;

            WBHousingReclamationController controller =
                WBHousingReclamationController.Instance;

            if (controller == null)
                return;

            WBHouseReviewEntry entry = new WBHouseReviewEntry();
            entry.HouseSerial = house.Serial;
            entry.State = WBHouseReviewState.Deferred;
            entry.DeferUntilUtc = DateTime.UtcNow.AddDays(DefaultDeferDays);
            entry.StaffName = staff.Name;
            entry.ChangedUtc = DateTime.UtcNow;

            controller.Review[house.Serial] = entry;

            staff.SendMessage(
                68,
                "House {0} deferred for {1} days.",
                house.Serial,
                DefaultDeferDays);
        }

        public static void Exempt(Mobile staff, BaseHouse house)
        {
            if (staff == null || house == null || house.Deleted)
                return;

            WBHousingReclamationController controller =
                WBHousingReclamationController.Instance;

            if (controller == null)
                return;

            WBHouseReviewEntry entry = new WBHouseReviewEntry();
            entry.HouseSerial = house.Serial;
            entry.State = WBHouseReviewState.Exempt;
            entry.DeferUntilUtc = DateTime.MinValue;
            entry.StaffName = staff.Name;
            entry.ChangedUtc = DateTime.UtcNow;

            controller.Review[house.Serial] = entry;

            staff.SendMessage(
                68,
                "House {0} permanently exempted from housing reclamation.",
                house.Serial);
        }

        public static void ClearReview(Mobile staff, BaseHouse house)
        {
            if (staff == null || house == null)
                return;

            WBHousingReclamationController controller =
                WBHousingReclamationController.Instance;

            if (controller == null)
                return;

            if (controller.Review.Remove(house.Serial))
                staff.SendMessage(68, "Review state cleared for house {0}.", house.Serial);
        }

        public static void TeleportToHouse(Mobile staff, BaseHouse house)
        {
            if (staff == null || house == null || house.Deleted)
                return;

            Map map = house.Map;

            if (map == null || map == Map.Internal)
            {
                staff.SendMessage(33, "That house is not on a valid world map.");
                return;
            }

            Point3D location = house.BanLocation;

            if (location == Point3D.Zero)
                location = house.Location;

            staff.MoveToWorld(location, map);
            staff.SendMessage(
                68,
                "Teleported to candidate house {0}, owner {1}.",
                house.Serial,
                house.Owner != null ? house.Owner.Name : "(none)");
        }
    }

    public class WBHousingReviewGump : Gump
    {
        private const int PerPage = 8;
        private readonly List<WBHouseCandidate> m_Candidates;
        private readonly int m_Page;

        public WBHousingReviewGump(
            Mobile from,
            List<WBHouseCandidate> candidates,
            int page)
            : base(30, 35)
        {
            m_Candidates = candidates != null
                ? candidates
                : new List<WBHouseCandidate>();

            int maxPage = m_Candidates.Count == 0
                ? 0
                : (m_Candidates.Count - 1) / PerPage;

            if (page < 0)
                page = 0;
            if (page > maxPage)
                page = maxPage;

            m_Page = page;
            bool canApprove = from != null && from.AccessLevel >= AccessLevel.Administrator;

            AddPage(0);
            AddBackground(0, 0, 1010, 540, 9270);
            AddBackground(15, 15, 980, 62, 9200);

            AddHtml(
                25, 24, 960, 26,
                "<CENTER><BASEFONT COLOR=#E8C468><BIG>WOLVESBANE - ABANDONED HOUSING REVIEW</BIG></BASEFONT></CENTER>",
                false, false);

            AddHtml(
                25, 52, 960, 20,
                String.Format(
                    "<CENTER><BASEFONT COLOR=#DDDDDD>{0} candidate(s) - more than {1} days since ACCOUNT last login</BASEFONT></CENTER>",
                    m_Candidates.Count,
                    WBHousingReclamationSystem.InactiveDaysRequired),
                false, false);

            AddLabel(25, 88, 88, "Owner / Account");
            AddLabel(250, 88, 88, "House");
            AddLabel(405, 88, 88, "Inactive");
            AddLabel(495, 88, 88, "Location");
            AddLabel(675, 88, 88, "Review");

            if (canApprove)
                AddLabel(920, 88, 88, "Batch");

            int start = m_Page * PerPage;
            int end = Math.Min(start + PerPage, m_Candidates.Count);
            int y = 116;

            for (int i = start; i < end; ++i)
            {
                WBHouseCandidate c = m_Candidates[i];
                BaseHouse house = c.House;

                string owner = c.Owner != null && !String.IsNullOrEmpty(c.Owner.Name)
                    ? c.Owner.Name
                    : "(unnamed)";

                string type = house != null ? house.GetType().Name : "(missing)";
                string map = house != null && house.Map != null
                    ? house.Map.Name
                    : "(none)";
                Point3D loc = house != null ? house.Location : Point3D.Zero;
                int localIndex = i - start;

                AddLabel(25, y, 1152, Truncate(owner + " / " + c.AccountName, 30));
                AddLabel(250, y, 1152, Truncate(type, 20));
                AddLabel(405, y, 1152, ((int)c.InactiveDays).ToString() + " days");
                AddLabel(495, y, 1152,
                    String.Format("{0} {1},{2},{3}", map, loc.X, loc.Y, loc.Z));

                AddButton(675, y, 4011, 4012, 1000 + localIndex, GumpButtonType.Reply, 0);
                AddLabel(710, y, 68, "Go");

                AddButton(755, y, 4029, 4030, 2000 + localIndex, GumpButtonType.Reply, 0);
                AddLabel(790, y, 53, "Defer");

                AddButton(675, y + 24, 4026, 4027, 3000 + localIndex, GumpButtonType.Reply, 0);
                AddLabel(710, y + 24, 33, "Exempt");

                if (canApprove)
                {
                    AddButton(805, y + 24, 4005, 4006, 4000 + localIndex, GumpButtonType.Reply, 0);
                    AddLabel(840, y + 24, 68, "Approve");

                    AddCheck(934, y + 10, 210, 211, false, 5000 + localIndex);
                }

                y += 47;
            }

            if (m_Page > 0)
            {
                AddButton(25, 492, 4014, 4015, 10, GumpButtonType.Reply, 0);
                AddLabel(60, 492, 1152, "Previous");
            }

            if (m_Page < maxPage)
            {
                AddButton(895, 492, 4005, 4006, 11, GumpButtonType.Reply, 0);
                AddLabel(930, 492, 1152, "Next");
            }

            AddLabel(
                410, 492, 1152,
                String.Format("Page {0} of {1}", m_Page + 1, maxPage + 1));

            if (canApprove)
            {
                AddButton(585, 488, 4005, 4006, 20, GumpButtonType.Reply, 0);
                AddLabel(620, 492, 68, "Review Selected Batch");
            }
        }

        private static string Truncate(string value, int max)
        {
            if (String.IsNullOrEmpty(value))
                return "";

            if (value.Length <= max)
                return value;

            return value.Substring(0, max - 3) + "...";
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (from == null || from.AccessLevel < AccessLevel.GameMaster)
                return;

            if (info.ButtonID == 10 || info.ButtonID == 11)
            {
                int nextPage = info.ButtonID == 10 ? m_Page - 1 : m_Page + 1;

                from.SendGump(
                    new WBHousingReviewGump(
                        from,
                        WBHousingReclamationSystem.GetCandidates(),
                        nextPage));
                return;
            }

            if (info.ButtonID == 20)
            {
                if (from.AccessLevel < AccessLevel.Administrator)
                    return;

                List<Serial> selected = new List<Serial>();
                int start = m_Page * PerPage;
                int end = Math.Min(start + PerPage, m_Candidates.Count);

                for (int i = start; i < end; ++i)
                {
                    int localIndex = i - start;

                    if (info.IsSwitched(5000 + localIndex))
                    {
                        BaseHouse house = m_Candidates[i].House;

                        if (house != null && !house.Deleted)
                            selected.Add(house.Serial);
                    }
                }

                if (selected.Count == 0)
                {
                    from.SendMessage(33, "Select at least one house on this page for batch review.");
                    from.SendGump(new WBHousingReviewGump(
                        from, WBHousingReclamationSystem.GetCandidates(), m_Page));
                    return;
                }

                from.SendGump(new WBBatchReclamationConfirmGump(selected));
                return;
            }

            int action = info.ButtonID / 1000;
            int local = info.ButtonID % 1000;
            int index = (m_Page * PerPage) + local;

            if (action < 1 || action > 4 ||
                index < 0 || index >= m_Candidates.Count)
                return;

            WBHouseCandidate candidate = m_Candidates[index];

            if (candidate == null ||
                candidate.House == null ||
                candidate.House.Deleted)
            {
                from.SendMessage(33, "That candidate house no longer exists.");
                return;
            }

            if (action == 1)
                WBHousingReclamationSystem.TeleportToHouse(from, candidate.House);
            else if (action == 2)
                WBHousingReclamationSystem.Defer(from, candidate.House);
            else if (action == 3)
            {
                from.SendGump(new WBHousingExemptConfirmGump(candidate.House));
                return;
            }
            else if (action == 4)
            {
                if (from.AccessLevel < AccessLevel.Administrator)
                    return;

                from.SendGump(new WBHouseDestroyConfirmGump(candidate));
                return;
            }

            from.SendGump(
                new WBHousingReviewGump(
                    from,
                    WBHousingReclamationSystem.GetCandidates(),
                    m_Page));
        }
    }

    public class WBBatchReclamationConfirmGump : Gump
    {
        private readonly List<Serial> m_HouseSerials;

        public WBBatchReclamationConfirmGump(List<Serial> serials)
            : base(130, 80)
        {
            m_HouseSerials = serials != null
                ? new List<Serial>(serials)
                : new List<Serial>();

            List<WBHouseCandidate> current = WBHousingReclamationSystem.GetCandidates();
            int valid = 0;
            long totalRefund = 0;
            int vendors = 0;
            int yardObjects = 0;

            for (int i = 0; i < m_HouseSerials.Count; ++i)
            {
                WBHouseCandidate c = FindCandidate(current, m_HouseSerials[i]);

                if (c == null || c.House == null || c.House.Deleted)
                    continue;

                valid++;
                totalRefund += Math.Max(0, c.House.Price);
                vendors += c.House.PlayerVendors != null ? c.House.PlayerVendors.Count : 0;
                yardObjects += WBHousingPhase2.CountYardObjects(c.House);
            }

            AddBackground(0, 0, 720, 430, 9270);
            AddBackground(18, 18, 684, 66, 9200);
            AddBackground(18, 96, 684, 205, 9200);
            AddBackground(18, 312, 684, 100, 9200);

            AddHtml(
                30, 31, 660, 28,
                "<CENTER><BASEFONT COLOR=#FF7777><BIG>CONFIRM BATCH RECLAMATION</BIG></BASEFONT></CENTER>",
                false, false);

            AddHtml(
                35, 62, 650, 18,
                "<CENTER><BASEFONT COLOR=#DDDDDD>Every house is re-checked immediately before processing.</BASEFONT></CENTER>",
                false, false);

            int lx = 55;
            int vx = 285;
            int y = 120;

            AddLabel(lx, y, 88, "Selected houses:");
            AddLabel(vx, y, 1153, m_HouseSerials.Count.ToString()); y += 32;

            AddLabel(lx, y, 88, "Currently eligible:");
            AddLabel(vx, y, 1153, valid.ToString()); y += 32;

            AddLabel(lx, y, 88, "Combined house refund:");
            AddLabel(vx, y, 1153, totalRefund.ToString("N0")); y += 32;

            AddLabel(lx, y, 88, "Player vendors:");
            AddLabel(vx, y, 1153, vendors.ToString()); y += 32;

            AddLabel(lx, y, 88, "Yard objects:");
            AddLabel(vx, y, 1153, yardObjects.ToString());

            AddHtml(
                45, 265, 630, 32,
                "<CENTER><BASEFONT COLOR=#F3C969>Batch processing is permanent. A failure on one house will not prevent later selected houses from being attempted.</BASEFONT></CENTER>",
                false, false);

            AddCheck(55, 332, 210, 211, false, 1);
            AddLabel(90, 334, 53, "I have reviewed this batch and understand the houses will be destroyed.");

            AddButton(150, 370, 247, 248, 1, GumpButtonType.Reply, 0);
            AddHtml(
                70, 397, 300, 24,
                "<CENTER><BASEFONT COLOR=#55FF55>CONFIRM BATCH RECLAMATION</BASEFONT></CENTER>",
                false, false);

            AddButton(535, 370, 241, 242, 0, GumpButtonType.Reply, 0);
            AddHtml(
                455, 397, 180, 24,
                "<CENTER><BASEFONT COLOR=#FF7777>CANCEL</BASEFONT></CENTER>",
                false, false);
        }

        private static WBHouseCandidate FindCandidate(
            List<WBHouseCandidate> candidates,
            Serial serial)
        {
            for (int i = 0; i < candidates.Count; ++i)
            {
                if (candidates[i].House != null &&
                    candidates[i].House.Serial == serial)
                    return candidates[i];
            }

            return null;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (from == null ||
                from.AccessLevel < AccessLevel.Administrator ||
                info.ButtonID != 1)
                return;

            if (!info.IsSwitched(1))
            {
                from.SendMessage(33, "Batch reclamation was not started. Check the confirmation box first.");
                from.SendGump(new WBBatchReclamationConfirmGump(m_HouseSerials));
                return;
            }

            int success = 0;
            int failed = 0;

            for (int i = 0; i < m_HouseSerials.Count; ++i)
            {
                List<WBHouseCandidate> current =
                    WBHousingReclamationSystem.GetCandidates();

                WBHouseCandidate candidate =
                    FindCandidate(current, m_HouseSerials[i]);

                if (candidate == null ||
                    candidate.House == null ||
                    candidate.House.Deleted)
                {
                    failed++;
                    from.SendMessage(33,
                        "Batch skipped {0}: it is no longer an eligible candidate.",
                        m_HouseSerials[i]);
                    continue;
                }

                string error;

                if (WBHousingPhase2.ProcessHouse(
                    from,
                    candidate.House,
                    out error,
                    "Batch"))
                {
                    success++;
                }
                else
                {
                    failed++;
                    from.SendMessage(
                        33,
                        "Batch failed for house {0}: {1}",
                        m_HouseSerials[i],
                        error);
                }
            }

            from.SendMessage(
                68,
                "Batch reclamation complete: {0} succeeded, {1} failed/skipped.",
                success,
                failed);

            from.SendGump(
                new WBHousingReviewGump(
                    from,
                    WBHousingReclamationSystem.GetCandidates(),
                    0));
        }
    }

    public class WBHousingExemptConfirmGump : Gump
    {
        private readonly BaseHouse m_House;

        public WBHousingExemptConfirmGump(BaseHouse house)
            : base(160, 110)
        {
            m_House = house;

            string owner = house != null && house.Owner != null
                ? house.Owner.Name
                : "(none)";

            Account account = house != null && house.Owner != null
                ? house.Owner.Account as Account
                : null;

            string accountName = account != null
                ? account.Username
                : "(none)";

            string houseType = house != null
                ? house.GetType().Name
                : "(missing)";

            string serial = house != null
                ? house.Serial.ToString()
                : "(missing)";

            string map = house != null && house.Map != null
                ? house.Map.Name
                : "(none)";

            Point3D loc = house != null
                ? house.Location
                : Point3D.Zero;

            AddPage(0);

            // Outer stone frame.
            AddBackground(0, 0, 700, 460, 9270);

            // Dark inset panels to give the gump a heavier "administrative" look.
            AddBackground(18, 18, 664, 64, 9200);
            AddBackground(18, 92, 664, 230, 9200);
            AddBackground(18, 332, 664, 110, 9200);

            // Title.
            AddHtml(
                36,
                30,
                628,
                28,
                "<CENTER><BASEFONT COLOR=#E8C468><BIG>PERMANENT HOUSE EXEMPTION</BIG></BASEFONT></CENTER>",
                false,
                false);

            AddHtml(
                36,
                59,
                628,
                18,
                "<CENTER><BASEFONT COLOR=#DDDDDD>Please review the house details before confirming.</BASEFONT></CENTER>",
                false,
                false);

            // Detail block.
            int labelX = 48;
            int valueX = 205;
            int y = 112;

            AddLabel(labelX, y, 88, "House Owner:");
            AddLabel(valueX, y, 1153, owner);
            y += 30;

            AddLabel(labelX, y, 88, "Account:");
            AddLabel(valueX, y, 1153, accountName);
            y += 30;

            AddLabel(labelX, y, 88, "House:");
            AddLabel(valueX, y, 1153, houseType);
            y += 30;

            AddLabel(labelX, y, 88, "Serial:");
            AddLabel(valueX, y, 1153, serial);
            y += 30;

            AddLabel(labelX, y, 88, "Map:");
            AddLabel(valueX, y, 1153, map);
            y += 30;

            AddLabel(labelX, y, 88, "Location:");
            AddLabel(
                valueX,
                y,
                1153,
                String.Format("{0}, {1}, {2}", loc.X, loc.Y, loc.Z));

            // Warning panel.
            AddHtml(
                44,
                277,
                612,
                38,
                "<CENTER><BASEFONT COLOR=#F3C969>This house will be permanently excluded from abandoned-property reclamation.<BR>It will remain exempt until staff manually removes the exemption.</BASEFONT></CENTER>",
                false,
                false);

            // Bottom buttons.  The stock artwork remains the clickable control;
            // the descriptive text sits BELOW it so it does not overlap the art.
            AddButton(115, 352, 247, 248, 1, GumpButtonType.Reply, 0);
            AddHtml(
                66,
                391,
                250,
                28,
                "<CENTER><BASEFONT COLOR=#55FF55>CONFIRM EXEMPTION</BASEFONT></CENTER>",
                false,
                false);

            AddButton(485, 352, 241, 242, 0, GumpButtonType.Reply, 0);
            AddHtml(
                414,
                391,
                250,
                28,
                "<CENTER><BASEFONT COLOR=#FF7777>CANCEL</BASEFONT></CENTER>",
                false,
                false);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (from == null ||
                from.AccessLevel < AccessLevel.GameMaster ||
                info.ButtonID != 1)
                return;

            if (m_House == null || m_House.Deleted)
            {
                from.SendMessage(33, "That house no longer exists.");
                return;
            }

            WBHousingReclamationSystem.Exempt(from, m_House);

            from.SendGump(
                new WBHousingReviewGump(
                    from,
                    WBHousingReclamationSystem.GetCandidates(),
                    0));
        }
    }

}