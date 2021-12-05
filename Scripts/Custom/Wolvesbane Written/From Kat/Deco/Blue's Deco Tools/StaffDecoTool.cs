/*
coded by Nika.
derived from the interiorDecorator, yarddecorator, and maybe more sources....
- it allows items/addons to be moved Decorator.range outside of the house
- items have to be lockeddown/secured to the house
- as reference for whats in the house the player & the item are used, one of the two has to be in house.
- heavy items can be moved without restrictions
- non players(e.g. GM) can move anything anywhere
- no guarantees, no live shard testing
- it has 2 modes sticky & non sticky
    -sticky you select what to move and then control it with the buttons
    -non sticky you select which action to take and click the item to do the action

1.1  6/sep/2017 trying to log & prevent crash (null pointer exception in Move)
1.0             first released version
   */

using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Regions;
using Server.Multis;
using Server.Gumps;
using Server.Targeting;
using Server.ACC.YS;
/*

	        AddImage(98, 71, 5010); //circle
	            AddImage(174, 105, 4500);//NE
	            AddImage(200, 116, 4501);//N
	            AddImage(211, 142, 4502);//NW
	            AddImage(200, 169, 4503);//W
	            AddImage(174, 179, 4504);//SW
	            AddImage(146, 169, 4505);//S
	            AddImage(135, 142, 4506);//SE
	            AddImage(147, 115, 4507);//E


*/
namespace Server.Items
{
	public class StaffDecoTool : Item
	{
    static int range=10;//distance you can move something out of your house
		[Constructable]
		public StaffDecoTool() : base( 0xFC1 )
		{
			Name="Staff Deco Tool";			
			Weight = 1.0;
			LootType = LootType.Blessed;
		}
	  public StaffDecoTool(Serial serial) : base(serial) { }
	  
	  public override void Serialize(GenericWriter writer)
	        {
	            base.Serialize(writer);
	            writer.Write((int)0); // version
	        }

	  public override void Deserialize(GenericReader reader)
	        {
	            base.Deserialize(reader);
	            /*int version = */reader.ReadInt();
	        }

		public override void OnDoubleClick( Mobile from )
		{
			//if ( !CheckUse(from ) ) return; //why complain ? as long as the items comply
			from.SendGump( new InternalGump(from) );
		}

		private class InternalTarget : Target
		{
			private InternalGump m_InternalGump;

			public InternalTarget( InternalGump gmp) : base( -1, false, TargetFlags.None )
			{
				CheckLOS = false;
				m_InternalGump = gmp;
			}
			protected override void OnTargetNotAccessible( Mobile from, object targeted )
			{
				OnTarget( from, targeted );
			}
			protected override void OnTarget( Mobile from, object target ) {
  			if (!(target is Item)) {
  			  m_InternalGump.handle(null);
  		    return;
  			}
				Item item=(Item)target;         
				m_InternalGump.handle(item); 
				from.Target = new InternalTarget(m_InternalGump );
			}
		}
		private class InternalGump : Gump{
			private Item target=null;
      public bool sticky=false;
			int action=-1;
			Mobile user=null;
      Point3D refLoc;
      BaseHouse refHouse=null;
			public InternalGump( Mobile from ) : base( 50, 50 ){
				user=from;
				MakeGump();
				from.Target = new InternalTarget( this );
			}
			public InternalGump( InternalGump gm ) : base( 50, 50 ){
				user=gm.user;
				sticky=gm.sticky;
				target=gm.target;
				action=gm.action;
				MakeGump();
				user.Target = new InternalTarget( this );
			}
			void MakeGump(){
				AddBackground( 0, 0, 175, 175, 3500 ); //2600
				AddLabel(25,10,0x384,"up/down");
				AddButton( 20 ,30, 4500, 4500, 1, GumpButtonType.Reply, 0 ); //up
				AddButton( 20 ,70, 4504, 4504, 2, GumpButtonType.Reply, 0 ); //down

				AddLabel(35,115,0x384,"turn");
				AddButton( 35 ,135, 4014, 4016, 4, GumpButtonType.Reply, 0 ); //cw

				AddLabel(95,10,0x384,"move");
				AddButton(105, 30, 4501, 4501, 8, GumpButtonType.Reply, 0); //back N
				AddButton( 70, 30, 4507, 4507, 5, GumpButtonType.Reply, 0); //left E
				AddButton(105, 70,4503, 4503, 6, GumpButtonType.Reply, 0); //right W
				AddButton(70, 70, 4505, 4505, 7, GumpButtonType.Reply, 0); //forward S

				AddLabel(95,115,0x384,"sticky");
				AddButton( 100, 135, sticky?2715:2711, 2711, 9, GumpButtonType.Reply, 0 );
			}
			public void handle(Item id){
				target=id;
				if (!sticky) doAction();
			}
			public override void OnResponse( NetState sender, RelayInfo info )
			{
				action=info.ButtonID;
				if (action==9) sticky=!sticky;
				else if (sticky) doAction();
				if (action!=0) sender.Mobile.SendGump( new InternalGump(this) );
				else Target.Cancel(user);
			}
			void doAction( ){
				//Console.WriteLine("doaction:"+target+" "+action);
				int v= Check();
				if (v!=0) {
				  user.SendLocalizedMessage( v );
				  return;
				}
				switch ( action ) {
				case 1: v=Move( 0,0, 1); break;
				case 2: v=Move( 0,0,-1); break;
				case 3: v=Turn(user); break;
				case 4: v=Turn(user); break;
				case 5: v=Move(-1, 0,0);break;
				case 6: v=Move( 1, 0,0);break;
				case 7: v=Move( 0, 1,0);break;
				case 8: v=Move( 0,-1,0);break;
				}
        if (v!=0) {
         user.SendLocalizedMessage( v );
        }
			}
			
