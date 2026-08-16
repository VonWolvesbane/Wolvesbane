WOLVESBANE SAVE OPTIMIZATION - PHASE 15B
===========================================

Purpose
-------
Remove Phase 15's duplicate warm-save recipe scan while preserving the same
Phase 6 compact OWLTR save format and all safety checks.

Phase 15 warm saves did:
  1. scan every holder recipe to validate the cached catalog
  2. scan every holder recipe again to build bitsets

Phase 15B does both jobs in ONE traversal.

Safety behavior
---------------
If the one-pass traversal encounters a recipe Type that is not in the cached
catalog, it immediately abandons the prepared data, rebuilds the shared catalog,
then prepares holders once against that rebuilt catalog.

No partial holder data is written against a stale catalog.

Save format
-----------
UNCHANGED:
  OWLTR version 1
  same sorted shared recipe-name catalog
  same compact holder bitsets
  same resource lists

Files
-----
Replace:
  Scripts/Custom/New Systems/OWLTR/New/Daat99 OWLTR Control Center.cs
  Scripts/Custom/New Systems/OWLTR/New/Daat99 Holder.cs

The packaged Control Center also includes the explicit ArrayList cast fix for
the CS0266 error encountered during Phase 15 installation.

Test
----
1. Stop TEST server.
2. Replace the two files.
3. Compile/restart.
4. Run [WBOWLTRAudit and verify the existing counts.
5. Make one cold save.
6. Make 3 warm saves without restarting.

Console timing now reports:
  cacheValidate+Prepare
  catalogRebuild
  prepareAfterRebuild
  catalogWrite
  holdersWrite

Expected:
  First save after restart:
    rebuilt=yes
    catalogRebuild > 0
    prepareAfterRebuild > 0

  Warm saves:
    rebuilt=no
    catalogRebuild = 0
    prepareAfterRebuild = 0
    cacheValidate+Prepare should replace the two ~60-70ms passes from Phase 15.

7. Restart again and run [WBOWLTRAudit to verify data integrity.

Rollback contains the Phase 15 files.
