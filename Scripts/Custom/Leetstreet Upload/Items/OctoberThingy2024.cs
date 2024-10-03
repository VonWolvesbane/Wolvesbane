using System;
using Server;
using Server.Items;
using System.Threading;

namespace Server.Items
{
    public class OctoberThingy2024 : Item
    {
        private static readonly int[] hues = { 232, 137, 139, 142, 143, 144, 147, 148, 149 };
        private int currentHueIndex = 0;
        private Timer colorCycleTimer;

        [Constructable]
        public OctoberThingy2024() : base(41369)
        {
            Name = "October Thingy 2024";
            Weight = 2.0;
            StartColorCycle();
        }

        public OctoberThingy2024(Serial serial) : base(serial)
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
            StopColorCycle();
            base.OnDelete();
        }

        private void StartColorCycle()
        {
            StopColorCycle(); // Ensure any previous timer is stopped before starting a new one
            colorCycleTimer = Timer.DelayCall(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0), CycleColor);
        }

        private void StopColorCycle()
        {
            if (colorCycleTimer != null)
            {
                colorCycleTimer.Stop();
                colorCycleTimer = null;
            }
        }

        private void CycleColor()
        {
            if (hues == null || hues.Length == 0)
            {
                return; // Ensure hues array is not null or empty
            }

            currentHueIndex = (currentHueIndex + 1) % hues.Length;
            Hue = hues[currentHueIndex];
        }
    }
}
