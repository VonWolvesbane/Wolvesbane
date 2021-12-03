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
	public class SewingMachineE : BaseAddon
	{
		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } }
		public override BaseAddonDeed Deed{ get{ return new SewingMachineEDeed(); } }

		[Constructable]
		public SewingMachineE()
		{
			AddonComponent ac;
			ac = new AddonComponent( 0x9A48 );
			ac.Name = "Sewing Machine East";
			AddComponent( ac, 0, 0, 0 );

			ac = new AddonComponent( 0x9A4A );
			ac.Name = "Sewing Machine Stool";
			AddComponent( ac, -1, 0, 0 );

            		Name = "Sewing Machine East";
		}

        	public override void OnComponentUsed(AddonComponent ac, Mobile from)
        	{
                	if (!from.InRange(GetWorldLocation(), 2))
                   	from.SendMessage("You are too far away.");

				switch ( ac.ItemID )
				{
				  case 0x9A48: ac.ItemID = 0x9A38; break;   // on
				  case 0x9A38: ac.ItemID = 0x9A48; break;   // off
				}
        	}

		public SewingMachineE( Serial serial ) : base( serial )
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

	public class SewingMachineEDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new SewingMachineE(); } }

		[Constructable]
		public SewingMachineEDeed()
		{
            		Name = "Sewing Machine East Deed";
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( "Double Click to On & Off" );
		}

		public SewingMachineEDeed( Serial serial ) : base( serial )
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
	public class SewingMachineS : BaseAddon
	{
		public override bool ForceShowProperties{ get{ return ObjectPropertyList.Enabled; } }
		public override BaseAddonDeed Deed{ get{ return new SewingMachineSDeed(); } }

		[Constructable]
		public SewingMachineS()
		{
			AddonComponent ac;
			ac = new AddonComponent( 0x9A49 );
			ac.Name = "Sewing Machine South";
			AddComponent( ac, 0, 0, 0 );

			ac = new AddonComponent( 0x9A4A );
			ac.Name = "Sewing Machine Stool";
			AddComponent( ac, 0, -1, 0 );

            		Name = "Sewing Machine South";
		}

        	public override void OnComponentUsed(AddonComponent ac, Mobile from)
        	{
                	if (!from.InRange(GetWorldLocation(), 2))
                   	from.SendMessage("You are too far away.");

				switch ( ac.ItemID )
				{
				  case 0x9A49: ac.ItemID = 0x9A40; break;   // on
				  case 0x9A40: ac.ItemID = 0x9A49; break;   // off
				}
        	}

		public SewingMachineS( Serial serial ) : base( serial )
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

	public class SewingMachineSDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new SewingMachineS(); } }

		[Constructable]
		public SewingMachineSDeed()
		{
            		Name = "Sewing Machine South Deed";
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );
			list.Add( "Double Click to On & Off" );
		}

		public SewingMachineSDeed( Serial serial ) : base( serial )
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