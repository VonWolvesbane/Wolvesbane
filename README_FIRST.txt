WOLVESBANE TREASURE MAPS - PHASE 1 COMPATIBILITY REPAIR
=========================================================

GOAL
----
Keep the newer five-tier treasure system as Wolvesbane's canonical system:

    0 = Stash
    1 = Supply
    2 = Cache
    3 = Hoard
    4 = Trove

while safely supporting older Wolvesbane/ServUO content that still generates
classic treasure levels 0-7.

IMPORTANT INSTALL RULE
----------------------
This package contains ONLY changed/new files.
Copy its Scripts folder over your server's Scripts folder, preserving paths.
Back up the existing files first.

Do not leave duplicate renamed .cs copies under Scripts, because ServUO will
compile both.

CONFIRMED PROBLEMS REPAIRED
---------------------------

1. LEGACY CREATURE DROP LEVELS WERE NOT CONVERTED

BaseCreature used:
    new TreasureMap(treasureLevel, map)

But the new TreasureMap Level property only accepts canonical levels 0-4.
Classic creature levels therefore mapped incorrectly, and levels 5-7 were
simply clamped to Trove.

Now legacy producers use:
    TreasureMapInfo.CreateLegacyMap(...)

Legacy conversion:
    old 0/1 -> Stash
    old 2/3 -> Supply
    old 4/5 -> Cache
    old 6   -> Hoard
    old 7   -> Trove

The new system's OWN map-upgrade logic remains:
    new TreasureMap(tMap.Level + 1, ...)
because tMap.Level is already canonical 0-4 and must not be converted twice.

2. EODON CREATURE DROPS

BaseCreature now passes the Eodon flag when a creature dies in Eodon:
    SpellHelper.IsEodon(map, Location)

This allows a Ter Mur map generated in Eodon to select the Eodon treasure
location data rather than ordinary Ter Mur data.

3. SUPPLY SPECIAL-LOOT CRASH

Wolvesbane has _SpecialSupplyLoot intentionally commented out, which leaves
a zero-length outer array.

GetSpecialLootList previously indexed:
    _SpecialSupplyLoot[(int)package]

A Supply special-loot roll could therefore throw IndexOutOfRangeException.

It is now bounds/null safe.

4. IGNORED LINQ CONCAT + MALAS NULLREFERENCE

Two methods called LINQ Concat() without assigning its return value.

GetDecorativeList was more serious:
    list was null
    list.Concat(...)

Cache/Hoard/Trove Malas treasure could therefore throw NullReferenceException.

Malas high-tier decorative loot now safely returns CoffinPiece directly.
Special loot now correctly combines enabled lists using ToArray().

5. TREASURE MAP RESET DID NOT MOVE THE TREASURE

ResetLocation() called GetRandomLocation(...) but discarded the Point2D.

The decoder was cleared, but ChestLocation and map bounds were unchanged.

ResetLocation now rebuilds:
    ChestLocation
    map Bounds

for the newly selected location.

6. OLD SAVED MAP COMPATIBILITY

Current package-aware TreasureMap serialization is version 3.

When NewSystem is enabled, serialized maps from versions below 3 are now
treated as legacy maps and converted once to the five-tier model, then receive
a package assignment.

7. OTHER LEGACY PRODUCERS

Legacy conversion was also applied to:
- BaseCreature
- Uzeraan
- Grizelda
- SkullRug
- ParagonChest
- UnknownRogueSkeleton
- Fishing level-0 map
- old TreasureMapChest/SOS special-map producers
- TestCenter legacy maps
- Commands/Test.cs treasure-map generator

NEW STAFF DIAGNOSTIC COMMANDS
-----------------------------

[WBTMapCreate <0-4>

Creates a canonical new-system map:
    [WBTMapCreate 0  = Stash
    [WBTMapCreate 1  = Supply
    [WBTMapCreate 2  = Cache
    [WBTMapCreate 3  = Hoard
    [WBTMapCreate 4  = Trove

[WBTMapLegacy <0-7>

Creates a map from an OLD numeric treasure level and reports the converted
tier. This directly validates the compatibility layer.

Expected:
    0 -> Stash
    1 -> Stash
    2 -> Supply
    3 -> Supply
    4 -> Cache
    5 -> Cache
    6 -> Hoard
    7 -> Trove

[WBTMapAudit

Target any TreasureMap. Reports:
- serial
- canonical numeric level
- tier name
- package
- facet
- TreasureFacet
- chest location
- completion state
- decoder
- next reset

RECOMMENDED VALIDATION
----------------------
1. Compile/restart.
2. Run all eight:
       [WBTMapLegacy 0
       ...
       [WBTMapLegacy 7
   Verify conversion table above.
3. Test:
       [WBTMapCreate 0 through 4
   Decode and dig one of each tier.
4. Test at least:
   - Trammel/Felucca
   - Malas Cache-or-higher (tests former null crash path)
   - Ter Mur
   - Eodon if available
5. Kill known treasure-map creatures from multiple classic levels and audit
   the resulting map with [WBTMapAudit.
6. Open an existing OLD saved treasure map and audit its level/package.
7. For a non-completed decoded map, allow/force its reset and verify that its
   chest coordinates actually change.

FILES MODIFIED
--------------
- Scripts/Commands/Test.cs
- Scripts/Custom/Edits to New Pub/TreasureMaps/TreasureMap.cs
- Scripts/Custom/Edits to New Pub/TreasureMaps/TreasureMapChest.cs
- Scripts/Custom/Edits to New Pub/TreasureMaps/TreasureMapInfo.cs
- Scripts/Custom/Wolvesbane/TreasureMaps/WBTreasureMapDiagnostics.cs
- Scripts/Items/Addons/SkullRug.cs
- Scripts/Items/Containers/ParagonChest.cs
- Scripts/Items/Containers/UnknownSkeletons.cs
- Scripts/Mobiles/NPCs/Grizelda.cs
- Scripts/Mobiles/NPCs/Uzeraan.cs
- Scripts/Mobiles/Normal/BaseCreature.cs
- Scripts/Services/Harvest/Fishing.cs
- Scripts/Services/TestCenter.cs
