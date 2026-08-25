WOLVESBANE TREASURE MAPS - PHASE 5: CUSTOM FACET MAP RENDERING
================================================================

This package is cumulative and includes Phases 1-4.

ROOT CAUSE FOUND
----------------
NewWolvesbane is registered in this server's MapDefinitions.cs as:

    RegisterMap(6, 6, 6, 6144, 4096, 2, "NewWolvesbane", ...)

However Scripts/Items/Tools/MapItem.cs built packet 0xF5 with a hard-coded
facet list containing only:

    Felucca  = 0
    Trammel  = 1
    Ilshenar = 2
    Malas    = 3
    Tokuno   = 4
    TerMur   = 5

For every other facet, mapValue remained 0.

So a TreasureMap could correctly have:
    Facet = NewWolvesbane
    valid NewWolvesbane ChestLocation
    correct NewWolvesbane Bounds

...while the map packet told the CLIENT to render facet 0 / Felucca.

That explains the solid-blue/ocean map: the pin/bounds were NewWolvesbane
coordinates, but the background artwork was being requested from the wrong
facet.

FIX
---
NewMapDetails now sends:

    map.Facet.MapID

NewWolvesbane's MapID is 6, so packet 0xF5 now identifies it as facet 6.

TEST
----
1. Compile/restart.
2. Delete old test treasure maps.
3. Keep the approved NewWolvesbane treasure areas from Phase 3.
4. Run:
       [WBTMapLocationTest
   to confirm a valid location.
5. Run:
       [WBTMapCreate 0
6. Decode/open the NEW map.

EXPECTED
--------
If your client supports map-art rendering for its installed map6 files, the
treasure map should now render NewWolvesbane terrain instead of Felucca/ocean.

IMPORTANT CLIENT LIMIT
----------------------
This server fix makes the packet correct. If the client still shows a blank or
solid map after receiving MapID 6, that means the client build/map renderer
does not provide automap/multimap artwork for custom facet 6. That cannot be
fixed by changing treasure coordinates; it becomes a client map-art support
issue.

The next diagnostic in that case is to confirm the client family/version and
whether its normal world/radar map can render NewWolvesbane/map6.
