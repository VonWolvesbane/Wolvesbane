WOLVESBANE TREASURE MAPS - PHASE 7C2
CORRECT CUSTOM-PATH LOOT FIX
======================================

DO NOT install the previous Phase 7C package.

This corrected package respects your custom treasure-system layout:

    Scripts/Custom/Edits to New Pub/TreasureMaps/TreasureMap.cs
    Scripts/Custom/Edits to New Pub/TreasureMaps/TreasureMapInfo.cs

The already-tested Remove Trap fix stays at:

    Scripts/Skills/RemoveTrap.cs

There is NO stock-path Scripts/Items/Misc/TreasureMapInfo.cs in this package.

TreasureMap.cs includes the Phase 7 NewWolvesbane guardian routing and
Griffin/Moose/Panda/WolvesbanianImp Trove guardian additions.

TreasureMapInfo.cs includes the Phase 7C loot compatibility changes:
- explicit NewWolvesbane TreasureFacet support
- classic Trammel/Felucca weapon and armor baseline for NewWolvesbane
- Mage Stash classic reagent handling
- safe Artisan Supply material handling for the custom facet

RemoveTrap.cs includes the working Phase 7B Trove disarm difficulty fix.

INSTALL:
Back up the three matching files first, then replace using the exact paths
shown above. Compile/restart.

TEST:
1. NewWolvesbane Mage Stash: verify reagent loot.
2. NewWolvesbane Artisan Supply: verify it generates without an index error.
3. NewWolvesbane Trove: verify custom guardians still appear.
4. Trove at 120 Remove Trap: verify the Phase 7B behavior is still working.

The commented-out high-value special/decorative reward block remains disabled
for now; it will be handled separately after its referenced classes are
verified against the Wolvesbane tree.
