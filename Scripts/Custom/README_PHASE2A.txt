WOLVESBANE HOUSING RECLAMATION - PHASE 2A
============================================

KEEP THE VALIDATED PHASE 1 FILE INSTALLED.
Add this Phase 2A file beside it.

THIS BUILD ADDS REAL DESTRUCTIVE PROCESSING.
TEST SERVER ONLY UNTIL VALIDATED.

Commands
--------
[WBHousingApproveTest
  Administrator-only.
  Target one CURRENT Phase 1 candidate house.
  Opens a second confirmation gump.
  No batch mode exists in Phase 2A.

[WBHousingRecords
  Read-only summary of destroyed-house reclamation records.

NPC
---
[add AbandonedPropertyReclamationOfficer

Players double-click the NPC to see unclaimed records for THEIR ACCOUNT.
Any character on the same account may claim.
Claim places the complete reclamation crate directly into that character's
bank box.

Processing order
----------------
1. Re-check that the house is still a Phase 1 candidate.
2. BLOCK houses with legacy VendorInventory records.
3. Create/persist reclamation record + storage.
4. Capture PlayerVendor items into:
     Vendor Held Items - <vendor name>
   Vendor held gold is preserved as "Vendor held gold" bank check.
5. Call BaseHouse.MoveAllToCrate() so ServUO handles:
   lockdowns, secures, rental contracts, addon redeeding, etc.
6. Move moving-crate contents into permanent account reclamation storage.
7. Deposit "Destroyed House Refund" container into original owner's bank.
   If original owner is unavailable, fall back to another valid account character.
8. Delete the house.
9. Player can reclaim the storage from the NPC forever.

IMPORTANT LIMITATIONS / SAFETY
------------------------------
- No batch destruction yet.
- Test ONE purpose-built test house first.
- Houses containing existing legacy VendorInventory records are BLOCKED.
- Phase 1 defer/exempt logic remains in force.
- If an exception occurs, the reclamation record/storage are retained for investigation.
- Do not deploy Phase 2A to production until we validate:
    * locked-down items
    * secure containers and nested contents
    * addons / deeds
    * one or more player vendors
    * vendor gold
    * refund bank container/check
    * account-wide reclamation claim
    * save/restart persistence after destruction and after claim

Suggested first test
--------------------
Create/use a disposable account whose Account.LastLogin can be made >30 days,
with:
- one small house
- a locked-down item
- a secured container with nested items
- an addon
- a player vendor with 2-3 items and some held gold

Then:
1. [WBHousingAudit
2. [WBHousingApproveTest
3. Target test house
4. Review confirmation carefully
5. APPROVE & RECLAIM
6. Check owner's bank refund
7. [add AbandonedPropertyReclamationOfficer
8. Log a character on the same account and claim property
9. Save/restart and verify persistence
