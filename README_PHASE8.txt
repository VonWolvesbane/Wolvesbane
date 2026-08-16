WOLVESBANE SAVE OPTIMIZATION - PHASE 8
========================================

Purpose
-------
Read-only mount-forensics pass.

This phase investigates the large correlation discovered in Phase 7:
- ~32k Horse mobiles, mostly Map.Internal at (0,0,0)
- ~41k MountItem items, mostly parentless Map.Internal at (0,0,0)

NO OBJECTS ARE DELETED OR MOVED.

Install
-------
Copy:
  Scripts/Custom/Wolvesbane/WBMountAudit.cs
to the matching folder on the TEST server, then restart/recompile.

Commands
--------
[WBMountAudit
  Summary classification.

[WBMountAudit verbose
  Same summary plus sample Internal (0,0,0) Horse and MountItem records.

What it measures
----------------
Horse state:
- Internal / Internal (0,0,0)
- Controlled + ControlMaster
- IsStabled when exposed by the shard's BaseCreature/BaseMount implementation
- Summoned
- Rider
- Backpack item count
- Whether a MountItem's Mount reference points back to that Horse
- Creation range when a Created/CreationTime member is exposed

MountItem state:
- Parent / parentless
- Internal (0,0,0)
- Whether MountItem.Mount resolves
- Whether that reference is a Horse

"Strong orphan-like horse signature" is ONLY a diagnostic bucket:
Internal (0,0,0), no rider, no ControlMaster, not Controlled, not Stabled,
not Summoned, empty/no backpack, and no MountItem reference.

Even objects in that bucket are NOT automatically safe to delete.

What to send back
-----------------
Screenshots of:
  [WBMountAudit
  [WBMountAudit verbose]

Then Phase 9 can be designed around the actual relationship data.
