WOLVESBANE BOD TIMER/CACHE FIX

Replace:
  Scripts/Engines/BulkOrders/BulkOrderSystem.cs
(or the matching BulkOrders/BulkOrderSystem.cs location in your shard)

Fixes:
- Removes the duplicated/unreachable CanGetBulkOrder condition.
- Cached deeds now mean immediate BOD eligibility.
- Merely checking BOD status no longer resets a full cache to a fresh 6-hour wait.
- Corrects the legacy GetNextBulkOrder countdown sign.
- OWLTR-style SetNextBulkOrder calls consume a cached deed under the new system.
- Setting a BOD timer to zero makes the player eligible now.
- Cache regeneration remains 1 deed per 6 hours, maximum 3.

FIRST TEST
1. Back up your existing BulkOrderSystem.cs.
2. Replace it with this file and compile/restart.
3. Use your admin BOD reset on a test character.
4. Run [OWLTRBOD.
   Smith/Tailor should show as available instead of ~6 hours remaining.
5. Request one Smith BOD.
6. Re-open [OWLTRBOD.
   Because the cache starts at 3, another cached deed should still be available.
7. Consume all 3 cached deeds. Only then should the 6-hour countdown appear.
8. Test a normal NPC vendor as well.

NOTE:
This patch changes only scheduling/cache behavior. BOD creation, rewards,
profession tables, and turn-in logic are left untouched.
