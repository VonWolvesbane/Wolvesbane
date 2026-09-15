using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class WolvesbaneGreeter : BaseCreature
    {
        private static readonly string[] Tips =
        {
            "Welcome to Wolvesbane! Take your time, explore, and don't be afraid to ask questions.",
            "Keep an eye on world chat. Staff and other players are often happy to help new adventurers.",
            "Explore beyond the familiar roads. Wolvesbane has custom lands, creatures, treasures, and surprises.",
            "Treasure hunters, keep those maps! High-level maps can lead to unusual creatures and rare rewards.",
            "Tamers, train your companions well and remember to spend their ability points as they grow.",
            "Crafters can find plenty to do here. Blacksmithing, tailoring, carpentry, fletching and more can be rewarding.",
            "Bulk Order Deeds are worth exploring if you enjoy crafting and collecting special rewards.",
            "Keep an eye out for invasions and special events. Wolvesbane is always in need of heroes.",
            "If you find a system you don't understand, ask in world chat. Someone usually knows the answer!",
            "Some of Wolvesbane's best rewards are rare. Persistence can be every bit as important as luck.",
            "There is more to Wolvesbane than combat: try crafting, taming, treasure hunting, gardening, housing and events.",
            "Visit the Wolvesbane website and Discord for server news, updates, events and development.",
            "Most importantly, have fun. Wolvesbane is your world to explore, build in, and leave your mark upon."
        };

        private DateTime _NextTip;
        private readonly HashSet<Serial> _Welcomed = new HashSet<Serial>();

        // Characters with less than this much in-game time are treated as new.
        private static readonly TimeSpan NewPlayerTime = TimeSpan.FromHours(1.0);

        [Constructable]
        public WolvesbaneGreeter() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Elias";
            Title = "the Wolvesbane Greeter";
            Body = 0x190;
            Hue = Utility.RandomSkinHue();

            Blessed = true;
            CantWalk = true;

            InitStats(100, 100, 100);

            AddItem(new Shirt(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Shoes(Utility.RandomNeutralHue()));

            HairItemID = Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047, 0x2048, 0x2049);
            HairHue = Utility.RandomHairHue();

            if (Utility.RandomBool())
            {
                FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D);
                FacialHairHue = HairHue;
            }

            _NextTip = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        public WolvesbaneGreeter(Serial serial) : base(serial)
        {
        }

        public override bool IsInvulnerable { get { return true; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.UtcNow < _NextTip)
                return;

            _NextTip = DateTime.UtcNow + TimeSpan.FromMinutes(Utility.RandomMinMax(2, 4));

            if (Map == null || Map == Map.Internal)
                return;

            bool nearby = false;

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != null && m.Player && m.Alive && !m.Hidden)
                {
                    nearby = true;
                    break;
                }
            }

            if (nearby)
                Say(Tips[Utility.Random(Tips.Length)]);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            PlayerMobile pm = m as PlayerMobile;

            if (pm == null || !pm.Alive || pm.Hidden || pm.Map != Map)
                return;

            // Only trigger when the character actually enters the greeter's nearby area.
            bool wasNearby = Utility.InRange(oldLocation, Location, 6);
            bool isNearby = pm.InRange(Location, 6);

            if (wasNearby || !isNearby || _Welcomed.Contains(pm.Serial))
                return;

            // This keeps established characters from receiving the "new arrival" greeting.
            if (pm.GameTime > NewPlayerTime)
                return;

            _Welcomed.Add(pm.Serial);

            Direction = GetDirectionTo(pm);

            SayTo(pm, "Welcome to Wolvesbane, {0}! I see you're new to our world.", pm.Name);
            SayTo(pm, "Double-click me if you'd like some advice on where to begin.");
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null)
                return;

            if (!from.InRange(Location, 4))
            {
                from.SendLocalizedMessage(500446);
                return;
            }

            Direction = GetDirectionTo(from);

            SayTo(from, "Welcome to Wolvesbane, {0}! If you're unsure where to begin, explore, speak with the locals, and ask questions in world chat. ( use [c )", from.Name);
            SayTo(from, "Adventure, crafting, taming, treasure hunting, gardening, events and custom content all await you.");
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
            _NextTip = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }
    }
}
