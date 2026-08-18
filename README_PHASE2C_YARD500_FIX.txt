WOLVESBANE HOUSING RECLAMATION - PHASE 2C YARD 500 REFUND + ENUMERATION FIX
============================================================================

This replaces the previous Phase 2C file.
Keep the validated Phase 1 file installed.

Changes
-------
1. Yard refunds are now exactly 500 gold per top-level placed YardItem/YardGate.
2. Internal YardPiece children are NOT counted or refunded separately.
3. World.Items is snapshotted before yard scanning.
4. Refund check creation and YardItem/YardGate deletion happen only AFTER the
   World.Items enumeration has completed.

This fixes:
    InvalidOperationException:
    Collection was modified; enumeration operation may not execute.

Refund example
--------------
9 placed yard objects = 4,500 gold total, represented as 9 x 500-gold checks
inside:
    Yard Item Refunds

Each check is named:
    Yard refund - <yard object name>
when the yard object has a name.

Unchanged
---------
- 30-day eligibility
- Phase 1 defer/exempt logic
- house-sign targeting
- individual Administrator approval
- organized reclamation sub-bags
- vendor item/gold handling
- Destroyed House Refund
- account-wide reclamation ownership
- permanent records / no expiration
- no batch destruction yet

Validation
----------
Use ONE candidate house with known yard decorations:
1. [WBHousingApproveTest
2. Target house sign.
3. Confirm Yard objects count.
4. Approve/reclaim.
5. Claim property from Reclamation Officer.
6. Verify Yard Item Refunds.
7. Verify each top-level yard object produced exactly one 500-gold check.
8. Save world.
9. Confirm no new orphan-yard cleanup message appears for that destroyed house.
