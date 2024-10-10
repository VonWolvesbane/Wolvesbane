using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class WDTRANSFERSTONE : Item
    {
        [Constructable]
        public WDTRANSFERSTONE() : base(0xEDE)
        {
            Name = "WD SCROLL TO WD COINS";
            Movable = false;
            Hue = 1153;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(this.GetWorldLocation(), 3))
            {
                from.SendMessage("Target the item to transform.");
                from.Target = new InternalTarget();
            }
            else
            {
                from.SendMessage("You are too far away to use the Transformation Stone.");
            }
        }

        private class InternalTarget : Target
        {
            public InternalTarget() : base(10, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is WolvesbaneDollar item && item.IsChildOf(from.Backpack))
                {
                    int stackAmount = item.Amount;
                    item.Delete();

                    WDollar newItem = new WDollar
                    {
                        Amount = stackAmount
                    };

                    from.Backpack.DropItem(newItem);

                    from.SendMessage("The items have been transformed into a single stack!");
                }
                else
                {
                    from.SendMessage("You can only transform Wolvesbane Dollar items from your backpack.");
                }
            }
        }

        public WDTRANSFERSTONE(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
