

using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
    public class VIPCloak : Cloak
    {
       

       

        [Constructable]
        public VIPCloak()
        {
            Name = "VIP Cloak";
            Hue = 2050;
            LootType = LootType.Blessed;
            Attributes.NightSight = 1;
            Attributes.BonusStr = 350;
            Attributes.BonusDex = 350;
            Attributes.BonusInt = 350;
            Attributes.RegenHits = 25;
            Attributes.RegenStam = 25;
            Attributes.RegenMana = 25;



        }
		public override bool OnEquip(Mobile from)
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
        }

        public VIPCloak(Serial serial) : base( serial )
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
    } // End Class
} // End Namespace
