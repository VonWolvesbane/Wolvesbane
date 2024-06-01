using System;
using Server;
using Server.Items;
using System.Threading;

namespace Server.Items
{
    public class JuneThingy2024 : Item
    {
        private static readonly int[] hues = { 33, 43, 53, 63, 8, 13 };
        private int currentHueIndex = 0;
        private System.Threading.Timer colorCycleTimer;

        [Constructable]
        public JuneThingy2024() : base(0x42C9)
        {
            Name = "June Thingy 2024";
            Weight = 2.0;
            StartColorCycle();
        }

        public JuneThingy2024(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)currentHueIndex); // Serialize currentHueIndex
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            currentHueIndex = reader.ReadInt(); // Deserialize currentHueIndex
            StartColorCycle(); // Restart color cycling after deserialization
        }

        public override void OnDelete()
        {
            base.OnDelete();
            StopColorCycle();
        }

        private void StartColorCycle()
        {
            colorCycleTimer = new System.Threading.Timer(CycleColor, null, TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0));
        }

        private void StopColorCycle()
        {
            if (colorCycleTimer != null)
            {
                colorCycleTimer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
                colorCycleTimer.Dispose();
                colorCycleTimer = null;
            }
        }

        private void CycleColor(object state)
        {
            currentHueIndex = (currentHueIndex + 1) % hues.Length;
            Hue = hues[currentHueIndex];
        }
    }
}
