WOLVESBANE TREASURE MAPS - PHASE 7B
REMOVE TRAP / TROVE COMPATIBILITY FIX
=====================================

ROOT CAUSE
----------
The newer TreasureMapInfo.Fill() sets these trap powers:

    Stash   = 25
    Supply  = 75
    Cache   = 125
    Hoard   = 150
    Trove   = 170

Wolvesbane's existing Scripts/Skills/RemoveTrap.cs uses:

    CheckTargetSkill(RemoveTrap, target, TrapPower, TrapPower + 10)

That makes Trove check against 170-180 skill.

With an individual skill cap of 120.0, Trove is therefore impossible to
disarm. Hoard and Cache are also beyond the intended practical skill range.

FIX
---
TrapPower is left unchanged. This is important because TreasureMapChest also
uses TrapPower / TrapLevel for trap strength and damage.

Only the Remove Trap SKILL CHECK for newer-system TreasureMapChest objects is
given a separate difficulty curve:

    Tier      Min    Max
    -------------------
    Stash      20     50
    Supply     40     70
    Cache      65     95
    Hoard      85    115
    Trove     105    125

At 120.0 Remove Trap, Trove is deliberately difficult but viable rather than
impossible. With the normal ServUO linear CheckSkill range, 120 lies near the
top of the 105-125 Trove window.

All non-treasure containers retain the original behavior:
    TrapPower .. TrapPower + 10

The prerequisites of 50 Lockpicking and 50 Detect Hidden are unchanged.

INSTALL
-------
Replace:

    Scripts/Skills/RemoveTrap.cs

Compile/restart.

TEST
----
1. Use a character with:
       Remove Trap 120.0
       Lockpicking >= 50.0
       Detect Hidden >= 50.0

2. Dig a NEW Trove chest.

3. Attempt Remove Trap multiple times.
   It should now succeed, though not necessarily on the first try.

4. Also test a Hoard and Cache.
   They should be progressively easier.

5. Confirm an ordinary trapped container still uses its original TrapPower
   based difficulty.

NOTE
----
This patch changes DISARM difficulty only. It does not weaken the explosion
damage/power of Trove or any other treasure chest.
