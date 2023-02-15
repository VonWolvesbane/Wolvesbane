using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server;

namespace Server.Items
{
    public class WeaponRangeDeedTarget : Target // Create our targeting class (which we derive from the base target class)
    {
        private WeaponRangeDeed m_Deed;

        public WeaponRangeDeedTarget(WeaponRangeDeed deed) : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target) // Override the protected OnTarget() for our feature
        {
            if (target is BaseWeapon)
            {
                Item item = (Item)target;
				
                if (item.RootParent != from) // Make sure its in their pack or they are wearing it
                    {
                        from.SendMessage("You cannot put self repair on that there!"); // You cannot bless that object
                    }
					
                else 
				{
					if (((BaseWeapon)item).MaxRange >= 2)
                {
                    from.SendMessage("That is already at max range");
                }
					if (((BaseWeapon)item).MaxRange == 1)
                {
            
                        ((BaseWeapon)item).MaxRange = 2;
                        from.SendMessage("You magically add +1 Range to your weapon.");

                        m_Deed.Delete(); // Delete the deed
                }
					else if (((BaseWeapon)item).MaxRange == 2)
                {
            
                        ((BaseWeapon)item).MaxRange = 3;
                        from.SendMessage("You magically add +1 Range to your weapon.");

                        m_Deed.Delete(); // Delete the deed
                    }
                }
            }
            else
				{
                    from.SendMessage("You cannot add range to that.");
				}
		}

        public class WeaponRangeDeed : Item // Create the item class which is derived from the base item class
        {
            [Constructable]
            public WeaponRangeDeed() : base(0x14F0)
            {
                Weight = 1.0;
                Name = "Weapon Range increase +1";
                LootType = LootType.Blessed;
                Hue = 1156;
            }

            public WeaponRangeDeed(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                LootType = LootType.Blessed;

                int version = reader.ReadInt();
            }

            public override bool DisplayLootType { get { return false; } }

            public override void OnDoubleClick(Mobile from) // Override double click of the deed to call our target
            {
                if (!IsChildOf(from.Backpack)) // Make sure its in their pack
                {
                    from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                }
                else
                {
                    from.SendMessage("What Weapon would you like to add +1 Range to?");
                    from.Target = new WeaponRangeDeedTarget(this); // Call our target
                }
            }
        }
    }
}


