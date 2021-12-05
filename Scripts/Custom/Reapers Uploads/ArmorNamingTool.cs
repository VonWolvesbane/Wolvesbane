using System;
using Server.Gumps;
using Server.Multis;
using Server.Targeting;
using Server.Mobiles;
using Server.Network;

using Server.Engines.VeteranRewards;

namespace Server.Items
{

    public class BaseNamingTool : Item, IUsesRemaining, IRewardItem
    {
        private int m_UsesRemaining;
        private bool m_IsRewardItem;

        [CommandProperty(AccessLevel.GameMaster)]
        public int UsesRemaining
        {
            get { return m_UsesRemaining; }
            set
            {
                m_UsesRemaining = value;
                InvalidateProperties();
            }
        }

        public virtual bool ShowUsesRemaining
        {
            get { return false; }
            set { }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsRewardItem
        {
            get
            {
                return m_IsRewardItem;
            }
            set
            {
                m_IsRewardItem = value;
                InvalidateProperties();
            }
        }

        [Constructable]
        public BaseNamingTool(int itemID)
            : this(itemID, 1)
        {
        }

        [Constructable]
        public BaseNamingTool(int itemID, int uses)
            : base(itemID)
        {
            Weight = 1.0;
            Hue = 0x48D;

            LootType = LootType.Blessed;

            m_UsesRemaining = uses;
        }

        public BaseNamingTool(Serial serial)
            : base(serial)
        {
        }

        public virtual bool DeletedItem { get { return true; } }
        public virtual int LowSkillMessage { get { return 0; } }
        public virtual int VeteranRewardCliloc { get { return 0; } }

        public virtual Type[] Engraves { get { return null; } }

        public virtual int SuccessMessage { get { return 1072361; } } // You engraved the object.
        public virtual int TargetMessage { get { return 1072357; } } // Select an object to engrave.
        public virtual int RemoveMessage { get { return 1072362; } } // You remove the engraving from the object.
        public virtual int ReChargesMessage { get { return 1076166; } } // You do not have a blue diamond needed to recharge the engraving tool.
        public virtual int OutOfChargesMessage { get { return 1076163; } } // There are no charges left on this engraving tool.
        public virtual int NotAccessibleMessage { get { return 1072310; } } // The selected item is not accessible to engrave.
        public virtual int CannotEngraveMessage { get { return 1072309; } } // The selected item cannot be engraved by this engraving tool.
        public virtual int ObjectWasNotMessage { get { return 1072363; } } // The object was not engraved.        

        public virtual bool CheckType(IEntity entity)
        {
            if (Engraves == null || entity == null)
                return false;

            Type type = entity.GetType();

            for (int i = 0; i < Engraves.Length; i++)
            {
                if (type == Engraves[i] || type.IsSubclassOf(Engraves[i]))
                    return true;
            }

            return false;
        }

        public static BaseNamingTool Find(Mobile from)
        {
            if (from.Backpack != null)
            {
                BaseNamingTool tool = from.Backpack.FindItemByType(typeof(BaseNamingTool)) as BaseNamingTool;

                if (tool != null && !tool.DeletedItem && tool.UsesRemaining <= 0)
                    return tool;
                else
                    return null;
            }

            return null;
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);

            if (!from.NetState.SupportsExpansion(Expansion.ML))
            {
                from.SendLocalizedMessage(1072791); // You must upgrade to Mondain's Legacy in order to use that item.				
                return;
            }

            if (m_UsesRemaining > 0)
            {
                from.SendMessage("What would you like to rename?");
                from.Target = new InternalTarget(this);
            }
            else
            {
                if (!DeletedItem)
                {
                    if (CheckSkill(from))
                    {
                        Item diamond = from.Backpack.FindItemByType(typeof(StaffRing));

                        if (diamond != null)
                        {
                            from.SendGump(new ConfirmGump(this, null));
                        }
                        else
                        {
                            from.SendLocalizedMessage(ReChargesMessage);
                        }
                    }
                }

                from.SendMessage("The tool Vanishes");
            }
        }

        private bool IsValid(IEntity entity, Mobile m)
        {
            if (entity is Item)
            {
                Item item = entity as Item;

                BaseHouse house = BaseHouse.FindHouseAt(item);

                if (m.InRange(item.GetWorldLocation(), 3))
                {
                    if (item.Movable && !item.IsLockedDown && !item.IsSecure && (item.RootParent == null || item.RootParent == m))
                    {
                        return true;
                    }
                    else if (house != null && house.IsFriend(m))
                    {
                        return true;
                    }
                }
            }
            else if (entity is BaseCreature)
            {
                BaseCreature bc = entity as BaseCreature;

                if (bc.Controlled && bc.ControlMaster == m)
                    return true;
            }

            return false;
        }

        public bool CheckSkill(Mobile from)
        {
            if (from.Skills[SkillName.Tinkering].Value < 100.0)
            {
                from.SendLocalizedMessage(LowSkillMessage);
                return false;
            }

            return true;
        }

