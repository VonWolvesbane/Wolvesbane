//
// 05-aug-2017 v1.1 copy function, and repeating placement
// xx-xxx-xxxx v1.0 first version released by Nika
//                  - configureable size (set W/H for size of grid and T for size of each items
//                  - click a item to get a static/item of it
// xx-xxx-xxxx      Written by Haazen Feb 2011
//
using System;
using System.Collections;
using System.IO;
using Server;
using Server.Commands;
using Server.Items;
using Server.Network;
using Server.Prompts;
using Server.Multis;
using Server.Targeting;
using Server.Gumps;

namespace ShowArt
{
  //move to world target
  public class ShowartTarget: Target {
    private int _id;
    private bool _static;
    public ShowartTarget(int id,bool stat) : base(-1, true, TargetFlags.None)  {
      _id=id;
      _static=stat;
      CheckLOS = false;
      //AllowGround  ==true
      DisallowMultis = false;
      //Range = 30; == -1
    }
    Item getItem(){
        if (_static) return new Static(_id);
        return new Item(_id);
    }
    protected override void OnTarget(Mobile from, object o) {
      if (o is LandTarget) {
        LandTarget l = o as LandTarget;
        getItem().MoveToWorld(l.Location, from.Map);
        from.Target=this;
        return;
      } 
      if (o is StaticTarget) {
        StaticTarget l = o as StaticTarget;
        getItem().MoveToWorld(l.Location, from.Map);
        from.Target=this;
        return;
      } 
      if (o is Mobile) {
        ((Mobile)o).AddToBackpack(getItem());
        from.Target=this;
        return;
      }       
      if (o is Item) {
        Item l = o as Item;
        if (l.Parent is Item) {
          from.SendMessage(37,"Item owned target not supported");
          return;
        }
        if (l.Parent is Mobile) {
          from.SendMessage(37,"Mobile owned target not supported");
          return;
        }
        Point3D p=l.Location;
        p.Z++;
        getItem().MoveToWorld(p, from.Map);
        from.Target=this;
        return;
      } 
      from.SendMessage(37,"target not supported");
    }
  }
  //name target
  public class ShowartName: Target {
    private String _name;
    public ShowartName(String name) : base(-1, false, TargetFlags.None)  {
      _name=name;
      CheckLOS = false;
      //AllowGround  ==true
      DisallowMultis = false;
      //Range = 30; == -1
    }
    protected override void OnTarget(Mobile from, object o) {
      if (o is Item) {
        Item l = o as Item;
        l.Name=_name;
        return;
      } 
      from.SendMessage(37,"target not supported");
    }
  }
  //copy index
  public class ShowartCTarget: Target {
    private ShowartGump _gmp;
    private String _name;
    public ShowartCTarget(ShowartGump gmp,String name) : base(-1, true, TargetFlags.None)  {
      _gmp=gmp;
      _name=name;
      CheckLOS = false;
      //AllowGround ==true
      DisallowMultis = false;
      //Range = 30; == -1
    }
    protected override void OnTarget(Mobile from, object o) {
      if (o is StaticTarget) {
        StaticTarget l = o as StaticTarget;
        _gmp.setView(l.ItemID,_name);
        return;
      } 
      if (o is Item) {
        Item l = o as Item;
        from.SendMessage("item:{0} hue:{1}", l.ItemID,l.Hue);
        _gmp.setView(l.ItemID,_name);
        return;
      } 
      _gmp.setView(-1,_name);
      from.SendMessage(37,"target not supported");
    }
  }

  
  public class ShowartGump : Gump
  {
    static int W=10;
    static int H=10;
    static int T=75;
    private Mobile m_From;
    private int m_newstart;
    private bool _static;
    private bool _target;

    public static void Initialize()
    {
      CommandSystem.Register( "Showart", AccessLevel.GameMaster, new CommandEventHandler( Showart_OnCommand ) );
    }

    [Usage( "Showart" )]
    [Description( "Show art" )]
    public static void Showart_OnCommand( CommandEventArgs e )
    {
      e.Mobile.SendGump( new ShowartGump( e.Mobile ) );
    }

    public ShowartGump(ShowartGump gump,String name): base(50,40) {
      _static=gump._static;
      _target=gump._target;
      m_From = gump.m_From;
      m_newstart = gump.m_newstart;
      m_From.CloseGump( typeof( ShowartGump ) );
      AddDetails( m_newstart,name);
    }

    public ShowartGump( Mobile from ) : base( 50, 40 )
    {
      _static=true;
      _target=true;
      m_From = from;
      m_newstart = 0;
      AddDetails( m_newstart,"" );
    }

