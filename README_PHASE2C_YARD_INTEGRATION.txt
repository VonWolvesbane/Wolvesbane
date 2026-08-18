WOLVESBANE HOUSING RECLAMATION - PHASE 2C YARD INTEGRATION
============================================================

This replaces the previous Phase 2B file.
Keep the validated Phase 1 file installed.
ACC Yard System must remain installed.

Why this exists
---------------
ACC Yard System marks YardItem/YardGate objects as orphaned during a world
save if their House reference is null/deleted. Its normal orphan cleanup then
calls Refund(), which refunds the yard object's Price to the placer and deletes
the object.

After our first house-reclamation tests, the server correctly printed:
    Cleaning 9 Orphaned Yard Items...

Phase 2C integrates that behavior directly into house reclamation instead.

New behavior
------------
BEFORE vendors, MoveAllToCrate(), refund, and house.Delete():

1. Scan World.Items for:
   - Server.ACC.YS.YardItem with YardItem.House == target house
   - Server.ACC.YS.YardGate with YardGate.House == target house
2. Create:
      Yard Item Refunds
   inside the account reclamation crate.
3. For each yard object with Price > 0:
   create a BankCheck for the same refund value.
   Check name:
      Yard refund - <original yard object name>
4. Delete the yard object before the house is deleted.
   YardItem.OnAfterDelete() also deletes its child YardPiece objects.
5. The later world save should therefore NOT see those objects as new orphans.

Important
---------
This preserves the ACC Yard System's ECONOMIC behavior. ACC does not return
the original yard decoration/deed when Refund() is called; it refunds Price
as currency and deletes the placed yard object. Phase 2C keeps that same
value, but puts the refunds into the permanent reclamation storage instead
of directly depositing them during a later save.

Also retained from Phase 2B
---------------------------
- Organized numbered house-item bags (~100 direct items per bag)
- Vendor Held Items - <vendor name>
- vendor held-gold preservation
- Destroyed House Refund bank container/check
- account-wide reclamation ownership
- permanent records / no expiration
- house-sign targeting
- individual Administrator approval only
- no batch mode yet

Validation test
---------------
Choose ONE candidate house with known ACC yard decorations:
1. Count/identify visible yard objects.
2. [WBHousingApproveTest and target house sign.
3. Confirmation gump should show Yard objects: N.
4. Approve/reclaim.
5. Claim property from Reclamation Officer.
6. Verify "Yard Item Refunds" exists and contains checks.
7. Save world.
8. Confirm there is NO new "Cleaning N Orphaned Yard Items..." message for
   the destroyed house.
