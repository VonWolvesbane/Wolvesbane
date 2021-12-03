/*Script Modded by: Leonel Strouse A.K.A. AlphaDragon
 * 11/09/2019 18:00 HRS
 * Version 0.002
 */
using Server;
using Server.Gumps;
using System.Linq;
using Server.Multis;
using Server.Network;
using Server.Regions;
using Server.Targeting;
using System;

namespace Server.Items
{
    public enum DecorateCommand
    {
        None,
        Secure,
        Lockdown,
        Release,
        Turn,
        Up,
        Down,
        North,
        East,
        South,
        West,
        GetHue,
        Close
    }

    public class InteriorDecorator : Item
    {
//        public override int LabelNumber { get { return 1041280; } } // an interior decorator

        private DecorateCommand m_Command;

        [Constructable]
        public InteriorDecorator()
            : base(0xFC1)
        {
            Name = " An Advance Interior Decorator";
            Weight = 1.0;
            LootType = LootType.Regular;
        }

        public InteriorDecorator(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DecorateCommand Command
        {
            get { return m_Command; }
            set
            {
                m_Command = value;
                InvalidateProperties();
            }
        }

        public static bool InHouse(Mobile from)
        {
            //			BaseHouse house = BaseHouse.FindHouseAt( from );
            Region reg = Region.Find(from.Location, from.Map);
            BaseHouse house = null;
            if (reg is HouseRegion)
            {
                HouseRegion hreg = reg as HouseRegion;
                house = hreg.House;

            }
            return (house != null && house.IsCoOwner(from));
        }

        public static bool CheckUse(InteriorDecorator tool, Mobile from)
        {
            if (!InHouse(from))
                from.SendLocalizedMessage(502092); // You must be in your house to do this.
            else
                return true;

            return false;
        }

        //public override void GetProperties(ObjectPropertyList list)
        //{
        //    base.GetProperties(list);

        //    if (m_Command != DecorateCommand.None)
        //        list.Add(1018322 + (int)m_Command); // Turn/Up/Down
        //}

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

        public override void OnDoubleClick(Mobile from)
        {
            //if (!CheckUse(this, from))
            //    return;

            if (!InHouse(from))
                m_Command = DecorateCommand.GetHue;

            if (from.FindGump(typeof(InternalGump)) == null)
                from.SendGump(new InternalGump(from, this));

            if (m_Command != DecorateCommand.None)
                from.Target = new InternalTarget(this);
        }

        private class InternalGump : Gump
        {
            private readonly InteriorDecorator m_Decorator;

            public InternalGump(Mobile from, InteriorDecorator decorator)
                : base(150, 50)
            {
                m_Decorator = decorator;


                AddBackground(0, 0, 170, 559 , 2600);

                AddPage(0);

                AddButton(40, 45, (decorator.Command == DecorateCommand.Secure ? 2154 : 2152), 2154, 1, GumpButtonType.Reply, 0);
                AddLabel(80, 50, 0, @"Secure"); //secure

                AddButton(40, 85, (decorator.Command == DecorateCommand.Lockdown ? 2154 : 2152), 2154, 2, GumpButtonType.Reply, 0);
                AddLabel(80, 90, 0, @"Lockdown"); // lockdown

                AddButton(40, 125, (decorator.Command == DecorateCommand.Release ? 2154 : 2152), 2154, 3, GumpButtonType.Reply, 0);
                AddLabel(80, 130, 0, @"Release"); // release

                AddButton(40, 165, (decorator.Command == DecorateCommand.Turn ? 2154 : 2152), 2154, 4, GumpButtonType.Reply, 0);
                AddHtmlLocalized(80, 170, 70, 40, 1018323, false, false); // Turn

                AddButton(40, 205, (decorator.Command == DecorateCommand.Up ? 2154 : 2152), 2154, 5, GumpButtonType.Reply, 0);
                AddHtmlLocalized(80, 210, 70, 40, 1018324, false, false); // Up

                AddButton(40, 245, (decorator.Command == DecorateCommand.Down ? 2154 : 2152), 2154, 6, GumpButtonType.Reply, 0);
                AddHtmlLocalized(80, 250, 70, 40, 1018325, false, false); // Down

                AddButton(40, 285, (decorator.Command == DecorateCommand.North ? 2154 : 2152), 2154, 7, GumpButtonType.Reply, 0);
                AddHtmlLocalized(80, 290, 70, 40, 1075389, false, false); // north

                AddButton(40, 325, (decorator.Command == DecorateCommand.East ? 2154 : 2152), 2154, 8, GumpButtonType.Reply, 0);
                AddHtmlLocalized(80, 330, 70, 40, 1075387, false, false); // east

                AddButton(40, 365, (decorator.Command == DecorateCommand.South ? 2154 : 2152), 2154, 9, GumpButtonType.Reply, 0);
                AddHtmlLocalized(80, 370, 70, 40, 1075386, false, false); // south

                AddButton(40, 405, (decorator.Command == DecorateCommand.West ? 2154 : 2152), 2154, 10, GumpButtonType.Reply, 0);
                AddHtmlLocalized(80, 410, 70, 40, 1075390, false, false); // west

                AddButton(40, 445, (decorator.Command == DecorateCommand.GetHue ? 2154 : 2152), 2154, 11, GumpButtonType.Reply, 0);
                AddHtmlLocalized(80, 450, 100, 20, 1158863, false, false); // Get Hue

                AddButton(40, 485, (decorator.Command == DecorateCommand.Close ? 2154 : 2152), 2154, 13, GumpButtonType.Reply, 0);
                AddLabel(80, 490, 0, @"Close"); // close

                AddHtmlLocalized(0, 0, 0, 0, 4, false, false);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                DecorateCommand command = DecorateCommand.None;
                Mobile m = sender.Mobile;

                int cliloc = 0;
                string c_String = null;

                switch (info.ButtonID)
                {
                    case 1://secure
                        c_String = "Select an object to secure."; // Select an object to secure.
                        command = DecorateCommand.Secure;
                        break;
                    case 2://lockdown
                        c_String = "Select an object to lock down."; // Select an object to lock down.
                        command = DecorateCommand.Lockdown;
                        break;
                    case 3://release
                        c_String = "Select an object to release."; // Select an object to release.
                        command = DecorateCommand.Release;
                        break;
                    case 4://turn
                        cliloc = 1073404; // Select an object to turn.
                        command = DecorateCommand.Turn;
                        break;
                    case 5://up
                        cliloc = 1073405; // Select an object to increase its height.
                        command = DecorateCommand.Up;
                        break;
                    case 6://down
                        cliloc = 1073406; // Select an object to lower its height.
                        command = DecorateCommand.Down;
                        break;
                    case 7://north
                        c_String = "Select an object to move north."; // Select an object to move north.
                        command = DecorateCommand.North;
                        break;
                    case 8://east
                        c_String = "Select an object to move east."; // Select an object to move east.
                        command = DecorateCommand.East;
                        break;
                    case 9://south
                        c_String = "Select an object to move south."; // Select an object to move south.
                        command = DecorateCommand.South;
                        break;
                    case 10://west
                        c_String = "Select an object to move west."; // Select an object to move west.
                        command = DecorateCommand.West;
                        break;
                    case 11://get hue
                        cliloc = 1158864; // Select an object to get the hue.
                        command = DecorateCommand.GetHue;
                        break;
                    case 12://Close
                        c_String = "Close"; // Close
                        command = DecorateCommand.Close;
                        break;
                }

                if (command != DecorateCommand.None & command != DecorateCommand.Close)
                {
                    m_Decorator.Command = command;
                    m.SendGump(new InternalGump(m, m_Decorator));

                    if (cliloc != 0)
                        m.SendLocalizedMessage(cliloc);
                    if (c_String != null)
                        m.SendMessage(c_String);

                    m.Target = new InternalTarget(m_Decorator);
                }
                else
                {
                    Target.Cancel(m);
                }
            }
        }

        private class InternalTarget : Target
        {
            private readonly InteriorDecorator m_Decorator;

            public InternalTarget(InteriorDecorator decorator)
                : base(-1, false, TargetFlags.None)
            {
                CheckLOS = false;

                m_Decorator = decorator;
            }

            protected override void OnTargetNotAccessible(Mobile from, object targeted)
            {
                OnTarget(from, targeted);
            }

            private static Type[] m_KingsCollectionTypes = new Type[]
            {
                //typeof(BirdLamp),    typeof(DragonLantern),
               // typeof(KoiLamp),   typeof(TallLamp)
            };

            private static bool IsKingsCollection(Item item)
            {
                return m_KingsCollectionTypes.Any(t => t == item.GetType());
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Decorator.Command == DecorateCommand.GetHue)
                {
                    int hue = 0;

                    if (targeted is Item)
                        hue = ((Item)targeted).Hue;
                    else if (targeted is Mobile)
                        hue = ((Mobile)targeted).Hue;
                    else
                    {
                        from.Target = new InternalTarget(m_Decorator);
                        return;
                    }

                    from.SendLocalizedMessage(1158862, String.Format("{0}", hue)); // That object is hue ~1_HUE~
                }
                else if (targeted is Item && CheckUse(m_Decorator, from))
                {
                    //					BaseHouse house = BaseHouse.FindHouseAt( from );
                    Region reg = Region.Find(from.Location, from.Map);
                    BaseHouse house = null;
                    if (reg is HouseRegion)
                    {
                        HouseRegion hreg = reg as HouseRegion;
                        house = hreg.House;
                    }

                    Item item = (Item)targeted;

                    bool isDecorableComponent = false;

                    if (m_Decorator.Command == DecorateCommand.Turn && IsKingsCollection(item))
                    {
                        isDecorableComponent = true;
                    }
                    else if (item is AddonComponent || item is AddonContainerComponent || item is BaseAddonContainer)
                    {
                        object addon = null;
                        int count = 0;

                        if (item is AddonComponent)
                        {
                            AddonComponent component = (AddonComponent)item;
                            count = component.Addon.Components.Count;
                            addon = component.Addon;
                        }
                        else if (item is AddonContainerComponent)
                        {
                            AddonContainerComponent component = (AddonContainerComponent)item;
                            count = component.Addon.Components.Count;
                            addon = component.Addon;
                        }
                        else if (item is BaseAddonContainer)
                        {
                            BaseAddonContainer container = (BaseAddonContainer)item;
                            count = container.Components.Count;
                            addon = container;
                        }

                        if (count == 1 && Core.SE)
                            isDecorableComponent = true;

                        if (item is EnormousVenusFlytrapAddon)
                            isDecorableComponent = true;

                        if (m_Decorator.Command == DecorateCommand.Turn)
                        {
                            FlipableAddonAttribute[] attributes = (FlipableAddonAttribute[])addon.GetType().GetCustomAttributes(typeof(FlipableAddonAttribute), false);

                            if (attributes.Length > 0)
                                isDecorableComponent = true;
                        }
                    }
                    else if (item is Banner && m_Decorator.Command != DecorateCommand.Turn)
                    {
                        isDecorableComponent = true;
                    }

                    if (house == null || /*!house.IsCoOwner(from)*/ !house.IsFriend(from))
                    {
                        from.SendLocalizedMessage(502092); // You must be in your house to do
                    }
                    //					else if ( item.Parent != null || !house.IsInside( item ) )
                    else if (item.Parent != null || !house.IsHouseRegion(item))
                    {
                        from.SendLocalizedMessage(1042270); // That is not in your house.
                    }
                    else if (!house.IsLockedDown(item) && !house.IsSecure(item) && !isDecorableComponent)
                    {
                        if (item is AddonComponent && m_Decorator.Command == DecorateCommand.Turn)
                            from.SendLocalizedMessage(1042273); // You cannot turn that.
                        else if (item is AddonComponent && m_Decorator.Command == DecorateCommand.Up)
                            from.SendLocalizedMessage(1042274); // You cannot raise it up any higher.
                        else if (item is AddonComponent && m_Decorator.Command == DecorateCommand.Down)
                            from.SendLocalizedMessage(1042275); // You cannot lower it down any further.
                        else if (m_Decorator.Command == DecorateCommand.Secure)
                            Secure(item, from);
                        else if (m_Decorator.Command == DecorateCommand.Lockdown)
                            Lockdown(item, from);
                        else
                            from.SendMessage("That is not locked down or secured.");
                    }
                    else if (item is VendorRentalContract)
                    {
                        from.SendLocalizedMessage(1062491); // You cannot use the house decorator on that object.
                    }
                    /*else if (item.TotalWeight + item.PileWeight > 100)
                    {
                        from.SendLocalizedMessage(1042272); // That is too heavy.
                    }*/
                    else
                    {
                        switch (m_Decorator.Command)
                        {
                            case DecorateCommand.None:
                                None(item, from);
                                break;
                            case DecorateCommand.Secure:
                                Secure(item, from);
                                break;
                            case DecorateCommand.Lockdown:
                                Lockdown(item, from);
                                break;
                            case DecorateCommand.Release:
                                Release(item, from);
                                break;
                            case DecorateCommand.Turn:
                                Turn(item, from);
                                break;
                            case DecorateCommand.Up:
                                Up(item, from);
                                break;
                            case DecorateCommand.Down:
                                Down(item, from);
                                break;
                            case DecorateCommand.North:
                                North(item, from);
                                break;
                            case DecorateCommand.East:
                                East(item, from);
                                break;
                            case DecorateCommand.South:
                                South(item, from);
                                break;
                            case DecorateCommand.West:
                                West(item, from);
                                break;
                            case DecorateCommand.GetHue:
                                GetHue(item, from);
                                break;
                            case DecorateCommand.Close:
                                Close(item, from);
                                break;
                        }
                    }
                }

                from.Target = new InternalTarget(m_Decorator);
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if (cancelType == TargetCancelType.Canceled)
                    from.CloseGump(typeof(InteriorDecorator.InternalGump));
            }

            private static void None(Item item, Mobile from)
            {
            }

            private static void Secure(Item item, Mobile from)
            {
                BaseHouse house = BaseHouse.FindHouseAt(from);
                if (house.IsLockedDown(item))
                    from.SendMessage("That is already locked down.");
                else if (house.IsSecure(item))
                    from.SendMessage("That is already secured.");
                else if (house.IsFriend(from)&&!house.IsCoOwner(from))
                { from.SendMessage("Only Owners and CoOwners are allowed to secure things in a house.");
                    return;
                }
                else
                    house.AddSecure(from, item);
            }

            private static void Lockdown(Item item, Mobile from)
            {
                BaseHouse house = BaseHouse.FindHouseAt(from);
                if (house.IsLockedDown(item))
                    from.SendMessage("That is already locked down.");
                else if (house.IsSecure(item))
                    from.SendMessage("That is already secured.");
                else
                    house.LockDown(from, item, true);
            }

            private static void Release(Item item, Mobile from)
            {
                BaseHouse house = BaseHouse.FindHouseAt(from);
                if (!house.IsLockedDown(item) && !house.IsSecure(item) && (item.Movable))
                    from.SendMessage("That is not locked down or secured.");
                else
                    house.Release(from, item);
            }

            private static void Turn(Item item, Mobile from)
            {
                    if (item is IFlipable)
                    {
                        ((IFlipable)item).OnFlip(from);
                        return;
                    }

                    if (item is AddonComponent || item is AddonContainerComponent || item is BaseAddonContainer)
                    {
                        object addon = null;

                        if (item is AddonComponent)
                            addon = ((AddonComponent)item).Addon;
                        else if (item is AddonContainerComponent)
                            addon = ((AddonContainerComponent)item).Addon;
                        else if (item is BaseAddonContainer)
                            addon = (BaseAddonContainer)item;

                        FlipableAddonAttribute[] aAttributes = (FlipableAddonAttribute[])addon.GetType().GetCustomAttributes(typeof(FlipableAddonAttribute), false);

                        if (aAttributes.Length > 0)
                        {
                            aAttributes[0].Flip(from, (Item)addon);
                            return;
                        }
                    }

                    FlipableAttribute[] attributes = (FlipableAttribute[])item.GetType().GetCustomAttributes(typeof(FlipableAttribute), false);

                    if (attributes.Length > 0)
                        attributes[0].Flip(item);
                    else
                        from.SendLocalizedMessage(1042273); // You cannot turn that.                
            }

            private static void Up(Item item, Mobile from)
            {
                int floorZ = GetFloorZ(item);

                if (floorZ > int.MinValue && item.Z < (floorZ + 14)) // Confirmed : no height checks here
                    item.Location = new Point3D(item.Location, item.Z + 1);
                else
                    from.SendLocalizedMessage(1042274); // You cannot raise it up any higher.
            }

            private static void Down(Item item, Mobile from)
            {
                int floorZ = GetFloorZ(item);

                if (floorZ > int.MinValue && item.Z > GetFloorZ(item))
                    item.Location = new Point3D(item.Location, item.Z - 1);
                else
                    from.SendLocalizedMessage(1042275); // You cannot lower it down any further.
            }

            private static void North(Item item, Mobile from)
            {
                BaseHouse house = BaseHouse.FindHouseAt(item);

                Point3D ourLoc = item.GetWorldLocation();
                Point3D goingLoc = new Point3D(ourLoc.X, ourLoc.Y -2, ourLoc.Z);

                if (house.IsInside(goingLoc, ourLoc.Z))
                    item.Y = (item.Y -1);
                else
                    from.SendMessage("You cannot move it to the north any further.");

            }

            private static void East(Item item, Mobile from)
            {
                BaseHouse house = BaseHouse.FindHouseAt(item);

                Point3D ourLoc = item.GetWorldLocation();
                Point3D goingLoc = new Point3D(ourLoc.X +1, ourLoc.Y, ourLoc.Z);

                if (house.IsInside(goingLoc, ourLoc.Z))
                    item.X = (item.X +1);
                else
                    from.SendMessage("You cannot move it to the east any further.");

            }

            private static void South(Item item, Mobile from)
            {
                BaseHouse house = BaseHouse.FindHouseAt(item);

                Point3D ourLoc = item.GetWorldLocation();
                Point3D goingLoc = new Point3D(ourLoc.X, ourLoc.Y +1, ourLoc.Z);

                if (house.IsInside(goingLoc, ourLoc.Z))
                     item.Y = (item.Y +1); 
                else
                    from.SendMessage("You cannot move it to the south any further.");
            }

            private static void West(Item item, Mobile from)
            {
                BaseHouse house = BaseHouse.FindHouseAt(item);

                Point3D ourLoc = item.GetWorldLocation();
                Point3D goingLoc = new Point3D(ourLoc.X -2, ourLoc.Y, ourLoc.Z);

                if (house.IsInside(goingLoc, ourLoc.Z))
                    item.X = (item.X -1);
                else
                    from.SendMessage("You cannot move it to the west any further.");
            }

            private static void GetHue(Item item, Mobile from)
            {
            }

            private static void Close(Item item, Mobile from)
            {
                from.CloseGump(typeof(InteriorDecorator.InternalGump));
                Target.Cancel(from);
            }
            private static void Command(Item item, Mobile from)
            {
            }
            private static int GetFloorZ(Item item)
            {
                Map map = item.Map;

                if (map == null)
                    return int.MinValue;

                StaticTile[] tiles = map.Tiles.GetStaticTiles(item.X, item.Y, true);

                int z = int.MinValue;

                for (int i = 0; i < tiles.Length; ++i)
                {
                    StaticTile tile = tiles[i];
                    ItemData id = TileData.ItemTable[tile.ID & 0x3FFF];

                    int top = tile.Z; // Confirmed : no height checks here

                    if (id.Surface && !id.Impassable && top > z && top <= item.Z)
                        z = top;
                }

                if (z == int.MinValue)
                    z = map.Tiles.GetLandTile(item.X, item.Y).Z;

                return z;
            }
        }
    }
}
