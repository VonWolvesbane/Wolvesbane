WOLVESBANE TREASURE MAPS - PHASE 7E
HIGH-TIER LOOT: LESS CLUTTER, BETTER QUALITY
=============================================

This is a surgical replacement for:

Scripts/Custom/Edits to New Pub/TreasureMaps/TreasureMapInfo.cs

It is based directly on the WORKING Phase 7D file, so the restored purple-bag
special artifact rewards remain intact.

CHANGES
-------
Ordinary magic-equipment counts:

    Stash   6       unchanged
    Supply  8       unchanged
    Cache   12      unchanged (Assassin Cache remains 24)
    Hoard   18 -> 14
    Trove   36 -> 18

Party-member bonus rolls still work exactly as before.

Magic-item reforging budgets now progress by high tier:

    Stash/Supply    existing behavior retained
    Cache           500 - 900
    Hoard           650 - 1150
    Trove           800 - 1300

Previously Cache, Hoard and Trove all used the exact same 500-1300 budget.
That meant the highest tiers mostly increased QUANTITY, not the quality band.

WHY
---
The live chest tests showed:
- Cache felt reasonable.
- Hoard was noticeably more crowded.
- Trove contained a very large pile of ordinary gear.
- Phase 7D's special artifact (for example the tested Luna Lance) was exciting,
  but visually buried in ordinary equipment.

Phase 7E shifts Hoard/Trove toward fewer, stronger ordinary items while keeping:
- gold unchanged
- gems unchanged
- profession/package identity unchanged
- guardian difficulty unchanged
- Phase 7D special-artifact chances unchanged
- Artisan special rewards unchanged
- Remove Trap behavior unchanged

DEFENSIVE FIX
-------------
GetMinMaxBudget now safely handles a null item before calling
Imbuing.GetMaxWeight(). Normal valid loot behavior is unchanged.

TEST
----
After compile/restart:

    [WBTMapCreate Hoard Assassin
    [WBTMapCreate Trove Warrior

Compare with the screenshots from the previous tests.

Expected:
- Hoard equipment bag visibly less crowded than before.
- Trove roughly half the previous ordinary equipment count.
- Trove ordinary magic gear should trend stronger than Hoard.
- Purple Phase 7D artifact bag should still appear according to its existing
  chance (75% for Trove).
