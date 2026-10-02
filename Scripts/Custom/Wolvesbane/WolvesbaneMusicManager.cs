using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Network;

namespace Server.Custom.Wolvesbane
{
    public enum WBMusicCategory { General, Town, Wilderness, Dungeon, Tavern, Boss, Event, Bard, Story, Ambient }

    public sealed class WBMusicTrack
    {
        public short ID;
        public string Name;
        public string FileName;
        public WBMusicCategory Category;
        public bool Enabled;

        public WBMusicTrack(short id, string name, string fileName, WBMusicCategory category, bool enabled)
        { ID=id; Name=name; FileName=fileName; Category=category; Enabled=enabled; }
    }

    public static class WolvesbaneMusicManager
    {
        private static readonly string SaveDirectory = Path.Combine(Core.BaseDirectory, "Saves", "Wolvesbane");
        private static readonly string SaveFile = Path.Combine(SaveDirectory, "MusicLibrary.xml");
        private static readonly List<WBMusicTrack> _Tracks = new List<WBMusicTrack>();
        public static List<WBMusicTrack> Tracks { get { EnsureLoaded(); return _Tracks; } }

        // Defensive lazy-load: some custom mobiles/gumps can be opened before the
        // WorldLoad callback has populated the library on certain ServUO startup orders.
        public static void EnsureLoaded()
        {
            if (_Tracks.Count > 0)
                return;

            Load();

            if (_Tracks.Count == 0)
            {
                AddDefaults();
                Save();
            }
        }

        public static void Initialize()
        {
            CommandSystem.Register("WBMusic", AccessLevel.GameMaster, OnMusicCommand);
            CommandSystem.Register("WBMusicStop", AccessLevel.GameMaster, delegate(CommandEventArgs e) { Stop(e.Mobile); });
            EventSink.WorldLoad += OnWorldLoad;
            EventSink.WorldSave += delegate(WorldSaveEventArgs e) { Save(); };
        }

        private static void OnWorldLoad() { EnsureLoaded(); }

        private static void AddDefaults()
        {
            _Tracks.Clear();
            _Tracks.Add(new WBMusicTrack(103,"British vs Blackthorn","BritishVsBlackthorn.mp3",WBMusicCategory.Story,true));
            _Tracks.Add(new WBMusicTrack(104,"Death in Destard","DeathInDestard.mp3",WBMusicCategory.Dungeon,true));
            _Tracks.Add(new WBMusicTrack(105,"The Ballad of Michael Kashmir #Ufffc","BalladOfMichaelKashmirUfffc.mp3",WBMusicCategory.Bard,true));
            _Tracks.Add(new WBMusicTrack(106,"The Lore of Wolvesbane","LoreOfWolvesbane.mp3",WBMusicCategory.Story,true));
            _Tracks.Add(new WBMusicTrack(107,"The Price of the Strong","PriceOfTheStrong.mp3",WBMusicCategory.Story,true));
            _Tracks.Add(new WBMusicTrack(108,"Town Travels","TownTravels.mp3",WBMusicCategory.Town,true));
            _Tracks.Add(new WBMusicTrack(109,"War Mode","WarMode.mp3",WBMusicCategory.Event,true));
            _Tracks.Add(new WBMusicTrack(110,"Wednesdays With Wolvesbane","WednesdaysWithWolvesbane.mp3",WBMusicCategory.Bard,true));
            _Tracks.Add(new WBMusicTrack(111,"Wolvesbane Reflection","WolvesbaneReflection.mp3",WBMusicCategory.Story,true));
            _Tracks.Add(new WBMusicTrack(112,"Wolvesbane UO","WolvesbaneUO1.mp3",WBMusicCategory.General,true));
        }

        private static void OnMusicCommand(CommandEventArgs e)
        {
            Mobile from=e.Mobile; if(from==null)return;
            if(e.Length>0)
            {
                short id; if(!Int16.TryParse(e.GetString(0),out id)){from.SendMessage(33,"Usage: [WBMusic <id> or [WBMusic");return;}
                WBMusicTrack t=Find(id); if(t==null){from.SendMessage(33,"Music ID {0} is not registered.",id);return;} Play(from,t); return;
            }
            Open(from,0,"",-1);
        }

        public static void Open(Mobile from,int page,string search,int category)
        { from.CloseGump(typeof(WolvesbaneMusicGump)); from.SendGump(new WolvesbaneMusicGump(page,search,category)); }

