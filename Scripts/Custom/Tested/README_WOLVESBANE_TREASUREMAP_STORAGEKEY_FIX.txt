WOLVESBANE - UNIVERSAL STORAGE KEYS TREASURE MAP CRASH FIX

Crash signature
---------------
System.NullReferenceException
Solaris.ItemStore.TreasureMapListEntry.get_Columns()
ItemListEntries.cs line 227

Root cause
----------
Some stored TreasureMapListEntry records contain a null _ChestMap (facet).
Universal Storage Keys called _ChestMap.ToString() while opening the stored
treasure-map list. One bad legacy entry could therefore crash the shard.

Replace
-------
Universal Storage Keys Version 2.0.6/
  Main Data Management/
    ItemListEntries.cs
    ListEntry.cs

Changes
-------
- Null treasure-map facets display as "Unknown / Legacy" instead of crashing.
- Null-facet stored maps are NOT silently rebuilt onto a random facet.
- Failed withdrawal gives the player a GM message instead of failing silently.
- SOS entries received the same null-map display protection.
- Valid treasure maps are otherwise unchanged.

TEST FIRST
----------
1. Install on test server and compile/restart.
2. Open the same storage-key treasure-map list that crashed before.
3. Expected: gump opens.
4. Bad entries show "Unknown / Legacy".
5. Clicking a bad entry is refused cleanly; no crash.
6. Valid maps still withdraw normally.

NOTE
----
The patch deliberately does not guess a replacement facet. Treasure coordinates
overlap across facets, so guessing could silently convert a damaged stored map
into the wrong treasure map.