        public virtual void Recharge(Mobile from, Mobile guildmaster)
        {
            if (from.Backpack != null)
            {
                Item diamond = from.Backpack.FindItemByType(typeof(StaffRing));

                if (guildmaster != null)
                {
                    if (m_UsesRemaining <= 0)
                    {
                        if (diamond != null && Banker.Withdraw(from, 100000))
                        {
                            diamond.Consume();
                            UsesRemaining = 10;
                            guildmaster.Say(1076165); // Your weapon engraver should be good as new!
                        }
                        else
                            guildmaster.Say(1076167); // You need a 100,000 gold and a blue diamond to recharge the weapon engraver.
                    }
                    else
                        guildmaster.Say(1076164); // I can only help with this if you are carrying an engraving tool that needs repair.
                }
                else
                {
                    if (CheckSkill(from))
                    {
                        if (diamond != null)
                        {
                            diamond.Consume();

                            if (Utility.RandomDouble() < from.Skills[SkillName.Tinkering].Value / 100)
                            {
                                UsesRemaining = 10;
                                from.SendLocalizedMessage(1076165); // Your engraver should be good as new!
                            }
                            else
                                from.SendLocalizedMessage(1076175); // You cracked the diamond attempting to fix the engraver.
                        }
                        else
                            from.SendLocalizedMessage(1076166); // You do not have a blue diamond needed to recharge the engraving tool.
                    }
                }
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (m_IsRewardItem)
                list.Add(VeteranRewardCliloc);	
        }

        /*public override void AddUsesRemainingProperties(ObjectPropertyList list)
        {
            if (ShowUsesRemaining)
            {
                list.Add(1060584, m_UsesRemaining.ToString()); // uses remaining: ~1_val~
            }
        } */  

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)2); // version

