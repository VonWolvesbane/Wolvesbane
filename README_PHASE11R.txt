WOLVESBANE SAVE OPTIMIZATION - PHASE 11R
==========================================

WHAT CHANGED FROM PHASE 11
--------------------------
Phase 11 incorrectly included WBSaveProfileCommand.cs as a normal Script.
Scripts compile against the already-built ServUO.exe, so the command could not
see new members added to Server.World until the core executable itself had
been rebuilt.

Phase 11R removes that script completely.

The profiler now lives entirely in the ServUO core and is automatically ON
after a successful core rebuild. No in-game profiler command is required.

IMPORTANT: THIS IS A CORE REBUILD
---------------------------------
Replacing Server/*.cs source files is not enough. ServUO.exe must be rebuilt.

Your Wolvesbane source archive contains these root-level build files:
  Compile.WIN - Release.bat
  Compile.WIN - Debug.bat
  ServUO.sln

RECOMMENDED TEST INSTALL
------------------------
1. STOP the TEST server normally.

2. Make a copy of the current ServUO.exe and Saves folder.

3. Remove the old Phase 11 script if it still exists:
     Scripts\Custom\Wolvesbane\WBSaveProfileCommand.cs

4. Copy from this package:
     Server\World.cs
   over:
     <Wolvesbane>\Server\World.cs

5. Copy:
     Server\Persistence\StandardSaveStrategy.cs
   over:
     <Wolvesbane>\Server\Persistence\StandardSaveStrategy.cs

6. From the Wolvesbane root, run:
     Compile.WIN - Release.bat

   Let the build finish and verify that it reports a successful build.
   Do NOT continue if the core build has errors.

7. Start the newly rebuilt ServUO.exe.

8. You do NOT need to run [WBSaveProfile.
   Make 5 normal manual world saves.

WHAT YOU SHOULD SEE
-------------------
In the server console, each save will print lines beginning with:

  WB SAVE PROFILE:

Administrators in game will also receive lines similar to:

  WB Profile [Dual] Displayed ...
  WB Stages: Items ... | Mobiles ... | Guilds ... | Customs ...
  WB Outside displayed timer: BeforeHook ... | NetFlush/Pause ... | PriorWriteWait ...

Send screenshots of those lines for 5 saves.

WHAT IS TIMED
-------------
World save:
  BeforeWorldSave event
  NetState Flush/Pause
  wait for any previous background write
  selected SaveStrategy total
  WorldSave event
  displayed world-save stopwatch

Standard/Dual save stages:
  Items
  Mobiles
  Guilds
  CustomsFramework SaveData

DUAL STRATEGY NOTE
------------------
On Dual, Items run on a worker thread while Mobiles/Guilds/CustomData run on
the main save thread. Stage times therefore overlap and can add up to more
than the total save time. That is expected.

ROLLBACK
--------
The Rollback folder contains the original World.cs and
StandardSaveStrategy.cs from the Wolvesbane archive used for this project.

To roll back:
  1. Stop the server.
  2. Copy the files from Rollback back into their matching Server locations.
  3. Run Compile.WIN - Release.bat again.
  4. Start ServUO.exe.

SAFETY
------
This phase changes no save-data format and deletes/moves no world objects.
It adds timing instrumentation only.
