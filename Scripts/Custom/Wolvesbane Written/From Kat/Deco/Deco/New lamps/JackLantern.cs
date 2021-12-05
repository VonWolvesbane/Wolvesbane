using System;

namespace Server.Items
{
    [Flipable]
    public class JackLantern : BaseLight 	//name (this item has 11 animated jack-o-lanterns Flip2Change)
    {
        [Constructable]
        public JackLantern() 			//NameAgain
            : base(0x4691 )  			// litsouth facing
        {
	    Name = "Jack-o-Lantern";
            Movable = true;
            Duration = TimeSpan.Zero; 	// Never burnt out
            Burning = false;  		//use oil?
            Light = LightType.Circle150;
            Weight = 40;
        }

        public JackLantern(Serial serial)	// Name Again.
            : base(serial)
        {
        }

        public override int LitItemID  // turn on
        {
            get
            {
                if (this.ItemID == 0x4694) 	//Unlit East
                    return 0x4691; 		// Lit East
		else if (this.ItemID == 0x4698)
		    return 0x4695;
		else if (this.ItemID == 0x9934)
		    return 0x9931;
		else if (this.ItemID == 0x9935)
		    return 0x9936;
		else if (this.ItemID == 0x9939)
		    return 0x993A;
		else if (this.ItemID == 0x993D)
		    return 0x993E;
		else if (this.ItemID == 0x9941)
		    return 0x9942;
		else if (this.ItemID == 0x9945)
		    return 0x9946;
		else if (this.ItemID == 0x9949)
		    return 0x994A;
		else if (this.ItemID == 0x994D)
		    return 0x994E;
                else
                    return 0x9952; 		// lit south
            }
        }
        public override int UnlitItemID  // turn off
        {
            get
            {
                if (this.ItemID == 0x4691) 	// lit east
                    return 0x4694; 		// unlit east
		else if (this.ItemID == 0x4695)
		    return 0x4698;
		else if (this.ItemID == 0x9931)
		    return 0x9934;
		else if (this.ItemID == 0x9936)
		    return 0x9935;
		else if (this.ItemID == 0x993A)
		    return 0x9939;
		else if (this.ItemID == 0x993E)
		    return 0x993D;
		else if (this.ItemID == 0x9942)
		    return 0x9941;
		else if (this.ItemID == 0x9946)
		    return 0x9945;
		else if (this.ItemID == 0x994A)
		    return 0x9949;
		else if (this.ItemID == 0x994E)
		    return 0x994D;
                else
                    return 0x9951; 		// unlit south
            }
        }
        public void Flip() // switch too
        {
            switch ( this.ItemID )
            {
                case 0x4694: 	     	 // Flip between unlighted itemids
                    this.ItemID = 0x4698; 
                    break;
                case 0x4698: 		   
                    this.ItemID = 0x9934; 
                    break;
                case 0x9934: 	     	 
                    this.ItemID = 0x9935; 
                    break;
                case 0x9935: 		  
                    this.ItemID = 0x9939; 
                    break;
                case 0x9939: 	     	 
                    this.ItemID = 0x993D;
                    break;
                case 0x993D: 		  
                    this.ItemID = 0x9941; 
                    break;
                case 0x9941: 	     	 
                    this.ItemID = 0x9945; 
                    break;
                case 0x9945: 		  
                    this.ItemID = 0x9949;
                    break;
                case 0x9949: 	     	  
                    this.ItemID = 0x994D; 
                    break;
                case 0x994D: 		   
                    this.ItemID = 0x9951; 
                    break;
                case 0x9951: 	      	  
                    this.ItemID = 0x4694; // End Flip between Unlighted Id's
                    break;
                case 0x4691: 	      // flip between lighted item id's
                    this.ItemID = 0x4695; 
                    break;
                case 0x4695: 		   
                    this.ItemID = 0x9931; 
                    break;
                case 0x9931: 	     
                    this.ItemID = 0x9936; 
                    break;
                case 0x9936: 		  
                    this.ItemID = 0x993A; 
                    break;
                case 0x993A: 	     
                    this.ItemID = 0x993E; 
                    break;
                case 0x993E: 		   
                    this.ItemID = 0x9942;
                    break;
                case 0x9942: 	      
                    this.ItemID = 0x9946; 
                    break;
                case 0x9946: 		   
                    this.ItemID = 0x994A; 
                    break;
                case 0x994A: 	      
                    this.ItemID = 0x994E; 
                    break;
                case 0x994E: 	 
                    this.ItemID = 0x9952; 
                    break;
                case 0x9952: 	      
                    this.ItemID = 0x4691;
                    break;

            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}