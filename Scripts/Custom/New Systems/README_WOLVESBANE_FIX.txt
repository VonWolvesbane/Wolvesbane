Wolvesbane Invasions - Batched Cleanup + Batched Reward Processing

The first patch batched reward item creation only.

This second patch also batches the expensive END-OF-INVASION WORLD CLEANUP.

Original Stop() synchronously called:
    DeleteInvaders()
    DeletePortals()
    InvalidateGates()

DeleteInvaders() called BaseCreature.Delete() for every remaining invasion
creature in a single ServUO tick. BaseCreature deletion can cascade through
world/region/combat/item/event bookkeeping and is capable of freezing the
main loop even when reward creation is already batched.

New shutdown flow:
1. Mark invasion Finished immediately.
2. Snapshot and clear live Invaders / Portals / TownGates lists.
3. Delete at most 10 world objects per 50ms timer tick.
4. Restore guarded-region state.
5. Stage at most 3 defenders per tick.
6. Create/deliver at most 25 prize Items per tick.
7. Invoke InvasionService.OnFinished only after the pipeline finishes.

Console now reports:
    beginning batched shutdown. Invaders=X, Portals=Y, Gates=Z, Defenders=N

This line is useful diagnostically:
- If the shard freezes BEFORE it prints, the problem is outside this end
  pipeline.
- If it prints and the shard remains responsive, the synchronous cleanup was
  the freeze source.
- If one individual deletion still causes a long hitch, reduce
  FinishCleanupObjectsPerTick from 10 to 1.

No rank, prize amount, GoldPool, or placement rules were intentionally changed.
