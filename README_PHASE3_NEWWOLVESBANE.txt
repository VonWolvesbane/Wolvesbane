WOLVESBANE TREASURE MAPS - PHASE 3: NEWWOLVESBANE FACET
========================================================

This package is cumulative: it includes Phase 1 + Phase 2 fixes.

WHY THIS PATCH EXISTS
---------------------
Map.NewWolvesbane is registered in Wolvesbane as:
    6144 x 4096
    MapRules.TrammelRules

The newer treasure system did not know this facet existed.

TreasureMapInfo.GetFacet() therefore fell through to Trammel, which caused:
- maps on NewWolvesbane to display "For Somewhere In Trammel"
- NewWolvesbane to receive Trammel treasure identity
- no dedicated safe treasure-location source
- random fallback coordinates that could be technically walkable but
  inaccessible to players

WHAT CHANGED
------------
1. Added TreasureFacet.NewWolvesbane.

2. TreasureMapInfo.GetFacet() now recognizes Map.NewWolvesbane.

3. NewWolvesbane uses classic/Trammel equipment and reagent profiles, while
   remaining a distinct facet for map identity and location handling.

4. Supply Artisan special-material lookup safely maps NewWolvesbane to the
   Trammel material slot instead of indexing beyond the stock 7-facet table.

5. Treasure map properties now display:
       For Somewhere In New Wolvesbane

6. NewWolvesbane map bounds use the actual custom map dimensions:
       6144 x 4096

7. NewWolvesbane treasure locations MUST come from staff-approved hunting
   rectangles. There is deliberately no random whole-map fallback.

WHY APPROVED AREAS ARE REQUIRED
-------------------------------
The Scripts package defines the custom map's dimensions, but it does NOT
contain the custom terrain MUL/UOP data or enough saved-world travel data to
prove which portions of the map are accessible.

A tile can be:
- non-water
- non-impassable
and still be isolated behind terrain or otherwise impossible for players to
reach.

So the safe production solution is an explicit persistent whitelist.

NEW AREA COMMANDS
-----------------
Stand on NewWolvesbane in a known-good outdoor hunting area.

    [WBTMapAreaAdd 250

This approves a square extending 250 tiles around your current position.

The radius may be 25 through 1500.

For precise rectangles:
    [WBTMapAreaAddRect <x1> <y1> <x2> <y2>

List areas:
    [WBTMapAreaList

Remove one:
    [WBTMapAreaRemove <index>

Clear all:
    [WBTMapAreaClear

PERSISTENCE
-----------
Approved areas are saved automatically to:

    Data/Wolvesbane/TreasureMaps/NewWolvesbaneAreas.cfg

They reload when ServUO starts.

FIRST TEST
----------
1. Install/compile/restart.
2. Go to a safe, ordinary wilderness location on NewWolvesbane.
3. Run:
       [WBTMapAreaAdd 250
4. Run:
       [WBTMapAreaList
5. Run:
       [WBTMapCreate 0
6. Decode it.
7. The item should now say:
       For Somewhere In New Wolvesbane
8. Open the map and inspect the red pin.
9. Use:
       [WBTMapAudit
   and target the map.
10. Travel to the location and confirm it is accessible before digging.

ADDING MORE COVERAGE
--------------------
Once the first zone works, travel to several geographically separated safe
wilderness areas and add additional zones. Treasure maps will randomly select
valid dig tiles from all approved rectangles.

This gives Wolvesbane precise control over where treasure hunts can occur
without assuming the entire custom facet is safely traversable.