        public static WBMusicTrack Find(short id) { EnsureLoaded(); for(int i=0;i<_Tracks.Count;i++) if(_Tracks[i].ID==id)return _Tracks[i]; return null; }
        public static bool IDUsed(short id, WBMusicTrack except) { WBMusicTrack t=Find(id); return t!=null && t!=except; }
        public static short NextID() { for(short id=103;id<150;id++) if(Find(id)==null)return id; return -1; }

        public static void Play(Mobile m,WBMusicTrack t)
        { if(m==null||m.NetState==null||t==null||!t.Enabled)return; m.Send(new WBMusicPacket(t.ID)); m.SendMessage(68,"Playing {0}: {1}",t.ID,t.Name); }
        public static void PlaySilent(Mobile m, WBMusicTrack t)
        { if(m==null||m.NetState==null||t==null||!t.Enabled)return; m.Send(new WBMusicPacket(t.ID)); }
        public static void Stop(Mobile m) { if(m!=null&&m.NetState!=null)m.Send(new WBMusicPacket(0x1FFF)); }

        public static void Save()
        {
            try { if(!Directory.Exists(SaveDirectory))Directory.CreateDirectory(SaveDirectory);
                XmlWriterSettings s=new XmlWriterSettings();s.Indent=true;
                using(XmlWriter w=XmlWriter.Create(SaveFile,s)) { w.WriteStartElement("WolvesbaneMusicLibrary");w.WriteAttributeString("version","1");
                    foreach(WBMusicTrack t in _Tracks){w.WriteStartElement("Track");w.WriteAttributeString("id",t.ID.ToString());w.WriteAttributeString("name",t.Name??"");w.WriteAttributeString("file",t.FileName??"");w.WriteAttributeString("category",t.Category.ToString());w.WriteAttributeString("enabled",t.Enabled.ToString());w.WriteEndElement();} w.WriteEndElement(); }
            } catch(Exception ex){Console.WriteLine("WolvesbaneMusic Save: {0}",ex);}
        }

        private static void Load()
        {
            _Tracks.Clear(); if(!File.Exists(SaveFile))return;
            try { XmlDocument d=new XmlDocument();d.Load(SaveFile); foreach(XmlNode n in d.SelectNodes("/WolvesbaneMusicLibrary/Track")){
                    short id; bool enabled; WBMusicCategory cat; if(!Int16.TryParse(A(n,"id"),out id))continue; if(!Boolean.TryParse(A(n,"enabled"),out enabled))enabled=true; if(!Enum.TryParse<WBMusicCategory>(A(n,"category"),out cat))cat=WBMusicCategory.General;
                    _Tracks.Add(new WBMusicTrack(id,A(n,"name"),A(n,"file"),cat,enabled)); }
            } catch(Exception ex){Console.WriteLine("WolvesbaneMusic Load: {0}",ex);_Tracks.Clear();}
        }
        private static string A(XmlNode n,string a){return n.Attributes[a]==null?"":n.Attributes[a].Value;}

        public static bool SafeFileName(string f)
        { return !String.IsNullOrWhiteSpace(f) && f.EndsWith(".mp3",StringComparison.OrdinalIgnoreCase) && f.IndexOfAny(new char[]{' ', '\t', ','})<0; }

        private sealed class WBMusicPacket:Packet { public WBMusicPacket(short n):base(0x6D,3){UnderlyingStream.Write(n);} }
    }

