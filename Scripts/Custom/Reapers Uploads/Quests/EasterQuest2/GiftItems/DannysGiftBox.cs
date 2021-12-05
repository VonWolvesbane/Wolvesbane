using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0x232A, 0x232B )]
	public class DannysGiftBox : BaseContainer
	{
		public override int DefaultGumpID{ get{ return 0x102; } }
		public override int DefaultDropSound{ get{ return 0x42; } }

		public override Rectangle2D Bounds
		{
			get{ return new Rectangle2D( 35, 10, 155, 85 ); }
		}

		[Constructable]
		public DannysGiftBox() : this( Utility.RandomDyedHue() )
		{
		}

		[Constructable]
		public DannysGiftBox( int hue ) : base( Utility.Random( 0x232A, 2 ) )
		{
			Weight = 2.0;
			Name = "A Gift from Danny";
			
			
             switch ( Utility.Random( 6 ) )
             {
             	
             		
             	case 0:
             		DropItem( new  EasterBasketLargeGiftAddonDeed()  );break;
             	case 1:
             		DropItem( new StuffedBunny() );break;
                case 2:
             		DropItem( new EasterHat() );break;
                case 3:
             		DropItem( new MovingEasterEgg() );break;
                 case 4:
             		DropItem( new ChocolateRabbit() );break;
                 case 5:
             	                DropItem( new EasterBunnyPetStatue() );break;
                
             		

                                }
             	
		}

		public DannysGiftBox( Serial serial ) : base( serial )
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