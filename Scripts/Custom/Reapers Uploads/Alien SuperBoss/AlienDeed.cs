using System;
using Server;
using Server.Network;
using Server.Prompts;
using Server.Mobiles;
using Server.Multis;
using Server.Targeting;

namespace Server.Items
{
	public class AlienDeed : Item
	{
		[Constructable]
		public AlienDeed() : base( 0x14F0 )
		{
			ItemID = 18406;
			Weight = 1.0;
			Name = "A Strange SoS Device";
			Hue = 2816;
		}

		public AlienDeed( Serial serial ) : base ( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			else
				Use( from );
		}

		public void Use( Mobile from )
		{
			if ( from.AccessLevel >= AccessLevel.GameMaster )
			{
					from.SendMessage( "Your godly powers allow you to place The Alien wherever you want." );
					(new Alien1()).MoveToWorld( from.Location, from.Map );
					from.SendMessage( "An Alien seems drawn to you!" );
					this.Delete();
			}
			else
			{
				BaseHouse house = BaseHouse.FindHouseAt( from );

				if ( house == null )
				{	
					(new Alien1()).MoveToWorld( from.Location, from.Map );
					from.SendMessage( "You set off an SOS Signal and an Alien apears!" );
					this.Delete();
				}
					from.LocalOverheadMessage( MessageType.Regular, 0x3B2, false, "You can't use this inside.." );
					return;
			}
		
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class AlienDeedTarget : Target
	{
		private AlienDeed m_Deed;

		public AlienDeedTarget( AlienDeed deed ) : base( 1, false, TargetFlags.None )
		{
			m_Deed = deed;
		}

		protected override void OnTarget( Mobile from, object target )// Override the protected OnTarget() for our feature
		{
			if ( m_Deed.Deleted )
				return;

			if ( !( target is Item && ( target is BaseArmor || target is BaseWeapon )))
				from.SendMessage( "You must target the ground" );
			
			else
			{
				m_Deed.Delete(); // Delete the deed
			}
		}
	}
}
