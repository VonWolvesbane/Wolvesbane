using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Custom.Wolvesbane
{
    public enum WBBardPlayMode { Single, Sequential, Random }

    public class WolvesbaneMusicBard : BaseCreature
    {
        private static readonly Dictionary<Mobile, WolvesbaneMusicBard> _ActiveListeners = new Dictionary<Mobile, WolvesbaneMusicBard>();

        public static bool IsControlling(Mobile m)
        {
            WolvesbaneMusicBard bard;
            return m != null && _ActiveListeners.TryGetValue(m, out bard) && bard != null && !bard.Deleted;
        }
        private int _Radius = 12;
        private WBBardPlayMode _Mode = WBBardPlayMode.Single;
        private List<short> _Playlist = new List<short>();
        private int _SequenceIndex;
        private Timer _Timer;
        private Dictionary<Mobile, short> _Listeners = new Dictionary<Mobile, short>();

        [CommandProperty(AccessLevel.GameMaster)] public int MusicRadius { get { return _Radius; } set { _Radius = Math.Max(1, Math.Min(30, value)); } }
        [CommandProperty(AccessLevel.GameMaster)] public WBBardPlayMode PlayMode { get { return _Mode; } set { _Mode = value; } }

        [Constructable]
        public WolvesbaneMusicBard() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "a Wolvesbane bard";
            Title = "the minstrel";
            Blessed = true;
            CantWalk = true;
            Race = Race.Human;
            Female = false;
            Body = 0x190;
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();
            InitStats(100, 100, 100);
            StartTimer();
        }

        public WolvesbaneMusicBard(Serial serial) : base(serial) { }
        public override bool IsInvulnerable { get { return true; } }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel >= AccessLevel.GameMaster)
            {
                from.CloseGump(typeof(WolvesbaneBardGump));
                from.SendGump(new WolvesbaneBardGump(this));
                DisplayPaperdollTo(from);
            }
            else
                from.SendMessage(68, "{0} is performing music for this area.", Name);
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (from != null && from.AccessLevel >= AccessLevel.GameMaster && dropped != null)
            {
                if (EquipItem(dropped))
                {
                    from.SendMessage(68, "You equip {0} on {1}.", dropped.Name ?? dropped.GetType().Name, Name);
                    return true;
                }
                from.SendMessage(33, "That item cannot be equipped on this bard in its current form.");
                return false;
            }
            return base.OnDragDrop(from, dropped);
        }

        public List<short> Playlist { get { return _Playlist; } }
        public void AddTrack(short id) { if (!_Playlist.Contains(id) && WolvesbaneMusicManager.Find(id) != null) _Playlist.Add(id); }
        public void RemoveTrack(short id) { _Playlist.Remove(id); }
        public void ClearTracks() { _Playlist.Clear(); }

        public void ApplyAppearance(Race race, bool female, bool randomHair)
        {
            Race = race ?? Race.Human;
            Female = female;
            if (Race == Race.Elf) Body = female ? 0x25E : 0x25D;
            else if (Race == Race.Gargoyle) Body = female ? 0x29B : 0x29A;
            else Body = female ? 0x191 : 0x190;
            Hue = Race.RandomSkinHue();
            if (randomHair)
            {
                HairItemID = Race.RandomHair(Female);
                HairHue = Race.RandomHairHue();
                FacialHairItemID = Female ? 0 : Race.RandomFacialHair(Female);
                FacialHairHue = HairHue;
            }
        }

        private void StartTimer()
        {
            if (_Timer != null) _Timer.Stop();
            _Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), Tick);
        }

        private void Tick()
        {
            if (Deleted || Map == null || Map == Map.Internal) return;
            List<Mobile> now = new List<Mobile>();
            IPooledEnumerable eable = GetMobilesInRange(_Radius);
            foreach (Mobile m in eable)
                if (m is PlayerMobile && m.NetState != null && m.Alive && m.AccessLevel == AccessLevel.Player) now.Add(m);
            eable.Free();

            for (int i = 0; i < now.Count; i++)
            {
                Mobile m = now[i];
                if (!_Listeners.ContainsKey(m)) StartFor(m);
            }

            List<Mobile> old = new List<Mobile>(_Listeners.Keys);
            for (int i = 0; i < old.Count; i++)
            {
                Mobile m = old[i];
                if (m == null || m.Deleted || !now.Contains(m))
                {
                    _Listeners.Remove(m);
                    WolvesbaneMusicBard owner;
                    if (_ActiveListeners.TryGetValue(m, out owner) && owner == this)
                    {
                        _ActiveListeners.Remove(m);
                        WolvesbaneRegionMusicManager.RestoreFor(m);
                    }
                }
            }
        }

        private short ChooseTrack()
        {
            List<short> valid = new List<short>();
            for (int i = 0; i < _Playlist.Count; i++) { WBMusicTrack t = WolvesbaneMusicManager.Find(_Playlist[i]); if (t != null && t.Enabled) valid.Add(t.ID); }
            if (valid.Count == 0) return -1;
            if (_Mode == WBBardPlayMode.Random) return valid[Utility.Random(valid.Count)];
            if (_Mode == WBBardPlayMode.Sequential) { if (_SequenceIndex >= valid.Count) _SequenceIndex = 0; return valid[_SequenceIndex++]; }
            return valid[0];
        }

        private void StartFor(Mobile m)
        {
            short id = ChooseTrack();
            if (id < 0) return;
            WBMusicTrack t = WolvesbaneMusicManager.Find(id);
            if (t == null) return;
            WolvesbaneMusicManager.PlaySilent(m, t);
            _Listeners[m] = id;
            _ActiveListeners[m] = this;
        }

        public void Preview(Mobile m)
        {
            short id = ChooseTrack(); WBMusicTrack t = WolvesbaneMusicManager.Find(id);
            if (t != null) WolvesbaneMusicManager.Play(m, t);
        }

        public override void OnDelete()
        {
            if (_Timer != null) _Timer.Stop();
            foreach (Mobile m in new List<Mobile>(_Listeners.Keys))
            {
                WolvesbaneMusicBard owner;
                if (_ActiveListeners.TryGetValue(m, out owner) && owner == this)
                {
                    _ActiveListeners.Remove(m);
                    WolvesbaneRegionMusicManager.RestoreFor(m);
                }
            }
            _Listeners.Clear();
            base.OnDelete();
        }

        public override void Serialize(GenericWriter w)
        {
            base.Serialize(w); w.Write(1); w.Write(_Radius); w.Write((int)_Mode); w.Write(_Playlist.Count);
            for (int i = 0; i < _Playlist.Count; i++) w.Write((int)_Playlist[i]);
        }
        public override void Deserialize(GenericReader r)
        {
            base.Deserialize(r); int v = r.ReadInt(); _Radius = r.ReadInt(); _Mode = (WBBardPlayMode)r.ReadInt(); int c = r.ReadInt(); _Playlist = new List<short>();
            for (int i = 0; i < c; i++) _Playlist.Add((short)r.ReadInt()); _Listeners = new Dictionary<Mobile, short>(); StartTimer();
        }
    }

    public class WolvesbaneBardGump : Gump
    {
        private readonly WolvesbaneMusicBard _Bard;
        private readonly int _Page;
        private const int TracksPerPage = 7;

        public WolvesbaneBardGump(WolvesbaneMusicBard bard) : this(bard, 0) { }

        public WolvesbaneBardGump(WolvesbaneMusicBard bard, int page) : base(120, 55)
        {
            _Bard = bard;
            WolvesbaneMusicManager.EnsureLoaded();
            int enabledCount = 0;
            for (int n = 0; n < WolvesbaneMusicManager.Tracks.Count; n++)
                if (WolvesbaneMusicManager.Tracks[n].Enabled) enabledCount++;
            int pages = Math.Max(1, (enabledCount + TracksPerPage - 1) / TracksPerPage);
            _Page = Math.Max(0, Math.Min(page, pages - 1));
            AddBackground(0,0,650,520,9270); AddAlphaRegion(12,12,626,496); AddLabel(205,18,1152,"WOLVESBANE BARD MANAGER");
            AddLabel(25,55,68,"Name:"); AddTextEntry(100,53,250,22,1152,1,bard.Name ?? "");
            AddLabel(25,88,68,"Title:"); AddTextEntry(100,86,250,22,1152,2,bard.Title ?? "");
            AddLabel(390,55,68,"Radius:"); AddTextEntry(465,53,55,22,1152,3,bard.MusicRadius.ToString());
            AddButton(25,125,4005,4007,10,GumpButtonType.Reply,0); AddLabel(60,127,68,"Cycle Sex"); AddLabel(160,127,1152,bard.Female?"Female":"Male");
            AddButton(250,125,4005,4007,11,GumpButtonType.Reply,0); AddLabel(285,127,68,"Cycle Race"); AddLabel(390,127,1152,bard.Race == null ? "Human" : bard.Race.Name);
            AddButton(25,160,4005,4007,12,GumpButtonType.Reply,0); AddLabel(60,162,68,"Randomize Hair / Skin");
            AddButton(250,160,4005,4007,13,GumpButtonType.Reply,0); AddLabel(285,162,68,"Open Paperdoll");
            AddButton(25,195,4005,4007,14,GumpButtonType.Reply,0); AddLabel(60,197,68,"Cycle Play Mode"); AddLabel(190,197,1152,bard.PlayMode.ToString());
            AddButton(250,195,4005,4007,15,GumpButtonType.Reply,0); AddLabel(285,197,68,"Preview"); AddButton(390,195,4017,4019,16,GumpButtonType.Reply,0); AddLabel(425,197,33,"Stop Preview");
            AddLabel(25,235,68,"Playlist (click track to add/remove):");
            int y=265; int shown=0; int skip=_Page*TracksPerPage; int seen=0;
            for(int x=0;x<WolvesbaneMusicManager.Tracks.Count && shown<TracksPerPage;x++) { WBMusicTrack t=WolvesbaneMusicManager.Tracks[x]; if(!t.Enabled)continue; if(seen++<skip)continue; bool on=bard.Playlist.Contains(t.ID); AddButton(25,y, on?4023:4005, on?4025:4007,1000+x,GumpButtonType.Reply,0); AddLabel(60,y+2,on?68:1152,String.Format("{0}  {1}",t.ID,t.Name)); y+=30; shown++; }
            if(_Page>0){AddButton(25,450,4014,4016,30,GumpButtonType.Reply,0);AddLabel(60,452,68,"Previous");}
            if((_Page+1)*TracksPerPage<enabledCount){AddButton(145,450,4005,4007,31,GumpButtonType.Reply,0);AddLabel(180,452,68,"Next");}
            AddLabel(275,452,946,String.Format("Page {0}/{1}",_Page+1,pages));
            if(enabledCount==0)AddLabel(25,265,33,"No enabled tracks found in the central music library.");
            AddLabel(390,235,68,"Selected:"); y=260; for(int x=0;x<bard.Playlist.Count && x<7;x++){WBMusicTrack t=WolvesbaneMusicManager.Find(bard.Playlist[x]);AddLabel(390,y,1152,t==null?bard.Playlist[x].ToString():t.Name);y+=25;}
            AddButton(25,480,4005,4007,20,GumpButtonType.Reply,0); AddLabel(60,482,68,"Save / Refresh"); AddButton(210,480,4017,4019,21,GumpButtonType.Reply,0); AddLabel(245,482,33,"Clear Playlist");
            AddLabel(390,455,946,"GM+: drag equipment onto the bard"); AddLabel(390,477,946,"or use the paperdoll to restyle it.");
        }
        private string E(RelayInfo i,int n,string d){TextRelay t=i.GetTextEntry(n);return t==null?d:(t.Text??"").Trim();}
        private void ApplyText(RelayInfo i){_Bard.Name=E(i,1,_Bard.Name);_Bard.Title=E(i,2,_Bard.Title);int r;if(Int32.TryParse(E(i,3,_Bard.MusicRadius.ToString()),out r))_Bard.MusicRadius=r;}
        public override void OnResponse(NetState s,RelayInfo i)
        {
            Mobile m=s.Mobile;if(m==null||_Bard==null||_Bard.Deleted||m.AccessLevel<AccessLevel.GameMaster)return; ApplyText(i);
            if(i.ButtonID==10){_Bard.ApplyAppearance(_Bard.Race,!_Bard.Female,true);}
            else if(i.ButtonID==11){Race r=_Bard.Race==Race.Human?Race.Elf:(_Bard.Race==Race.Elf?Race.Gargoyle:Race.Human);_Bard.ApplyAppearance(r,_Bard.Female,true);}
            else if(i.ButtonID==12){_Bard.ApplyAppearance(_Bard.Race,_Bard.Female,true);}
            else if(i.ButtonID==13){_Bard.DisplayPaperdollTo(m);}
            else if(i.ButtonID==14){_Bard.PlayMode=(WBBardPlayMode)(((int)_Bard.PlayMode+1)%3);}
            else if(i.ButtonID==15){_Bard.Preview(m);}
            else if(i.ButtonID==16){WolvesbaneMusicManager.Stop(m);}
            else if(i.ButtonID==21){_Bard.ClearTracks();}
            else if(i.ButtonID==30){m.CloseGump(typeof(WolvesbaneBardGump));m.SendGump(new WolvesbaneBardGump(_Bard,_Page-1));return;}
            else if(i.ButtonID==31){m.CloseGump(typeof(WolvesbaneBardGump));m.SendGump(new WolvesbaneBardGump(_Bard,_Page+1));return;}
            else if(i.ButtonID>=1000){int x=i.ButtonID-1000;if(x>=0&&x<WolvesbaneMusicManager.Tracks.Count){short id=WolvesbaneMusicManager.Tracks[x].ID;if(_Bard.Playlist.Contains(id))_Bard.RemoveTrack(id);else _Bard.AddTrack(id);}}
            if(i.ButtonID!=0){m.CloseGump(typeof(WolvesbaneBardGump));m.SendGump(new WolvesbaneBardGump(_Bard,_Page));}
        }
    }
}
