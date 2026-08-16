WOLVESBANE SAVE OPTIMIZATION - PHASE 11
=========================================

PURPOSE
-------
Measure where the remaining world-save time is actually spent.

This phase does NOT delete, move, or alter world objects.
It adds Stopwatch timing instrumentation to the existing save path.

FILES
-----
REPLACE on the TEST server:
  Server/World.cs
  Server/Persistence/StandardSaveStrategy.cs

ADD:
  Scripts/Custom/Wolvesbane/WBSaveProfileCommand.cs

IMPORTANT
---------
Back up the two original Server source files before replacing them.

The patched files were made from the exact Server source in the Wolvesbane.rar
archive supplied for this project.

COMMANDS
--------
[WBSaveProfile status
  Shows whether profiling is enabled and which SaveStrategy your server selects.

[WBSaveProfile on
  Enables timing output.

[WBSaveProfile off
  Disables timing output.

TEST
----
1. Install these files on the TEST server.
2. Restart/recompile.
3. Run:
     [WBSaveProfile status
4. Run:
     [WBSaveProfile on
5. Perform 5 normal manual world saves.
6. Send screenshots of the WB Profile / WB Stages lines.

WHAT IS MEASURED
----------------
World.Save:
  - BeforeWorldSave event
  - NetState Flush/Pause
  - waiting for a previous background disk write
  - selected SaveStrategy total
  - WorldSave event
  - the same displayed world-save stopwatch you already see

Standard / Dual strategy:
  - Items
  - Mobiles
  - Guilds
  - CustomsFramework SaveData

DUAL SAVE NOTE
--------------
On Wolvesbane's Dual strategy, Items serialize on a worker thread while
Mobiles/Guilds/CustomData serialize on the main save thread.

Therefore:
  Item time + Mobile time + Guild time + Custom time
can be GREATER than the Strategy total.

That is normal because the work overlaps. The longest concurrent branch is
usually what determines the overall strategy time.

PARALLEL / DYNAMIC NOTE
-----------------------
Phase 11 always measures total SaveStrategy time. The detailed Item/Mobile
sub-stage timers are implemented in StandardSaveStrategy and therefore cover
Standard and Dual (Dual inherits those methods).

If [WBSaveProfile status] reports Parallel or Dynamic, send that result;
the total/core event timings are still useful and we can add strategy-specific
per-category instrumentation next.

SAFETY
------
No serialization format changes are made.
No cleanup is performed.
Profiling is OFF after every server restart until an Administrator enables it.
