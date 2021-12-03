using Server; 
using Server.Items;
using Server.Multis;
using Server.Prompts;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Targeting;
using Server.ContextMenus;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.Items
{
	public class BlacksmithMachineE : BaseAddon
	{
		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } }
		public override BaseAddonDeed Deed{ get{ return new BlacksmithMachineEDeed(); } }

		[Constructable]
		public BlacksmithMachineE()
		{
			AddonComponent ac;
			ac = new AddonComponent( 0x9AA8 );
			ac.Name = "Blacksmith Machine East";
			AddComponent( ac, 0, 0, 0 );

			ac = new AddonComponent( 0x9A91 );
			ac.Name = "Blacksmith Machine";
			AddComponent( ac, 0, -1, 0 );

            		Name = "Blacksmith Machine East";
		}

        	public override void OnComponentUsed(AddonComponent ac, Mobile from)
        	{
                	if (!from.InRange(GetWorldLocation(), 2))
                   	from.SendMessage("You are too far away.");

				switch ( ac.ItemID )
				{
				  case 0x9AA8: ac.ItemID = 0x9A81; break;   // on
				  case 0x9A81: ac.ItemID = 0x9AA8; break;   // off
				}

				if ( ac.ItemID == 0x9A81 )
				{					        
				    from.PlaySound( 42 );
				}
				if ( ac.ItemID == 0x9AA8 )
				{					        
				    from.PlaySound( 42 );
				}
				return;
        	}

		public BlacksmithMachineE( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class BlacksmithMachineEDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new BlacksmithMachineE(); } }

		[Constructable]
		public BlacksmithMachineEDeed()
		{
            		Name = "Blacksmith Machine East Deed";
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( "Double Click Machine On & Off" );
		}

		public BlacksmithMachineEDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
//----------------------------------
	public class BlacksmithMachineS : BaseAddon
	{
		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } }
		public override BaseAddonDeed Deed{ get{ return new BlacksmithMachineSDeed(); } }

		[Constructable]
		public BlacksmithMachineS()
		{
			AddonComponent ac;
			ac = new AddonComponent( 0x9AA9 );
			ac.Name = "Blacksmith Machine South";
			AddComponent( ac, 0, 0, 0 );

			ac = new AddonComponent( 0x9A91 );
			ac.Name = "Blacksmith Machine";
			AddComponent( ac, -1, 0, 0 );

            		Name = "Blacksmith Machine South";
		}

        	public override void OnComponentUsed(AddonComponent ac, Mobile from)
        	{
                	if (!from.InRange(GetWorldLocation(), 2))
                   	from.SendMessage("You are too far away.");

				switch ( ac.ItemID )
				{
				  case 0x9AA9: ac.ItemID = 0x9A89; break;   // on
				  case 0x9A89: ac.ItemID = 0x9AA9; break;   // off
				}

				if ( ac.ItemID == 0x9AA9 )
				{					        
				    from.PlaySound( 42 );
				}
				if ( ac.ItemID == 0x9A89 )
				{					        
				    from.PlaySound( 42 );
				}
				return;
        	}

		public BlacksmithMachineS( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class BlacksmithMachineSDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new BlacksmithMachineS(); } }

		[Constructable]
		public BlacksmithMachineSDeed()
		{
            		Name = "Blacksmith Machine South Deed";
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( "Double Click Machine On & Off" );
		}

		public BlacksmithMachineSDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}