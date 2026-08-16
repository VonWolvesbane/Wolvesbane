WOLVESBANE SAVE OPTIMIZATION - PHASE 12
==========================================

PURPOSE
-------
Profile the Item serialization stage by exact runtime Item type.

Phase 11R showed that Items dominate the remaining save time.
Phase 12 measures, for every Item type:
  - object count
  - total bytes written to Items.bin
  - total time spent inside item.Serialize()
  - average Serialize() time per object
  - percentage of measured Item Serialize() time

It then ranks the top 30 types by total Serialize() time.

SAFETY
------
This changes NO world objects and NO save-data format.
It only adds Stopwatch timing and reporting.

IMPORTANT PROFILER OVERHEAD
---------------------------
Phase 12 calls Stopwatch.GetTimestamp() around every Item.Serialize().
With roughly 700,000 Items this adds some overhead.

DO NOT treat the total save time while Phase 12 is installed as the new
production benchmark. Use the per-type rankings to find bottlenecks.
After diagnosis, roll back to Phase 11R (or the optimized replacement)
before measuring final save performance.

INSTALL
-------
Phase 11R must already be installed, including its World.cs profiler code.

1. Stop the TEST server.
2. Back up ServUO.exe.
3. Replace ONLY:
     Server\Persistence\StandardSaveStrategy.cs
   with the Phase 12 version in this package.
4. Run:
     Compile.WIN - Release.bat
5. Continue only if the build succeeds.
6. Start the rebuilt ServUO.exe.

There are NO new Scripts files and NO in-game command.

TEST
----
Perform ONE normal manual save first.

The console will print:
  WB ITEM PROFILE: ...
  WB ITEM #1: ...
  WB ITEM #2: ...
  ...
  WB ITEM #30: ...

Administrators in-game will see only the compact top 8.

For a more reliable ranking, perform 3 saves. The exact milliseconds will
vary, but the same major types should stay near the top.

WHAT TO SEND BACK
-----------------
Best:
  screenshots/copy of the server console containing:
    WB ITEM PROFILE
    WB ITEM #1 through #30

If the console is inconvenient, screenshots of the in-game top 8 from
three saves are still useful.

HOW TO READ IT
--------------
Example:
  WB ITEM #1: Server.Items.SomeItem
    count=5,000
    serialize=400ms
    avg=80us
    data=12MB

That tells us whether a class is expensive because:
  A) there are huge numbers of cheap objects,
  B) each object's Serialize() is unusually expensive,
  C) it writes a huge amount of data,
  or a combination.

The profile also prints:
  Items stage
  measured Serialize()
  other/index/writer/cache~

That separation tells us whether the bottleneck is mostly Item.Serialize()
logic or the generic persistence/writer/index machinery.

ROLLBACK
--------
The Rollback folder contains the Phase 11R StandardSaveStrategy.cs.

To remove Phase 12 instrumentation:
  1. Stop server.
  2. Restore Rollback\Server\Persistence\StandardSaveStrategy.cs
  3. Rebuild with Compile.WIN - Release.bat
  4. Restart.
