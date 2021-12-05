using System; 
using Server; 
using Server.Items;

namespace Server.Items
{ 
   public class GargNewbieArmor : Bag 
   { 
		[Constructable] 
		public GargNewbieArmor() : this( 1 ) 
		{ 
			Movable = true;  
			Name = "A Bag Of Newbie Garg Armor";
			Hue = 1910;
		}
		[Constructable]
		public GargNewbieArmor( int amount )
		{
			DropItem( new NewbieGargWings() );
			DropItem( new NewbieGargChest() );
			
            DropItem( new NewbieGargKilt() );
			DropItem( new NewbieGargLegs() );
			DropItem( new NewbieGargArms() );
            
		}

      public GargNewbieArmor( Serial serial ) : base( serial ) 
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
