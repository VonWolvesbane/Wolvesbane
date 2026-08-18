WOLVESBANE HOUSING RECLAMATION - PHASE 3 PRODUCTION WORKFLOW
================================================================

REPLACES BOTH existing housing-reclamation .cs files.

Install ONLY these two files in:
Scripts\Custom\Wolvesbane\HousingReclamation\

    WBHousingReclamationPhase1.cs
    WBHousingReclamationPhase2A.cs

Remove/move out older Phase1/Phase2A/2B/2C/2D copies before compiling.

PRODUCTION STAFF WORKFLOW
-------------------------
[WBHousingReview

GameMaster+
- receives login notification when candidates exist
- Go / teleport to candidate
- Defer 7 days
- Permanently Exempt

Administrator
- all GameMaster actions
- individual Approve button
- current-page batch checkboxes
- Review Selected Batch
- destructive batch confirmation with required acknowledgement checkbox

BATCH SAFETY
------------
- Selection is intentionally limited to the CURRENT PAGE (max 8 houses).
- A separate summary gump shows:
    selected count
    currently eligible count
    combined house refunds
    player vendor count
    yard object count
- Administrator must check the acknowledgement box.
- Every house is re-checked against the 30-day candidate rules immediately
  before that house is processed.
- A failure on one house is logged and does not automatically destroy an
  ineligible house.
- Batch completion reports succeeded vs failed/skipped.

AUDIT TRAIL
-----------
The persistent WBReclamationRecord already records:
- record ID
- account
- original owner
- house type
- house serial
- map/location
- house refund
- destruction timestamp
- approving staff member
- reclamation storage / claim status

Phase 3 additionally appends one line per SUCCESS or FAILED attempt to:

    Logs\WBHousingReclamation.log

Each line includes:
- UTC timestamp
- result
- Individual or Batch mode
- staff
- account / owner
- house type / serial
- map/location
- house refund
- vendor count
- yard object count
- consolidated yard refund amount
- reclaimed item count
- reclamation record ID
- error text when failed

IMPORTANT
---------
The validated reclamation rules remain unchanged:
- >30 days since ACCOUNT LastLogin
- one active character protects all houses on the account
- staff accounts excluded
- permanent account-owned reclamation storage
- organized numbered storage bags
- Vendor Held Items - <vendor name>
- vendor held-gold preservation
- Yard Item Refunds with ONE consolidated check at 500 gold per top-level yard object
- Destroyed House Refund in original owner's bank, account-character fallback
- no expiration

RECOMMENDED FIRST PRODUCTION CHECK
----------------------------------
1. Compile/restart.
2. [WBHousingAudit
3. [WBHousingReview
4. Verify individual Approve opens the familiar confirmation gump.
5. Select TWO known-safe candidate houses on one page.
6. Click Review Selected Batch.
7. Inspect summary.
8. Cancel the first time.
9. Reopen and process a controlled two-house batch.
10. Check Logs\WBHousingReclamation.log.
