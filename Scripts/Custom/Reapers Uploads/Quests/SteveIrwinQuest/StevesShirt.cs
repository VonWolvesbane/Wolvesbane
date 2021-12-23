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
			DefineMods();
		} 

		private void DefineMods()
		{
			SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15);
			SkillBonuses.SetValues(1, SkillName.AnimalLore, 15);
			SkillBonuses.SetValues(2, SkillName.Healing, 15);
			SkillBonuses.SetValues(3, SkillName.Veterinary, 15);
		}

		public override bool Dye( Mobile from, DyeTub sender )
		{
			from.SendLocalizedMessage( 1042083 ); // You cannot dye that.
			return false;
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
			base.Deserialize(reader);
			int version = reader.ReadInt();
			// This line is only needed for 1 world reload. After that the saved data will be correct and this can be removed.
			DefineMods();
		} 
	} 
} 
