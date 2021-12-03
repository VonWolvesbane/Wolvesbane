// Created by ReApEr
using System;
using System.Collections.Generic;
using CustomsFramework;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class Minichampspawner : MiniChampionStone
    {
        public int i { get; private set; }

        [Constructable]
        public Minichampspawner()
        {
            Hue = 150;
            MiniChampName = "Shredder was here!";
        }

        public Minichampspawner(Serial serial) : base(serial)
        {
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

        private static int GetRandomMaxSpawn(int i)
        {
            if (i == 0)
                return Utility.RandomMinMax(1, 2);
            return Utility.RandomMinMax(1, 4);
        }

        private static int GetRandomTickSpawn(int i)
        {
            if (i == 0)
                return 1;
            return Utility.RandomMinMax(1, 2);
        }

        private static TimeSpan GetRespawnMaxTimer(int i)
        {
            if (i == 0)
                return new TimeSpan(0, 0, 0);
            return new TimeSpan(0, 0, 1);
        }

        public override void ActivateStone()
        {

            #region MiniChamp Spawn

            if (i == 0)
            {
                var MiniChamp = new XmlSpawner();
                MiniChamp.Map = Map.Felucca;
                MiniChamp.Name = "Shredder";
                MiniChamp.MoveToWorld(new Point3D(6200, 2582, 0));
                MiniChamp.MaxCount = 30;
                MiniChamp.HomeRange = 10;
                MiniChamp.SpawnRange = 10;
                MiniChamp.Group = true;
                MiniChamp.SmartSpawning = true;

                // Wave 1
                MiniChamp.AddSpawn = typeof(mouser).Name;
                if (MiniChamp.SpawnObjects.Length > 0)
                {
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].Available = true;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].SubGroup = MiniChamp.SpawnObjects.Length;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].SpawnsPerTick = 20;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].KillsNeeded = 25;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].MaxCount = 25;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].RestrictKillsToSubgroup = true;
                }
				
                // Wave 2
                MiniChamp.AddSpawn = typeof(mouser).Name;
                if (MiniChamp.SpawnObjects.Length > 1)
                {
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].Available = true;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].SubGroup = MiniChamp.SpawnObjects.Length;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].SpawnsPerTick = 20;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].KillsNeeded = 25;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].MaxCount = 25;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].RestrictKillsToSubgroup = true;
                }

                // Wave 3
                MiniChamp.AddSpawn = typeof(FootSoldier).Name;
                if (MiniChamp.SpawnObjects.Length > 2)
                {
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].Available = true;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].SubGroup = MiniChamp.SpawnObjects.Length;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].SpawnsPerTick = 10;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].KillsNeeded = 20;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].MaxCount = 20;
                }

                // MiniChamp Spawn

                MiniChamp.AddSpawn = typeof(Shredder).Name;
                if (MiniChamp.SpawnObjects.Length > 3)
                {
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].Available = true;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].SubGroup = MiniChamp.SpawnObjects.Length;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].SpawnsPerTick = 1;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].KillsNeeded = 1;
                    MiniChamp.SpawnObjects[MiniChamp.SpawnObjects.Length - 1].MaxCount = 1;
                }

                MiniChamp.KillReset = 0;
                for (int o = 0; o < MiniChamp.SpawnObjects.Length; o++)
                {
                    MiniChamp.KillReset += MiniChamp.SpawnObjects[o].KillsNeeded;
                }
                MiniChamp.MinDelay = new TimeSpan(0, 0, 1);
                MiniChamp.MaxDelay = new TimeSpan(0, 0, 15);
                MiniChamp.NextSpawn = new TimeSpan(0, 0, 0);
                MiniChamp.SortSpawns();
                MiniChamp.DoReset = true;
                MiniChamp.SequentialSpawn = 1;
                MiniChamp.Start();
            }

            #endregion

            base.ActivateStone();
        }
    }
}
