using System;

namespace Server.Items
{
    public class BaseDecayingItem : Item
    {
        public virtual int m_Lifespan { get; set; }
		private Timer m_Timer;

		public virtual bool UseSeconds => true;

        [CommandProperty(AccessLevel.GameMaster)]
        public DecayingItemSocket DecayInfo { get { return GetSocket<DecayingItemSocket>(); } set { } }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TimeLeft
        {
            get
            {
                var socket = GetSocket<DecayingItemSocket>();

                if (socket != null)
                {
                    return socket.Remaining;
                }

                return 0;
            }
            set
            {
                var socket = GetSocket<DecayingItemSocket>();

                if (socket != null)
                {
                    socket.Expires = DateTime.UtcNow + TimeSpan.FromSeconds(value);
                }
                else if (value > 0)
                {
                    AttachSocket(new DecayingItemSocket(value, UseSeconds));
                }

                InvalidateProperties();
            }
        }

        public BaseDecayingItem(int itemID) : base(itemID)
        {
            LootType = LootType.Blessed;

            if (m_Lifespan > 0)
            {
                AttachSocket(new DecayingItemSocket(m_Lifespan, UseSeconds));
            }
        }

        public BaseDecayingItem(Serial serial)
            : base(serial)
        {
        }

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			if (m_Lifespan > 0)
			{
				if (UseSeconds)
					list.Add(1072517, m_Lifespan.ToString()); // Lifespan: ~1_val~ seconds
				else
				{
					TimeSpan t = TimeSpan.FromSeconds(TimeLeft);

					int weeks = (int)t.Days / 7;
					int days = t.Days;
					int hours = t.Hours;
					int minutes = t.Minutes;

					if (weeks > 1)
						list.Add(1153092, (t.Days / 7).ToString()); // Lifespan: ~1_val~ weeks
					else if (days > 1)
						list.Add(1153091, t.Days.ToString()); // Lifespan: ~1_val~ days
					else if (hours > 1)
						list.Add(1153090, t.Hours.ToString()); // Lifespan: ~1_val~ hours
					else if (minutes > 1)
						list.Add(1153089, t.Minutes.ToString()); // Lifespan: ~1_val~ minutes
					else
						list.Add(1072517, t.Seconds.ToString()); // Lifespan: ~1_val~ seconds
				}
			}
		}

		public virtual void StartTimer()
		{
			if (m_Timer != null || m_Lifespan == 0)
				return;

			m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10), new TimerCallback(Slice));
			m_Timer.Priority = TimerPriority.OneSecond;
		}

		public virtual void StopTimer()
		{
			if (m_Timer != null)
				m_Timer.Stop();

			m_Timer = null;
		}

		public virtual void Slice()
		{
			m_Lifespan -= 10;

			InvalidateProperties();

			if (m_Lifespan <= 0)
				Decay();
		}

		public virtual void Decay()
        {
            if (RootParent is Mobile)
            {
                Mobile parent = (Mobile)RootParent;

                if (Name == null)
                    parent.SendLocalizedMessage(1072515, "#" + LabelNumber); // The ~1_name~ expired...
                else
                    parent.SendLocalizedMessage(1072515, Name); // The ~1_name~ expired...

                Effects.SendLocationParticles(EffectItem.Create(parent.Location, parent.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
                Effects.PlaySound(parent.Location, parent.Map, 0x201);
            }
            else
            {
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
                Effects.PlaySound(Location, Map, 0x201);
            }

            Delete();
        }

        public virtual void SendTimeRemainingMessage(Mobile to)
        {
            var socket = GetSocket<DecayingItemSocket>();

            if (socket != null && socket.Expires > DateTime.UtcNow)
            {
                to.SendLocalizedMessage(1072516, string.Format("{0}\t{1}", (Name == null ? string.Format("#{0}", LabelNumber) : Name), socket.Remaining)); // ~1_name~ will expire in ~2_val~ seconds!
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
            {
                var lifespan = reader.ReadInt();

                if (lifespan > 0)
                {
                    AttachSocket(new DecayingItemSocket(lifespan, UseSeconds));
                }
            }
        }
    }
}
