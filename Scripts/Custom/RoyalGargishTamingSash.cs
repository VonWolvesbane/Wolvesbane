using System;
using Server.Misc;

namespace Server.Items
{
	
	public class RoyalGargishTamingSash : GargishSash 
	{ 
	
		
		[Constructable] 
		public RoyalGargishTamingSash() : base( 0x1EFD ) 
		{ 
			Name = "Royal Gargish Taming Sash";
            Weight = 1.0; 
            Layer = Layer.MiddleTorso;
			Hue = 67;
			this.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
			this.SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
			this.SkillBonuses.SetValues(2, SkillName.Healing, 15.0);
			this.SkillBonuses.SetValues(3, SkillName.Veterinary, 15.0);
		} 


		public override bool Dye( Mobile from, DyeTub sender )
		{
			from.SendLocalizedMessage( 1042083 ); // You cannot dye that.
			return false;
		}

		public RoyalGargishTamingSash(Serial serial) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); 
		} 

		public override void Deserialize(GenericReader reader) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	} 
} 
