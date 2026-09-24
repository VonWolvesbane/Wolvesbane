using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a greeter's corpse")]
    public class WolvesbaneGreeter : BaseCreature
    {
        private DateTime m_NextTip;
        private DateTime m_NextAnnouncement;
        private TimeSpan m_MinTipDelay = TimeSpan.FromMinutes(2.0);
        private TimeSpan m_MaxTipDelay = TimeSpan.FromMinutes(5.0);
        private TimeSpan m_AnnouncementDelay = TimeSpan.FromMinutes(1.0);

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan MinTipDelay
        {
            get { return m_MinTipDelay; }
            set { m_MinTipDelay = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan MaxTipDelay
        {
            get { return m_MaxTipDelay; }
            set { m_MaxTipDelay = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan AnnouncementDelay
        {
            get { return m_AnnouncementDelay; }
            set { m_AnnouncementDelay = value; }
        }

        [Constructable]
        public WolvesbaneGreeter()
            : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Wolvesbane Greeter";
            Title = "the Newcomer Guide";
            Body = 0x190;
            Hue = Utility.RandomSkinHue();
            Blessed = true;
            CantWalk = false;

            SetStr(100);
            SetDex(100);
            SetInt(100);

            SetHits(1000);

            AddItem(new Shirt(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Shoes(Utility.RandomNeutralHue()));

            HairItemID = Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047, 0x2048, 0x2049);
            HairHue = Utility.RandomHairHue();

            m_NextTip = DateTime.UtcNow + RandomDelay();
            m_NextAnnouncement = DateTime.UtcNow + AnnouncementDelaySafe();
        }

        public WolvesbaneGreeter(Serial serial)
            : base(serial)
        {
        }

        public override bool IsInvulnerable
        {
            get { return true; }
        }

        public override bool CanTeach
        {
            get { return false; }
        }

        public override bool ShowFameTitle
        {
            get { return false; }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || from.Deleted)
                return;

            if (!from.InRange(Location, 4))
            {
                from.SendLocalizedMessage(500446); // That is too far away.
                return;
            }

            Direction = GetDirectionTo(from);
            from.CloseGump(typeof(WolvesbaneGreeterGump));
            from.SendGump(new WolvesbaneGreeterGump(from, this, 0));
        }

        public override void OnThink()
        {
            base.OnThink();

            DateTime now = DateTime.UtcNow;

            if (now >= m_NextAnnouncement)
            {
                SayAnnouncement();
                m_NextAnnouncement = now + AnnouncementDelaySafe();
            }

            if (now >= m_NextTip)
            {
                SayRandomTip();
                m_NextTip = now + RandomDelay();
            }
        }

        private TimeSpan RandomDelay()
        {
            double min = Math.Max(10.0, m_MinTipDelay.TotalSeconds);
            double max = Math.Max(min, m_MaxTipDelay.TotalSeconds);

            return TimeSpan.FromSeconds(min + (Utility.RandomDouble() * (max - min)));
        }

        private TimeSpan AnnouncementDelaySafe()
        {
            return TimeSpan.FromSeconds(Math.Max(10.0, m_AnnouncementDelay.TotalSeconds));
        }

        private void SayAnnouncement()
        {
            if (!String.IsNullOrEmpty(WolvesbaneGreeterConfig.AnnouncementText))
                Say(WolvesbaneGreeterConfig.AnnouncementText);
        }

        private void SayRandomTip()
        {
            if (WolvesbaneGreeterConfig.SpeechTips.Count == 0)
                return;

            int index = Utility.Random(WolvesbaneGreeterConfig.SpeechTips.Count);
            Say(WolvesbaneGreeterConfig.SpeechTips[index]);
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            if (from != null && from.AccessLevel >= AccessLevel.GameMaster && dropped != null)
            {
                // Let GM+ staff dress the greeter directly by dropping wearable items on him.
                if (dropped.Layer != Layer.Invalid && dropped.Layer != Layer.Backpack && dropped.Layer != Layer.Bank)
                {
                    Item existing = FindItemOnLayer(dropped.Layer);

                    if (existing != null && existing != dropped)
                    {
                        if (Backpack == null)
                            AddItem(new Backpack());

                        Backpack.DropItem(existing);
                    }

                    AddItem(dropped);
                    from.SendMessage(68, "You equip {0} on the Wolvesbane Greeter.", dropped.Name ?? dropped.GetType().Name);
                    return true;
                }
            }

            return base.OnDragDrop(from, dropped);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(2); // version
            writer.Write(m_MinTipDelay);
            writer.Write(m_MaxTipDelay);
            writer.Write(m_NextTip);
            writer.Write(m_AnnouncementDelay);
            writer.Write(m_NextAnnouncement);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    m_MinTipDelay = reader.ReadTimeSpan();
                    m_MaxTipDelay = reader.ReadTimeSpan();
                    m_NextTip = reader.ReadDateTime();
                    m_AnnouncementDelay = reader.ReadTimeSpan();
                    m_NextAnnouncement = reader.ReadDateTime();
                    break;
                case 1:
                    m_MinTipDelay = reader.ReadTimeSpan();
                    m_MaxTipDelay = reader.ReadTimeSpan();
                    m_NextTip = reader.ReadDateTime();
                    m_AnnouncementDelay = TimeSpan.FromMinutes(1.0);
                    m_NextAnnouncement = DateTime.UtcNow + AnnouncementDelaySafe();
                    break;
                case 0:
                    m_MinTipDelay = TimeSpan.FromMinutes(2.0);
                    m_MaxTipDelay = TimeSpan.FromMinutes(5.0);
                    m_NextTip = DateTime.UtcNow + RandomDelay();
                    m_AnnouncementDelay = TimeSpan.FromMinutes(1.0);
                    m_NextAnnouncement = DateTime.UtcNow + AnnouncementDelaySafe();
                    break;
            }

            if (m_NextTip < DateTime.UtcNow)
                m_NextTip = DateTime.UtcNow + RandomDelay();

            if (m_NextAnnouncement < DateTime.UtcNow)
                m_NextAnnouncement = DateTime.UtcNow + AnnouncementDelaySafe();
        }
    }
}
