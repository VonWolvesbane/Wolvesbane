using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Regions;

namespace Server.Custom.Wolvesbane
{
    public sealed class WBRegionMusicOverride
    {
        public int ControllerSerial;
        public string RegionName;
        public string MapName;
        public short TrackID;
        public bool Enabled;
    }

    public static class WolvesbaneRegionMusicManager
    {
        private static readonly string SaveDirectory = Path.Combine(Core.BaseDirectory, "Saves", "Wolvesbane");
        private static readonly string SaveFile = Path.Combine(SaveDirectory, "RegionMusic.xml");
        private static readonly Dictionary<int, WBRegionMusicOverride> _Overrides = new Dictionary<int, WBRegionMusicOverride>();
        private static bool _Loaded;

        public static void Initialize()
        {
            CommandSystem.Register("WBRegionMusic", AccessLevel.GameMaster, OnCommand);
            EventSink.WorldLoad += Load;
            EventSink.WorldSave += delegate(WorldSaveEventArgs e) { Save(); };
        }

        private static void EnsureLoaded() { if (!_Loaded) Load(); }

        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from == null) return;
            CustomRegion region = from.Region as CustomRegion;
            if (region == null || region.Controller == null || region.Controller.Deleted)
            {
                from.SendMessage(33, "You must be standing inside one of the Custom Region system regions.");
                return;
            }
            Open(from, region.Controller, 0);
        }

        public static void Open(Mobile from, RegionControl controller, int page)
        {
            if (from == null || controller == null || controller.Deleted) return;
            EnsureLoaded(); WolvesbaneMusicManager.EnsureLoaded();
            from.CloseGump(typeof(WolvesbaneRegionMusicGump));
            from.SendGump(new WolvesbaneRegionMusicGump(controller, page));
        }

        public static WBRegionMusicOverride Get(RegionControl c)
        {
            EnsureLoaded(); if (c == null) return null;
            WBRegionMusicOverride o; _Overrides.TryGetValue(c.Serial.Value, out o); return o;
        }

        public static void Set(RegionControl c, short trackID)
        {
            if (c == null) return;
            WBMusicTrack t = WolvesbaneMusicManager.Find(trackID); if (t == null || !t.Enabled) return;
            WBRegionMusicOverride o = new WBRegionMusicOverride();
            o.ControllerSerial = c.Serial.Value; o.RegionName = c.RegionName ?? ""; o.MapName = c.Map == null ? "" : c.Map.Name; o.TrackID = trackID; o.Enabled = true;
            _Overrides[c.Serial.Value] = o; Save();
        }

        public static void Remove(RegionControl c)
        { if (c != null) { EnsureLoaded(); _Overrides.Remove(c.Serial.Value); Save(); } }

        public static void OnEnter(Mobile m, CustomRegion region)
        {
            if (m == null || m.NetState == null || region == null || WolvesbaneMusicBard.IsControlling(m)) return;
            PlayOverride(m, region.Controller);
        }

        public static void OnExit(Mobile m, CustomRegion oldRegion)
        {
            if (m == null || m.NetState == null) return;
            Timer.DelayCall(TimeSpan.Zero, delegate { RestoreFor(m); });
        }

        public static void RestoreFor(Mobile m)
        {
            if (m == null || m.Deleted || m.NetState == null || WolvesbaneMusicBard.IsControlling(m)) return;
            CustomRegion cr = m.Region as CustomRegion;
            if (cr != null && PlayOverride(m, cr.Controller)) return;

            // No Wolvesbane override: restore the normal music belonging to the region the player is now in.
            if (m.Region != null)
                m.Send(new PlayMusic(m.Region.Music));
            else
                WolvesbaneMusicManager.Stop(m);
        }

        private static bool PlayOverride(Mobile m, RegionControl c)
        {
            WBRegionMusicOverride o = Get(c); if (o == null || !o.Enabled) return false;
            WBMusicTrack t = WolvesbaneMusicManager.Find(o.TrackID); if (t == null || !t.Enabled) return false;
            WolvesbaneMusicManager.PlaySilent(m, t); return true;
        }

        public static void Save()
        {
            EnsureLoaded();
            try
            {
                if (!Directory.Exists(SaveDirectory)) Directory.CreateDirectory(SaveDirectory);
                XmlWriterSettings s = new XmlWriterSettings(); s.Indent = true;
                using (XmlWriter w = XmlWriter.Create(SaveFile, s))
                {
                    w.WriteStartElement("WolvesbaneRegionMusic"); w.WriteAttributeString("version", "1");
                    foreach (WBRegionMusicOverride o in _Overrides.Values)
                    {
                        w.WriteStartElement("Region"); w.WriteAttributeString("serial", o.ControllerSerial.ToString()); w.WriteAttributeString("name", o.RegionName ?? ""); w.WriteAttributeString("map", o.MapName ?? ""); w.WriteAttributeString("track", o.TrackID.ToString()); w.WriteAttributeString("enabled", o.Enabled.ToString()); w.WriteEndElement();
                    }
                    w.WriteEndElement();
                }
            }
            catch (Exception ex) { Console.WriteLine("WolvesbaneRegionMusic Save: {0}", ex); }
        }

        private static void Load()
        {
            _Overrides.Clear(); _Loaded = true; if (!File.Exists(SaveFile)) return;
            try
            {
                XmlDocument d = new XmlDocument(); d.Load(SaveFile);
                foreach (XmlNode n in d.SelectNodes("/WolvesbaneRegionMusic/Region"))
                {
                    int serial; short track; bool enabled;
                    if (!Int32.TryParse(A(n,"serial"), out serial) || !Int16.TryParse(A(n,"track"), out track)) continue;
                    if (!Boolean.TryParse(A(n,"enabled"), out enabled)) enabled = true;
                    WBRegionMusicOverride o = new WBRegionMusicOverride(); o.ControllerSerial=serial; o.RegionName=A(n,"name"); o.MapName=A(n,"map"); o.TrackID=track; o.Enabled=enabled; _Overrides[serial]=o;
                }
            }
            catch (Exception ex) { Console.WriteLine("WolvesbaneRegionMusic Load: {0}", ex); _Overrides.Clear(); }
        }
        private static string A(XmlNode n,string a){return n.Attributes[a]==null?"":n.Attributes[a].Value;}
    }

    public class WolvesbaneRegionMusicGump : Gump
    {
        private const int PerPage = 7;
        private readonly RegionControl _Controller;
        private readonly int _Page;

        public WolvesbaneRegionMusicGump(RegionControl c, int page) : base(120, 55)
        {
            _Controller=c; WolvesbaneMusicManager.EnsureLoaded();
            List<WBMusicTrack> tracks = EnabledTracks(); int pages=Math.Max(1,(tracks.Count+PerPage-1)/PerPage); _Page=Math.Max(0,Math.Min(page,pages-1));
            WBRegionMusicOverride current=WolvesbaneRegionMusicManager.Get(c); WBMusicTrack selected=current==null?null:WolvesbaneMusicManager.Find(current.TrackID);
            AddBackground(0,0,650,485,9270); AddAlphaRegion(12,12,626,461); AddLabel(190,18,1152,"WOLVESBANE REGION MUSIC");
            AddLabel(25,55,68,"Region:"); AddLabel(100,55,1152,c.RegionName??"(unnamed)"); AddLabel(390,55,68,"Map:"); AddLabel(435,55,1152,c.Map==null?"None":c.Map.Name);
            AddLabel(25,85,68,"Override:"); AddLabel(100,85,current!=null?68:946,current==null?"Default UO region music":String.Format("{0} - {1}",current.TrackID,selected==null?"Missing/disabled track":selected.Name));
            AddLabel(25,125,68,"Choose a track from the Wolvesbane Music Library:");
            int start=_Page*PerPage,y=155; for(int x=start;x<tracks.Count&&x<start+PerPage;x++,y+=36){WBMusicTrack t=tracks[x];bool on=current!=null&&current.TrackID==t.ID;AddButton(25,y,on?4023:4005,on?4025:4007,1000+x,GumpButtonType.Reply,0);AddLabel(60,y+3,on?68:1152,String.Format("{0}  {1}",t.ID,t.Name));AddLabel(455,y+3,946,t.Category.ToString());}
            if(_Page>0){AddButton(25,420,4014,4016,20,GumpButtonType.Reply,0);AddLabel(60,422,68,"Previous");} if(_Page<pages-1){AddButton(145,420,4005,4007,21,GumpButtonType.Reply,0);AddLabel(180,422,68,"Next");} AddLabel(280,422,946,String.Format("Page {0}/{1}",_Page+1,pages));
            AddButton(390,415,4017,4019,30,GumpButtonType.Reply,0);AddLabel(425,417,33,"Use Default / Remove Override");
            AddLabel(25,455,946,"Selecting a track saves immediately. Bard music has priority while a player is in bard range.");
        }
        private List<WBMusicTrack> EnabledTracks(){List<WBMusicTrack> r=new List<WBMusicTrack>();foreach(WBMusicTrack t in WolvesbaneMusicManager.Tracks)if(t.Enabled)r.Add(t);return r;}
        public override void OnResponse(NetState s, RelayInfo i)
        {
            Mobile m=s.Mobile;if(m==null||m.AccessLevel<AccessLevel.GameMaster||_Controller==null||_Controller.Deleted)return;
            if(i.ButtonID==20){WolvesbaneRegionMusicManager.Open(m,_Controller,_Page-1);return;} if(i.ButtonID==21){WolvesbaneRegionMusicManager.Open(m,_Controller,_Page+1);return;}
            if(i.ButtonID==30){WolvesbaneRegionMusicManager.Remove(_Controller);m.SendMessage(68,"Custom music override removed from {0}.",_Controller.RegionName);WolvesbaneRegionMusicManager.RestoreFor(m);WolvesbaneRegionMusicManager.Open(m,_Controller,_Page);return;}
            if(i.ButtonID>=1000){List<WBMusicTrack> tracks=EnabledTracks();int x=i.ButtonID-1000;if(x>=0&&x<tracks.Count){WolvesbaneRegionMusicManager.Set(_Controller,tracks[x].ID);m.SendMessage(68,"{0} now uses {1} ({2}).",_Controller.RegionName,tracks[x].Name,tracks[x].ID);WolvesbaneRegionMusicManager.RestoreFor(m);}WolvesbaneRegionMusicManager.Open(m,_Controller,_Page);}
        }
    }
}
