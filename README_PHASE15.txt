WOLVESBANE SAVE OPTIMIZATION - PHASE 15
==========================================

PURPOSE
-------
Reduce Data99OWLTR save CPU time without changing any player data or the
Phase 6 compact save format.

Phase 14 measured roughly:
  catalog build: ~151 ms
  holder bitset build: ~161 ms
out of ~322 ms total for the one Data99OWLTR object.

OPTIMIZATION
------------
1. The shared recipe catalog is cached between saves.
2. The cache is keyed by actual System.Type instead of Type.ToString().
3. Before every save, the cache is validated against every holder.
4. If any genuinely new recipe Type appears, the catalog is rebuilt in stable,
   sorted order BEFORE any OWLTR data is written.
5. Holder bitsets are prepared in one pass using Type-keyed lookups.
6. SerializeCompactPrepared writes those already-built bitsets.

SAVE FORMAT
-----------
UNCHANGED.

The file still writes:
  OWLTR version 1
  the same shared sorted recipe-name catalog
  the same holder version 1 bitsets
  the same resource lists

The existing Phase 6 deserializer remains compatible.

FIRST SAVE VS LATER SAVES
-------------------------
The first save after restart will normally say:
  rebuilt=yes

because the in-memory cache starts empty.

Later saves should normally say:
  rebuilt=no

and should be substantially faster.

If a new recipe Type appears at runtime, the next save should safely detect it
and show rebuilt=yes.

INSTALL
-------
On the TEST server replace only:
  Scripts/Custom/New Systems/OWLTR/New/Daat99 OWLTR Control Center.cs
  Scripts/Custom/New Systems/OWLTR/New/Daat99 Holder.cs

Compile/restart normally.

TEST
----
1. Run [WBOWLTRAudit before saving and record:
     holders
     recipe entries
     resource entries
     distinct recipe types
2. Make one save. This is the cold-cache save.
3. Make 3 more saves without restarting.
4. Watch console for:
     WB OWLTR PROFILE
     WB OWLTR COUNTS
     WB OWLTR HOLDERS
5. Restart the server.
6. Run [WBOWLTRAudit again and verify the same holder/recipe/resource counts.

EXPECTED
--------
Cold first save:
  catalogCheck/Rebuild may still be significant
  rebuilt=yes

Warm subsequent saves:
  rebuilt=no
  holderPrepare should be far below Phase 14's ~160 ms bitsetBuild
  holdersWrite should be only the bitset/resource writer cost
  Data99OWLTR total should drop substantially.

SAFETY
------
No objects are deleted or moved.
No holder recipes/resources are changed.
No serialization version is changed.

Rollback contains the Phase 14 files.
