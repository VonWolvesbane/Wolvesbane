WOLVESBANE TREASURE MAPS - PHASE 7C3
SURGICAL CUSTOM-TREE FIX
=====================================

THIS REPLACES THE BAD 7C / 7C2 PACKAGES.

The previous 7C2 TreasureMapInfo.cs was derived from the wrong source version
and removed Wolvesbane's CreateLegacyMap compatibility methods. That caused
CS0117 errors throughout WB treasure diagnostics, TestCenter, Fishing,
Uzeraan, Grizelda, PublicTreasureMaps, BaseCreature, SkillRings,
ParagonChest, UnknownSkeletons, and other scripts.

7C3 is built from the LAST KNOWN-WORKING CUSTOM TreasureMapInfo.cs from the
Phase 6B custom tree.

VERIFIED BEFORE PACKAGING:
- CreateLegacyMap(int legacyLevel, Map map) EXISTS
- CreateLegacyMap(int legacyLevel, Map map, bool eodon) EXISTS
- NewWolvesbane TreasureFacet support EXISTS
- NewWolvesbane weapon/armor loot routing EXISTS
- NewWolvesbane Mage reagent routing EXISTS
- NewWolvesbane Artisan special-material safety EXISTS

FILES
-----
Scripts/Custom/Edits to New Pub/TreasureMaps/TreasureMapInfo.cs
    Correct custom Wolvesbane version with CreateLegacyMap preserved.

Scripts/Custom/Edits to New Pub/TreasureMaps/TreasureMap.cs
    Phase 7 NewWolvesbane guardian routing, including:
    Griffin, Moose, Panda, WolvesbanianImp for highest-tier maps.

Scripts/Skills/RemoveTrap.cs
    Phase 7B working Trove Remove Trap compatibility fix.

NO REFERENCE_CREATURES FOLDER IS INCLUDED.
Those creatures already exist in your server.

NO stock-path TreasureMapInfo.cs is included.
There is deliberately no:
    Scripts/Items/Misc/TreasureMapInfo.cs

INSTALL
-------
1. Restore the working custom TreasureMapInfo.cs first if you still have the
   bad 7C2 version installed.
2. Back up these three files.
3. Copy the contents of this ZIP over the matching paths.
4. Compile/restart.

FIRST THING TO CHECK
--------------------
The CS0117 'CreateLegacyMap' errors should be completely gone.

Then test:
1. [WBTMapCreate 0
2. A NewWolvesbane Mage Stash
3. A NewWolvesbane Artisan Supply
4. A NewWolvesbane Trove
5. Confirm custom Trove guardians
6. Confirm 120 Remove Trap still works

The disabled special/decorative reward restoration is NOT included yet.
We will do that separately after this compatibility baseline compiles cleanly.
