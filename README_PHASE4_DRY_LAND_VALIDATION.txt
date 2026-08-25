WOLVESBANE TREASURE MAPS - PHASE 4: DRY LAND VALIDATION
=========================================================

This package is cumulative and includes Phases 1-3.

SYMPTOM
-------
NewWolvesbane maps were consistently selecting water / shoreline locations,
even inside staff-approved hunting rectangles.

CAUSE
-----
The prior validation checked only TileData's generic Wet flag. VitaNex already
contains much more complete Ultima Online water knowledge, including:
- known land water tile IDs
- known static water tile IDs
- tile-name water detection
- coastline detection

Custom map terrain can use water/coast graphics that are not rejected by a
simple Wet-flag check.

FIX
---
TreasureMap.ValidateLocation now requires:

- coordinate is inside map bounds
- non-zero/valid land tile
- not Impassable
- not Wet
- VitaNex LandTile.IsWater() == false
- VitaNex LandTile.IsCoastline() == false
- no impassable/wet/water statics on the tile
- map.CanSpawnMobile() succeeds at average Z

For Map.NewWolvesbane ONLY, the selected tile must additionally have a
5-tile dry-land buffer around it.

The buffer rejects:
- shorelines
- docks/water-edge tiles
- tiny islands
- narrow land strips surrounded by water
- tiles where the center is dry but adjacent tiles are water

NEW STAFF COMMAND
-----------------
[WBTMapLocationTest

Run this while on NewWolvesbane after approved areas are configured.

It selects a location using the exact production treasure-location algorithm
and reports its X,Y,Z and whether final production validation passes.

TEST PLAN
---------
1. Compile/restart.
2. Keep your existing approved area(s).
3. Run [WBTMapLocationTest several times.
4. Use [go or [tele to the reported coordinates if desired and visually
   verify them.
5. When those are dry land, run [WBTMapCreate 0 and decode a new map.

IMPORTANT
---------
Existing maps keep their already-selected ChestLocation. Delete old test maps
and create NEW ones after installing this patch.
