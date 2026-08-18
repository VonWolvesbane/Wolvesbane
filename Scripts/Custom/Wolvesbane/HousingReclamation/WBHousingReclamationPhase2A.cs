using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Accounting;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Targeting;
using Server.ACC.YS;

namespace Wolvesbane.HousingReclamation
{
    public class WBReclamationRecord
    {
        public string Id;
        public string AccountName;
        public string OriginalOwnerName;
        public Serial OriginalOwnerSerial;
        public string HouseType;
        public Serial HouseSerial;
        public string MapName;
        public Point3D HouseLocation;
        public int RefundAmount;
        public DateTime ReclaimedUtc;
        public DateTime DestroyedUtc;
        public string ApprovedBy;
        public WBReclamationCrate Storage;
        public bool Claimed;

        public WBReclamationRecord()
        {
        }

        public int ReclaimedItemCount;
        public int VendorCount;
        public int YardObjectCount;
        public int YardRefundAmount;

        public WBReclamationRecord(GenericReader reader)
        {
            int version = reader.ReadInt();
            Id = reader.ReadString();
            AccountName = reader.ReadString();
            OriginalOwnerName = reader.ReadString();
            OriginalOwnerSerial = (Serial)reader.ReadInt();
            HouseType = reader.ReadString();
            HouseSerial = (Serial)reader.ReadInt();
            MapName = reader.ReadString();
            HouseLocation = reader.ReadPoint3D();
            RefundAmount = reader.ReadInt();
            DestroyedUtc = reader.ReadDateTime();
            ApprovedBy = reader.ReadString();
            Storage = reader.ReadItem() as WBReclamationCrate;
            Claimed = reader.ReadBool();
            ReclaimedUtc = reader.ReadDateTime();

            if (version >= 1)
            {
                ReclaimedItemCount = reader.ReadInt();
                VendorCount = reader.ReadInt();
                YardObjectCount = reader.ReadInt();
                YardRefundAmount = reader.ReadInt();
            }
            else
            {
                ReclaimedItemCount = Storage != null && !Storage.Deleted ? Storage.TotalItems : 0;
                VendorCount = 0;
                YardObjectCount = 0;
                YardRefundAmount = 0;
            }
        }

        public void Serialize(GenericWriter writer)
        {
            writer.Write(1);
            writer.Write(Id);
            writer.Write(AccountName);
            writer.Write(OriginalOwnerName);
            writer.Write((int)OriginalOwnerSerial);
            writer.Write(HouseType);
            writer.Write((int)HouseSerial);
            writer.Write(MapName);
            writer.Write(HouseLocation);
            writer.Write(RefundAmount);
            writer.Write(DestroyedUtc);
            writer.Write(ApprovedBy);
            writer.Write(Storage);
            writer.Write(Claimed);
            writer.Write(ReclaimedUtc);
            writer.Write(ReclaimedItemCount);
            writer.Write(VendorCount);
            writer.Write(YardObjectCount);
            writer.Write(YardRefundAmount);
        }
    }

    public class WBHousingPhase2Controller : Item
    {
        private static WBHousingPhase2Controller m_Instance;
        private List<WBReclamationRecord> m_Records = new List<WBReclamationRecord>();

        public static WBHousingPhase2Controller Instance { get { return m_Instance; } }
        public List<WBReclamationRecord> Records { get { return m_Records; } }

        [Constructable]
        public WBHousingPhase2Controller() : base(0x1)
        {
            Name = "Wolvesbane Housing Reclamation Phase 2 Controller";
            Movable = false;
            Visible = false;
            MoveToWorld(Point3D.Zero, Map.Internal);
            m_Instance = this;
        }

        public WBHousingPhase2Controller(Serial serial) : base(serial)
        {
            m_Instance = this;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(m_Records.Count);

            for (int i = 0; i < m_Records.Count; ++i)
                m_Records[i].Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            int count = reader.ReadInt();

            m_Records = new List<WBReclamationRecord>();

            for (int i = 0; i < count; ++i)
                m_Records.Add(new WBReclamationRecord(reader));

            Visible = false;
            Movable = false;
            m_Instance = this;
        }
    }

    public class WBReclamationCrate : Backpack
    {
        public string AccountName { get; set; }
        public string RecordId { get; set; }

        [Constructable]
        public WBReclamationCrate() : this("(unassigned)", "")
        {
        }

        public WBReclamationCrate(string accountName, string recordId)
        {
            AccountName = accountName;
            RecordId = recordId;
            Name = "Abandoned Property Reclamation Storage";
            Hue = 1153;
            Movable = false;
        }

        public WBReclamationCrate(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(AccountName);
            writer.Write(RecordId);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            AccountName = reader.ReadString();
            RecordId = reader.ReadString();
            Movable = false;
        }
    }

    public class WBVendorHeldItems : Bag
    {
        public WBVendorHeldItems(string vendorName)
        {
            Name = "Vendor Held Items" +
                (!String.IsNullOrEmpty(vendorName) ? " - " + vendorName : "");
            Hue = 1150;
        }

