using System;
using Server.Gumps;
using Server.Mobiles; 
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
    public class EtherealMountID : Item
    {
        [Constructable]
        public EtherealMountID()
            : base(0x367A)
        {
			Name = "Chimera SkillMount ID Change Deed";
            LootType = LootType.Blessed;
			Hue = 1156;
            Weight = 1.0;
        }

        public EtherealMountID(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this.IsChildOf(from.Backpack))
            {
        			this.SendLocalizedMessageTo(from, 1010086); 
					from.Target = new MountTarget(this);
			} 
			else 
			{ 
				from.SendLocalizedMessage( 500446 ); // That is too far away. 
			}
      	} 

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }

		private class MountTarget : Target
		{
			private EtherealMountID m_Item;

			public MountTarget(EtherealMountID item) : base(1, false, TargetFlags.None)
			{
				m_Item = item;
			}

			protected override void OnTarget(Mobile from, object target)
			{
				if (target == from)
				{
					from.SendMessage("Why would you target yourself?");
				}
				else if (target is SkillMountChimera && m_Item.IsChildOf(from.Backpack))
				{
					SkillMountChimera mount = (SkillMountChimera)target;
					from.SendGump(new MountIDGump(mount, from));
					m_Item.Delete();
				}
				else
				{
					from.SendMessage("You can't do that.");
				}
			}
		}
    }
}