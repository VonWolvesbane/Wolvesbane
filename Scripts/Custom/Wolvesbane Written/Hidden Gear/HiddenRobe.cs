using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class HiddenRobe : BaseOuterTorso
	{




		[Constructable]
		public HiddenRobe() :base(0xA2CA)
		{
			Name = "Hidden Robe Skin";
			Hue = 2050;
			ItemID = 50345;
			LootType = LootType.Blessed;
			Attributes.NightSight = 1;

			this.SkillBonuses.SetValues(0, SkillName.Veterinary, 10.0);

			this.SkillBonuses.SetValues(1, SkillName.AnimalTaming, 10.0);

			this.SkillBonuses.SetValues(2, SkillName.AnimalLore, 10.0);
		}

		/*public override bool OnEquip(Mobile from)
		{
			// Check if the mobile is a PlayerMobile and has IsPatreon set to true
			if (from is PlayerMobile player && player.IsPatreon)
			{
				// Call the base method to allow the equip operation
				return base.OnEquip(from);
			}
			else
			{
				// Send a message to the player indicating they cannot equip the item
				from.SendMessage("You are not a Wolvesbane UO Patreon Member, and therefore are not authorized to equip this item.");
				return false; // Deny the equip operation
			}
		}

		public override void OnSingleClick(Mobile from)
		{
			this.LabelTo(from, Name);
		}*/

		public HiddenRobe(Serial serial) : base(serial)
		{
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
	} // End Class
} // End Namespace
