using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Gumps;
using System.Collections;
using Server.Targeting;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{

   public class PetWarsscroll : Item
   {
	   
	    private int m_AI;
	    private Mobile m_Pet;
	   	private int m_Str;
		private int m_Dex;
		private int m_Int;
		private int m_Hits;
		private int m_Stam;
		private int m_Mana;
		private int m_Phys;
		private int m_Fire;
		private int m_Cold;
		private int m_Nrgy;
		private int m_Pois;
		private int m_Dmin;
		private int m_Dmax;
		private int m_Mlev;
	   	
		
	
		[CommandProperty( AccessLevel.Administrator )]
		public Mobile Pet
		{
			get{ return m_Pet; }
			set{ m_Pet = value; }
		}
				[CommandProperty( AccessLevel.Administrator )]
		public int Str
		{
			get{ return m_Str; }
			set{ m_Str = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Dex
		{
			get{ return m_Dex; }
			set{ m_Dex = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Int
		{
			get{ return m_Int; }
			set{ m_Int = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Hits
		{
			get{ return m_Hits; }
			set{ m_Hits = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Stam
		{
			get{ return m_Stam; }
			set{ m_Stam = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Mana
		{
			get{ return m_Mana; }
			set{ m_Mana = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Phys
		{
			get{ return m_Phys; }
			set{ m_Phys = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Fire
		{
			get{ return m_Fire; }
			set{ m_Fire = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Cold
		{
			get{ return m_Cold; }
			set{ m_Cold = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Nrgy
		{
			get{ return m_Nrgy; }
			set{ m_Nrgy = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Pois
		{
			get{ return m_Pois; }
			set{ m_Pois = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Dmin
		{
			get{ return m_Dmin; }
			set{ m_Dmin = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Dmax
		{
			get{ return m_Dmax; }
			set{ m_Dmax = value; }
		}

		[CommandProperty( AccessLevel.Administrator )]
		public int Mlev
		{
			get{ return m_Mlev; }
			set{ m_Mlev = value; }
		}
		public int AI
		{
			get{ return m_AI; }
			set{ m_AI = value; }
		}
		
      [Constructable]
      public PetWarsscroll() : base(0x14F0)
      {
         Name = "Pet Wars scroll Claim your pet!";
		 LootType = LootType.Blessed;
         Hue = 1156;
		 //ItemID = 0x1726;
		 m_Pet = null;
		 m_Str = 250;
		 m_Dex = 200;
		 m_Int = 250;
		 m_Hits = 500;
		 m_Stam = 200;
		 m_Mana = 500;
		 m_Phys = 20;
		 m_Fire = 20;
		 m_Cold = 20;
		 m_Nrgy = 20;
		 m_Pois = 20;
		 m_Dmin = 25;
		 m_Dmax = 35;
		 m_Mlev = 50;
		 
		 this.AI = 1;
      }
	  
      public override void OnDoubleClick( Mobile from )
      {			
		if ( !IsChildOf( from.Backpack ) )
			{
				from.SendMessage("This must be in your backpack to use it.");
			}
			else if ( m_Pet != null )
			{
				//
				Type pettype = this.Pet.GetType();
				BaseCreature bc = (BaseCreature)this.Pet;
				BaseCreature baby = null;
				//BaseCreature baby = (BaseCreature)Activator.CreateInstance(m_Pet.GetType);
				
				if ( pettype != null )
						{
							object o = Activator.CreateInstance( pettype );
        						baby = o as BaseCreature;
						}
						
				from.FixedParticles( 0x373A, 10, 15, 5036, EffectLayer.Head ); 
				from.PlaySound( 521 );
				baby.Controlled = true;
				baby.ControlMaster = from;
				baby.IsBonded = true;
				baby.Title = "Pet Wars";
				
				if ( this.AI == 1 )
				baby.AI = AIType.AI_Mage;
				else if ( this.AI == 2 )
				baby.AI = AIType.AI_Melee;
			
				baby.Str = this.Str;
				baby.Dex = this.Dex;
				baby.Int = this.Int;
				baby.HitsMaxSeed = this.Hits;
				baby.StamMaxSeed = this.Stam;
				baby.ManaMaxSeed = this.Mana;
				baby.PhysicalResistanceSeed = this.Phys;
				baby.FireResistSeed = this.Fire;
				baby.ColdResistSeed = this.Cold;
				baby.EnergyResistSeed = this.Nrgy;
				baby.PoisonResistSeed = this.Pois;
				baby.DamageMin = this.Dmin;
				baby.DamageMax = this.Dmax;
				//baby.MaxLevel = this.Mlev;
				baby.Location = from.Location;
				baby.Map = from.Map;
				World.AddMobile( baby );

				from.SendMessage( "You Claimed your petwars pet." );
				this.Delete();
			} 

		else
			{	
		        from.SendMessage("You can't use this!");
               // from.Target = new PetWarsscrollTarget(this); // Call our target
			}
      }
	  
      public PetWarsscroll( Serial serial ) : base( serial )
      {
      }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		 writer.Write( m_AI );
		 
		 	writer.Write( m_Pet );
		 	writer.Write( m_Str );
			writer.Write( m_Dex );
			writer.Write( m_Int );
			writer.Write( m_Hits );
			writer.Write( m_Stam );
			writer.Write( m_Mana );
			writer.Write( m_Phys );
			writer.Write( m_Fire );
			writer.Write( m_Cold );
			writer.Write( m_Nrgy );
			writer.Write( m_Pois );
			writer.Write( m_Dmin );
			writer.Write( m_Dmax );
			writer.Write( m_Mlev );

		 
		 
      }

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{

		case 1:
				{
					m_AI = reader.ReadInt(); // AI Fix
					goto case 0;
				}
				case 0:
				{
					m_Pet = reader.ReadMobile();
					m_Str = reader.ReadInt();
					m_Dex = reader.ReadInt();
					m_Int = reader.ReadInt();
					m_Hits = reader.ReadInt();
					m_Stam = reader.ReadInt();
					m_Mana = reader.ReadInt();
					m_Phys = reader.ReadInt();
					m_Fire = reader.ReadInt();
					m_Cold = reader.ReadInt();
					m_Nrgy = reader.ReadInt();
					m_Pois = reader.ReadInt();
					m_Dmin = reader.ReadInt();
					m_Dmax = reader.ReadInt();
					m_Mlev = reader.ReadInt();					

				break;
				}
			}
		}
	}
}

