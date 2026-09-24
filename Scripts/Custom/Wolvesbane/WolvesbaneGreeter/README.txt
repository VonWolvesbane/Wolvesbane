WOLVESBANE GREETER SYSTEM
=========================

FILES
-----
WolvesbaneGreeter.cs        - The actual NPC
WolvesbaneGreeterGump.cs    - Double-click newcomer/help gump
WolvesbaneGreeterConfig.cs  - Loads editable config + [ReloadGreeter command
WolvesbaneGreeter.cfg       - Editable live content

INSTALL
-------
1. Create a folder such as:
   Scripts/Custom/Wolvesbane/Greeter/

2. Put these three .cs files in that folder:
   WolvesbaneGreeter.cs
   WolvesbaneGreeterGump.cs
   WolvesbaneGreeterConfig.cs

3. Put WolvesbaneGreeter.cfg here:
   Data/WolvesbaneGreeter.cfg

4. Restart the server once so the scripts compile.

5. Spawn the NPC using:
   [add WolvesbaneGreeter

LIVE EDITING
------------
Edit Data/WolvesbaneGreeter.cfg while the server is running.
Then use:
   [ReloadGreeter

The NPC does NOT have to be deleted or recreated after changing help text/tips.

DRESSING THE NPC
----------------
GM+ staff can drag wearable clothing/equipment directly onto the Greeter.
If an item already occupies that layer, the old item is moved into the Greeter's backpack.

The outfit is serialized normally with the NPC, so the clothes remain after a world save/restart.

GM PROPERTIES
-------------
[props the NPC to change:
   MinTipDelay
   MaxTipDelay

Defaults are 2 to 5 minutes between spoken tips.

CONFIG FORMAT
-------------
[SpeechTips]
One spoken phrase per line.

[Page:Page Name]
Each following line is shown as help text until the next [Page:...] header.

Lines beginning with # or // are comments.

NOTES
-----
The gump currently displays up to 14 section buttons. If Wolvesbane eventually needs more,
we can add page navigation or category/subcategory menus without changing the config format.
