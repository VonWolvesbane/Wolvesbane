// This Script was Scripted by Espcevan
using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server;

namespace Server.Items
{
    public class LuckyHorseshoe : Item
{
    [Constructable]
    public LuckyHorseshoe() : base(0xFB6)
    {
        Name = "Lucky Horseshoes";
        Hue = 0x8A5;
    }

        public LuckyHorseshoe(Serial serial) : base(serial) { }


    public class LuckTarget : Target
    {
        private LuckyHorseshoe m_Deed;

        public LuckTarget(LuckyHorseshoe deed) : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (target is BaseWeapon || target is BaseShield || target is BaseJewel || target is BaseArmor)
            {
                int augment = ((Utility.Random(2)) /* scalar*/ ) + 25;
                int augmentper = ((Utility.Random(4)) /* scalar*/ ) + 50;

                Item item = (Item)target;
                if (item is BaseWeapon)
                {
                    if (((BaseWeapon)item).Attributes.Luck == 1000) from.SendMessage("That already has luck!");
                    else
                    {
                        if (item.RootParent != from) from.SendMessage("You can not put luck on that there!");
                        else
                        {
                            ((BaseWeapon)item).Attributes.Luck += augmentper;
                            from.SendMessage("You magically add luck to your item....");
                            m_Deed.Delete();
                        }
                    }
                }
                if (item is BaseShield)
                {
                    if (((BaseShield)item).Attributes.Luck == 1000) from.SendMessage("That already has luck!");
                    else
                    {
                        if (item.RootParent != from) from.SendMessage("You cannot put luck on that there!");
                        else
                        {
                            ((BaseShield)item).Attributes.Luck += augmentper;
                            from.SendMessage("You magically add luck to your item....");
                            m_Deed.Delete();
                        }
                    }
                }
                if (item is BaseArmor)
                {
                    if (((BaseArmor)item).Attributes.Luck == 1000) from.SendMessage("That already has luck!");
                    else
                    {
                        if (item.RootParent != from) from.SendMessage("You cannot put luck on that there!");
                        else
                        {
                            ((BaseArmor)item).Attributes.Luck += augmentper;
                            from.SendMessage("You magically add luck to your item....");
                            m_Deed.Delete();
                        }
                    }
                }
                if (item is BaseJewel)
                {
                    if (((BaseJewel)item).Attributes.Luck == 1000) from.SendMessage("That already has luck!");
                    else
                    {
                        if (item.RootParent != from) from.SendMessage("You cannot put luck on that there!");
                        else
                        {
                            ((BaseJewel)item).Attributes.Luck += augmentper;
                            from.SendMessage("You magically add luck to your item....");
                            m_Deed.Delete();
                        }
                    }
                }
            }
            else from.SendMessage("You can not put luck on that");
        }
    }

   

    
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write((int)0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }

        public override bool DisplayLootType { get { return false; } }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack)) from.SendLocalizedMessage(1042001);
            else
            {
                from.SendMessage("What item would you like to add luck to?");
                from.Target = new LuckTarget(this);
            }
        }
    }
}
