/////////////////////////////////////////////////
// Air Conditioner Addon by: Rockstar         //
// Addon Generated With AddonGen              //
///////////////////////////////////////////////

using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AirUnit : BaseAddon
    {
        public override BaseAddonDeed Deed
        {
            get
            {
                return new AirUnitDeed();
            }
        }

        [Constructable]
        public AirUnit()
        {
            AddComponent(new AddonComponent(7978), 0, 1, 0);
            AddComponent(new AddonComponent(7978), 0, -1, 0);
            AddComponent(new AddonComponent(2716), 0, 0, 0);
            AddonComponent ac;
            ac = new AddonComponent(7978);
            ac.Hue = 1153;
            ac.Name = "Air Conditioner Unit";
            AddComponent(ac, 0, 1, 0);
            ac = new AddonComponent(7978);
            ac.Hue = 1153;
            ac.Name = "Air Conditioner Unit";
            AddComponent(ac, 0, -1, 0);
            ac = new AddonComponent(2716);
            ac.Hue = 1153;
            ac.Name = "Air Conditioner Unit";
            AddComponent(ac, 0, 0, 0);

        }

        public AirUnit(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // Version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
     
        public override void OnComponentUsed(AddonComponent ac, Mobile from)
        {
            if (!from.InRange(GetWorldLocation(), 4))
                from.SendMessage("You are to far away to use that");
            else
            {
                if (ac.ItemID == 2716)
                {
                    from.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Waist);
                    from.PlaySound(20);
                    from.Hits += 30;

                    from.SendMessage("Here. Let me Cool you down. You must be tired.");
                    from.Say("Ahh! The air is Soothing Enough to Heal my Wounds");
 
                }
            }
        }
        



        public class AirUnitDeed : BaseAddonDeed
        {
            public override BaseAddon Addon
            {
                get
                {
                    return new AirUnit();
                }
            }

            [Constructable]
            public AirUnitDeed()
            {
                Name = "AG_AirUnit";
            }

            public AirUnitDeed(Serial serial)
                : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write(0); // Version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }
    }
}