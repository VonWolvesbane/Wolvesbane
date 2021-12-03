///////////////////////////////////////////////
///                                        ///
///	  Originaly made by System 32    	  ///
///        Edits made by KaNJi           ///
///                                     ///
//////////////////////////////////////////
/*Enhanced Items by Kitten
*
("`-''-/").___..--''"`-._
   `6_ 6  )   `-.  (     ).`-.__.`)
   (_Y_.)'  ._   )  `._ `. ``-..-'
 _..`--'_..-_/  /--'_.' ,'
(il),-''  (li),'  ((!.-'
	  		   
*/
using System;
using Server;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Targeting;
using Server.Items;
using Server.Gumps;

namespace Server.Items
{
	public class EnhancedItemIdDeed : Item
	{

		[Constructable]
		public EnhancedItemIdDeed() : base( 0x14F0 )
		{
			Weight = 1.0;
			LootType = LootType.Blessed;
			Hue = 2181;
			Name = "Enhanced Item ID Deed";
		}

		public EnhancedItemIdDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else
			{
				from.CloseGump( typeof( EnhancedItemIdDeedGump));
				from.SendGump( new EnhancedItemIdDeedGump() );
			}
		}
	}
}
namespace Server.Gumps
{
	public class EnhancedItemIdDeedGump : Gump
	{
		public EnhancedItemIdDeedGump()
			: base( 0, 0 )
		{
			Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=true;
			AddPage(1);			
			AddBackground(73, 38, 334, 236, 9200);
			AddBackground(83, 45, 312, 73, 9250);
			AddLabel(164, 58, 0, @"Enhanced ItemID Deed");
			AddLabel(210, 78, 0, "By Kitten");
			AddBackground(103, 132, 63, 60, 9250);
			AddBackground(301, 132, 63, 60, 9250);
			//AddImage(98, 60, 2274);
			//AddImage(339, 60, 2274);
			AddItem(115, 155, 30753);
			AddItem(305, 152, 30742);
			//AddItem(200, 153, 2981);
			//AddItem(227, 154, 3016);
			//AddItem(198, 136, 2971);
			//AddItem(230, 136, 2972);
			AddButton(200, 199, 1149, 1148, 0, GumpButtonType.Page, 300);//Playground
			AddLabel(202, 235, 0, @"Enhanced Items");



			AddPage(300);
			AddBackground(0, 0, 504, 444, 9250);
			this.AddCheck(20, 20, 2152, 2153, false,439);
			AddItem(50, 16, 30760);
			AddLabel(110, 20, 0, @"Tiger Mask");
			this.AddCheck(20, 55, 2152, 2153, false,440);
			AddItem(47, 49, 30758);
			AddLabel(110, 55, 0, @"Tiger Skirt Long");
			this.AddCheck(20, 90, 2152, 2153, false,441);
			AddItem(54, 92, 30759);
			AddLabel(110, 94, 0, @"Tiger Skirt Short");
			this.AddCheck(20, 125, 2152, 2153, false,442);
			AddItem(54, 133, 30756);
			AddLabel(110, 125, 0, @"Tiger Legs");
			this.AddCheck(20, 160, 2152, 2153, false,443);
			AddItem(46, 165, 30757);
			AddLabel(110, 165, 0, @"Tiger Shorts");
			this.AddCheck(20, 200, 2152, 2153, false,444);
			AddItem(56, 190, 30755);
			AddLabel(110, 200, 0, @"Tiger Bustier");
			this.AddCheck(20, 235, 2152, 2153, false,445);
			AddItem(49, 235, 30761);
			AddLabel(110, 235, 0, @"Tiger Collar");
			this.AddCheck(20, 271, 2152, 2153, false,446);
			AddItem(52, 259, 30754);
			AddLabel(110, 270, 0, @"Tiger Chest");
			this.AddCheck(241, 20, 2152, 2153, false,447);
			AddItem(263, 21, 30763);
			AddLabel(325, 20, 0, @"DT Bustier");
			this.AddCheck(241, 55, 2152, 2153, false,448);
			AddItem(241, 48, 30764);
			AddLabel(325, 50, 0, @"DT Legs");
			this.AddCheck(241, 90, 2152, 2153, false,449);
			AddItem(242, 80, 30765);
			AddLabel(325, 90, 0, @"DT Helm");
			this.AddCheck(241, 125, 2152, 2153, false,450);
			AddItem(239, 125, 30766);
			AddLabel(325, 125, 0, @"DT Arms");
			this.AddCheck(241, 160, 2152, 2153, false,451);
			AddItem(243, 154, 30753);
			AddLabel(325, 160, 0, @"Evening Gown");
			this.AddCheck(241, 200, 2152, 2153, false,452);
			AddItem(276, 201, 30750);
			AddLabel(325, 200, 0, @"Flowered Dress");
			this.AddCheck(241, 235, 2152, 2153, false,453);
			AddItem(270, 236, 30742);
			AddLabel(325, 235, 0, @"Fancy Hooded Robe");
			this.AddCheck(242, 271, 2152, 2153, false,454);
			AddItem(267, 277, 30746);
			AddLabel(325, 270, 0, @"Chef Hat");
			this.AddCheck(242, 305, 2152, 2153, false,455);
			AddItem(269, 312, 30747);
			AddLabel(325, 310, 0, @"Gilded Kilt");
			this.AddCheck(242, 340, 2152, 2153, false,456);
			AddItem(265, 347, 30748);
			AddLabel(325, 345, 0, @"Fancy Kilt");
			this.AddCheck(20, 305, 2152, 2153, false,457);
			AddItem(48, 308, 30762);
			AddLabel(110, 300, 0, @"DT Chest");
			this.AddButton(248, 396, 239, 240, 1, GumpButtonType.Reply, 0);
			AddButton(403, 396, 242, 241, 0, GumpButtonType.Page, 1);//cancel	
			AddButton(85, 375, 1234, 1233, 0, GumpButtonType.Page, 700);
			AddLabel(94, 354, 0, "MORE");

AddPage(700);
			AddBackground(0, 0, 499, 385, 9250);
			this.AddCheck(20, 20, 2152, 2153, false,458);
			AddItem(50, 20, 30743);
			AddLabel(105, 20, 0, @"Vice Shield");
			this.AddCheck(20, 55, 2152, 2153, false,459);
			AddItem(49, 53, 30744);
			AddLabel(105, 55, 0, @"Virtue Shield");
			this.AddCheck(20, 90, 2152, 2153, false,460);
			AddItem(51, 89, 30745);
			AddLabel(105, 90, 0, @"Jester Shoes");
			this.AddCheck(20, 125, 2152, 2153, false,461);
			AddItem(51, 127, 16913);
			AddLabel(105, 125, 0, @"Gargish Bracelet");
			this.AddCheck(20, 160, 2152, 2153, false,462);
			AddItem(45, 154, 17829);
			AddLabel(105, 160, 0, @"Wings");
			this.AddCheck(20, 195, 2152, 2153, false,463);
			AddItem(46, 187, 8271);
			AddLabel(105, 195, 0, @"GM LOOK ALIKE ROBE");
			this.AddButton(311, 335, 239, 240, 1, GumpButtonType.Reply, 0);
			AddButton(416, 335, 242, 241, 0, GumpButtonType.Page, 300);//cancel

		}

public enum Buttons
		{
		}
		
		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile m = state.Mobile;
			int m_ItemID;
			
