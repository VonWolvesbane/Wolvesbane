WOLVESBANE SAVE OPTIMIZATION - PHASE 4
OWLTR MASTER STORAGE TOKEN LEAK FIX + GUARDED HISTORICAL CLEANUP

WHAT WAS FOUND
==============
Two bugs in OWLTR MasterStorage explain the parentless Map.Internal Daat99Tokens population:

1) MasterStorageUtils.TakeTypeFromPlayer()
   The old code called backpack.TryExtractType(type, amount).
   TryExtractType first deducts the virtual MasterStorage ledger balance, then creates REAL Item objects
   representing the extracted amount. TakeTypeFromPlayer never used or deleted those returned Items.
   Result: every consume/take operation could leave materialized Items parentless on Map.Internal.

2) MasterStorageUtils.CreateItemsFromType()
   The old code created an Item once before the while loop, then immediately replaced that variable with
   another newly created Item inside the loop. The first Item was registered in World.Items but was never
   returned or deleted. Result: one extra orphan Item per extraction, commonly Amount=1.

The Phase 4 MasterStorageUtils.cs fixes both causes:
- TakeTypeFromPlayer uses TryConsume() because this method is consumption, not withdrawal/materialization.
- CreateItemsFromType no longer constructs the unused probe Item.

IMPORTANT: TryExtractType() is NOT removed or changed. It is still used where real objects are actually
needed, such as withdrawing gold/tokens into the player's backpack or extracting stored items.

INSTALL ON TEST SERVER FIRST
============================
1. Back up the entire test server and Saves folder.
2. Replace your existing:
   Scripts/Custom/New Systems/OWLTR/MasterStorage/MasterStorageUtils.cs
   with the patched copy included here.
3. Add:
   Scripts/Custom/Wolvesbane/WorldCleanup/WBTokenCleanup.cs
4. Keep WBTokenAudit.cs from Phase 3 installed.
5. Restart/recompile the test shard.

TEST THE LEAK FIX BEFORE CLEANUP
================================
Before deleting historical objects, perform a small functional test:
- Run [WBTokenAudit and record the Internal/no-parent number.
- Use a normal token-spending action that calls TakePlayerTokens (buy/spend tokens).
- Run [WBTokenAudit again.
The Internal/no-parent count should NOT increase from that token spend after this patch.

CLEANUP PROCEDURE
=================
Run:
  [WBTokenCleanup preview

The strict candidate count should be close to the 1,052,535 parentless/Internal tokens previously audited.
The cleanup targets ONLY objects that meet ALL of these conditions:
- exact type Daat99Tokens
- not deleted
- Parent == null
- Map == Map.Internal
- Location == (0,0,0)
- Stackable == true
- Amount > 0

Nothing is deleted by preview.

If the numbers are expected, within 10 minutes run:
  [WBTokenCleanup confirm

Safety guards:
- same administrator must preview first
- preview expires after 10 minutes
- both candidate object count AND represented token Amount must still match
- candidate list is snapshotted before deletion
- every object is revalidated immediately before Delete()
- no automatic world save

AFTER CLEANUP
=============
1. Run [WBTokenAudit again.
2. Confirm legitimate tokens in banks/backpacks/containers still exist.
3. Manually save the world and record the save time.
4. Restart the TEST shard from that save and verify it loads cleanly.
5. Test spending and withdrawing OWLTR tokens.

DO NOT move this to the live shard until the test-save restart succeeds.

OWLTR CONTROL OBJECT NOTE
=========================
The large Daat99OWLTR control object is a separate issue. Its static holder table serializes a
NewDaat99Holder for each remembered Mobile, including recipe/resource lists. Phase 4 intentionally does
NOT alter that persistence format. We can optimize/prune it as a later phase after the million-item leak
is removed and measured.
