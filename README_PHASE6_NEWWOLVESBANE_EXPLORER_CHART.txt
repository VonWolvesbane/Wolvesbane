WOLVESBANE TREASURE MAPS - PHASE 6: NEW WOLVESBANE EXPLORER'S CHART
=====================================================================

This package is cumulative and includes Phases 1-5.

WHY
---
The server now correctly identifies NewWolvesbane and sends MapID 6, but the
client renders custom-facet treasure maps as blank parchment because it lacks
usable cartography artwork for that facet.

ServUO community documentation points to client cartography artwork
(Multimap.rle on older clients / facet map artwork on newer clients) as the
source of the rendered terrain. Rather than require every player to install a
new client cartography asset, Phase 6 provides a server-side chart.

WHAT CHANGED
------------
ONLY TreasureMap.DisplayTo() for Map.NewWolvesbane is redirected.

Stock facets still use the normal MapItem / client map display.

NewWolvesbane keeps:
- the real ChestLocation
- approved-area selection
- dry-land validation
- normal decoding rules
- normal digging target
- normal guardians
- normal treasure chest and rewards
- normal completion state

The replacement gump shows:
- New Wolvesbane identity
- treasure tier
- treasure package/type
- region name when available
- coordinates rounded to the nearest 50 tiles
- cardinal/intercardinal direction from the player
- distance rounded to the nearest 25 paces
- Begin Dig button
- Refresh Bearings button

The coordinates are deliberately approximate so the treasure hunt remains a
hunt rather than exposing the exact ChestLocation.

TEST
----
1. Compile/restart.
2. Delete old test maps if desired.
3. Create:
       [WBTMapCreate 0
4. Decode/open it.
5. Instead of blank parchment, the Explorer's Treasure Chart should appear.
6. Move around NewWolvesbane and click Refresh Bearings.
7. Direction/distance should change.
8. Travel near the indicated approximate coordinates.
9. Click Begin Dig.
10. The normal TreasureMap dig targeting cursor should appear.
11. Dig near the true location and confirm normal chest/guardian behavior.

NOTE
----
Phase 5's MapItem.cs fix is retained because it is still the correct general
behavior for custom facets and may help clients that do support custom facet
cartography. Phase 6 simply avoids relying on that client artwork for
NewWolvesbane treasure maps.
