WOLVESBANE HOUSING RECLAMATION - PHASE 1
===========================================

THIS PHASE IS READ-ONLY WITH RESPECT TO HOUSES AND PROPERTY.

Purpose
-------
Validate the candidate-selection and staff-review workflow before any
destructive property handling is enabled.

Eligibility
-----------
A house is shown only when:
- it has a valid owner and account
- Account.LastLogin is more than 30 days ago
- the account/owner does not have staff access
- no character on that account has staff access
- the house is not currently Deferred
- the house is not Exempt

Account.LastLogin is used intentionally so a login by ANY character on
the account protects every house owned by that account.

Commands
--------
[WBHousingAudit
  Read-only candidate count and review-state summary.

[WBHousingReview
  Opens staff review gump.

Staff login
-----------
GameMaster+ characters are notified a few seconds after login when
candidate houses are waiting.

Review actions
--------------
Go
  Teleports staff to the house for visual inspection.

Defer
  Removes the house from the candidate queue for 7 days.

Exempt
  Permanently removes that house from the queue until its review state
  is cleared in a future admin tool.

There is deliberately NO approve/destroy button in Phase 1.

Installation
------------
1. TEST SERVER FIRST.
2. Add:
   Scripts\Custom\Wolvesbane\HousingReclamation\WBHousingReclamationPhase1.cs
3. Compile/restart.
4. Run [WBHousingAudit.
5. Run [WBHousingReview.
6. Compare several candidate accounts against actual account last-login
   information.
7. Test Go, Defer, and Exempt on known test houses.
8. Restart and confirm Deferred/Exempt state persists.

Phase 2
-------
Once the candidate rules are proven, Phase 2 will add:
- approval confirmation
- account-owned permanent reclamation storage
- "Vendor Held Items - <vendor name>" containers
- "Destroyed House Refund" bank container
- refund to original owner, account-character fallback
- property capture before house deletion
- permanent destruction/reclamation audit record
- batch approval with final confirmation
- Abandoned Property Reclamation Officer NPC
