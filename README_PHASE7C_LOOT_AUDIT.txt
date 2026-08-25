WOLVESBANE TREASURE MAPS - PHASE 7C
LOOT / CHEST AUDIT + NEW WOLVESBANE COMPATIBILITY
==================================================

AUDIT FINDINGS
--------------
GOOD:
- Five tiers are present: Stash, Supply, Cache, Hoard, Trove.
- Five packages are present: Artisan, Assassin, Mage, Ranger, Warrior.
- Equipment quantity scales 6 / 8 / 12 (24 Assassin) / 18 / 36.
- Gold scales upward by tier.
- Gem quantities scale by both tier and chest quality.
- Chest quality affects refinements, reagents, gems, and special resource amounts.
- New-system lock/trap values scale by tier.
- Phase 7B separately fixes Remove Trap compatibility.

BUG FIXED HERE:
TreasureFacet originally contained only the seven official facets. NewWolvesbane
was therefore either treated as Trammel by GetFacet(), or (after adding an enum
entry) could index outside _SpecialMaterialTable for Artisan Supply maps.

Phase 7C explicitly adds/handles NewWolvesbane and deliberately uses the classic
Trammel/Felucca loot profile for:
- weapons
- armor
- Mage Stash reagents
- Artisan Supply special-material table

This gives NewWolvesbane a safe, predictable loot baseline without altering the
official facets.

IMPORTANT AUDIT FINDING - NOT AUTO-ENABLED
------------------------------------------
The source's high-value "Special Loot" creation block inside Fill() is commented
out. GetSpecialLootList() still calculates intended lists/chances, but the item
construction/drop code is disabled. The high-tier decorative table assignment is
also commented out.

I did NOT blindly uncomment those blocks in this phase because this Wolvesbane
codebase is older/custom and the disabled code references item systems/types that
must be verified before restoring them. Enabling it without that verification is
exactly the sort of shoehorned-new-system change that can cause compile/runtime
problems.

CURRENT SPECIAL-LOOT ROLL CHANCES IN SOURCE:
Supply 10%
Cache  20%
Hoard  50%
Trove  75%
But because the creation/drop body is commented, those successful rolls currently
drop nothing from that special-loot block.

RECOMMENDED NEXT PHASE
----------------------
Phase 7D should verify the referenced special/decorative item classes against the
full Wolvesbane Scripts tree and restore only the classes that actually exist.
That will make the five profession packages more rewarding/distinct without
introducing missing-type compile failures.

TEST PHASE 7C
-------------
1. Compile/restart.
2. Test NewWolvesbane Mage Stash: verify a reagent bag is present.
3. Test NewWolvesbane Artisan Supply: verify chest generation completes without
   an index/out-of-range error and contains the expected crafting resources.
4. Test one map of each package at Cache/Hoard/Trove and confirm equipment is
   package-appropriate.
5. Confirm Trammel/Felucca/etc. behavior is unchanged.
