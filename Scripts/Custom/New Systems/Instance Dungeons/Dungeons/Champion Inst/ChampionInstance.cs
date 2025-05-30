#region References
using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Mobiles;
using Xanthos.Evo;
#endregion

namespace VitaNex.Dungeons
{
    public sealed class ChampInstance : Dungeon
    {
        public override DungeonID ID { get { return DungeonID.Champ; } }

        public override Map MapParent { get { return Server.Map.NewWolvesbane; } }

        public override TimeSpan Duration { get { return TimeSpan.FromHours(1.0); } }
        public override TimeSpan Lockout { get { return TimeSpan.FromHours(5.0); } }

        public override Point3D Entrance { get { return new Point3D(5836, 176, 0); } }
        public override Point3D Exit { get { return new Point3D(5741, 344, 0); } }

        public override int GroupMax { get { return 5; } }

        public override int GoldMin { get { return 100; } }
        public override int GoldMax { get { return 3500000; } }

        public override int LootPropsMin { get { return 4; } }
        public override int LootPropsMax { get { return 25; } }

        public override int LootIntensityMin { get { return 40; } }
        public override int LootIntensityMax { get { return 1000; } }

        public override string Name { get { return "Champion Instance"; } }
        public override string Desc { get { return "This will Spawn a Random Champion"; } }

        public Mobile Boss1 { get; private set; }
        public Mobile Boss2 { get; private set; }
        public Mobile Boss3 { get; private set; }

        private List<Static>[] _BossFields = { new List<Static>(), new List<Static>(), new List<Static>() };

        public ChampInstance()
        { }

        public ChampInstance(DungeonSerial serial)
            : base(serial)
        { }

        protected override void OnGenerate()
        {
            base.OnGenerate();

            CreateZone("ChampionInstance", new Rectangle2D(5608, 174, 221, 190));
            GenerateEasySpawn();
            GenerateBossSpawn();
            //GenerateBlockers();
			GenerateChampionSpawn();
		}
		private ChampionSpawn _ChampionSpawn;

		private void GenerateChampionSpawn()
		{
			var spawnTypes = new[]
   {
		ChampionSpawnType.Abyss,
		ChampionSpawnType.Arachnid,
		ChampionSpawnType.ColdBlood,
		ChampionSpawnType.UnholyTerror,
		ChampionSpawnType.VerminHorde,
		ChampionSpawnType.ForestLord,
		ChampionSpawnType.Glade,
		ChampionSpawnType.Corrupt,
		ChampionSpawnType.Terror,
		ChampionSpawnType.SleepingDragon,
		ChampionSpawnType.Khaldun,
		ChampionSpawnType.Infuse,
		ChampionSpawnType.Crafter
	};

			// Pick one at random
			ChampionSpawnType randomType = spawnTypes[Utility.Random(spawnTypes.Length)];

			_ChampionSpawn = new ChampionSpawn(); // Use the default constructor
			_ChampionSpawn.Type = randomType; // Set the type
			_ChampionSpawn.Map = Map;
			_ChampionSpawn.MoveToWorld(new Point3D(5715, 237, 20), Map);

			// Optional: configure spawn settings
			_ChampionSpawn.Active = true;
		


		}
		/*private void GenerateBlockers()
{
    var points = new[]
    {
        new Point3D(5237, 872, 1), //
        new Point3D(5238, 872, 1), //
        new Point3D(5239, 872, 1), //
        new Point3D(5240, 872, 1), //
        new Point3D(5241, 872, 1), //
        new Point3D(5242, 872, 1), //
        new Point3D(5243, 872, 1), //
        new Point3D(5244, 872, 1), //
        new Point3D(5245, 872, 1), //
        new Point3D(5246, 872, 1), //
        new Point3D(5247, 872, 1), //
        new Point3D(5248, 872, 1), //
        new Point3D(5249, 872, 1), //
        new Point3D(5250, 872, 1), //
        new Point3D(5251, 872, 1), //
        new Point3D(5252, 872, 1), //
        new Point3D(5253, 872, 1), //
        new Point3D(5254, 872, 1), //
        new Point3D(5255, 872, 1), //
        new Point3D(5256, 872, 1), //
        new Point3D(5257, 872, 1), //
        new Point3D(5258, 872, 1), //
        new Point3D(5259, 872, 1), //
        new Point3D(5260, 872, 1), //
        new Point3D(5261, 872, 1), //
    };

    // Reference to the instance's map
    var dungeonMap = Map;

    foreach (var p in points)
    {
        // Create and place a LOSBlocker in the instanced dungeon map
        var losBlocker = new LOSBlocker();
        losBlocker.MoveToWorld(p, dungeonMap);

        // Create and place a Blocker in the instanced dungeon map
        var blocker = new Blocker();
        blocker.MoveToWorld(p, dungeonMap);
    }
}*/
		private void GenerateEasySpawn()
        {
            var types = new[] { typeof(Dragon), typeof(Drake) };

            var points = new[]
            {
                new Point3D(5733, 256, 0),
                new Point3D(5705, 276, 0),
                new Point3D(5692, 250, 0)
            };

            foreach (var p in points)
            {
                CreateMobile(types.GetRandom(), p, false, true);
            }
        }

        private void GenerateBossSpawn()
        {
            Boss1 = CreateMobile<VonWolvesbane>(new Point3D(5682, 311, 0), true, true);
        }

