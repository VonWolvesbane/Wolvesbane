WOLVESBANE SAVE OPTIMIZATION - PHASE 10
=========================================

Finding
-------
The Phase 9 Gold fingerprint traces back to the SAME OWLTR/MasterStorage
materialization leak fixed in Phase 4.

Lady Luck:
  Scripts/Custom/New Systems/OWLTR/Tokens/Lady Luck.cs

uses:
  MasterStorageUtils.TakeGoldFromPlayer(...)

OLD behavior:
  TakeGoldFromPlayer -> TakeTypeFromPlayer -> TryExtractType
  -> CreateItemsFromType

CreateItemsFromType split stackable items into chunks with:
  Math.Min(amount, 60000)

The caller wanted to CONSUME the MasterStorage balance, not physically
withdraw Gold. The returned Item list was discarded, leaving the created
Gold registered in World.Items on Map.Internal at (0,0,0).

This explains:
  - the huge population of exact Amount=60,000 Gold objects
  - smaller remainder amounts
  - Amount=1 objects caused by the separate throwaway probe Item leak

Phase 4 already changed TakeTypeFromPlayer to TryConsume and removed the
throwaway probe, so the source leak is already fixed on the test shard.

IMPORTANT VERIFICATION BEFORE CLEANUP
-------------------------------------
Run:
  [WBGoldAudit

Record the Internal (0,0,0) count.

Then use Lady Luck to BUY tokens using Gold from Master Storage.
Use a transaction costing more than 60,000 gold if practical.

Run:
  [WBGoldAudit

again.

The Internal (0,0,0) Gold count should NOT increase. If it increases,
STOP and do not run the cleanup.

Cleanup
-------
[WBGoldCleanup preview

Expected from the Phase 9 audit:
  approximately 40,649 candidates
  approximately 2,200,343,542 represented gold

The cleanup fingerprint is intentionally stricter than location alone:
  exact Server.Items.Gold
  Parent == null
  Map == Map.Internal
  Location == (0,0,0)
  Stackable && Amount > 0
  Hue == 0
  Name == null
  LootType == Regular
  Movable
  Visible

Nothing is deleted during preview.

If the preview matches the audit:
  [WBGoldCleanup confirm

Confirmation requires:
  - the same administrator
  - preview within the last 10 minutes
  - BOTH candidate count and represented amount unchanged
  - each object passing the fingerprint again immediately before Delete()

No automatic save is performed.

After cleanup
-------------
1. Run [WBGoldAudit
2. Manually save 3-5 times.
3. Record the save times.
4. Run [WBWorldAudit suspicious 30 again.

TEST SERVER FIRST.
