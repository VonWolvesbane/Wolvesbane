WOLVESBANE HOUSING RECLAMATION - PHASE 4 PLAYER RECLAMATION UI
================================================================

This replaces the Phase 3 package files.
Install the two .cs files included in this bundle only.

PLAYER EXPERIENCE
-----------------
Double-click:
    Abandoned Property Reclamation Officer

The player now receives a proper account-based reclamation interface with:

AVAILABLE tab
- Shows each destroyed house that still has unclaimed property.
- Each house remains a distinct reclamation record.
- List shows owner, house type, map/location, destruction date and status.
- Details screen shows:
    original owner
    house type
    former location
    reclaimed date/time
    reclaimed item count
    house refund
    yard refund
    yard object count
    player vendor count
    permanent record ID
- CLAIM PROPERTY is only available from the detail screen.

HISTORY tab
- Claimed records remain permanently visible.
- Shows the same destruction/reclamation details.
- Shows claimed date/time.
- Claimed records are no longer silently hidden.

ACCOUNT OWNERSHIP
-----------------
The lookup is still by Account.Username.
Any valid character on that same account can inspect and claim available
reclamation records.

BACKWARD COMPATIBILITY
----------------------
WBReclamationRecord serialization was upgraded from version 0 to version 1.

Old Phase 2/3 records still deserialize.
For older records:
- item count is inferred from existing storage when available
- vendor count / yard object count / yard refund may display as 0 because
  those summary values were not previously stored in the record itself

New reclamations store these values permanently for the player-facing UI.

UNCHANGED
---------
- 30-day account inactivity rule
- GameMaster staff review / defer / exempt
- Administrator individual and batch approval
- organized property storage
- vendor held items/gold
- consolidated yard refund at 500 gold per top-level yard object
- Destroyed House Refund
- permanent reclamation records
- Logs\WBHousingReclamation.log
- no expiration