        protected override void OnSpawnActivate(Mobile m)
        {
            base.OnSpawnActivate(m);

            if (m != null && (m == Boss1 || m == Boss2 || m == Boss3))
            {
                CheckBossFields();
            }
        }

        protected override void OnSpawnDeactivate(Mobile m)
        {
            base.OnSpawnDeactivate(m);

            if (m != null && (m == Boss1 || m == Boss2 || m == Boss3))
            {
                CheckBossFields();
            }
        }

        public override bool CheckCanMoveThrough(DungeonZone zone, Mobile m, IEntity e)
        {
            if (m != null && !m.Deleted && m.Alive && e != null && !e.Deleted && e is Static)
            {
                var s = (Static)e;

                if (s.Name == "Magical Barrier" && s.ItemID == 130)
                {
                    return false;
                }
            }

            return base.CheckCanMoveThrough(zone, m, e);
        }

        private void CheckBossFields()
        {
            if (Deleted)
            {
                return;
            }

            var heat = TimeSpan.FromSeconds(1.0);

            var wipe1 = (Boss1 == null || Boss1.Deleted || !Boss1.Alive || Boss1.Map != Map || !Boss1.InCombat(heat));
            var wipe2 = (Boss2 == null || Boss2.Deleted || !Boss2.Alive || Boss2.Map != Map || !Boss2.InCombat(heat));
            var wipe3 = (Boss3 == null || Boss3.Deleted || !Boss3.Alive || Boss3.Map != Map || !Boss3.InCombat(heat));

            var wipe = (wipe1 && wipe2 && wipe3) || !FindMobiles<PlayerMobile>(p => p.Alive).Any();

            if (wipe)
            {
                foreach (var fields in _BossFields)
                {
                    fields.ForEachReverse(f => f.Delete());
                }

                _BossFields.Free(true);
                return;
            }

            foreach (var fields in _BossFields)
            {
                fields.RemoveAll(f => f == null || f.Deleted);
            }

            // Boss 1 (Cave)
            if (wipe1)
            {
                _BossFields[0].ForEachReverse(f => f.Delete());
                _BossFields[0].Free(true);
            }
            else
            {
                var range = Enumerable.Empty<Static>() //
                    .With(TileStatic(130, new Point3D(6123, 1440, 5), 1, 3, true))
                    .With(TileStatic(130, new Point3D(6123, 1444, 5), 1, 2, true))
                    .With(TileStatic(130, new Point3D(6140, 1431, 5), 2, 1, true)) //
                    .Not(_BossFields[0].Contains);

                _BossFields[0].AddRange(range);
            }

            // Boss 2 (Sewage Intake)
            if (wipe2)
            {
                _BossFields[1].ForEachReverse(f => f.Delete());
                _BossFields[1].Free(true);
            }
            else
            {
                var range = Enumerable.Empty<Static>() //
                    .With(TileStatic(130, new Point3D(6078, 1443, 22), 1, 4, true))
                    .With(CreateStatic(130, new Point3D(6078, 1447, 7), true))
                    .With(TileStatic(130, new Point3D(6078, 1448, 9), 1, 2, true))
                    .With(CreateStatic(130, new Point3D(6096, 1445, 5), true))
                    .With(CreateStatic(130, new Point3D(6096, 1445, 25), true))
                    .With(TileStatic(130, new Point3D(6084, 1455, 5), 8, 1, true)) //
                  .Not(_BossFields[1].Contains);

                _BossFields[1].AddRange(range);
            }

            if (wipe3)
            {
                _BossFields[2].ForEachReverse(f => f.Delete());
                _BossFields[2].Free(true);
            }
            else
            {
                var range = Enumerable.Empty<Static>() //
                    .With(TileStatic(130, new Point3D(6044, 1453, 5), 13, 1, true)) //
                    .Not(_BossFields[2].Contains);

                _BossFields[2].AddRange(range);
            }

            foreach (var field in _BossFields.SelectMany(fields => fields))
            {
                field.Light = LightType.Circle300;
                field.Name = "Magical Barrier";
            }
        }

        protected override void OnSlice()
        {
            base.OnSlice();

            CheckBossFields();
        }

        protected override void OnDelete()
        {
            base.OnDelete();

            foreach (var fields in _BossFields)
            {
                fields.ForEachReverse(f => f.Delete());
            }

            _BossFields.Free(true);
			if (_ChampionSpawn != null && !_ChampionSpawn.Deleted)
				_ChampionSpawn.Delete();

			_ChampionSpawn = null;
		}

        protected override void OnAfterDelete()
        {
            base.OnAfterDelete();

            Boss1 = Boss2 = Boss3 = null;
			if (_ChampionSpawn != null && !_ChampionSpawn.Deleted)
				_ChampionSpawn.Delete();

			_ChampionSpawn = null;
		}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.SetVersion(0);

            writer.Write(Boss1);
            writer.Write(Boss2);
            writer.Write(Boss3);

            writer.WriteBlockArray(_BossFields, (w, f) => w.WriteItemList(f));
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.GetVersion();

            Boss1 = reader.ReadMobile();
            Boss2 = reader.ReadMobile();
            Boss3 = reader.ReadMobile();

            _BossFields = reader.ReadBlockArray(r => r.ReadStrongItemList<Static>());
        }
    }
}
