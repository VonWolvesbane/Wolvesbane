WOLVESBANE SAVE OPTIMIZATION - PHASE 9
========================================

Purpose
-------
Read-only forensic audit of the large Gold population discovered by Phase 7.

The suspicious population was approximately:
  40,649 Server.Items.Gold objects
  Parent == null
  Map == Map.Internal
  Location == (0,0,0)

This phase DOES NOT DELETE OR MOVE GOLD.

Install
-------
Copy:
  Scripts/Custom/Wolvesbane/WBGoldAudit.cs
to the matching folder on the TEST server, then restart/recompile.

Commands
--------
[WBGoldAudit
  Summary + amount distribution.

[WBGoldAudit verbose
  Same summary plus sample serials and item state.

The audit reports
-----------------
- Gold object counts and total represented value
- contained / world-placed / parentless / Internal-zero counts
- total represented value of the Internal-zero population
- Amount=1 vs Amount>1
- min/max/average stack
- stack-size bands
- 15 most frequent exact Amount values
- common Hue, Name, and LootType values
- creation range when exposed by the shard's Item implementation
- verbose sample serials

What to send back
-----------------
Screenshots from:
  [WBGoldAudit
  [WBGoldAudit verbose]

We will not create a cleanup command until the population pattern has been
identified and we have evidence that the objects are actually orphaned.