    public class WolvesbaneMusicGump:Gump
    {
        private const int PerPage=9; private readonly int _Page; private readonly string _Search; private readonly int _Category;
        public WolvesbaneMusicGump(int page,string search,int category):base(80,50)
        {
            _Page=page<0?0:page;_Search=search??"";_Category=category; Closable=true;Dragable=true;
            List<WBMusicTrack> list=Filtered(); int pages=Math.Max(1,(list.Count+PerPage-1)/PerPage); if(_Page>=pages)_Page=pages-1;
            AddBackground(0,0,720,485,9270);AddAlphaRegion(12,12,696,461);AddLabel(235,18,1152,"WOLVESBANE MUSIC MANAGER");
            AddLabel(25,50,68,"Search:");AddTextEntry(85,48,250,22,1152,1,_Search);AddButton(345,48,4005,4007,10,GumpButtonType.Reply,0);AddLabel(380,50,68,"Search");
            AddButton(475,48,4005,4007,20,GumpButtonType.Reply,0);AddLabel(510,50,68,"Add Track");AddButton(610,48,4017,4019,30,GumpButtonType.Reply,0);AddLabel(645,50,33,"Stop");
            AddLabel(25,80,68,"ID");AddLabel(70,80,68,"Track");AddLabel(365,80,68,"Category");AddLabel(470,80,68,"Play");AddLabel(545,80,68,"Edit");AddLabel(615,80,68,"On");
            int start=_Page*PerPage,y=105; for(int i=start;i<list.Count&&i<start+PerPage;i++,y+=34){WBMusicTrack t=list[i];int master=WolvesbaneMusicManager.Tracks.IndexOf(t);
                AddLabel(25,y+3,t.Enabled?1152:946,t.ID.ToString());AddLabel(70,y+3,t.Enabled?1152:946,t.Name);AddLabel(365,y+3,68,t.Category.ToString());
                AddButton(470,y,4005,4007,1000+master,GumpButtonType.Reply,0);AddButton(545,y,4011,4013,2000+master,GumpButtonType.Reply,0);AddLabel(615,y+3,t.Enabled?68:33,t.Enabled?"Yes":"No");}
            if(_Page>0){AddButton(25,430,4014,4016,40,GumpButtonType.Reply,0);AddLabel(60,432,68,"Previous");} if(_Page<pages-1){AddButton(590,430,4005,4007,41,GumpButtonType.Reply,0);AddLabel(625,432,68,"Next");} AddLabel(310,432,68,String.Format("Page {0}/{1}",_Page+1,pages));
        }
        private List<WBMusicTrack> Filtered(){List<WBMusicTrack> r=new List<WBMusicTrack>();foreach(WBMusicTrack t in WolvesbaneMusicManager.Tracks){if(_Category>=0&&(int)t.Category!=_Category)continue;if(_Search.Length>0&&(t.Name??"").IndexOf(_Search,StringComparison.OrdinalIgnoreCase)<0&&(t.FileName??"").IndexOf(_Search,StringComparison.OrdinalIgnoreCase)<0)continue;r.Add(t);}return r;}
        public override void OnResponse(NetState s,RelayInfo i){Mobile m=s.Mobile;if(m==null)return;string search=i.GetTextEntry(1)==null?_Search:i.GetTextEntry(1).Text;
            if(i.ButtonID==10){WolvesbaneMusicManager.Open(m,0,search,_Category);return;} if(i.ButtonID==20){m.SendGump(new WolvesbaneMusicEditGump(null,_Page,search,_Category));return;} if(i.ButtonID==30){WolvesbaneMusicManager.Stop(m);WolvesbaneMusicManager.Open(m,_Page,search,_Category);return;} if(i.ButtonID==40){WolvesbaneMusicManager.Open(m,_Page-1,search,_Category);return;}if(i.ButtonID==41){WolvesbaneMusicManager.Open(m,_Page+1,search,_Category);return;}
            if(i.ButtonID>=1000&&i.ButtonID<2000){int x=i.ButtonID-1000;if(x>=0&&x<WolvesbaneMusicManager.Tracks.Count)WolvesbaneMusicManager.Play(m,WolvesbaneMusicManager.Tracks[x]);WolvesbaneMusicManager.Open(m,_Page,search,_Category);}
            else if(i.ButtonID>=2000&&i.ButtonID<3000){int x=i.ButtonID-2000;if(x>=0&&x<WolvesbaneMusicManager.Tracks.Count)m.SendGump(new WolvesbaneMusicEditGump(WolvesbaneMusicManager.Tracks[x],_Page,search,_Category));}
        }
    }

