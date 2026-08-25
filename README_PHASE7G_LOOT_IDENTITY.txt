WOLVESBANE TREASURE MAPS - PHASE 7G
LOOT IDENTITY + VERY RARE CUSTOM-GEAR JACKPOT
==============================================

CHANGES
-------
1. Assassin Cache equipment: 24 -> 18.
2. Trove gold: 60,000-90,000 (was 50,000-70,000).
3. Equipment category weighting:
      weapons 30%
      armor   50%
      jewels  20%
4. High-tier special artifact pools are now profession-specific.
5. Artisan Hoard/Trove ordinary combat-equipment volume is reduced.
6. Artisan Cache/Hoard/Trove gets one compact high-tier crafting-material bonus:
      Cache 50
      Hoard 75
      Trove 100
   of one quality-appropriate material type.
7. Added an independent VERY RARE gear jackpot:
      Cache 0.25%  (about 1 in 400)
      Hoard 0.75%  (about 1 in 133)
      Trove 1.50%  (about 1 in 67)
      Stash/Supply 0%
   The jackpot selects from the map profession's curated artifact-equipment pool.
   It is placed in the distinctive hue-1278 artifact backpack.

WHY EXISTING ARTIFACT TYPES ARE USED
------------------------------------
The treasure-map source available for this pass proves these artifact classes are
already referenced/compiled in Wolvesbane. Using them avoids introducing a hard
dependency on an unknown custom-item class and makes this patch compile-safe.

If you later provide the actual class names/files for additional Wolvesbane-only
custom weapons/armor, they can be added to GetProfessionArtifactPool() or to a
separate jackpot pool without changing the probability system.

INSTALL
-------
Replace TreasureMapInfo.cs with the Phase 7G version.

This archive also includes the Phase 7F TreasureMap.cs facet-whitelist patch so
the package can be installed cumulatively.

RECOMMENDED TEST
----------------
Temporarily raise GetCustomGearChance(Trove) to 1.0 on a test server, dig one
Trove of each profession, verify the jackpot item constructs correctly, then
restore the shipped percentages before production.