			private int Check(){
			  if (target == null) return 502279; //no selection
			  if (target.Deleted) return 502279; //no selection
			  
        if ( target is AddonComponent ) {
             AddonComponent component = (AddonComponent) target;
             target = component.Addon;
           }
        if ( target is AddonContainerComponent )
           {
             AddonContainerComponent component = (AddonContainerComponent) target;
             target = component.Addon;
           }
			
 				//refHouse = BaseHouse.FindHouseAt( target);
				refLoc=target.Location;
 				if (refHouse==null) {
				//refHouse=BaseHouse.FindHouseAt(user);
				refLoc=user.Location;
			}
       
				if (target is VendorRentalContract ) return 1062491; // You cannot use the decorate on this
       

				return 0;
			}
			
		private int Move(int dx,int dy,int dz){
        //somehow this can generate a null pointer exception... no clue how Check is called before and guarantees all of these
        bool ok=true;
        if (target==null)   
		{  
		Console.WriteLine("StaffDecoTool target:null");     
		ok=false; 
		}
		
        if (user==null)     
		{  
		Console.WriteLine("StaffDecoTool user:null");       
		ok=false;   
		}
		
        if (refHouse==null) 
		{  Console.WriteLine("StaffDecoTool refHouse:null");  
		ok=true;  
		}
		
        if (ok == false) {
          Console.WriteLine("level:{0}",user.AccessLevel);
          Console.WriteLine("house:{0}",refHouse);
          Console.WriteLine("user:{0}",user);
          Console.WriteLine("target:{0}",target);
          if (user!=null) {
            user.SendMessage("You hit a bug, please explain shard owner what you tried to do");
          }
          return 502279; //no selection
			  }
        //admins can do whatever they want
			  Point3D loc=new Point3D(target.Location);
			  Point3D newloc=new Point3D(loc.X+dx,loc.Y+dy,loc.Z+dz);
  			int message=0;
 			  if (user.AccessLevel != AccessLevel.Player) {target.Location=newloc; return 0;}
  			if (dz!=0) {
			    int floorZ = GetFloorZ( target );
			    if (newloc.Z<floorZ) return 1042275; // You cannot lower it down any further.
			    if (newloc.Z>floorZ+15) return 1042274; // You cannot raise it up any higher.
			  }

        //ref_loc is in the house so moving it within range will always be within range from the house
        int mdx=newloc.X-refLoc.X;
        int mdy=newloc.Y-refLoc.Y;
        if (range>mdx && mdx>-range) {
          if (range>mdy && mdy>-range) {
            target.Location=newloc;
            return 0;
          }
        }
			  //ok moving more then range

			    target.Location=newloc;
 				  if (!refHouse.IsInside(target)) { //stay close to reference house
 				    target.Location=loc;
            return 1042270 ; // That is not in your house.
 				  }

 				return 0;
			}
			private int Turn( Mobile from ) {
				FlipableAttribute[] attributes = 
				(FlipableAttribute[])target.GetType().GetCustomAttributes( typeof( FlipableAttribute ), false );
				if( attributes.Length == 0 ) return 1042273 ; // You cannot turn that.
        attributes[0].Flip( target );
        return 0;
			}

			private int GetFloorZ( Item item ) {
				Map map = item.Map;
				if ( map == null ) return int.MinValue;
				StaticTile[] tiles = map.Tiles.GetStaticTiles( item.X, item.Y, true );
				int z = int.MinValue;
				for ( int i = 0; i < tiles.Length; ++i )
				{
					StaticTile tile = tiles[i];
					ItemData id = TileData.ItemTable[tile.ID & 0x3FFF];
					int top = tile.Z; // Confirmed : no height checks here
					if ( id.Surface && !id.Impassable && top > z && top <= item.Z )
						z = top;
				}
				return z;
			}
		}
	}
}