    public class WolvesbaneMusicEditGump:Gump
    {
        private readonly WBMusicTrack _Track;private readonly int _Page;private readonly string _Search;private readonly int _Filter;
        public WolvesbaneMusicEditGump(WBMusicTrack track,int page,string search,int filter):base(180,100){_Track=track;_Page=page;_Search=search;_Filter=filter;short id=track==null?WolvesbaneMusicManager.NextID():track.ID;
            AddBackground(0,0,560,390,9270);AddAlphaRegion(12,12,536,366);AddLabel(185,20,1152,track==null?"ADD MUSIC TRACK":"EDIT MUSIC TRACK");
            AddLabel(30,65,68,"Music ID:");AddTextEntry(150,63,100,22,1152,1,id.ToString());AddLabel(30,105,68,"Display Name:");AddTextEntry(150,103,360,22,1152,2,track==null?"":track.Name);
            AddLabel(30,145,68,"MP3 Filename:");AddTextEntry(150,143,360,22,1152,3,track==null?"":track.FileName);AddLabel(150,169,946,"No spaces, commas or tabs. Example: MySong.mp3");
            AddLabel(30,205,68,"Category:");int y=203;foreach(WBMusicCategory c in Enum.GetValues(typeof(WBMusicCategory))){if((int)c==(track==null?0:(int)track.Category))AddLabel(150,y,1152,c.ToString());}
            AddButton(150,240,4005,4007,10,GumpButtonType.Reply,0);AddLabel(185,242,68,"Cycle Category");AddLabel(325,242,1152,(track==null?WBMusicCategory.General:track.Category).ToString());
            AddButton(150,290,4005,4007,20,GumpButtonType.Reply,0);AddLabel(185,292,68,"Save");AddButton(300,290,4017,4019,30,GumpButtonType.Reply,0);AddLabel(335,292,33,"Cancel");
            if(track!=null){AddButton(150,330,4005,4007,40,GumpButtonType.Reply,0);AddLabel(185,332,track.Enabled?68:33,track.Enabled?"Disable Track":"Enable Track");AddButton(360,330,4020,4022,50,GumpButtonType.Reply,0);AddLabel(395,332,33,"Delete");}}
        private string E(RelayInfo i,int n,string d){TextRelay t=i.GetTextEntry(n);return t==null?d:(t.Text??"").Trim();}
        public override void OnResponse(NetState s,RelayInfo i){Mobile m=s.Mobile;if(m==null)return;if(i.ButtonID==30){WolvesbaneMusicManager.Open(m,_Page,_Search,_Filter);return;}if(i.ButtonID==40&&_Track!=null){_Track.Enabled=!_Track.Enabled;WolvesbaneMusicManager.Save();m.SendGump(new WolvesbaneMusicEditGump(_Track,_Page,_Search,_Filter));return;}if(i.ButtonID==50&&_Track!=null){WolvesbaneMusicManager.Tracks.Remove(_Track);WolvesbaneMusicManager.Save();WolvesbaneMusicManager.Open(m,_Page,_Search,_Filter);return;}
            // Category cycling intentionally uses the existing value and reopens; saving uses current track category.
            if(i.ButtonID==10){if(_Track!=null)_Track.Category=(WBMusicCategory)(((int)_Track.Category+1)%Enum.GetValues(typeof(WBMusicCategory)).Length);m.SendGump(new WolvesbaneMusicEditGump(_Track,_Page,_Search,_Filter));return;}
            if(i.ButtonID==20){short id;if(!Int16.TryParse(E(i,1,""),out id)||id<0||id>=150){m.SendMessage(33,"Music ID must be between 0 and 149.");m.SendGump(new WolvesbaneMusicEditGump(_Track,_Page,_Search,_Filter));return;}if(WolvesbaneMusicManager.IDUsed(id,_Track)){m.SendMessage(33,"Music ID {0} is already in use.",id);m.SendGump(new WolvesbaneMusicEditGump(_Track,_Page,_Search,_Filter));return;}string name=E(i,2,"");string file=E(i,3,"");if(String.IsNullOrWhiteSpace(name)||!WolvesbaneMusicManager.SafeFileName(file)){m.SendMessage(33,"Enter a name and a CUO-safe .mp3 filename with no spaces, commas, or tabs.");m.SendGump(new WolvesbaneMusicEditGump(_Track,_Page,_Search,_Filter));return;}WBMusicTrack t=_Track;if(t==null){t=new WBMusicTrack(id,name,file,WBMusicCategory.General,true);WolvesbaneMusicManager.Tracks.Add(t);}else{t.ID=id;t.Name=name;t.FileName=file;}WolvesbaneMusicManager.Save();m.SendMessage(68,"Music track saved. Remember: the MP3 and Config.txt entry must also exist on each client.");WolvesbaneMusicManager.Open(m,_Page,_Search,_Filter);}
        }
    }
}
