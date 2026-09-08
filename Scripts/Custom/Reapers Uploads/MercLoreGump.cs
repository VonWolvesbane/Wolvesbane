using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using Server.Misc;
using Server.SkillHandlers;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Server.Targeting;
using Server.ContextMenus;
using Server.HuePickers;
using Xanthos.Interfaces;
//using Xanthos.ShrinkSystem;
using Xanthos.Evo;

namespace Server.Gumps
{
	public enum MercLorePage
	{
		Stats,
		Skills
	}
	
	public class MercLoreGump : Gump
	{
		private int SkillID;
		private MercLorePage m_Page;
		
		public int GetButtonID( int type, int index )
		{
			return 1 + (index * 15) + type;
		}
		
		private static string FormatSkill(BaseCreature c, SkillName name)
        {
            return String.Format("<basefont color = #A3D7FF><div align=right>{0:F1}/{1}</div></basefont>", c.Skills[name].Base, c.Skills[name].Cap);
        }
		
		private static string FormatAttributes( int cur, int max )
		{
			if ( max == 0 )
				return "<basefont color = #A3D7FF><div align=right>---</div></basefont>";

			return String.Format( "<basefont color = #A3D7FF><div align=right>{0}/{1}</div></basefont>", cur, max );
		}

		private static string FormatStat( int val )
		{
			if ( val == 0 )
				return "<basefont color = #A3D7FF><div align=right>---</div></basefont>";

			return String.Format( "<basefont color = #A3D7FF><div align=right>{0}</div></basefont>", val );
		}

		private static string FormatDouble( double val )
		{
			if ( val == 0 )
				return "<basefont color = #A3D7FF><div align=right>---</div></basefont>";

			return String.Format( "<basefont color = #A3D7FF><div align=right>{0:F1}</div></basefont>", val );
		}

		private static string FormatElement( int val )
		{
			if ( val <= 0 )
				return "<basefont color = #A3D7FF><div align=right>---</div></basefont>";

			return String.Format( "<basefont color = #A3D7FF><div align=right>{0}%</div></basefont>", val );
		}

		#region Mondain's Legacy
		private static string FormatDamage( int min, int max )
		{
			if ( min <= 0 || max <= 0 )
				return "<basefont color = #A3D7FF><div align=right>---</div></basefont>";

			return String.Format( "<basefont color = #A3D7FF><div align=right>{0}-{1}</div></basefont>", min, max );
		}
		#endregion
		
		private const int LabelColor = 0x7FFF;
		private Mobile m_Mercenary;
		private Mobile m_From;
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			int buttonID = info.ButtonID - 1;

			int index = buttonID / 15;
			int type = buttonID % 15;
			
			BaseCreature c = ((BaseCreature)m_Mercenary);
			
			switch( type )
			{
				default:
				{
					m_From.CloseGump( typeof( MercLoreGump ) );
					break;
				}
				case 1:
				{
					m_From.CloseGump( typeof( MercLoreGump ) );
					m_From.SendGump( new MercLoreGump( ((BaseCreature)m_Mercenary), m_From, MercLorePage.Skills ) );
					break;
				}
				case 2:
				{
					switch( index )
					{
						default:
						{
							m_From.CloseGump( typeof( MercLoreGump ) );
							break;
						}
						case 1:
						{
							m_From.CloseGump( typeof( MercLoreGump ) );
							m_From.SendGump( new MercLoreGump( ((BaseCreature)m_Mercenary), m_From, MercLorePage.Skills ) );
							break;
						}
						case 2:
						{
							m_From.CloseGump( typeof( MercLoreGump ) );
							m_From.SendGump( new MercLoreGump( ((BaseCreature)m_Mercenary), m_From, MercLorePage.Stats ) );
							break;
						}
					}
					break;
				}
			}
		}
		
