WOLVESBANE SAVE OPTIMIZATION - PHASE 16 FINALIZATION
=====================================================

GOALS
-----
1. Remove the temporary Phase 11/12 save profiler output from the in-game
   save message and console.
2. Restore the normal Wolvesbane save message:
     "World save complete. The entire process took X.X seconds."
3. Add automatic world backups at:
     12:00 AM (midnight)
     12:00 PM (noon)
   using the SERVER'S LOCAL CLOCK.
4. Keep the real script-side fixes/optimizations already installed
   (OWLTR Phase 15B, Master Key protection, leak fixes, etc.).

IMPORTANT: CORE REBUILD REQUIRED
--------------------------------
Phase 11/12 changed Server core source. To remove profiling cleanly, this
package restores the original pre-profiler versions of:

  Server\World.cs
  Server\Persistence\StandardSaveStrategy.cs

After replacing them, rebuild ServUO.exe with:

  Compile.WIN - Release.bat

BACKUP SCRIPT
-------------
Add:
  Scripts\Custom\Wolvesbane\WBTwiceDailyBackup.cs

Scheduled behavior:
  - At noon and midnight, the script performs a normal foreground World.Save().
  - After the save is complete, the Saves directory is copied on a background
    thread into:
      Backups\Wolvesbane\yyyy-MM-dd_HH-mm-ss
  - No old backups are automatically deleted.
  - If a world save is already running, the backup retries in one minute.
  - It uses server local time.

ADMIN COMMANDS
--------------
[WBBackup status
  Shows next scheduled run, last successful backup, path, and last error.

[WBBackup now
  Performs a manual save + backup immediately.
  Use this on the TEST SERVER before relying on the schedule.

TEST INSTALL
------------
1. Stop the TEST server.
2. Back up ServUO.exe, Saves, World.cs, and StandardSaveStrategy.cs.
3. Replace:
     Server\World.cs
     Server\Persistence\StandardSaveStrategy.cs
4. Add:
     Scripts\Custom\Wolvesbane\WBTwiceDailyBackup.cs
5. Run:
     Compile.WIN - Release.bat
6. Start ServUO.exe.
7. Verify a normal manual save shows ONLY the normal save messages, not:
     WB Profile
     WB Stages
     WB Item Profile
8. Run:
     [WBBackup status
9. Run:
     [WBBackup now
10. Wait for the backup-completed console line.
11. Verify a new folder exists under:
      Backups\Wolvesbane\
    and contains a complete copy of Saves.

DISK SPACE
----------
This version intentionally does NOT delete older backups automatically.
Once the system is proven, we can add a retention rule (for example,
keep 14 or 30 days) if desired.

PRODUCTION
----------
Do NOT copy the temporary profiling core files (Phase 11/12) to production.

After Phase 16 is verified on the test shard, build a production deployment
from the final proven script files and the clean core source. A separate
production checklist/package should be used so the temporary audit/cleanup
scripts are not accidentally deployed.
