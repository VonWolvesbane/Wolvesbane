Wolvesbane BOD Diagnostics

Install:
Scripts/Custom/Wolvesbane/Commands/WBBODDiagnostics.cs

Commands:
[BODStatus  (GM)
  Target a player. Read-only. Shows DateTime fields/properties whose names look
  like BOD/Bulk Order cooldowns.

[BODReset   (Administrator)
  Target a player. Resets ONLY these recognized Smith/Tailor timer names:
    m_NextSmithBulkOrder
    m_NextTailorBulkOrder
    NextSmithBulkOrder
    NextTailorBulkOrder

It deliberately does NOT reset arbitrary DateTime members.

Test flow:
1. [BODStatus -> target your character.
2. Screenshot/copy the output.
3. [BODReset -> target your character.
4. Run [OWLTRBOD. Expected Smith/Tailor timer should show available/expired.
5. Visit a Blacksmith and Tailor NPC and request/trigger a BOD.

Interpretation:
- If [OWLTRBOD becomes eligible AND NPC gives a BOD: cooldown was the blocker.
- If [OWLTRBOD becomes eligible but NPC still gives nothing: vendor handoff is broken.
- If [BODReset finds no recognized timers: OWLTR stores cooldowns in its own holder,
  and the [BODStatus output tells us what to trace next.
