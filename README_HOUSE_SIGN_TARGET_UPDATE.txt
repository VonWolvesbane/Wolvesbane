Wolvesbane Housing Reclamation - Phase 2A House Sign Target Update

Change:
[WBHousingApproveTest now targets a HouseSign instead of trying to target the
BaseHouse multi directly.

Usage:
1. Run [WBHousingApproveTest
2. Target the physical house sign.
3. The script resolves sign.Structure back to the BaseHouse.
4. It re-checks that the house is a current Phase 1 candidate.
5. If valid, the existing destructive confirmation gump opens.

All Phase 2A safety checks and reclamation logic are unchanged.
This remains TEST SERVER ONLY.
