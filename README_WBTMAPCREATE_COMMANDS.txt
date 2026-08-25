WOLVESBANE TREASURE MAP STAFF COMMAND UPDATE
=============================================

Replace:
Scripts/Custom/Wolvesbane/TreasureMaps/WBTreasureMapDiagnostics.cs

NEW WBTMapCreate SYNTAX
-----------------------
[WBTMapCreate <tier> [package]

Tier can be:
0 or Stash
1 or Supply
2 or Cache
3 or Hoard
4 or Trove

Package can be:
Artisan
Assassin
Mage
Ranger
Warrior

Examples:
[WBTMapCreate Stash Mage
[WBTMapCreate Supply Artisan
[WBTMapCreate Cache Ranger
[WBTMapCreate Hoard Assassin
[WBTMapCreate Trove Warrior
[WBTMapCreate Trove Mage
[WBTMapCreate 4 Mage

If you omit the package:
[WBTMapCreate Trove

the normal constructor keeps its RANDOM package.

This only changes the staff diagnostic command. It does not change normal
treasure-map drops or random profession-package generation.

Useful existing commands remain:
[WBTMapLegacy <0-7>
[WBTMapAudit
[WBTMapLocationTest
[WBTMapAreaList
