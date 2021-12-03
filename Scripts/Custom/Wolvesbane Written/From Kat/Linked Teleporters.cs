using System;
using Server.Items;
using Server;
using Server.Mobiles;

/******************************\
* Linked Teleporters By Misha  *
\******************************/

namespace Server.Items
{
	// LinkedTeleporter Item, Useless alone, use [add LinkedTeleporters instead.
	public class LinkedTeleporter : Item
	{
		public LinkedTeleporter otherteleporter;
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
		public LinkedTeleporter() : base( 7107 ) // teleporter
		{
			Weight = 20.0;
			Movable = false;
			Name = "Linked Teleporters (Must Be Placed By Staff)";
			Hue = 38;
			AllowPets = true;
			AllowCreatures = false;
		}
		
		public LinkedTeleporter( Serial serial ) : base( serial )
		{
		}
		#endregion
		
		public override void Delete()
		{
			
			if ( this.otherteleporter != null )
			{
				LinkedTeleporter theother;
				theother = this.otherteleporter;
			
				theother.otherteleporter = null;
				this.otherteleporter.Delete();
			}
			
			this.otherteleporter = null;
				
			base.Delete();
		}
		
		public override bool OnMoveOver( Mobile m )
		{
			if ( (this.otherteleporter != null) )
			{
				if (this.otherteleporter.Parent == null)
				{
					if ( !(m is PlayerMobile) && !m_allowCreatures)
						return false;
					if (m_allowPets)
						BaseCreature.TeleportPets( m, otherteleporter.Location, otherteleporter.Map );
					m.Location = otherteleporter.Location;
					m.Map = otherteleporter.Map;
					return false;
				}
				else
				{
					m.SendMessage("The other teleporter is in a container and cant be used!");
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
			writer.Write( this.otherteleporter );
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
			otherteleporter = (LinkedTeleporter) reader.ReadItem();
			m_allowPets = reader.ReadBool();

		}
		#endregion
	}


	// A bag with 2 teleporters linked together.
	// Removing one will delete both.
	public class LinkedTeleporters : Bag
	{	
		#region Constructors
		[Constructable]
		public LinkedTeleporters() // Bag
		{
			int randomhue = Utility.RandomList(38, 33, 53, 83, 88, 13, 18, 43, 3, 6, 926); // Various Colors
			Name = "Linked Teleporters (Must Be Placed By Staff)";
			Hue = randomhue;
			
			LinkedTeleporter lg1 = new LinkedTeleporter();
			LinkedTeleporter lg2 = new LinkedTeleporter();
			
			// Set Up.
			lg1.otherteleporter = lg2;
			lg1.Hue = randomhue;
			lg2.otherteleporter = lg1;
			lg2.Hue = randomhue;
			
			// Put them in bag.
			this.AddItem(lg1);
			lg1.Location = new Point3D (29,34,0);
			this.AddItem(lg2);
			lg1.Location = new Point3D (93,34,0);
			
		}
		
		public LinkedTeleporters( Serial serial ) : base( serial )
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