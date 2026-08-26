WOLVESBANE TAMING BOD - 6 HOUR ALIGNMENT
==========================================

This is a surgical replacement for the currently compiled PlayerMobile.cs.

CHANGE
------
NextTamingBulkOrder remains on the existing legacy FS:ATS DateTime timer,
but any positive cooldown shorter than 6 hours is normalized to 6 hours.

Why:
- The taming vendor currently hands out a BOD and asks for a 60-minute delay.
- Smith/Tailor and the repaired BOD scheduler use a 6-hour cadence.
- This change brings Taming's player-visible cooldown in line without trying
  to force the custom Taming BOD reward/deed system into ServUO's BODType enum.

Admin reset behavior is preserved:
- Setting NextTamingBulkOrder to TimeSpan.Zero still means immediately eligible.

Existing saves remain compatible:
- No PlayerMobile serialization version changes.
- m_NextTamingBulkOrder remains the same DateTime field.
- Existing remaining taming cooldowns load normally.

TEST
----
1. Replace your current PlayerMobile.cs and compile/restart.
2. Reset the test character's taming BOD timer to zero.
3. Receive one Taming BOD.
4. Check the NPC/status again.
5. It should now report approximately 6 hours, not 60 minutes.

This does NOT change Taming BOD rewards, deed generation, BOB filters, or turn-in logic.
