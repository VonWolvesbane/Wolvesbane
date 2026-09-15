using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.ACC.PG
{
	[Flags]
	public enum	Conditions
	{
		None      = 0x00000000,
		Adding    = 0x00000001,
		Category  = 0x00000002,
	}

	public class PGAddEditGump : Gump
	{
		private Conditions m_Conditions;
		private int        m_CurCat;
		private int        m_CurLoc;
		private PGCategory m_Cat;
		private PGLocation m_Loc;
		private PublicGate m_Gate;

		private bool GetFlag( Conditions flag ) { return( (m_Conditions & flag) != 0 ); }

		public PGAddEditGump( Conditions conditions, int curC, int curL, PublicGate gate ) : base( 0, 0 )
		{
			if( !PGSystem.Running )
				return;

			m_Conditions = conditions;
			m_CurCat     = curC;
			m_CurLoc     = curL;
			m_Gate       = gate;

			if( !GetFlag( Conditions.Category ) || (GetFlag( Conditions.Category ) && !GetFlag( Conditions.Adding )) )
				m_Cat = PGSystem.CategoryList[curC];
			if( m_Cat != null && (!GetFlag( Conditions.Category ) && !GetFlag( Conditions.Adding )) )
				m_Loc = m_Cat.Locations[curL];


			string Name = "";
			if( !GetFlag( Conditions.Adding ) )
			{
				if( GetFlag( Conditions.Category ) )
					Name = m_Cat.Name;
				else
					Name = m_Loc.Name;
			}

			Point3D Loc = new Point3D( 0, 0, 0 );
			Map Map = Map.Trammel;
			bool Gen, Staff, Reds, Charge, Young;
			int Hue, Cost;
			Gen = Staff = Reds = Charge = Young = false;
			Hue = Cost = 0;

			if( GetFlag( Conditions.Category ) && !GetFlag( Conditions.Adding ) )
			{
				Gen    = m_Cat.GetFlag( EntryFlag.Generate );
				Staff  = m_Cat.GetFlag( EntryFlag.StaffOnly );
				Reds   = m_Cat.GetFlag( EntryFlag.Reds );
				Charge = m_Cat.GetFlag( EntryFlag.Charge );
				Young  = m_Cat.GetFlag( EntryFlag.Young );
				Cost   = m_Cat.Cost;
			}

			if( !GetFlag( Conditions.Category ) && !GetFlag( Conditions.Adding ) )
			{
				Loc    = m_Loc.Location;
				Map    = m_Loc.Map;
				Gen    = m_Loc.GetFlag( EntryFlag.Generate );
				Staff  = m_Loc.GetFlag( EntryFlag.StaffOnly );
				Reds   = m_Loc.GetFlag( EntryFlag.Reds );
				Charge = m_Loc.GetFlag( EntryFlag.Charge );
				Young  = m_Loc.GetFlag( EntryFlag.Young );
				Hue    = m_Loc.Hue;
				Cost   = m_Loc.Cost;
			}

			Closable   = true;
			Disposable = true;
			Dragable   = true;
			Resizable  = false;

			AddPage(0);

			// Wolvesbane: widened, higher-contrast editor for long category/location names.
			AddBackground( 100, 70, 620, 500, 2600 );
			AddHtml( 120, 88, 580, 28, "<BASEFONT COLOR=#58D3F7 SIZE=6><CENTER><B>" +
				string.Format("{0} {1}", (GetFlag(Conditions.Adding) ? "Add" : "Edit"), (GetFlag(Conditions.Category) ? "Category" : "Location")) +
				"</B></CENTER></BASEFONT>", false, false );

			AddLabel( 125, 130, 1152, "Name:" );
			AddImageTiled( 185, 127, 500, 26, 9274 );
			AddTextEntry( 192, 131, 485, 20, 1152, 2, Name );

			AddLabel( 500, 180, 1152, "Cost:" );
			AddImageTiled( 555, 177, 125, 26, 9274 );
			AddTextEntry( 562, 181, 110, 20, 1152, 15, Cost.ToString() );

			if( !GetFlag(Conditions.Category) )
			{
				AddHtml( 125, 170, 320, 22, "<BASEFONT COLOR=#FFD57A><B>Destination</B></BASEFONT>", false, false );

				AddLabel( 125, 205, 1152, "X:" );
				AddImageTiled( 155, 202, 115, 26, 9274 );
				AddTextEntry( 162, 206, 100, 20, 1152, 3, Loc.X.ToString() );

				AddLabel( 290, 205, 1152, "Y:" );
				AddImageTiled( 320, 202, 115, 26, 9274 );
				AddTextEntry( 327, 206, 100, 20, 1152, 4, Loc.Y.ToString() );

				AddLabel( 455, 205, 1152, "Z:" );
				AddImageTiled( 485, 202, 90, 26, 9274 );
				AddTextEntry( 492, 206, 75, 20, 1152, 5, Loc.Z.ToString() );

				AddLabel( 590, 205, 1152, "Hue:" );
				AddImageTiled( 630, 202, 55, 26, 9274 );
				AddTextEntry( 635, 206, 45, 20, 1152, 14, Hue.ToString() );

				AddHtml( 125, 250, 320, 22, "<BASEFONT COLOR=#FFD57A><B>Facet</B></BASEFONT>", false, false );

				AddRadio( 125, 280, 208, 209, (Map == Map.Trammel), 6 );
				AddLabel( 155, 280, 1152, "Trammel" );
				AddRadio( 265, 280, 208, 209, (Map == Map.Felucca), 7 );
				AddLabel( 295, 280, 1152, "Felucca" );
				AddRadio( 405, 280, 208, 209, (Map == Map.Malas), 8 );
				AddLabel( 435, 280, 1152, "Malas" );
				AddRadio( 535, 280, 208, 209, (Map == Map.Ilshenar), 9 );
				AddLabel( 565, 280, 1152, "Ilshenar" );

				AddRadio( 125, 315, 208, 209, (Map == Map.Tokuno), 10 );
				AddLabel( 155, 315, 1152, "Tokuno" );
				AddRadio( 265, 315, 208, 209, (Map == Map.TerMur), 20 );
				AddLabel( 295, 315, 1152, "TerMur" );
				AddRadio( 405, 315, 208, 209, (Map == Map.Wolvesbane), 25 );
				AddLabel( 435, 315, 1152, "Wolvesbane" );
				AddRadio( 535, 315, 208, 209, (Map == Map.NewWolvesbane), 26 );
				AddLabel( 565, 315, 1152, "New WB" );
			}

			AddHtml( 125, 365, 320, 22, "<BASEFONT COLOR=#FFD57A><B>Options</B></BASEFONT>", false, false );

			AddCheck( 125, 400, 210, 211, Gen, 11 );
			AddLabel( 155, 400, 1152, "Generate" );
			AddCheck( 265, 400, 210, 211, Young, 16 );
			AddLabel( 295, 400, 1152, "Young" );
			AddCheck( 405, 400, 210, 211, Reds, 13 );
			AddLabel( 435, 400, 1152, "Reds" );
			AddCheck( 535, 400, 210, 211, Charge, 17 );
			AddLabel( 565, 400, 1152, "Charge" );

			AddCheck( 125, 435, 210, 211, Staff, 12 );
			AddLabel( 155, 435, 1152, "Staff Only" );

			AddButton( 565, 500, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddLabel( 600, 502, 69, "Apply" );
			AddButton( 125, 500, 4017, 4019, 0, GumpButtonType.Reply, 0 );
			AddLabel( 160, 502, 1152, "Cancel" );
		}

		private EntryFlag Flags;
		private void SetFlag( EntryFlag flag, bool value )
		{
			if( value )
				Flags |= flag;
			else Flags &= ~flag;
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			if( state.Mobile.AccessLevel < PGSystem.PGAccessLevel )
				return;

			Mobile from = state.Mobile;
			int BID = info.ButtonID;
			if( BID == 0 )
				return;

			SetFlag( EntryFlag.Generate, info.IsSwitched( 11 ) );
			SetFlag( EntryFlag.StaffOnly, info.IsSwitched( 12 ) );
			SetFlag( EntryFlag.Reds, info.IsSwitched( 13 ) );
			SetFlag( EntryFlag.Young, info.IsSwitched( 16 ) );
			SetFlag( EntryFlag.Charge, info.IsSwitched( 17 ) );

			Map Map = null;
			for( int i = 0; i < info.Switches.Length; i++ )
			{
				int m = info.Switches[i];
				switch( m )
				{
					case 6:  Map = Map.Trammel;  break;
					case 7:  Map = Map.Felucca;  break;
					case 8:  Map = Map.Malas;    break;
					case 9:  Map = Map.Ilshenar; break;
					case 10: Map = Map.Tokuno;   break;
                    case 20: Map = Map.TerMur;   break;
                    case 25: Map = Map.Wolvesbane;  break;
					case 26: Map = Map.NewWolvesbane;  break;

				}
			}


			TextRelay NR = info.GetTextEntry( 2 );
			TextRelay XR = info.GetTextEntry( 3 );
			TextRelay YR = info.GetTextEntry( 4 );
			TextRelay ZR = info.GetTextEntry( 5 );
			TextRelay HR = info.GetTextEntry( 14 );
			TextRelay CR = info.GetTextEntry( 15 );
			string NS = (NR == null ? null : NR.Text.Trim());
			string XS = (XR == null ? null : XR.Text.Trim());
			string YS = (YR == null ? null : YR.Text.Trim());
			string ZS = (ZR == null ? null : ZR.Text.Trim());
			string HS = (HR == null ? null : HR.Text.Trim());
			string CS = (CR == null ? null : CR.Text.Trim());

			if( BID == 1 )
			{
				if( GetFlag( Conditions.Category ) )
				{
					if( GetFlag( Conditions.Adding ) )
					{
						if( NS == null || NS.Length == 0 || CS == null || CS.Length == 0 )
						{
							from.SendMessage( "Please enter a name and cost for this Category." );
							from.CloseGump( typeof( PGAddEditGump ) );
							from.SendGump( new PGAddEditGump( m_Conditions, m_CurCat, m_CurLoc, m_Gate ) );
							return;
						}

						int c = 0;
						try
						{
							c = Int32.Parse( CS );
							PGSystem.CategoryList.Add( new PGCategory( NS, Flags, c ) );
							from.SendMessage( "Added Category." );
						}
						catch
						{
							from.SendMessage( "Bad cost value, defaulting to 0." );
							PGSystem.CategoryList.Add( new PGCategory( NS, Flags ) );
						}
					}

					else
					{
						if( NS == null || NS.Length == 0 )
						{
							from.SendMessage( "Removed the Category." );
							PGSystem.CategoryList.RemoveAt( m_CurCat );
							m_CurCat = 0;
						}

						else
						{
							from.SendMessage( "Changed the Category." );
							PGSystem.CategoryList[m_CurCat].Name = NS;
							PGSystem.CategoryList[m_CurCat].Flags = Flags;
							if( CS == null || CS.Length == 0 )
								PGSystem.CategoryList[m_CurCat].Cost = 0;
							else
							{
								int c = 0;

								try
								{
									c = Int32.Parse( CS );
									PGSystem.CategoryList[m_CurCat].Cost = c;
								}
								catch
								{
									PGSystem.CategoryList[m_CurCat].Cost = 0;
								}
							}
						}
					}
				}

				else
				{
					if( NS == null || NS.Length == 0 ||
						XS == null || XS.Length == 0 ||
						YS == null || YS.Length == 0 ||
						ZS == null || ZS.Length == 0 ||
						HS == null || HS.Length == 0 ||
						CS == null || CS.Length == 0 )
					{
						if( GetFlag( Conditions.Adding ) )
						{
							from.SendMessage( "Please fill in each field." );
							from.CloseGump( typeof( PGAddEditGump ) );
							from.SendGump( new PGAddEditGump( m_Conditions, m_CurCat, m_CurLoc, m_Gate ) );
							return;
						}

						from.SendMessage( "Removed the Location." );
						PGSystem.CategoryList[m_CurCat].Locations.RemoveAt( m_CurLoc );
					}

					else if( Map == null )
					{
						from.SendMessage( "Please select a Map." );
						from.CloseGump( typeof( PGAddEditGump ) );
						from.SendGump( new PGAddEditGump( m_Conditions, m_CurCat, m_CurLoc, m_Gate ) );
						return;
					}

					else
					{
						int x, y, z, h, c = 0;
						Point3D Loc;
						int Hue;
						int Cost;
						try
						{
							x = Int32.Parse( XS );
							y = Int32.Parse( YS );
							z = Int32.Parse( ZS );
							h = Int32.Parse( HS );
							c = Int32.Parse( CS );
							Loc = new Point3D( x, y, z );
							Hue = h;
							Cost = c;
						}
						catch
						{
							from.SendMessage( "Please enter an integer in each of the info fields. (X, Y, Z, H, C)" );
							from.CloseGump( typeof( PGAddEditGump ) );
							from.SendGump( new PGAddEditGump( m_Conditions, m_CurCat, m_CurLoc, m_Gate ) );
							return;
						}

						PGLocation PGL = new PGLocation( NS, Flags, Loc, Map, Hue, Cost );
						if( PGL == null )
						{
							from.SendMessage( "Bad Location information, can't add!" );
							from.CloseGump( typeof( PGAddEditGump ) );
							from.SendGump( new PGAddEditGump( m_Conditions, m_CurCat, m_CurLoc, m_Gate ) );
							return;
						}

						if( GetFlag( Conditions.Adding ) )
						{
							from.SendMessage( "Added the Location." );
							PGSystem.CategoryList[m_CurCat].Locations.Add( PGL );
						}
						else
						{
							from.SendMessage( "Changed the Location." );
							PGSystem.CategoryList[m_CurCat].Locations[m_CurLoc] = PGL;
						}
					}
				}

				from.CloseGump( typeof( PGGump ) );
				from.SendGump( new PGGump( from, m_CurCat, m_Gate ) );
			}
		}
	}
}
