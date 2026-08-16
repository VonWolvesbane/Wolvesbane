WOLVESBANE SAVE OPTIMIZATION - PHASE 6
OWLTR COMPACT HOLDER SERIALIZATION
======================================

PURPOSE
-------
The live Wolvesbane OWLTR control item is ~21.7 MiB. Phase 5 measured:
  * 678 valid player holders
  * 771,580 recipe entries
  * only 1,203 distinct recipe type names
  * ~20.05 MiB of repeated recipe-name text

The old format writes each recipe's full type name separately for every holder.
This patch writes each recipe type name once in a shared catalog, then stores each
holder's recipe membership as a compact 32-bit bit-set.

WHAT CHANGES
------------
1. Daat99 OWLTR Control Center.cs
   * Control-item serialization version changes from 0 to 1.
   * Version 0 remains fully readable, so your EXISTING save loads normally.
   * Version 1 builds one shared sorted recipe catalog for the shard.
   * Each holder is written against that catalog.

2. Daat99 Holder.cs
   * The original version-0 holder Serialize/Deserialize remains intact for
     backward loading.
   * Adds SerializeCompact() and a compact reader used only by OWLTR format v1.
   * Recipe membership is packed 32 recipes per Int32.
   * NextReward, ItemsCrafted, and Resources are still serialized normally.

EXPECTED SIZE
-------------
With the Phase 5 numbers, 1,203 recipe flags require 38 Int32 words per holder.
For 678 holders that is ~103 KiB of recipe bit data, plus one ~30 KiB shared
catalog, instead of ~20 MiB of repeated recipe-name strings.

CRITICAL BACKUP NOTE
--------------------
MAKE A COPY OF THE TEST SERVER'S PRE-PHASE-6 SAVE BEFORE THE FIRST SAVE WITH THIS
CODE INSTALLED.

The new code reads both format 0 and format 1. The OLD OWLTR source only knows
format 0, so after a save has been written in format 1 you must keep this Phase 6
code (or restore the pre-migration save if rolling back).

TEST PROCEDURE
--------------
Use a TEST SERVER first.

1. Before installing Phase 6, retain a full backup of the current Saves folder.

2. Record the current Phase 5 audit numbers. From the test already performed:
     Static holders:          678
     Valid holders:           678
     Recipe entries:          771,580
     Resource entries:        28,476
     Distinct recipe types:   1,203

3. Replace ONLY these two files with the Phase 6 versions:
     Scripts/Custom/New Systems/OWLTR/New/Daat99 Holder.cs
     Scripts/Custom/New Systems/OWLTR/New/Daat99 OWLTR Control Center.cs

4. Start the test server. This startup is reading the OLD version-0 save.
   Run:
     [WBOWLTRAudit
   The holder/recipe/resource counts should still match the baseline.

5. Manually save the world once. This writes OWLTR version 1.

6. IMPORTANT: Restart the test server. This startup tests the NEW version-1
   deserializer.

7. Run again:
     [WBOWLTRAudit
   Confirm these are unchanged from the baseline:
     * valid holder count
     * total recipe entries
     * total resource entries
     * distinct recipe types

8. Test normal OWLTR behavior on a test character:
     * open OWLTR
     * check MissingRecipes / holder display
     * perform a recipe-related action if convenient
     * log out/in once

9. Perform 3-5 manual world saves and record the times.

DO NOT CONTINUE TO PRODUCTION unless the post-restart audit counts match the
pre-migration counts.

ROLLBACK
--------
If the FIRST startup with Phase 6 fails before a new save is written, restore the
old two scripts and restart; the save is still version 0.

If you have already SAVED with Phase 6, do not put the old OWLTR source back on
that version-1 save. Restore BOTH:
  * the old OWLTR source files, AND
  * the pre-Phase-6 Saves backup.

NOTES
-----
This phase does not delete holders, recipes, resources, mobiles, or items. It is
only a serialization format change.

This package does not replace the Phase 4 MasterStorageUtils leak fix. Keep the
Phase 4 MasterStorageUtils.cs already installed.
