WOLVESBANE TREASURE MAPS - PHASE 2 LOCATION / RESET REPAIR
===========================================================

This package INCLUDES the Phase 1 compatibility repairs plus the following
TreasureMap.cs fixes.

1. RESET STORM FIX
------------------
The previous TreasureMap.Serialize() scheduled ResetLocation() whenever an
expired map was saved.

That means every world save could schedule another callback for the same map.
A shard with many expired maps could therefore produce a large wave of
simultaneous location resets and console warnings after a save.

Serialize() no longer schedules gameplay timers.

On server load, each expired, incomplete map schedules ONE reset, randomly
staggered between 15 and 180 seconds.

2. TREASURE LOCATION PICKER
---------------------------
The script already contained facet-specific treasure regions:
- Felucca / Trammel
- Ilshenar
- Malas
- Tokuno
- Ter Mur
- Eodon

The previous GetRandomLocation() ignored all of them and randomly sampled the
ENTIRE facet only ten times.

The new picker:
- samples the configured treasure regions first
- makes up to 100 random region attempts
- performs a deterministic region-grid probe if random attempts fail
- only then performs 250 whole-facet fallback attempts
- never silently returns an invalid map center
- reports the exact facet if no valid location exists

3. MAP CENTER FALLBACK
----------------------
Map center is now used only if ValidateLocation confirms that the center is
actually a valid dig tile.

4. RESET FAILURE SAFETY
-----------------------
If no valid location exists, an old map's reset aborts safely and retries in
six hours instead of moving it to 0,0 or an invalid location.

5. TRAMMEL/FELUCCA MAP SIZE BUG
-------------------------------
GetWidthAndHeight() previously used:
    if (Trammel/Felucca) ...
    if (TerMur) ...
    else ...

The second 'else' overwrote Trammel/Felucca's 600x600 map size with 300x300.

This is now a correct if / else-if / else chain.

TEST
----
After compile/restart:

1. Watch the console for the old repeated warning:
   "Failed to find valid location after 10 attempts"

   That warning should be gone.

2. Continue:
   [WBTMapLegacy 5
   [WBTMapLegacy 6
   [WBTMapLegacy 7

3. Then create canonical maps:
   [WBTMapCreate 0
   through
   [WBTMapCreate 4

4. Use [WBTMapAudit on maps and verify ChestLocation values are plausible.

If a facet genuinely cannot select a location, the console now identifies the
facet instead of producing a generic map-center fallback message.
