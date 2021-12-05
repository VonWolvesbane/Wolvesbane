using System;

namespace Server.Items
{
    [Flipable]
    public class TrayCandles : BaseLight 		//name
    {
        [Constructable]
        public TrayCandles() 			//NameAgain
            : base(0x983F)  			// litsouth facing
        {
	    Name = "Tray of Candles";
            Movable = true;
            Duration = TimeSpan.Zero; 	// Never burnt out
            Burning = false;  		//use oil?
            Light = LightType.Circle300;
            Weight = 10;
        }

        public TrayCandles(Serial serial)	// Name Again.
            : base(serial)
        {
        }

        public override int LitItemID
        {
            get
            {
                if (this.ItemID == 0x983A) 	//Unlit East
                    return 0x983B; 		// Lit East
                else
                    return 0x983F; 		// lit south
            }
        }
        public override int UnlitItemID
        {
            get
            {
                if (this.ItemID == 0x983B) 	// lit east
                    return 0x983A; 		// unlit east
                else
                    return 0x983E; 		// unlit south
            }
        }
        public void Flip()
        {
            switch ( this.ItemID )
            {
                case 0x983A: 		  // unlit east
                    this.ItemID = 0x983E; // unlit south
                    break;
                case 0x983E: 		  // unlit south  
                    this.ItemID = 0x983A; // unlit east
                    break;
                case 0x983B: 		  // lit east  
                    this.ItemID = 0x983F; // lit south
                    break;
                case 0x983F: 		  // lit south 
                    this.ItemID = 0x983B; // lit east
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