using System;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.Mage
{
	public class MageSpellbook : CSpellbook
	{
		public override School School{ get{ return School.Magery; } }

		[Constructable]
		public MageSpellbook() : this( (ulong)0, CSSettings.FullSpellbooks )
		{
		}

		[Constructable]
		public MageSpellbook( bool full ) : this( (ulong)0, full )
		{
		}

		[Constructable]
		public MageSpellbook( ulong content, bool full ) : base( content, 0xEFA, full )
		{
			Name = "Mage Spellbook";
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel == AccessLevel.Player )
			{
				bool bookLocationUsable = false;
				if (Parent == from)
					bookLocationUsable = true;

				Container pack = from.Backpack;
				if (pack != null && Parent == pack)
					bookLocationUsable = true;

				Container pack2 = Parent as Container;
				if (pack2 != null && pack2.CanCastFrom)
					bookLocationUsable = true;
				if (!bookLocationUsable)
				{
					from.SendMessage( "The spellbook must be in your backpack [and not in a container within] to open." );
					return;
				}
				else if( SpellRestrictions.UseRestrictions && !SpellRestrictions.CheckRestrictions( from, this.School ) )
				{
					return;
				}
			}

			from.CloseGump( typeof( MageSpellbookGump ) );
			from.SendGump( new MageSpellbookGump( this ) );
		}

		public MageSpellbook( Serial serial ) : base( serial )
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
	}
}
