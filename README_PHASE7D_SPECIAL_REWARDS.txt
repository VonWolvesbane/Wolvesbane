WOLVESBANE TREASURE MAPS - PHASE 7D
SAFE SPECIAL REWARD RESTORATION
================================

This is a SURGICAL patch. It replaces ONLY:

Scripts/Custom/Edits to New Pub/TreasureMaps/TreasureMapInfo.cs

It is based on the compiling Phase 7C3 custom TreasureMapInfo.cs and preserves:
- CreateLegacyMap compatibility
- NewWolvesbane facet handling
- Mage reagent handling
- Artisan material safety
- all previous custom treasure compatibility code

WHAT WAS WRONG
--------------
TreasureMapInfo already rolled special-reward chances:

Supply = 10%
Cache  = 20%
Hoard  = 50%
Trove  = 75%

But the code that actually constructed and dropped the reward was commented out.

WHY IT WAS COMMENTED
--------------------
The original newer Publish code referenced several item classes that were also
commented out of Wolvesbane's tables. Blindly restoring the original block risks
missing-type compile failures in this older/custom server.

WHAT 7D DOES
------------
The reward-construction block is restored using ONLY item classes that are
already ACTIVE in Wolvesbane's TreasureMapInfo.cs and therefore already compile.

For Assassin / Mage / Ranger / Warrior:
    Special rolls use the existing _FunctionalMinorArtifacts pool.

Examples from that existing pool include:
    ArcticDeathDealer
    BlazeOfDeath
    BurglarsBandana
    CavortingClub
    DreadPirateHat
    EnchantedTitanLegBone
    GwennosHarp
    IolosLute
    LunaLance
    NightsKiss
    NoxRangersHeavyCrossbow
    PolarBearMask
    VioletCourage
    HeartOfTheLion
    ColdBlood
    AlchemistsBauble
    CaptainQuacklebushsCutlass
    ShieldOfInvulnerability

For Artisan Cache/Hoard/Trove:
    The special roll uses the already-active _DecorativeMinorArtifacts pool,
    preserving a crafter/collector flavor without adding unverified classes.

Special artifacts are placed inside the purple artifact backpack (Hue 1278),
matching the Supply decorative reward presentation you already saw.

NOT ENABLED
-----------
The still-commented newer classes remain disabled, including things such as:
LegendaryMapmakersGlasses, ManaPhasingOrb, RunedSashOfWarding,
SkullGnarledStaff, SkullLongsword, etc.

We are NOT introducing those until their exact Wolvesbane implementations are
verified.

TEST
----
Compile/restart, then use your staff command:

    [WBTMapCreate Trove Warrior

Trove has a 75% special reward chance. Look for an additional purple backpack
containing a functional artifact.

Also test:

    [WBTMapCreate Hoard Mage
    [WBTMapCreate Cache Ranger
    [WBTMapCreate Trove Artisan

Because the roll is random, a single chest can legitimately have no special
reward. Trove should produce one roughly 3 out of 4 chests.

EXPECTED RESULT
---------------
Normal gold/gems/equipment remain unchanged.
The special reward is an ADDITIONAL rare item; Phase 7D does not reduce the
ordinary equipment count yet.

Once this is confirmed working, the next optional balance pass can reduce
Hoard/Trove equipment clutter while keeping these rare rewards.
