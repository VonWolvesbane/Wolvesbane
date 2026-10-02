WOLVESBANE MUSIC MANAGER - FIRST TEST

SERVER
1. Copy WolvesbaneMusicManager.cs into:
   Scripts/Custom/Wolvesbane/
2. Restart/recompile the server.

CLIENT
1. Back up your current Music/Digital/Config.txt.
2. Replace it with the included Config.txt.
3. The MP3 files themselves are already present in the Music.zip supplied for this build.
4. Start/restart ClassicUO after changing Config.txt.

TEST
[WBMusic
  Opens the GM music manager.

[WBMusic 111
  Plays Wolvesbane Reflection directly.

[WBMusicStop
  Stops the current test music.

REGISTERED NEW IDS
103 British vs Blackthorn.mp3
104 Death in Destard.mp3
105 The Ballad of Michael Kashmir #Ufffc.mp3
106 The Lore of Wolvesbane.mp3
107 The Price of the Strong.mp3
108 Town Travels.mp3
109 War mode.mp3
110 Wednesdays With Wolvesbane.mp3
111 Wolvesbane Reflection.mp3
112 Wolvesbane UO (1).mp3

NOTE
This first version deliberately uses a raw 0x6D packet locally instead of editing XmlSpawner or the server core.
