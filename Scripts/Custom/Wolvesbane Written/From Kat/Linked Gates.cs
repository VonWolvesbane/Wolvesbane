using System;
using Server.Items;
using Server;
using Server.Mobiles;

/******************************\
*   Linked Gates Script v1.1   *
*                   By JuanI   *
\******************************/

namespace Server.Items
{
	// LinkedGate Item, Useless alone, use [add LinkedGates instead.
	public class LinkedGate : Item
	{
		public LinkedGate othergate;
		private bool m_allowPets, m_allowCreatures;


		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowPets
		{
			get { return m_allowPets; }
			set { m_allowPets = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowCreatures
		{
			get { return m_allowCreatures; }
			set { m_allowCreatures = value; }
		}
	
		#region Constructors
		[Constructable]
		public LinkedGate() : base( 19403 ) // gate
		{
			Weight = 1000.0;
			Movable = false;
			Name = "Linked Gate";
			Light = LightType.Circle300;
			Hue = 0;
			AllowPets = true;
			AllowCreatures = false;
		}
		
		public LinkedGate( Serial serial ) : base( serial )
		{
		}
		#endregion
		
		public override void Delete()
		{
			
			if ( this.othergate != null )
			{
				LinkedGate theother;
				theother = this.othergate;
			
				theother.othergate = null;
				this.othergate.Delete();
			}
			
			this.othergate = null;
				
			base.Delete();
		}
		
		public override bool OnMoveOver( Mobile m )
		{
			if ( (this.othergate != null) )
			{
				if (this.othergate.Parent == null)
				{
					if ( !(m is PlayerMobile) && !m_allowCreatures)
						return false;
					if (m_allowPets)
						BaseCreature.TeleportPets( m, othergate.Location, othergate.Map );
					m.Location = othergate.Location;
					m.Map = othergate.Map;
					return false;
				}
				else
				{
					m.SendMessage("The other gate is in a container and cant be used!");
					return false;
				}
			}
			return false;
		}
		
		#region Serialization
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			writer.Write( (bool) m_allowCreatures );
			writer.Write( this.othergate );
			writer.Write( (bool) m_allowPets );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			if ( version >= 1 )
				m_allowCreatures = reader.ReadBool();
			else
				m_allowCreatures = false;
			othergate = (LinkedGate) reader.ReadItem();
			m_allowPets = reader.ReadBool();

		}
		#endregion
	}


	// A bag with 2 gates linked together.
	// Removing one will delete both.
	public class LinkedGates : Bag
	{	
		#region Constructors
		[Constructable]
		public LinkedGates() // Bag
		{
			int randomhue = Utility.RandomList(68, 33, 53, 83, 88, 13, 18, 43, 3, 6, 926); // Various Colors
			Name = "Linked Gates";
			Hue = randomhue;
			
			LinkedGate lg1 = new LinkedGate();
			LinkedGate lg2 = new LinkedGate();
			
			// Set Up.
			lg1.othergate = lg2;
			lg1.Hue = randomhue;
			lg2.othergate = lg1;
			lg2.Hue = randomhue;
			
			// Put them in bag.
			this.AddItem(lg1);
			lg1.Location = new Point3D (29,34,0);
			this.AddItem(lg2);
			lg1.Location = new Point3D (93,34,0);
			
		}
		
		public LinkedGates( Serial serial ) : base( serial )
		{
		}
		#endregion
		
		#region Serialization
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
		#endregion		
	}
}