        public WBVendorHeldItems(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class WBReclamationStorageBag : Bag
    {
        public int BagNumber { get; set; }

        [Constructable]
        public WBReclamationStorageBag() : this(1)
        {
        }

        public WBReclamationStorageBag(int number)
        {
            BagNumber = number;
            Name = "Reclaimed House Items - Bag " + number;
            Hue = 1152;
        }

        public WBReclamationStorageBag(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(BagNumber);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            BagNumber = reader.ReadInt();
        }
    }

    public class WBYardItemRefunds : Bag
    {
        public WBYardItemRefunds()
        {
            Name = "Yard Item Refunds";
            Hue = 1161;
        }

        public WBYardItemRefunds(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class WBDestroyedHouseRefund : Bag
    {
        public WBDestroyedHouseRefund(int amount)
        {
            Name = "Destroyed House Refund";
            Hue = 1153;

            if (amount > 0)
            {
                BankCheck check = new BankCheck(amount);
                check.Name = "Refund from housing destruction";
                DropItem(check);
            }
        }

        public WBDestroyedHouseRefund(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class AbandonedPropertyReclamationOfficer : Mobile
    {
        [Constructable]
        public AbandonedPropertyReclamationOfficer()
        {
            Name = "Abandoned Property Reclamation Officer";
            Title = "property reclamation services";
            Body = 0x190;
            Hue = 0x83EA;
            Blessed = true;
            CantWalk = true;
        }

        public AbandonedPropertyReclamationOfficer(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !from.InRange(this, 4))
            {
                if (from != null)
                    from.SendMessage("You are too far away.");
                return;
            }

            Account account = from.Account as Account;

            if (account == null)
            {
                from.SendMessage("I cannot identify your account.");
                return;
            }

            from.CloseGump(typeof(WBReclamationGump));
            from.SendGump(new WBReclamationGump(from, account.Username));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            Blessed = true;
            CantWalk = true;
        }
    }

    public static class WBHousingPhase2
    {
        // Keep ordinary reclaimed house property spread across smaller bags.
        // This avoids leaving a single master container hundreds of items over
        // the normal container capacity.
        private const int ReclamationBagItemTarget = 100;
        public static void Initialize()
        {
            EnsureController();

            CommandSystem.Register(
                "WBHousingApproveTest",
                AccessLevel.Administrator,
                new CommandEventHandler(OnApproveTest));

            CommandSystem.Register(
                "WBHousingRecords",
                AccessLevel.GameMaster,
                new CommandEventHandler(OnRecords));
        }

        private static void EnsureController()
        {
            if (WBHousingPhase2Controller.Instance != null)
                return;

            Timer.DelayCall(TimeSpan.FromSeconds(2.5), delegate
            {
                if (WBHousingPhase2Controller.Instance == null)
                    new WBHousingPhase2Controller();
            });
        }

        private static void OnApproveTest(CommandEventArgs e)
        {
            e.Mobile.SendMessage(
                68,
                "Target the HOUSE SIGN of ONE Phase 1 candidate house. TEST SERVER ONLY.");
            e.Mobile.Target = new WBHouseApprovalTarget();
        }

        private static void OnRecords(CommandEventArgs e)
        {
            EnsureController();

            WBHousingPhase2Controller c = WBHousingPhase2Controller.Instance;

            if (c == null)
            {
                e.Mobile.SendMessage(33, "Phase 2 controller is not available.");
                return;
            }

            int unclaimed = 0;

            for (int i = 0; i < c.Records.Count; ++i)
                if (!c.Records[i].Claimed)
                    unclaimed++;

            e.Mobile.SendMessage(88, "Wolvesbane Housing Reclamation Records");
            e.Mobile.SendMessage("Total destroyed-house records: {0}", c.Records.Count);
            e.Mobile.SendMessage("Unclaimed property records: {0}", unclaimed);
        }

        private class WBHouseApprovalTarget : Target
        {
            public WBHouseApprovalTarget() : base(-1, true, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                HouseSign sign = targeted as HouseSign;

                if (sign == null)
                {
                    from.SendMessage(33, "Target the house sign.");
                    return;
                }

                BaseHouse house = sign.Owner;

                if (house == null || house.Deleted)
                {
                    from.SendMessage(33, "That sign is not attached to a valid house.");
                    return;
                }

                WBHouseCandidate candidate = FindCandidate(house);

                if (candidate == null)
                {
                    from.SendMessage(
                        33,
                        "That house is not currently a Phase 1 reclamation candidate.");
                    return;
                }

                if (house.VendorInventories != null && house.VendorInventories.Count > 0)
                {
                    from.SendMessage(
                        33,
                        "BLOCKED: this house contains legacy VendorInventory records. Phase 2A will not destroy it.");
                    return;
                }

                from.SendGump(new WBHouseDestroyConfirmGump(candidate));
            }
        }

        private static WBHouseCandidate FindCandidate(BaseHouse house)
        {
            List<WBHouseCandidate> list =
                WBHousingReclamationSystem.GetCandidates();

            for (int i = 0; i < list.Count; ++i)
                if (list[i].House == house)
                    return list[i];

            return null;
        }

        public static bool ProcessHouse(
            Mobile staff,
            BaseHouse house,
            out string error)
        {
            return ProcessHouse(staff, house, out error, "Individual");
        }

        public static bool ProcessHouse(
            Mobile staff,
            BaseHouse house,
            out string error,
            string approvalMode)
        {
            error = null;

            if (staff == null || house == null || house.Deleted)
            {
                error = "House or staff context is invalid.";
                return false;
            }

            WBHouseCandidate candidate = FindCandidate(house);

            if (candidate == null)
            {
                error = "House is no longer an eligible candidate.";
                return false;
            }

            if (house.VendorInventories != null && house.VendorInventories.Count > 0)
            {
                error = "Legacy VendorInventory records are present; processing is blocked.";
                return false;
            }

            EnsureController();
            WBHousingPhase2Controller controller =
                WBHousingPhase2Controller.Instance;

            if (controller == null)
            {
                error = "Phase 2 controller is unavailable.";
                return false;
            }

            Mobile owner = candidate.Owner;
            Account account = candidate.Account;

            if (owner == null || account == null)
            {
                error = "Owner/account is missing.";
                return false;
            }

            int vendorCountBefore = house.PlayerVendors != null ? house.PlayerVendors.Count : 0;
            int yardObjectCountBefore = CountYardObjects(house);
            int yardRefundBefore = yardObjectCountBefore * 500;

            string recordId = Guid.NewGuid().ToString("N");
            WBReclamationCrate storage =
                new WBReclamationCrate(account.Username, recordId);

            storage.Name = String.Format(
                "Reclaimed Property - {0} - {1}",
                owner.Name,
                house.GetType().Name);

            storage.MoveToWorld(Point3D.Zero, Map.Internal);

            WBReclamationRecord record = new WBReclamationRecord();
            record.Id = recordId;
            record.AccountName = account.Username;
            record.OriginalOwnerName = owner.Name;
            record.OriginalOwnerSerial = owner.Serial;
            record.HouseType = house.GetType().FullName;
            record.HouseSerial = house.Serial;
            record.MapName = house.Map != null ? house.Map.Name : "(none)";
            record.HouseLocation = house.Location;
            record.RefundAmount = Math.Max(0, house.Price);
            record.DestroyedUtc = DateTime.UtcNow;
            record.ApprovedBy = staff.Name;
            record.Storage = storage;
            record.Claimed = false;
            record.ReclaimedUtc = DateTime.MinValue;

            // Persist the record before destructive work begins.
            controller.Records.Add(record);

            try
            {
                CaptureYardRefunds(house, storage);
                CapturePlayerVendors(house, storage);

                // Use ServUO's own house relocation logic for lockdowns,
                // secures, contracts and addons/redeeding.
                house.MoveAllToCrate();

                MovingCrate moving = house.MovingCrate;

                if (moving != null && !moving.Deleted)
                {
                    List<Item> items = new List<Item>(moving.Items);

                    StoreHouseItemsOrganized(storage, items);

                    house.MovingCrate = null;
                    moving.Delete();
                }

                Mobile refundRecipient =
                    FindRefundRecipient(owner, account);

                if (refundRecipient == null || refundRecipient.BankBox == null)
                    throw new Exception("No valid bank box was available for the house refund.");

                WBDestroyedHouseRefund refund =
                    new WBDestroyedHouseRefund(record.RefundAmount);

                // Direct AddItem is intentional: reclamation refunds should not
                // fail because a bank is already near its normal item limit.
                refundRecipient.BankBox.AddItem(refund);

                house.Delete();

                WBHousingReclamationController phase1 =
                    WBHousingReclamationController.Instance;

                if (phase1 != null)
                    phase1.Review.Remove(record.HouseSerial);

                int reclaimedItems = storage.TotalItems;

                record.ReclaimedItemCount = reclaimedItems;
                record.VendorCount = vendorCountBefore;
                record.YardObjectCount = yardObjectCountBefore;
                record.YardRefundAmount = yardRefundBefore;

                WriteAuditLog(
                    record,
                    approvalMode,
                    vendorCountBefore,
                    yardObjectCountBefore,
                    yardRefundBefore,
                    reclaimedItems,
                    "SUCCESS",
                    null);

                staff.SendMessage(
                    68,
                    "House reclaimed successfully. Record {0}; refund {1:N0}; reclaimed items {2:N0}.",
                    record.Id,
                    record.RefundAmount,
                    reclaimedItems);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.GetType().Name + ": " + ex.Message;

                WriteAuditLog(
                    record,
                    approvalMode,
                    vendorCountBefore,
                    yardObjectCountBefore,
                    yardRefundBefore,
                    storage != null && !storage.Deleted ? storage.TotalItems : 0,
                    "FAILED",
                    error);

                staff.SendMessage(
                    33,
                    "RECLAMATION ERROR: {0}",
                    error);

                staff.SendMessage(
                    33,
                    "The reclamation record/storage were retained for investigation. The routine did not intentionally continue after the failure.");

                return false;
            }
        }

        public static int CountYardObjects(BaseHouse house)
        {
            if (house == null)
                return 0;

            // Snapshot first so World.Items can safely change elsewhere without
            // invalidating our enumeration.
            List<Item> worldSnapshot = new List<Item>(World.Items.Values);
            int count = 0;

            for (int i = 0; i < worldSnapshot.Count; ++i)
            {
                Item worldItem = worldSnapshot[i];

                if (worldItem == null || worldItem.Deleted)
                    continue;

                YardItem yi = worldItem as YardItem;

                if (yi != null && yi.House == house)
                {
                    count++;
                    continue;
                }

                YardGate yg = worldItem as YardGate;

                if (yg != null && yg.House == house)
                    count++;
            }

            return count;
        }

        private static void CaptureYardRefunds(
            BaseHouse house,
            WBReclamationCrate storage)
        {
            if (house == null || storage == null)
                return;

            // CRITICAL: snapshot World.Items before creating checks or deleting
            // yard objects. Item construction/deletion mutates World.Items.
            List<Item> worldSnapshot = new List<Item>(World.Items.Values);
            List<Item> yardObjects = new List<Item>();

            for (int i = 0; i < worldSnapshot.Count; ++i)
            {
                Item worldItem = worldSnapshot[i];

                if (worldItem == null || worldItem.Deleted)
                    continue;

                YardItem yardItem = worldItem as YardItem;

                if (yardItem != null && yardItem.House == house)
                {
                    yardObjects.Add(yardItem);
                    continue;
                }

                YardGate yardGate = worldItem as YardGate;

                if (yardGate != null && yardGate.House == house)
                    yardObjects.Add(yardGate);
            }

            if (yardObjects.Count == 0)
                return;

            WBYardItemRefunds refundBag = new WBYardItemRefunds();
            storage.DropItem(refundBag);

            const int YardRefundPerPlacedItem = 500;
            int refundedObjectCount = 0;

            for (int i = 0; i < yardObjects.Count; ++i)
            {
                Item obj = yardObjects[i];

                if (obj == null || obj.Deleted)
                    continue;

                refundedObjectCount++;

                // Each top-level YardItem/YardGate is worth exactly 500 gold.
                // Internal YardPiece children are not counted separately.
                obj.Delete();
            }

            if (refundedObjectCount > 0)
            {
                int totalRefund = refundedObjectCount * YardRefundPerPlacedItem;
                BankCheck check = new BankCheck(totalRefund);
                check.Name = "Yard Item Refund Total";
                refundBag.DropItem(check);
            }
        }

        private static void StoreHouseItemsOrganized(
            WBReclamationCrate storage,
            List<Item> items)
        {
            if (storage == null || items == null || items.Count == 0)
                return;

            WBReclamationStorageBag currentBag = null;
            int currentCount = 0;
            int bagNumber = 0;

            for (int i = 0; i < items.Count; ++i)
            {
                Item item = items[i];

                if (item == null || item.Deleted)
                    continue;

                // Vendor containers are already deliberately organized and
                // should stay directly under the master reclamation crate.
                if (item is WBVendorHeldItems)
                {
                    storage.DropItem(item);
                    continue;
                }

                if (currentBag == null ||
                    currentCount >= ReclamationBagItemTarget)
                {
                    bagNumber++;
                    currentBag = new WBReclamationStorageBag(bagNumber);
                    storage.DropItem(currentBag);
                    currentCount = 0;
                }

                currentBag.DropItem(item);
                currentCount++;
            }
        }

        private static void CapturePlayerVendors(
            BaseHouse house,
            WBReclamationCrate storage)
        {
            List<PlayerVendor> vendors = new List<PlayerVendor>();

            for (int i = 0; i < house.PlayerVendors.Count; ++i)
            {
                PlayerVendor pv = house.PlayerVendors[i] as PlayerVendor;

                if (pv != null && !pv.Deleted)
                    vendors.Add(pv);
            }

            for (int v = 0; v < vendors.Count; ++v)
            {
                PlayerVendor vendor = vendors[v];

                WBVendorHeldItems vendorBag =
                    new WBVendorHeldItems(vendor.Name);

                storage.DropItem(vendorBag);

                List<Item> held = new List<Item>();

                for (int i = 0; i < vendor.Items.Count; ++i)
                {
                    Item item = vendor.Items[i];

                    if (item == null ||
                        item.Deleted ||
                        item == vendor.Backpack ||
                        item.Layer == Layer.Hair ||
                        item.Layer == Layer.FacialHair)
                        continue;

                    if (item.Movable)
                        held.Add(item);
                }

                if (vendor.Backpack != null)
                {
                    List<Item> packItems =
                        new List<Item>(vendor.Backpack.Items);

                    for (int i = 0; i < packItems.Count; ++i)
                    {
                        Item item = packItems[i];

                        if (item != null && !item.Deleted && !held.Contains(item))
                            held.Add(item);
                    }
                }

                for (int i = 0; i < held.Count; ++i)
                    vendorBag.DropItem(held[i]);

                long vendorGold =
                    (long)Math.Max(0, vendor.HoldGold) +
                    (long)Math.Max(0, vendor.BankAccount);

                if (vendorGold > Int32.MaxValue)
                    vendorGold = Int32.MaxValue;

                if (vendorGold > 0)
                {
                    BankCheck check = new BankCheck((int)vendorGold);
                    check.Name = "Vendor held gold";
                    vendorBag.DropItem(check);
                }

                vendor.HoldGold = 0;
                vendor.BankAccount = 0;

                // Items have already been moved to our named vendor container,
                // so normal vendor destruction has nothing left to discard.
                vendor.Destroy(false);
            }
        }

        private static void WriteAuditLog(
            WBReclamationRecord record,
            string approvalMode,
            int vendorCount,
            int yardCount,
            int yardRefund,
            int reclaimedItems,
            string result,
            string error)
        {
            try
            {
                string logDir = "Logs";

                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                string path = Path.Combine(logDir, "WBHousingReclamation.log");

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(
                        "{0:u} | {1} | Mode={2} | Staff={3} | Account={4} | Owner={5} | House={6} | Serial={7} | Map={8} | Location={9},{10},{11} | HouseRefund={12} | Vendors={13} | YardObjects={14} | YardRefund={15} | ReclaimedItems={16} | Record={17}{18}",
                        DateTime.UtcNow,
                        result,
                        String.IsNullOrEmpty(approvalMode) ? "Unknown" : approvalMode,
                        record != null ? record.ApprovedBy : "(unknown)",
                        record != null ? record.AccountName : "(unknown)",
                        record != null ? record.OriginalOwnerName : "(unknown)",
                        record != null ? record.HouseType : "(unknown)",
                        record != null ? record.HouseSerial.ToString() : "(unknown)",
                        record != null ? record.MapName : "(unknown)",
                        record != null ? record.HouseLocation.X : 0,
                        record != null ? record.HouseLocation.Y : 0,
                        record != null ? record.HouseLocation.Z : 0,
                        record != null ? record.RefundAmount : 0,
                        vendorCount,
                        yardCount,
                        yardRefund,
                        reclaimedItems,
                        record != null ? record.Id : "(none)",
                        String.IsNullOrEmpty(error) ? "" : " | Error=" + error);
                }
            }
            catch
            {
                // A logging problem must never be allowed to break reclamation.
            }
        }

        private static Mobile FindRefundRecipient(
            Mobile originalOwner,
            Account account)
        {
            if (originalOwner != null &&
                !originalOwner.Deleted &&
                originalOwner.BankBox != null)
                return originalOwner;

            if (account == null)
                return null;

            for (int i = 0; i < account.Length; ++i)
            {
                Mobile m = account[i];

                if (m != null && !m.Deleted && m.BankBox != null)
                    return m;
            }

            return null;
        }
    }

    public class WBHouseDestroyConfirmGump : Gump
    {
        private readonly Serial m_HouseSerial;

        public WBHouseDestroyConfirmGump(WBHouseCandidate candidate)
            : base(150, 90)
        {
            BaseHouse house = candidate.House;
            m_HouseSerial = house.Serial;

            AddBackground(0, 0, 720, 500, 9270);
            AddBackground(18, 18, 684, 68, 9200);
            AddBackground(18, 96, 684, 270, 9200);
            AddBackground(18, 376, 684, 106, 9200);

            AddHtml(
                35, 32, 650, 30,
                "<CENTER><BASEFONT COLOR=#FF7777><BIG>APPROVE HOUSE RECLAMATION</BIG></BASEFONT></CENTER>",
                false, false);

            AddHtml(
                35, 64, 650, 18,
                "<CENTER><BASEFONT COLOR=#DDDDDD>This action permanently destroys the house after property capture.</BASEFONT></CENTER>",
                false, false);

            int lx = 48;
            int vx = 220;
            int y = 116;

            AddLabel(lx, y, 88, "Owner:");
            AddLabel(vx, y, 1153, candidate.Owner.Name); y += 30;
            AddLabel(lx, y, 88, "Account:");
            AddLabel(vx, y, 1153, candidate.Account.Username); y += 30;
            AddLabel(lx, y, 88, "House:");
            AddLabel(vx, y, 1153, house.GetType().Name); y += 30;
            AddLabel(lx, y, 88, "Serial:");
            AddLabel(vx, y, 1153, house.Serial.ToString()); y += 30;
            AddLabel(lx, y, 88, "Inactive:");
            AddLabel(vx, y, 1153, ((int)candidate.InactiveDays) + " days"); y += 30;
            AddLabel(lx, y, 88, "Refund:");
            AddLabel(vx, y, 1153, Math.Max(0, house.Price).ToString("N0")); y += 30;
            AddLabel(lx, y, 88, "Player vendors:");
            AddLabel(vx, y, 1153, house.PlayerVendors.Count.ToString()); y += 30;
            AddLabel(lx, y, 88, "Yard objects:");
            AddLabel(vx, y, 1153, WBHousingPhase2.CountYardObjects(house).ToString()); y += 30;
            AddLabel(lx, y, 88, "Legacy vendor records:");
            AddLabel(vx, y, 1153, house.VendorInventories.Count.ToString());

            AddHtml(
                45, 332, 630, 30,
                "<CENTER><BASEFONT COLOR=#F3C969>Property will be moved to permanent account reclamation storage BEFORE the house is deleted.</BASEFONT></CENTER>",
                false, false);

            AddButton(135, 397, 247, 248, 1, GumpButtonType.Reply, 0);
            AddHtml(
                65, 436, 280, 24,
                "<CENTER><BASEFONT COLOR=#55FF55>APPROVE &amp; RECLAIM</BASEFONT></CENTER>",
                false, false);

            AddButton(525, 397, 241, 242, 0, GumpButtonType.Reply, 0);
            AddHtml(
                440, 436, 220, 24,
                "<CENTER><BASEFONT COLOR=#FF7777>CANCEL</BASEFONT></CENTER>",
                false, false);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (from == null ||
                from.AccessLevel < AccessLevel.Administrator ||
                info.ButtonID != 1)
                return;

            BaseHouse house = null;
            List<BaseHouse> houses = BaseHouse.AllHouses;

            for (int i = 0; i < houses.Count; ++i)
            {
                if (houses[i] != null && houses[i].Serial == m_HouseSerial)
                {
                    house = houses[i];
                    break;
                }
            }

            if (house == null || house.Deleted)
            {
                from.SendMessage(33, "That house no longer exists.");
                return;
            }

            string error;

            if (!WBHousingPhase2.ProcessHouse(from, house, out error, "Individual"))
                from.SendMessage(33, "House reclamation did not complete: {0}", error);
        }
    }

    public class WBReclamationGump : Gump
    {
        private const int PerPage = 5;

        private readonly string m_AccountName;
        private readonly bool m_ShowHistory;
        private readonly int m_Page;
        private readonly List<WBReclamationRecord> m_Records;

        public WBReclamationGump(
            Mobile from,
            string accountName)
            : this(from, accountName, false, 0)
        {
        }

        public WBReclamationGump(
            Mobile from,
            string accountName,
            bool showHistory,
            int page)
            : base(90, 55)
        {
            m_AccountName = accountName;
            m_ShowHistory = showHistory;
            m_Records = new List<WBReclamationRecord>();

            WBHousingPhase2Controller c = WBHousingPhase2Controller.Instance;

            if (c != null)
            {
                for (int i = 0; i < c.Records.Count; ++i)
                {
                    WBReclamationRecord r = c.Records[i];

                    if (!String.Equals(
                        r.AccountName,
                        accountName,
                        StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (showHistory)
                    {
                        if (r.Claimed)
                            m_Records.Add(r);
                    }
                    else
                    {
                        if (!r.Claimed)
                            m_Records.Add(r);
                    }
                }
            }

            m_Records.Sort(
                delegate(WBReclamationRecord a, WBReclamationRecord b)
                {
                    return b.DestroyedUtc.CompareTo(a.DestroyedUtc);
                });

            int maxPage = m_Records.Count == 0
                ? 0
                : (m_Records.Count - 1) / PerPage;

            if (page < 0)
                page = 0;
            if (page > maxPage)
                page = maxPage;

            m_Page = page;

            AddPage(0);
            AddBackground(0, 0, 820, 560, 9270);
            AddBackground(18, 18, 784, 72, 9200);

            AddHtml(
                28, 28, 764, 28,
                "<CENTER><BASEFONT COLOR=#E8C468><BIG>ABANDONED PROPERTY RECLAMATION SERVICES</BIG></BASEFONT></CENTER>",
                false, false);

            AddHtml(
                28, 60, 764, 20,
                String.Format(
                    "<CENTER><BASEFONT COLOR=#DDDDDD>Account: {0}</BASEFONT></CENTER>",
                    accountName),
                false, false);

            // Tabs
            AddButton(35, 104, showHistory ? 4005 : 4006, showHistory ? 4007 : 4005,
                10, GumpButtonType.Reply, 0);
            AddLabel(70, 106, !showHistory ? 68 : 1152, "AVAILABLE");

            AddButton(170, 104, showHistory ? 4006 : 4005, showHistory ? 4005 : 4007,
                11, GumpButtonType.Reply, 0);
            AddLabel(205, 106, showHistory ? 68 : 1152, "HISTORY");

            AddLabel(
                610, 106, 1152,
                String.Format("{0} record{1}", m_Records.Count, m_Records.Count == 1 ? "" : "s"));

            if (m_Records.Count == 0)
            {
                AddBackground(18, 142, 784, 330, 9200);

                AddHtml(
                    45, 230, 730, 80,
                    showHistory
                        ? "<CENTER><BASEFONT COLOR=#DDDDDD>No claimed destroyed-house records are available for this account.</BASEFONT></CENTER>"
                        : "<CENTER><BASEFONT COLOR=#DDDDDD>No unclaimed destroyed-house property is currently recorded for this account.</BASEFONT></CENTER>",
                    false, false);

                AddButton(350, 495, 4014, 4015, 0, GumpButtonType.Reply, 0);
                AddLabel(385, 497, 1152, "Close");
                return;
            }

            int start = m_Page * PerPage;
            int end = Math.Min(start + PerPage, m_Records.Count);
            int y = 145;

            for (int i = start; i < end; ++i)
            {
                WBReclamationRecord r = m_Records[i];

                AddBackground(24, y, 772, 72, 9200);

                string houseName = FriendlyHouseName(r.HouseType);
                string date = r.DestroyedUtc.ToString("MMM d, yyyy");
                string status = r.Claimed ? "CLAIMED" : "AVAILABLE";

                AddLabel(40, y + 10, 88, Truncate(r.OriginalOwnerName + " - " + houseName, 38));
                AddLabel(40, y + 34, 1152,
                    String.Format("{0}  {1},{2},{3}   Reclaimed: {4}",
                        r.MapName,
                        r.HouseLocation.X,
                        r.HouseLocation.Y,
                        r.HouseLocation.Z,
                        date));

                AddLabel(455, y + 10, r.Claimed ? 53 : 68, status);

                AddButton(
                    610, y + 12, 4005, 4006,
                    1000 + (i - start),
                    GumpButtonType.Reply, 0);
                AddLabel(645, y + 14, 1153, "Details");

                y += 78;
            }

            int maxPageCount = maxPage + 1;

            if (m_Page > 0)
            {
                AddButton(35, 505, 4014, 4015, 20, GumpButtonType.Reply, 0);
                AddLabel(70, 507, 1152, "Previous");
            }

            if (m_Page < maxPage)
            {
                AddButton(700, 505, 4005, 4006, 21, GumpButtonType.Reply, 0);
                AddLabel(735, 507, 1152, "Next");
            }

            AddLabel(
                360, 507, 1152,
                String.Format("Page {0} of {1}", m_Page + 1, maxPageCount));
        }

        private static string FriendlyHouseName(string fullName)
        {
            if (String.IsNullOrEmpty(fullName))
                return "(unknown house)";

            int dot = fullName.LastIndexOf('.');

            return dot >= 0 && dot < fullName.Length - 1
                ? fullName.Substring(dot + 1)
                : fullName;
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

            if (from == null)
                return;

            Account account = from.Account as Account;

            if (account == null ||
                !String.Equals(
                    account.Username,
                    m_AccountName,
                    StringComparison.OrdinalIgnoreCase))
                return;

            if (info.ButtonID == 10)
            {
                from.SendGump(
                    new WBReclamationGump(
                        from,
                        m_AccountName,
                        false,
                        0));
                return;
            }

            if (info.ButtonID == 11)
            {
                from.SendGump(
                    new WBReclamationGump(
                        from,
                        m_AccountName,
                        true,
                        0));
                return;
            }

            if (info.ButtonID == 20)
            {
                from.SendGump(
                    new WBReclamationGump(
                        from,
                        m_AccountName,
                        m_ShowHistory,
                        m_Page - 1));
                return;
            }

            if (info.ButtonID == 21)
            {
                from.SendGump(
                    new WBReclamationGump(
                        from,
                        m_AccountName,
                        m_ShowHistory,
                        m_Page + 1));
                return;
            }

            if (info.ButtonID >= 1000)
            {
                int local = info.ButtonID - 1000;
                int index = (m_Page * PerPage) + local;

                if (index < 0 || index >= m_Records.Count)
                    return;

                from.SendGump(
                    new WBReclamationDetailGump(
                        from,
                        m_AccountName,
                        m_Records[index],
                        m_ShowHistory,
                        m_Page));
            }
        }
    }

    public class WBReclamationDetailGump : Gump
    {
        private readonly string m_AccountName;
        private readonly WBReclamationRecord m_Record;
        private readonly bool m_ShowHistory;
        private readonly int m_ReturnPage;

        public WBReclamationDetailGump(
            Mobile from,
            string accountName,
            WBReclamationRecord record,
            bool showHistory,
            int returnPage)
            : base(150, 75)
        {
            m_AccountName = accountName;
            m_Record = record;
            m_ShowHistory = showHistory;
            m_ReturnPage = returnPage;

            AddPage(0);
            AddBackground(0, 0, 700, 500, 9270);
            AddBackground(18, 18, 664, 66, 9200);
            AddBackground(18, 96, 664, 300, 9200);
            AddBackground(18, 408, 664, 74, 9200);

            AddHtml(
                30, 31, 640, 28,
                "<CENTER><BASEFONT COLOR=#E8C468><BIG>RECLAMATION RECORD DETAILS</BIG></BASEFONT></CENTER>",
                false, false);

            AddHtml(
                30, 62, 640, 18,
                String.Format(
                    "<CENTER><BASEFONT COLOR=#DDDDDD>{0}</BASEFONT></CENTER>",
                    record != null && record.Claimed ? "CLAIMED PROPERTY" : "AVAILABLE FOR RECLAMATION"),
                false, false);

            int lx = 45;
            int vx = 225;
            int y = 116;

            AddLabel(lx, y, 88, "Original Owner:");
            AddLabel(vx, y, 1153, record.OriginalOwnerName); y += 30;

            AddLabel(lx, y, 88, "House:");
            AddLabel(vx, y, 1153, FriendlyHouseName(record.HouseType)); y += 30;

            AddLabel(lx, y, 88, "Location:");
            AddLabel(vx, y, 1153,
                String.Format("{0}  {1}, {2}, {3}",
                    record.MapName,
                    record.HouseLocation.X,
                    record.HouseLocation.Y,
                    record.HouseLocation.Z)); y += 30;

            AddLabel(lx, y, 88, "House Reclaimed:");
            AddLabel(vx, y, 1153, record.DestroyedUtc.ToString("MMM d, yyyy h:mm tt") + " UTC"); y += 30;

            AddLabel(lx, y, 88, "Property:");
            AddLabel(vx, y, 1153,
                record.ReclaimedItemCount > 0
                    ? record.ReclaimedItemCount.ToString("N0") + " items"
                    : "(count unavailable / empty)"); y += 30;

            AddLabel(lx, y, 88, "House Refund:");
            AddLabel(vx, y, 1153, record.RefundAmount.ToString("N0") + " gold"); y += 30;

            AddLabel(lx, y, 88, "Yard Refund:");
            AddLabel(vx, y, 1153, record.YardRefundAmount.ToString("N0") + " gold"); y += 30;

            AddLabel(lx, y, 88, "Yard Objects:");
            AddLabel(vx, y, 1153, record.YardObjectCount.ToString("N0")); y += 30;

            AddLabel(lx, y, 88, "Player Vendors:");
            AddLabel(vx, y, 1153, record.VendorCount.ToString("N0")); y += 30;

            AddLabel(lx, y, 88, "Record ID:");
            AddLabel(vx, y, 1153, record.Id);

            if (record.Claimed)
            {
                AddLabel(lx, 365, 53, "Claimed:");
                AddLabel(vx, 365, 1153,
                    record.ReclaimedUtc == DateTime.MinValue
                        ? "(unknown)"
                        : record.ReclaimedUtc.ToString("MMM d, yyyy h:mm tt") + " UTC");
            }

            AddButton(55, 430, 4014, 4015, 2, GumpButtonType.Reply, 0);
            AddLabel(90, 432, 1152, "Back");

            if (!record.Claimed)
            {
                AddButton(500, 430, 247, 248, 1, GumpButtonType.Reply, 0);
                AddHtml(
                    410, 458, 240, 20,
                    "<CENTER><BASEFONT COLOR=#55FF55>CLAIM PROPERTY</BASEFONT></CENTER>",
                    false, false);
            }
            else
            {
                AddHtml(
                    405, 432, 245, 22,
                    "<CENTER><BASEFONT COLOR=#F3C969>This reclamation has already been claimed.</BASEFONT></CENTER>",
                    false, false);
            }
        }

        private static string FriendlyHouseName(string fullName)
        {
            if (String.IsNullOrEmpty(fullName))
                return "(unknown house)";

            int dot = fullName.LastIndexOf('.');

            return dot >= 0 && dot < fullName.Length - 1
                ? fullName.Substring(dot + 1)
                : fullName;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (from == null)
                return;

            Account account = from.Account as Account;

            if (account == null ||
                !String.Equals(
                    account.Username,
                    m_AccountName,
                    StringComparison.OrdinalIgnoreCase))
                return;

            if (info.ButtonID == 2)
            {
                from.SendGump(
                    new WBReclamationGump(
                        from,
                        m_AccountName,
                        m_ShowHistory,
                        m_ReturnPage));
                return;
            }

            if (info.ButtonID != 1 ||
                m_Record == null ||
                m_Record.Claimed)
                return;

            if (m_Record.Storage == null ||
                m_Record.Storage.Deleted)
            {
                from.SendMessage(
                    33,
                    "That reclamation storage is no longer available. Please contact staff.");
                return;
            }

            if (from.BankBox == null)
            {
                from.SendMessage(33, "Your bank box is unavailable.");
                return;
            }

            m_Record.Storage.Movable = true;
            from.BankBox.AddItem(m_Record.Storage);
            m_Record.Claimed = true;
            m_Record.ReclaimedUtc = DateTime.UtcNow;

            from.SendMessage(
                68,
                "Your reclaimed property has been placed in your bank box.");

            from.SendGump(
                new WBReclamationDetailGump(
                    from,
                    m_AccountName,
                    m_Record,
                    true,
                    0));
        }
    }

}