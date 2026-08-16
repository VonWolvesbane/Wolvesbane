WOLVESBANE SAVE OPTIMIZATION - PHASE 13
==========================================

PURPOSE
-------
Investigate MasterItemStoreKey performance AND disappearing-key stability reports.

This phase:
  1. Adds per-master-key / per-child-store serialization timing.
  2. Adds a read-only in-game integrity/location audit.
  3. Stops silently swallowing child-key synchronization exceptions during load.
     It LOGS those exceptions; it does not auto-repair player data.
  4. Detects null/missing key types and store/type count mismatches before they
     can be silently written into a bad save.

SAVE FORMAT
-----------
UNCHANGED. Version remains 0. No new fields are written.

FILES
-----
REPLACE the existing MasterKey.cs with the Phase 13 MasterKey.cs.
The exact path depends on where this file currently lives in your
Universal Storage Keys / ItemStore system.

ADD:
  Scripts\Custom\Wolvesbane\WBMasterKeyAudit.cs

A copy of the original uploaded MasterKey.cs is in Rollback.

IMPORTANT
---------
Phase 12's core item profiler can remain installed while testing Phase 13.
Phase 13 itself is Scripts-side only, so no ServUO core file changes are needed.

BUILD / START
-------------
1. Stop TEST server.
2. Back up Saves and the current MasterKey.cs.
3. Replace MasterKey.cs.
4. Add WBMasterKeyAudit.cs.
5. Compile/restart normally.
6. WATCH THE CONSOLE DURING LOAD for:
     WB MASTERKEY LOAD ERROR
     WB MASTERKEY INTEGRITY

If ANY load/integrity errors appear, screenshot/copy them before doing anything else.

AUDIT
-----
Run:
  [WBMasterKeyAudit

Then:
  [WBMasterKeyAudit verbose

The audit reports:
  - master-key count
  - total child stores / StoreEntries
  - backpack/bank/other/world/Internal location
  - Insured / Blessed / Regular loot protection state
  - mismatched Stores vs KeyTypes
  - null stores
  - unresolved/null key types
  - incorrect ItemStore.Owner references
  - duplicate child-key types
  - serial + owner + location in verbose mode

It changes NOTHING.

PERFORMANCE
-----------
Make one normal manual save.

Console lines beginning:
  WB MASTERKEY PROFILE:
  WB MASTERKEY STORE:

show which individual master keys and child key types are consuming the time.
Only master keys >= 2ms total and child stores >= 1ms are printed to keep the
output manageable.

STABILITY NOTES
---------------
The original MasterKey.cs had two risky patterns:

1. Deserialize used:
     ScriptCompiler.FindTypeByName(...)
   and then silently swallowed exceptions from Activator.CreateInstance /
   SynchronizeStore.

2. Serialize assumes Stores and KeyTypes are perfectly synchronized and
   dereferences KeyTypes[index].Name.

Phase 13 exposes those failures instead of hiding them. It does NOT automatically
repair a damaged master key because an incorrect repair could destroy or
mis-associate player storage.

The audit also reports Regular-loot / uninsured keys. CanUse() requires the key
to be in the backpack, but the class itself does not make the master key
intrinsically death-safe. Whether that explains player reports depends on the
current shard death/insurance rules and will be evaluated from the audit.

WHAT TO SEND BACK
-----------------
1. Any WB MASTERKEY LOAD ERROR / INTEGRITY console messages during startup.
2. Screenshot of [WBMasterKeyAudit
3. Screenshot(s) of [WBMasterKeyAudit verbose
4. Console output from one save containing WB MASTERKEY PROFILE / STORE lines.
