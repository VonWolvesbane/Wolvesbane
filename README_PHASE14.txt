WOLVESBANE SAVE OPTIMIZATION - PHASE 14
==========================================

PURPOSE
-------
Profile the single Daat99OWLTR persistent control object's compact serialization
without changing the OWLTR save format or player data.

Phase 12 repeatedly measured this ONE object at roughly 160-300ms.

FILES TO REPLACE ON TEST SERVER
-------------------------------
Scripts\Custom\New Systems\OWLTR\New\Daat99 OWLTR Control Center.cs
Scripts\Custom\New Systems\OWLTR\New\Daat99 Holder.cs

These files are based on the Wolvesbane Phase 6 compact OWLTR implementation
already tested for save/restart round-trip integrity.

SAVE FORMAT
-----------
UNCHANGED. Daat99OWLTR format remains version 1 and compact holder format remains
version 1. No new data is written. This phase adds timing/counter code only.

TEST
----
1. Stop TEST server.
2. Back up Saves and the two current OWLTR files.
3. Replace the two files above.
4. Compile/restart normally.
5. Make ONE normal manual world save.
6. Send the console lines beginning:
     WB OWLTR PROFILE:
     WB OWLTR COUNTS:
     WB OWLTR HOLDERS:

WHAT IS MEASURED
----------------
WB OWLTR PROFILE breaks total Serialize() into:
  base          Item base serialization
  header        OWLTR version + Deletable flag
  options       OWLTROptionsManager serialization
  tempSync      online TempHolders -> StaticHolders synchronization
  serialTable   filtering/rebuilding the serializable holder table
  catalogBuild  scanning all holder recipe lists, de-duplicating recipe names,
                sorting the shared catalog, and building its lookup table
  catalogWrite  writing the shared recipe names once
  holders       writing all compact holder records

WB OWLTR HOLDERS further breaks compact holder work into:
  bitsetBuild    scanning each holder's recipe Type list and looking up each type
                 in the shared catalog
  bitsetWrite    writing the compact recipe bitset
  resourcesWrite writing resource arrays

It also reports the number of holder calls, recipe membership checks, resource
values, and the slowest single holder observed.

PROFILER OVERHEAD
-----------------
This adds Stopwatch timing around OWLTR sections and each of ~678 holders. The
overhead is small compared with Phase 12's per-item timing, but use the output
for diagnosis rather than final production benchmarking.

ROLLBACK
--------
Rollback contains the prior Phase 6 versions of both files.
