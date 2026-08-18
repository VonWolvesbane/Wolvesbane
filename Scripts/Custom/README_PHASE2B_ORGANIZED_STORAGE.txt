WOLVESBANE HOUSING RECLAMATION - PHASE 2B ORGANIZED STORAGE
=============================================================

This replaces the previous Phase 2A file.
Keep your validated Phase 1 file installed.

What changed
------------
Large house reclamations are no longer dumped as hundreds of direct children
inside one master reclamation crate.

The master container now receives:
- Reclaimed House Items - Bag 1
- Reclaimed House Items - Bag 2
- ...
with a target of 100 direct house items per bag.

Vendor containers remain separately named:
- Vendor Held Items - <vendor name>

Nested containers are NOT flattened. If a secured chest contains 200 items,
that chest remains one reclaimed house item and keeps its own contents intact.

Why
---
The first live test produced a reclaimed container displaying hundreds of
items against a much smaller normal container capacity. ServUO allowed it,
but relying on an over-capacity master container is undesirable.

This revision keeps the master crate small and organized while preserving the
original contents and nested-container structure.

Unchanged
---------
- 30-day Phase 1 eligibility requirement
- house-sign targeting
- individual Administrator approval only
- second destructive confirmation gump
- vendor item/gold preservation
- house refund logic
- account-wide reclamation ownership
- permanent records
- no expiration
- no batch destruction yet

Next validation
---------------
Test one candidate house containing a PlayerVendor:
1. record vendor inventory and vendor held gold
2. approve/reclaim the house
3. verify "Vendor Held Items - <vendor name>"
4. verify vendor items and "Vendor held gold" bank check
5. verify ordinary house property is distributed across numbered bags
6. save/restart before claiming, then verify the record/storage survives