			switch( info.ButtonID )
			{
				case 1:
				{
					if( info.IsSwitched ( 2 )  )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5099;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 3 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5104;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 4 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5102;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 5 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5100;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}else if ( info.IsSwitched ( 6 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5051;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 7 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5054;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 8 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5055;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 9 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5136;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 10 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5140;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 11 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5137;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 12 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5141;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 13 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7172;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 14 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10105;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 15 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10109;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 16 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10112;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 17 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10120;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 18 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10125;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 19 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5132;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 20 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5128;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 21 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5130;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 22 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5134;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 23 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5138;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 24 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10100;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 25 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10101;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 26 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10103;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 27 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10113;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 28 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10116;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 29 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10104;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 30 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10117;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 31 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10121;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 32 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 11118;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 33 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 11119;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 34 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 11120;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 35 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7027;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 36 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7026;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 37 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7030;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 38 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7035;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 39 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7028;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 40 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7033;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 41 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7107;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 42 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7108;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 43 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 11009;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 44 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 9795;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 45 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 9797;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 46 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 9799;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 47 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 9815;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 48 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 9793;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 49 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5444;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 50 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5440;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 51 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5907;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 52 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5909;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 53 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5908;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 54 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5911;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 55 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5910;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 56 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5912;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 57 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5913;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 58 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5914;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 59 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5915;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 60 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5916;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 61 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 8966;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 62 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10127;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 63 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10136;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 64 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 8059;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 65 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5399;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 66 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7933;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 67 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 8097;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 68 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 8189;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 69 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7937;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 70 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7936;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 71 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5397;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 72 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7939;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 73 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 8095;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 74 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 8970;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 75 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 8974;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 76 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 8976;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 77 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10132;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 78 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10137;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 79 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10140;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 80 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10114;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 81 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10115;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 82 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10145;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 83 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5422;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 84 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5433;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					
					else if ( info.IsSwitched ( 85 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5431;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 86 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5398;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 87 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 8972;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 88 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10138;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 89 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10139;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 90 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5441;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 91 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5435;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 92 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5437;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 93 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 8967;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 94 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10135;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 95 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10134;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 96 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5901;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 97 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5903;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 98 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5899;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 99 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5905;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 400 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5063;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 401 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7609;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 402 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5062;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 403 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5069;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 404 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5067;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 405 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5068;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 406 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10102;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 407 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10106;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 408 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10182;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 409 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10110;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 410 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10118;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 412 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10122;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 413 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10129;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 414 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10131;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 415 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10128;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 416 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10130;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 417 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10126;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 418 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5078;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 419 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5077;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 420 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5084;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 421 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5082;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 422 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5083;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 423 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10141;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 424 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10183;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 425 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10111;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 426 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10194;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 427 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 10123;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 428 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7168;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 429 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7176;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 430 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7178;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 431 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7180;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 432 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7174;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 433 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7170;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 434 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5201;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 435 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5200;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 436 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5198;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 437 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5202;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 438 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 5199;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 439 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30760;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 440 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30758;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 441 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30759;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 442 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30756;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 443 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30757;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 444 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30755;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 445 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30761;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 446 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30754;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 447 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30763;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 448 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30764;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 449 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30765;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 450 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30766;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 451 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30753;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 452 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30750;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 453 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30742;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 454 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30746;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 455 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30747;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 456 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30748;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 457 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30762;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 458 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30743;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 459 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30744;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 460 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 30745;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 461 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 16913;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 462 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 17829;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 463 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 8271;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 464 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 8259;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 465 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12228;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 466 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12229;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 467 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12230;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 468 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12231;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 469 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12232;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 470 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12233;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 471 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12234;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 472 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12235;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 473 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12648;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 474 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12658;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 475 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12657;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 476 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12638;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 477 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12640;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 478 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12641;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 479 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12642;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 480 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12643;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 481 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12644;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 482 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12662;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 483 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 12663;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 484 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7943;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 485 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 7944;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 486 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 4234;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}
					else if ( info.IsSwitched ( 487 ) )
					{
						if( info.Switches.Length == 1 )
						{
							m_ItemID = 4230;
							m.Target = new ItemIDTarget( m_ItemID );
						}
						else 
						{
							m.SendMessage( 38,"You cant do this");
						}
					}					
					else
					{
						m.SendMessage( 38,"You cant do this");
					}
					break;
				}
				
				case 0:
				{
					m.SendMessage( 38,"Cancelled" ); 
					break;
				}
			}
		}					
					public class ItemIDTarget : Target
		{
			int m_ItemID;
			
			public ItemIDTarget( int itemid ) : base( -1, true, TargetFlags.None )
			{
				m_ItemID = itemid;
			}	
		
			protected override void OnTarget( Mobile from, object target ) // Override the protected OnTarget() for our feature
			{
				Item a = from.Backpack.FindItemByType( typeof( EnhancedItemIdDeed ) );
			
				if( target is BaseJewel || target is BaseArmor || target is BaseClothing ||target is BaseShield  )
				{
					if( a != null )
					{
						Item item = (Item)target;
					
							if( item.RootParent == from ) // Make sure its in their pack or they are wearing it
							{
								item.ItemID = m_ItemID;
								a.Delete();
								from.SendMessage( "You have changed the item id" ); 
							}
						
							else
							{
								from.SendMessage( 38,"It should be in your backpack");
							}
					}
					
					else
					{
						from.SendMessage( 38," You dont have a item id deed in your backpack ");
						from.CloseGump( typeof (EnhancedItemIdDeedGump));
					}
				}
			
				else
				{
					from.SendMessage( 38,"You can change only armors, jewellerys and clothings Item ID !");
				}
			}
		}
	}	
}			