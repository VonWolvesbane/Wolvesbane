using System;
using Server.Misc;

namespace Server.Items
{
	[Flipable (0x1F04, 0x1F03)] 
	public class StevesShirt : BaseShirt
	{ 	
		[Constructable] 
		public StevesShirt() : base( 0x1EFD ) 
		{ 
			Name = "Steve Irwins Shirt Of Nature";
            Weight = 1.0; 
            Layer = Layer.Shirt;
			Hue = 67;
			this.SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
			this.SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
			this.SkillBonuses.SetValues(2, SkillName.Veterinary, 15.0);
			this.SkillBonuses.SetValues(3, SkillName.Healing, 15.0);
		} 

		

		public override void OnSingleClick( Mobile from ) 
		{ 
			this.LabelTo( from, Name ); 
		} 

		public StevesShirt( Serial serial ) : base( serial ) 
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
