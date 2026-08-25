PHASE 6B - NAMESPACE COMPILE FIX
================================

Compile error:
    WBNewWolvesbaneTreasureChartGump could not be found

Cause:
TreasureMap.cs is in namespace:
    Server.Items

The chart gump is in:
    Wolvesbane.TreasureMaps

Phase 6 referenced the gump by short class name only.

Fix:
TreasureMap.cs now uses the fully-qualified type:

    Wolvesbane.TreasureMaps.WBNewWolvesbaneTreasureChartGump

No gameplay logic changed.
