using System;
using Server;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class PumpkinSludge : Item
	{
		[Constructable]
		public PumpkinSludge() : base( 0x913 )
		{
			Name = "a pile of pumpkin sludge";
			Hue = 0x2C;
			Weight = 1.0;
			LootType = LootType.Blessed;
		}

		public PumpkinSludge( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version == 0 )
			{
				Weight = 1.0;
				LootType = LootType.Blessed;
			}
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042010 ); // You must have the object in your backpack to use it.
			}
			else if ( from.CanBeginAction( typeof( SnowPile ) ) )
			{
				from.SendMessage( "You carefully pack the sludge into a ball..." );
				from.Target = new SnowTarget( from, this ); // **sludge
			}
			else
			{
				from.SendMessage( "The sludge is not ready to be packed yet. Keep trying." );
			}
		}

		private class InternalTimer : Timer
		{
			private Mobile m_From;

			public InternalTimer( Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_From = from;
			}

			protected override void OnTick()
			{
				m_From.EndAction( typeof( SnowPile ) ); //**sludge
			}
		}

		private class SnowTarget : Target
		{
			private Mobile m_Thrower;
			private Item m_Snow;

			public SnowTarget( Mobile thrower, Item snow ) : base ( 10, false, TargetFlags.None )
			{
				m_Thrower = thrower;
				m_Snow = snow;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				if ( target == from )
				{
					from.SendLocalizedMessage( 1005576 ); // You can't throw this at yourself.
				}
				else if ( target is Mobile )
				{
					Mobile targ = (Mobile) target;
					Container pack = targ.Backpack;

					if ( pack != null && pack.FindItemByType( new Type[]{ typeof( SnowPile ), typeof( PumpkinSludge ) } ) != null )
					{
						if ( from.BeginAction( typeof( SnowPile ) ) ) // **sludge
						{
							new InternalTimer( from ).Start();

							from.PlaySound( 0x145 );

							from.Animate( 9, 1, 1, true, false, 0 );

							targ.SendMessage( "You have just been hit by a ball of sludge!" );
							from.SendMessage( "You throw the ball of sludge and hit your mark!" );

							Effects.SendMovingEffect( from, targ, 0x36E4, 7, 0, false, true, 0x2C, 0 ); //0x36E4---0x47F
						}
						else
						{
							from.SendMessage( "The sludge is not ready to be packed yet." ); 
						}
					}
					else
					{
						from.SendMessage( "You can only throw sludge at something that can throw it back." );
					}
				}
				else
				{
					from.SendMessage( "You can only throw sludge at something that can throw it back." );
				}
			}
		}
	}
}