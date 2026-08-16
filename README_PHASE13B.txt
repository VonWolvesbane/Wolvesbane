WOLVESBANE SAVE OPTIMIZATION - PHASE 13B
===========================================

Purpose
-------
Make MasterItemStoreKey intrinsically death-safe while preserving its stores,
child-key types, location, parent, insurance flag, and stored resources.

Changes
-------
1. Newly constructed MasterItemStoreKey objects are Blessed.
2. Loaded MasterItemStoreKey objects are set Blessed after their existing
   version-0 data is read.
3. All Phase 13 diagnostics remain.
4. WBMasterKeyProtection provides guarded preview/confirm conversion for any
   currently loaded non-Blessed keys.

Save format
-----------
UNCHANGED. Version remains 0. No new fields are written.

Install on TEST server
----------------------
1. Stop the server.
2. Back up Saves and your current MasterKey.cs.
3. Replace MasterKey.cs with the Phase 13B version.
4. Add Scripts/Custom/Wolvesbane/WBMasterKeyProtection.cs.
5. Compile/restart normally.
6. Watch console for WB MASTERKEY LOAD ERROR or WB MASTERKEY INTEGRITY.
7. Run [WBMasterKeyAudit.
8. Run [WBMasterKeyProtection preview.

Important
---------
Because Deserialize now enforces Blessed, after a restart the protection
preview may report 0 candidates. That is expected. The audit should show
all MasterItemStoreKeys as Blessed/Newbied and 0 Regular.

The protection command only changes LootType to Blessed. It does not move,
delete, recreate, insure, or alter store contents.

Rollback
--------
Rollback/MasterKey_Phase13.cs contains the prior Phase 13 version.