    private void AddDetails( int index,String name )
    {
      try{
      AddPage( 0 );

      int w=W*T+72;
      int skip=10;
      
      AddBackground( 0, 0, w, 100+H*T, 9270 );
      AddBackground( 11, 35, W*T+50, 55+H*T, 3000 );

      AddLabelCropped( 13, 13, 150, 20, 1152, "Haazen's ShowartGump" );


      //adding items from right to left
      w-=13; //border
      w-=17; //button
      AddButton(w, 14, 0x15E1, 0x15E5, -2, GumpButtonType.Reply, 0 ); 
      w-=17; //button
      AddButton(w, 14, 0x15E3, 0x15E7, -1, GumpButtonType.Reply, 0 ); 
      
      w-=skip; //skip
      w-=17; //button
      AddButton( w, 13, 0x15E2, 0x15E6, -3, GumpButtonType.Reply, 0 );
      w-=60+4;
      AddBackground(w, 13, 60+4, 20, 3000 );
      AddTextEntry(w+2, 13, 60, 20, 10, 20, "" );
      w-=15; //20
      AddLabelCropped(w,13,65,20, 1152,"id");

      w-=skip;
      w-=17;
      AddButton( w, 13, _static?0x9cf:0x9ce,  _static?0x9ce:0x9cf, -4, GumpButtonType.Reply, 0 );
      w-=40;//65
      AddLabelCropped(w,13,65,20, 1152,"static");

      w-=skip;
      w-=17;
      AddButton(w, 13, _target?0x9cf:0x9ce,  _target?0x9ce:0x9cf, -5, GumpButtonType.Reply, 0 );
      w-=40;//65
      AddLabelCropped(w,13,65,20, 1152,"target");
      
      w-=skip;
      w-=17;
      AddButton(w, 13, 0x15E2, 0x15E6, -6, GumpButtonType.Reply, 0 );
      w-=100+4;
      AddBackground(w, 13, 100+4, 20, 3000 );
      AddTextEntry(w+2, 13, 100, 20, 10, 21, name );
      w-=40; //65 
      AddLabelCropped(w,13,65,20, 1152,"name");

      w-=skip;
      w-=17;
      AddButton( w, 13, 0x15E2, 0x15E6, -7, GumpButtonType.Reply, 0 );
      w-=40;//65
      AddLabelCropped(w,13,65,20, 1152,"copy");

              

      
      //grid
      for ( int i = 0; i < H; ++ i )
      {
        AddLabel( 18, 70 + (i * T), 1152, String.Format( "{0}", index + (i * W) ) ); //"0x{0:X}" for hex
        for ( int j = 0; j < W; ++ j )
        {
          AddButton( 70 + (j * T), 50 + (i * T), 0x24b2, 0x24b2, index + (i * W) + (j), GumpButtonType.Reply, 0 );
          AddItem( 70 + (j * T), 50 + (i * T), index + (i * W) + (j) );
        }
      }

      }catch{}
    }

    public void setView(int id,String name){
      if (id!=-1) {
        m_newstart=id;
      }
      m_From.SendGump( new ShowartGump( this,name));
    }


    //use in try as convert might give an exception    
    int toInt(String s){
      int b=10;
      if (s.StartsWith("0x") || s.StartsWith("0X")){
        s=s.Substring(2);
        b=16;
      }
      return Convert.ToInt32(s,b);
    }
    
    void updateGump(Mobile from, RelayInfo info ){
        from.SendGump( new ShowartGump( this,info.GetTextEntry(21).Text) );
    }
    
    public override void OnResponse( NetState state, RelayInfo info )
    {
      Mobile from = state.Mobile;
      int buttonID = info.ButtonID;
      switch(buttonID) {
      case 0: //dismiss
                        break;
      case -1: //back
        m_newstart -= W*H;
        if ( m_newstart < 1 )  m_newstart = 1;
        updateGump(from,info);
        break;
      case -2: //forward
        m_newstart += W*H;
        updateGump(from,info);
      break;
      case -3: //jump to page
        TextRelay entry = info.GetTextEntry( 20 );

        try { m_newstart = toInt(entry.Text); } 
        catch {}
        if ( m_newstart < 1 )  m_newstart = 1;

        updateGump(from,info);
                        break; 
      case -4: //toggle static
        _static=!_static;
        updateGump(from,info);
      break;
      case -5: //toggle target
        _target=!_target;
        updateGump(from,info);
      break;
      case -6: //setname
        from.Target=new ShowartName(info.GetTextEntry(21).Text);
        updateGump(from,info);
      break;
      case -7: //copy
        from.Target=new ShowartCTarget(this,info.GetTextEntry(21).Text);
        //updateGump(from,info);
        break;
      default: //push image
        from.SendMessage("id {0}", buttonID);
        if (!_static && !_target) {
          Item it=new Item(buttonID);
          from.AddToBackpack(it);
        } 
        if (_static && !_target) {
          Static it=new Static(buttonID);
          it.MoveToWorld(from.Location,from.Map);
        }
        if (_target) {
          from.Target=new ShowartTarget(buttonID,_static);
        }
        updateGump(from,info);
        break;
                        }
    }
  }
}