		public MercLoreGump( BaseCreature c, Mobile from, MercLorePage page ) : base( 250, 50 )
		{
			m_Mercenary = c;
			m_From = from;
			m_Page = page;
			
			from.CloseGump( typeof( MercLoreGump ) );
			
			AddPage( 0 );

			AddHtml( 130, 5, 260, 18, String.Format( "<basefont color = #FFFFFF><center><b>{0}</b></center></basefont>", c.Name ), false, false );

			int pages = ( Core.AOS ? 5 : 3 );
			int buttonID1, buttonID2;

			#region Stats Page
			if ( page == MercLorePage.Stats )
			{
				// Background Setup
				AddBackground( 0, 0, 520, 372, 9270 );
				//AddAlphaRegion( 10, 10, 430, 352 );
				
				//Middle Display
				AddHtml( 180, 28, 160, 18, "<basefont color = #FFD57A><center>Stats</center></basefont>", false, false );
				
				//First Half
				AddHtml( 20, 40, 160, 18, "<basefont color = #FFD57A>Attributes</basefont>", false, false ); // Attributes

				AddHtmlLocalized( 20, 58, 160, 18, 1049578, LabelColor, false, false ); // Hits
				AddHtml( 125, 58, 90, 18, FormatAttributes( c.Hits, c.HitsMax ), false, false );

				AddHtmlLocalized( 20, 76, 160, 18, 1049579, LabelColor, false, false ); // Stamina
				AddHtml( 125, 76, 90, 18, FormatAttributes( c.Stam, c.StamMax ), false, false );

				AddHtmlLocalized( 20, 94, 160, 18, 1049580, LabelColor, false, false ); // Mana
				AddHtml( 125, 94, 90, 18, FormatAttributes( c.Mana, c.ManaMax ), false, false );

				AddHtmlLocalized( 20, 112, 160, 18, 1028335, LabelColor, false, false ); // Strength
				AddHtml( 150, 112, 65, 18, FormatStat( c.Str ), false, false );

				AddHtmlLocalized( 20, 130, 160, 18, 3000113, LabelColor, false, false ); // Dexterity
				AddHtml( 150, 130, 65, 18, FormatStat( c.Dex ), false, false );

				AddHtmlLocalized( 20, 148, 160, 18, 3000112, LabelColor, false, false ); // Intelligence
				AddHtml( 150, 148, 65, 18, FormatStat( c.Int ), false, false );
				
				AddHtml( 20, 202, 160, 18, "<basefont color = #FFD57A>Damage</basefont>", false, false ); // Damage
				
				AddHtmlLocalized( 20, 220, 160, 18, 1076750, LabelColor, false, false ); // Base Damage
				AddHtml( 135, 220, 80, 18, FormatDamage(c.DamageMin, c.DamageMax), false, false);

				AddHtmlLocalized( 20, 238, 160, 18, 1061646, LabelColor, false, false ); // Physical
				AddHtml( 150, 238, 65, 18, FormatElement( c.PhysicalDamage ), false, false );

				AddHtmlLocalized( 20, 256, 160, 18, 1061647, LabelColor, false, false ); // Fire
				AddHtml( 150, 256, 65, 18, FormatElement( c.FireDamage ), false, false );

				AddHtmlLocalized( 20, 274, 160, 18, 1061648, LabelColor, false, false ); // Cold
				AddHtml( 150, 274, 65, 18, FormatElement( c.ColdDamage ), false, false );

				AddHtmlLocalized( 20, 292, 160, 18, 1061649, LabelColor, false, false ); // Poison
				AddHtml( 150, 292, 65, 18, FormatElement( c.PoisonDamage ), false, false );

				AddHtmlLocalized( 20, 310, 160, 18, 1061650, LabelColor, false, false ); // Energy
				AddHtml( 150, 310, 65, 18, FormatElement( c.EnergyDamage ), false, false );
								
				//Second Half
				AddHtml( 245, 55, 200, 18, "<basefont color = #FFD57A>Loyalty Rating</basefont>", false, false ); // Loyalty Rating
				AddHtmlLocalized( 245, 73, 240, 18, (!c.Controlled || c.Loyalty == 0) ? 1061643 : 1049595 + (c.Loyalty / 10), LabelColor, false, false );
				
				AddHtml( 245, 103, 200, 18, "<basefont color = #FFD57A>AI Type</basefont>", false, false ); // AI Type
				AddHtml( 245, 121, 240, 18, String.Format( "<basefont color = #FFFFFF>{0}</basefont>", c.AI ), false, false );
				
				AddHtml( 245, 150, 160, 18, "<basefont color = #FFD57A>Tithing Points</basefont>", false, false ); // Tithing Points
				AddHtml( 405, 150, 90, 18, String.Format( "<basefont color = #FFFFFF>{0}</basefont>", c.TithingPoints ), false, false );
				
				IEvoCreature evo = c as IEvoCreature;
				if (c is IEvoCreature)
				{
				AddHtml( 245, 168, 160, 18, "<basefont color = #FFD57A>Evoloution points</basefont>", false, false );
				AddHtml( 405, 168, 90, 18, String.Format( "<basefont color = #FFFFFF>{0}</basefont>", evo.Ep ), false, false );
				}
				
				AddHtml( 245, 220, 160, 18, "<basefont color = #FFD57A>Resistances</basefont>", false, false ); // Resistances

				AddHtmlLocalized( 245, 238, 160, 18, 1061646, LabelColor, false, false ); // Physical
				AddHtml( 430, 238, 80, 18, FormatElement( c.PhysicalResistance ), false, false );

				AddHtmlLocalized( 245, 256, 160, 18, 1061647, LabelColor, false, false ); // Fire
				AddHtml( 430, 256, 65, 18, FormatElement( c.FireResistance ), false, false );

				AddHtmlLocalized( 245, 274, 160, 18, 1061648, LabelColor, false, false ); // Cold
				AddHtml( 430, 274, 65, 18, FormatElement( c.ColdResistance ), false, false );

				AddHtmlLocalized( 245, 292, 160, 18, 1061649, LabelColor, false, false ); // Poison
				AddHtml( 430, 292, 65, 18, FormatElement( c.PoisonResistance ), false, false );

				AddHtmlLocalized( 245, 310, 160, 18, 1061650, LabelColor, false, false ); // Energy
				AddHtml( 430, 310, 65, 18, FormatElement( c.EnergyResistance ), false, false );

				//Navigation
				AddHtml( 455, 5, 55, 18, "<basefont color = #FFD57A>Skills</basefont>", false, false );
				AddButton( 490, 6, 5601, 5605, GetButtonID( 2, 1 ), GumpButtonType.Reply, 0 );
				
			}
			#endregion

			#region Skills
			else if ( page == MercLorePage.Skills )
			{

				// Background Setup
				AddBackground( 0, 0, 520, 372, 9270 );
				//AddAlphaRegion( 10, 10, 435, 352 );
				
				
				//Middle Display
				AddHtml( 180, 28, 160, 18, "<basefont color = #FFD57A><center>Skills</center></basefont>", false, false );
				
				//First Half
				AddHtml( 20, 40, 160, 18, "<basefont color = #FFD57A>Combat Skills</basefont>", false, false ); // Combat Ratings

				AddHtmlLocalized( 20, 58, 160, 18, 1044103, LabelColor, false, false ); // Wrestling
				AddHtml( 155, 58, 80, 18, FormatSkill( c, SkillName.Wrestling ), false, false );

				AddHtml( 20, 76, 160, 18, "<basefont color = #FFFFFF>Swordsmanship</basefont>", false, false ); // Swords
				AddHtml( 155, 76, 80, 18, FormatSkill( c, SkillName.Swords ), false, false );
				
				AddHtml( 20, 94, 160, 18, "<basefont color = #FFFFFF>Macefighting</basefont>", false, false ); // Macing
				AddHtml( 155, 94, 80, 18, FormatSkill( c, SkillName.Macing ), false, false );
				
				AddHtml( 20, 112, 160, 18, "<basefont color = #FFFFFF>Fencing</basefont>", false, false ); // Fencing
				AddHtml( 155, 112, 80, 18, FormatSkill( c, SkillName.Fencing ), false, false );
				
				AddHtml( 20, 130, 160, 18, "<basefont color = #FFFFFF>Archery</basefont>", false, false ); // Archery
				AddHtml( 155, 130, 80, 18, FormatSkill( c, SkillName.Archery ), false, false );

				AddHtmlLocalized( 20, 148, 160, 18, 1044087, LabelColor, false, false ); // Tactics
				AddHtml( 155, 148, 80, 18, FormatSkill( c, SkillName.Tactics ), false, false );

				AddHtmlLocalized( 20, 166, 160, 18, 1044061, LabelColor, false, false ); // Anatomy
				AddHtml( 155, 166, 80, 18, FormatSkill( c, SkillName.Anatomy ), false, false );

				AddHtml( 20, 184, 160, 18, "<basefont color = #FFFFFF>Parrying</basefont>", false, false ); // Parrying
				AddHtml( 155, 184, 80, 18, FormatSkill( c, SkillName.Parry ), false, false );

				AddHtml( 20, 202, 160, 18, "<basefont color = #FFFFFF>Healing</basefont>", false, false ); // Healing
				AddHtml( 155, 202, 80, 18, FormatSkill( c, SkillName.Healing ), false, false );

				AddHtml( 20, 220, 160, 18, "<basefont color = #FFFFFF>Focus</basefont>", false, false ); // Focus
				AddHtml( 155, 220, 80, 18, FormatSkill( c, SkillName.Focus ), false, false );
				
				//Second Half
				AddHtml( 285, 40, 180, 18, "<basefont color = #FFD57A>Magical Abilities</basefont>", false, false ); // Magical Abilities

				AddHtml( 285, 58, 125, 18, "<basefont color = #FFFFFF>Magery</basefont>", false, false ); // Magery
				AddHtml( 420, 58, 80, 18,FormatSkill( c, SkillName.Magery ), false, false );
				
				AddHtml( 285, 76, 125, 18, "<basefont color = #FFFFFF>Eval Int</basefont>", false, false ); // EvalInt
				AddHtml( 420, 76, 80, 18, FormatSkill( c, SkillName.EvalInt ), false, false );
				
				AddHtml( 285, 94, 125, 18, "<basefont color = #FFFFFF>Meditation</basefont>", false, false ); // Meditation
				AddHtml( 420, 94, 80, 18, FormatSkill( c, SkillName.Meditation ), false, false );
				
				AddHtml( 285, 112, 125, 18, "<basefont color = #FFFFFF>Necromancy</basefont>", false, false ); // Necromancy
				AddHtml( 420, 112, 80, 18, FormatSkill( c, SkillName.Necromancy ), false, false );
				
				AddHtml( 285, 130, 125, 18, "<basefont color = #FFFFFF>Spirit Speak</basefont>", false, false ); // Spirit Speak
				AddHtml( 420, 130, 80, 18, FormatSkill( c, SkillName.SpiritSpeak ), false, false );
				
				AddHtml( 285, 148, 125, 18, "<basefont color = #FFFFFF>Spellweaving</basefont>", false, false ); // Spellweaving
				AddHtml( 420, 148, 80, 18, FormatSkill( c, SkillName.Spellweaving ), false, false );
				
				AddHtml( 285, 166, 125, 18, "<basefont color = #FFFFFF>Chivalry</basefont>", false, false ); // Chivalry
				AddHtml( 420, 166, 80, 18, FormatSkill( c, SkillName.Chivalry ), false, false );
				
			    AddHtml( 285, 184, 125, 18, "<basefont color = #FFFFFF>Bushido</basefont>", false, false); // Bushido
                AddHtml(420, 184, 80, 18, FormatSkill(c, SkillName.Bushido), false, false);

				AddHtml( 285, 202, 125, 18, "<basefont color = #FFFFFF>Ninjitsu</basefont>", false, false); // Ninjitsu
				AddHtml(420, 202, 80, 18, FormatSkill(c, SkillName.Ninjitsu), false, false);
				
				AddHtml( 285, 220, 125, 18, "<basefont color = #FFFFFF>Mysticism</basefont>", false, false ); // Mysticism
				AddHtml(420, 220, 80, 18, FormatSkill(c, SkillName.Mysticism), false, false);
				
				AddHtml( 285, 238, 125, 18, "<basefont color = #FFFFFF>Magic Resist</basefont>", false, false ); // Magic Resist
				AddHtml(420, 238, 80, 18, FormatSkill(c, SkillName.MagicResist), false, false);
				
				//Navigation
				AddHtml( 40, 5, 160, 18, "<basefont color = #FFD57A>Stats</basefont>", false, false );
				AddButton( 20, 6, 5603, 5607, GetButtonID( 2, 2 ), GumpButtonType.Reply, 0 );
				
			}
		}
			#endregion
	}
}
