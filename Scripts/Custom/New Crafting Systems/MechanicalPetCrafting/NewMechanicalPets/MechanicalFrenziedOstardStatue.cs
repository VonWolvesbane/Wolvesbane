    //////////////////////////////////
   //			           //
  //      Scripted by Raelis      //
 //		             	 //
//////////////////////////////////
using System; 
using System.Collections;
using Server.Items; 
using Server.Mobiles; 
using Server.Misc;
using Server.Network;


namespace Server.Items 
{ 
   	public class MechanicalFrenziedOstardPetStatue: Item
   	{ 
		public bool m_AllowEvolution;
		public Timer m_EvolutionTimer;
		private DateTime m_End;

		[CommandProperty( AccessLevel.GameMaster )]
		public bool AllowEvolution
		{
			get{ return m_AllowEvolution; }
			set{ m_AllowEvolution = value; }
		}

		[Constructable]
		public MechanicalFrenziedOstardPetStatue() : base( 9652 )
		{
			Weight = 0.0;
			Name = "a Mechanical Frenzied Ostard Pet Statue";
			Hue = 981;
			AllowEvolution = true;

			m_EvolutionTimer = new EvolutionTimer( this, TimeSpan.FromDays( 1.0 ) );
			m_EvolutionTimer.Start();
			m_End = DateTime.Now + TimeSpan.FromDays( 1.0 );
		}

            	public MechanicalFrenziedOstardPetStatue( Serial serial ) : base ( serial )
            	{             
           	}

		public override void OnDoubleClick( Mobile from )
		{
            if (!(from is PlayerMobile && from.Skills[SkillName.Tinkering].Base >= 80.0))
            {
                from.SendLocalizedMessage(1044153); // You don't have the required skills to attempt this item.
            }

            else if ( !IsChildOf( from.Backpack ) ) 
                        { 
                                from.SendMessage( "You must have the Statue in your backpack to release the mechanical frenzied ostard pet." );
                        } 
			else if ( this.AllowEvolution == true )
			{
				this.Delete();
				from.SendMessage( "You have now freed the Mechanical Frenzied Ostard Pet from its Statue!!" );

				MechanicalFrenziedOstard dragon = new MechanicalFrenziedOstard ();

         			dragon.Map = from.Map; 
         			dragon.Location = from.Location; 

				dragon.Controlled = true;

				dragon.ControlMaster = from;

				dragon.IsBonded = true;
			}
			else
			{
				from.SendMessage( "The Mechanical Frenzied Ostard Pet is not yet ready to be released." );
			}
		}

           	public override void Serialize( GenericWriter writer ) 
           	{ 
              		base.Serialize( writer ); 
              		writer.Write( (int) 1 ); 
			writer.WriteDeltaTime( m_End );
           	} 
            
           	public override void Deserialize( GenericReader reader ) 
           	{ 
              		base.Deserialize( reader ); 
              		int version = reader.ReadInt();

			switch ( version )
			{
				case 1:
				{
					m_End = reader.ReadDeltaTime();
					m_EvolutionTimer = new EvolutionTimer( this, m_End - DateTime.Now );
					m_EvolutionTimer.Start();

					break;
				}
				case 0:
				{
					TimeSpan duration = TimeSpan.FromDays( 1.0 );

					m_EvolutionTimer = new EvolutionTimer( this, duration );
					m_EvolutionTimer.Start();
					m_End = DateTime.Now + duration;

					break;
				} 
			}
           	} 

		private class EvolutionTimer : Timer
		{ 
			private MechanicalFrenziedOstardPetStatue de;

			private int cnt = 0;

			public EvolutionTimer( MechanicalFrenziedOstardPetStatue owner, TimeSpan duration ) : base( duration )
			{ 
				de = owner;
			}

			protected override void OnTick() 
			{
				cnt += 1;

				if ( cnt == 1 )
				{
					de.AllowEvolution = true;
				}
			}
		}
        } 
} 
