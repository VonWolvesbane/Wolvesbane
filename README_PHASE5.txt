WOLVESBANE SAVE OPTIMIZATION - PHASE 5
OWLTR serialization audit (READ ONLY)

Purpose
-------
The non-deletable Daat99OWLTR control item serializes Daat99OWLTR.StaticHolders.
Each NewDaat99Holder serializes every recipe Type as a full type-name string.
This audit measures how much holder/recipe data exists before any format migration.

Install
-------
Copy:
  Scripts/Custom/Wolvesbane/WBOWLTRAudit.cs
into the same path on the TEST server (or another Scripts/Custom folder).
Restart/recompile.

Commands
--------
[WBOWLTRAudit
[WBOWLTRAudit verbose

Both commands are READ ONLY. They do not alter holders, recipes, resources, mobiles,
or the OWLTR control item.

Please capture the full normal output and the verbose output and send them back.
The important values are:
- Static holders / Serializable valid holders
- Recipe entries serialized
- Distinct recipe type strings
- Approx repeated recipe-name payload
- Deleted-mobile keys
- Largest holder recipe counts

Do NOT alter Daat99OWLTR serialization yet. Phase 5 is measurement only.