            writer.Write((int)m_UsesRemaining);
            writer.Write((bool)m_IsRewardItem);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        m_UsesRemaining = reader.ReadInt();
                        m_IsRewardItem = reader.ReadBool();
                        break;
                    }
                case 1:
                    {
                        m_UsesRemaining = reader.ReadInt();
                        break;
                    }
                case 0:
                    {
                        if (this is WeaponNamingTool)
                        {
                            InheritsItem = true;
                            m_UsesRemaining = reader.ReadInt();
                            m_IsRewardItem = reader.ReadBool();
                        }
                        else
                        {
                            LootType = LootType.Blessed;
                        }
                        break;
                    }
            }
        }

        #region Old Item Serialization Vars
        /* DO NOT USE! Only used in serialization of Weapon Naming Tool that originally derived from Item */
        public bool InheritsItem { get; protected set; }
        #endregion

        private class InternalTarget : Target
        {
            private readonly BaseNamingTool m_Tool;

            public InternalTarget(BaseNamingTool tool)
                : base(3, true, TargetFlags.None)
            {
                m_Tool = tool;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Tool == null || m_Tool.Deleted)
                    return;

                if (targeted is IEntity)
                {
                    IEntity entity = (IEntity)targeted;

                    if (m_Tool.IsValid(entity, from))
                    {
                        if (entity is IEngravable && m_Tool.CheckType(entity))
                        {
                            from.CloseGump(typeof(InternalGump));
                            from.SendGump(new InternalGump(m_Tool, entity));
                        }
                        else
                    from.SendMessage("You cannot rename that with this Tool.");
                    }
                    else
                    from.SendMessage("You cannot rename that with this Tool.");
                }
                else
                    from.SendMessage("You cannot rename that with this Tool.");
            }

            protected override void OnTargetOutOfRange(Mobile from, object targeted)
            {
                from.SendLocalizedMessage(m_Tool.NotAccessibleMessage);
            }            
        }

        public class ConfirmGump : Gump
        {
            private readonly BaseNamingTool Tool;
            private readonly Mobile m_NPC;

            public ConfirmGump(BaseNamingTool tool, Mobile npc)
                : base(200, 200)
            {
                Tool = tool;
                m_NPC = npc;

                AddPage(0);

                AddBackground(0, 0, 291, 133, 0x13BE);
                AddImageTiled(5, 5, 280, 100, 0xA40);

                if (npc != null)
                {
                    AddHtmlLocalized(9, 9, 272, 100, 1076169, 0x7FFF, false, false); // It will cost you 100,000 gold and a blue diamond to recharge your weapon engraver with 10 charges.
                    AddHtmlLocalized(195, 109, 120, 20, 1076172, 0x7FFF, false, false); // Recharge it
                }
                else
                {
                    AddHtmlLocalized(9, 9, 272, 100, 1076176, 0x7FFF, false, false); // You will need a blue diamond to repair the tip of the engraver.  A successful repair will give the engraver 10 charges.
                    AddHtmlLocalized(195, 109, 120, 20, 1076177, 0x7FFF, false, false); // Replace the tip.
                }

                AddButton(160, 107, 0xFB7, 0xFB8, 1, GumpButtonType.Reply, 0);
                AddButton(5, 107, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0);
                AddHtmlLocalized(40, 109, 100, 20, 1060051, 0x7FFF, false, false); // CANCEL
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                if (Tool == null || Tool.Deleted)
                    return;

                if (info.ButtonID == 1)
                    Tool.Recharge(state.Mobile, m_NPC);
            }
        }

        private class InternalGump : Gump
        {
            private readonly BaseNamingTool m_Tool;
            private readonly IEntity m_Target;

            public InternalGump(BaseNamingTool tool, IEntity target)
                : base(0, 0)
            {
                m_Tool = tool;
                m_Target = target;

                AddBackground(50, 50, 400, 300, 0xA28);

                AddPage(0);

                AddHtml(200, 70, 400, 20,"Renaming Tool", false, false);
                AddHtmlLocalized(75, 95, 350, 145, 1072360, 0x0, true, true);

                AddButton(125, 300, 0x81A, 0x81B, 1, GumpButtonType.Reply, 0);
                AddButton(320, 300, 0x819, 0x818, 0, GumpButtonType.Reply, 0);

                AddImageTiled(75, 245, 350, 40, 0xDB0);
                AddImageTiled(76, 245, 350, 2, 0x23C5);
                AddImageTiled(75, 245, 2, 40, 0x23C3);
                AddImageTiled(75, 285, 350, 2, 0x23C5);
                AddImageTiled(425, 245, 2, 42, 0x23C3);

                AddTextEntry(78, 246, 343, 37, 0x4FF, 15, "", 78);
            }

            public override void OnResponse(NetState state, RelayInfo info)
            {
                if (m_Tool == null || m_Tool.Deleted || m_Target == null || m_Target.Deleted)
                    return;

                Mobile from = state.Mobile;

                if (info.ButtonID == 1)
                {
                    if (!m_Tool.IsChildOf(from.Backpack))
                    {
                        from.SendLocalizedMessage(1062334); // This item must be in your backpack to be used.
                        return;
                    }
                    else if (!m_Tool.IsValid(m_Target, from))
                    {
                        from.SendMessage("Failed"); // The engraving failed.
                        return;
                    }
                    else
                    {
                        TextRelay relay = info.GetTextEntry(15);

                        Item item = (Item)m_Target;

                        if (relay != null)
                        {
                            if (relay.Text == null || relay.Text.Equals(""))
                            {
                                if (item.Name != null)
                                {
                                    item.Name = null;
                                    from.SendMessage("");
                                }
                                else
                                {
                                    from.SendMessage("you do not rename the item");
                                }
                            }
                            else
                            {
                                string text;

                                if (relay.Text.Length > 40)
                                    text = relay.Text.Substring(0, 40);
                                else
                                    text = relay.Text;

                                item.Name = text;

                                from.SendMessage("you Rename the Item");

                                m_Tool.UsesRemaining--;

                                if (m_Tool.UsesRemaining < 1)
                                {
                                    if (m_Tool.DeletedItem)
                                    {
                                        m_Tool.Delete();
                                        from.SendLocalizedMessage(1044038); // You have worn out your tool!
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public class SpellbookNamer : BaseNamingTool
    {

        [Constructable]
        public SpellbookNamer()
            : base(0xFBF, 1)
        {
			Name = "SpellBook renamer";
			Hue = 1153;
			ItemID = 4031;
        }

        public SpellbookNamer(Serial serial)
            : base(serial)
        {
        }

        public override Type[] Engraves
        {
            get
            {
                return new Type[]
                {
                    typeof(Spellbook)
                };
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
	
    public class ArmorNamingTool : BaseNamingTool
    {

        [Constructable]
        public ArmorNamingTool()
            : base(0x32F8, 1)
        {
			Name = "Armor Renamer";
            Hue = 1153;
			ItemID = 3997;
        }

        public ArmorNamingTool(Serial serial)
            : base(serial)
        {
        }

        public override Type[] Engraves 
		{ 
			get 
			{ 
				return new Type[] 
				{ 
				typeof(BaseArmor),
				typeof(BaseClothing) 				
				}; 
			} 
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
		public class WeaponNamingTool : BaseEngravingTool
    {

        public override bool DeletedItem { get { return false; } }
        public override int LowSkillMessage { get { return 1076178; } } // // Your tinkering skill is too low to fix this yourself.  An NPC tinkerer can help you repair this for a fee.
        public override int VeteranRewardCliloc { get { return 1076224; } } // 8th Year Veteran Reward

        [Constructable]
        public WeaponNamingTool()
            : base(0x32F8, 1)
        {
			Name = "Weapon Renamer";
            Hue = 1153;
			ItemID = 5091;
        }

        public WeaponNamingTool(Serial serial)
            : base(serial)
        {
        }

        public override Type[] Engraves
        {
            get
            {
                return new Type[]
                {
                    typeof(BaseWeapon)
                };
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = InheritsItem ? 0 : reader.ReadInt();
        }
    }
}

