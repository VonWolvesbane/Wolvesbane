//ReApEr
using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Gumps;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
 public class EasterEgg1 : Item
    {
		[Constructable]
        public EasterEgg1() : this(1) { }
		
        [Constructable]
        public EasterEgg1(int amount)
            : base(0x9B5)
        {
			Name = "Easter Egg";
            this.Stackable = true;
            this.Amount = amount;

            this.Weight = 1.0;
            Hue = 3 + (Utility.Random(20) * 5);
        }
		public override void GetProperties(ObjectPropertyList list)
			{
			base.GetProperties(list);
	
			list.Add("Eat Me");
			}
        public EasterEgg1(Serial serial)
            : base(serial)
        {
        }
        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042038); // You must have the object in your backpack to use it.
            }
            else if (from.GetStatMod("EasterEgg") != null)
            {
                from.SendLocalizedMessage(1062927); // You have eaten one of these recently and eating another would provide no benefit.
            }
            else
            {

                from.AddStatMod(new StatMod(StatType.Str, "EasterEgg", 200, TimeSpan.FromMinutes(15.0))); // Str Buff
				from.AddStatMod(new StatMod(StatType.Int, "EasterEgg1", 200, TimeSpan.FromMinutes(15.0))); // Int Buff
				from.AddStatMod(new StatMod(StatType.Dex, "EasterEgg2", 200, TimeSpan.FromMinutes(15.0))); // Dex Buff
				
				from.PlaySound(0x1EE);
                this.Consume();
            }
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
	}
}