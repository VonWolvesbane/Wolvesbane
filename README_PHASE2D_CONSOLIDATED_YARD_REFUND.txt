WOLVESBANE HOUSING RECLAMATION - PHASE 2D CONSOLIDATED YARD REFUND
===================================================================

This replaces the previous Phase 2C file.

Yard refund behavior
--------------------
Each top-level placed YardItem/YardGate remains worth exactly 500 gold.

Instead of creating one 500-gold check for every yard object, Phase 2D creates
ONE consolidated bank check:

    Yard Item Refund Total

Value:
    number of reclaimed top-level yard objects x 500 gold

Example:
    9 yard objects x 500 = one 4,500 gold check

The check remains inside:
    Yard Item Refunds

Internal YardPiece children are not counted separately.

Also retained
-------------
- World.Items snapshot / collection-modified fix
- organized reclamation storage bags
- vendor held item/gold handling
- house destruction refund
- account-wide reclamation
- permanent records
- 30-day Phase 1 eligibility
- house-sign targeting
