WOLVESBANE SAVE OPTIMIZATION - PHASE 7
========================================

Purpose
-------
Read-only population profiler for the remaining Wolvesbane world objects.

Install
-------
Copy:
  Scripts/Custom/Wolvesbane/WBWorldAudit.cs
into the matching Scripts/Custom/Wolvesbane folder on the TEST server.

Restart/recompile.

Commands
--------
[WBWorldAudit
  Overall summary only.

[WBWorldAudit items 30
  Top 30 Item runtime types by object count, including containment/location breakdown.

[WBWorldAudit mobiles 30
  Top 30 Mobile runtime types by object count.

[WBWorldAudit suspicious 30
  Ranks parentless Items and Mobiles sitting on Map.Internal at (0,0,0).
  This is an AUDIT ONLY. Internal objects are not automatically garbage.

Safety
------
This phase has NO Delete(), MoveToWorld(), Consume(), serialization mutation,
or cleanup action. It only enumerates World.Items / World.Mobiles and reports counts.

What to send back
-----------------
Please send screenshots of:
  1. [WBWorldAudit
  2. [WBWorldAudit items 30
  3. [WBWorldAudit mobiles 30
  4. [WBWorldAudit suspicious 30

The next optimization target will be selected from those measurements.
