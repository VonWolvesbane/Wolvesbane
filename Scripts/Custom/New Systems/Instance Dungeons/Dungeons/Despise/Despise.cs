#region Header
//  Von Wolvesbane -- Despise.cs
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Linq;

using Server;
using Server.Items;
using Server.Mobiles;
using Xanthos.Evo;
#endregion

namespace VitaNex.Dungeons
{
	public sealed class Despise : Dungeon
	{
		public override DungeonID ID { get { return DungeonID.Des; } }

		public override Map MapParent { get { return Server.Map.Felucca; } }

		public override TimeSpan Duration { get { return TimeSpan.FromHours(0.05); } }
		public override TimeSpan Lockout { get { return TimeSpan.FromHours(0.25); } }
		
		public override Point3D Entrance { get { return new Point3D(5413, 859, 65); } }
		public override Point3D Exit { get { return new Point3D(5610, 879, 30); } }

		public override int GroupMax { get { return 5; } }

		public override int GoldMin { get { return 100; } }
		public override int GoldMax { get { return 3500000; } }

		public override int LootPropsMin { get { return 4; } }
		public override int LootPropsMax { get { return 11; } }

		public override int LootIntensityMin { get { return 40; } }
		public override int LootIntensityMax { get { return 1000; } }

		public override string Name { get { return "Despise"; } }
		public override string Desc { get { return "Something evil lurks in the darkness, and it's very hungry..."; } }

		public Mobile Boss1 { get; private set; }
		public Mobile Boss2 { get; private set; }
		public Mobile Boss3 { get; private set; }

		private List<Static>[] _BossFields = {new List<Static>(), new List<Static>(), new List<Static>()};

		public Despise()
		{ }

		public Despise(DungeonSerial serial)
			: base(serial)
		{ }

		protected override void OnGenerate()
		{
			base.OnGenerate();

			CreateZone("Despise", new Rectangle2D(5490, 874, 148, 148));
			//CreateZone("Control Room", new Rectangle2D(6036, 1430, 27, 14), new Rectangle2D(6042, 1444, 15, 10));

			var valderian = CreateMobile<ObsidianWyvern>(new Point3D(5502, 1009, 5), false, true);

			valderian.Name = "Valderian";
			valderian.Hue = 2967;
			valderian.Hits = 1000000000;
			valderian.MagicDamageAbsorb = 100;


			if (Utility.RandomDouble() < 0.05)
			{
				valderian.Tamable = false;
				valderian.IsParagon = true;
			}

			/*GenerateStairs();
			GenerateTraps();*/

			GenerateEasySpawn();
			GenerateHardSpawn();
			GenerateBossSpawn();
		}

		/*private void GenerateStairs()
		{
			var tiles = new[] { 1959, 1958, 1957, 1956, 1962, 767, 766 };
			
			var points = new[]
			{
				//1959
				new[]
				{
					new Point3D(6082, 1449, 5), new Point3D(6082, 1450, 5), new Point3D(6036, 1469, 5), new Point3D(6035, 1457, 5),
					new Point3D(6047, 1467, 5), new Point3D(6077, 1485, 5), new Point3D(6115, 1450, 5), new Point3D(6112, 1482, -20),
					new Point3D(6113, 1482, -15), new Point3D(6112, 1483, -20), new Point3D(6113, 1483, -15),
					new Point3D(6041, 1442, 6), new Point3D(6041, 1442, 6), new Point3D(6041, 1440, 6), new Point3D(6045, 1435, 0),
					new Point3D(6045, 1434, 0), new Point3D(6051, 1435, 6), new Point3D(6051, 1434, 6)
				},
				//1958
				new[] {new Point3D(6084, 1447, 0), new Point3D(6036, 1480, 5), new Point3D(6041, 1489, 6)},
				//1957
				new[]
				{
					new Point3D(6084, 1449, 5), new Point3D(6084, 1450, 5), new Point3D(6037, 1469, 0), new Point3D(6036, 1457, 0),
					new Point3D(6048, 1467, 0), new Point3D(6078, 1485, 0), new Point3D(6116, 1450, 0), new Point3D(6046, 1435, 6),
					new Point3D(6046, 1434, 6)
				},
				//1956
				new[] {new Point3D(6036, 1481, 0), new Point3D(6041, 1490, 0)},
				//1962,
				new[] {new Point3D(6084, 1448, 5)},
				//767,
				new[] {new Point3D(6105, 1454, 25), new Point3D(6105, 1455, 25)},
				//766,
				new[] {new Point3D(6106, 1455, 25)}
			};

			for (int i = 0, id; i < tiles.Length; i++)
			{
				id = tiles[i];

				foreach (var p in points[i])
				{
					CreateStatic(id, p, true);
				}
			}
		}

		private void GenerateTraps()
		{
			var traps = new[]
			{
				new Point3D(6032, 1488, 5), new Point3D(6035, 1479, 5), new Point3D(6034, 1463, 5), new Point3D(6049, 1470, 5),
				new Point3D(6064, 1472, 5), new Point3D(6064, 1468, 5), new Point3D(6048, 1492, 5), new Point3D(6066, 1491, 5),
				new Point3D(6073, 1506, 5), new Point3D(6074, 1484, 5), new Point3D(6090, 1433, 5), new Point3D(6090, 1457, 5),
				new Point3D(6086, 1443, 0), new Point3D(6089, 1443, 0), new Point3D(6084, 1443, 0)
			};

			foreach (var t in traps.Select(p => CreateItem<GasTrap>(p, false)).Where(t => t != null))
			{
				t.Movable = false;

				t.Type = GasTrapType.Floor;
				t.Poison = Poison.GetPoison(Utility.Random(5)) ?? Poison.Deadly;
			}
		}*/

		private void GenerateEasySpawn()
		{
			var types = new[] {typeof(Moose), typeof(Panda), typeof(Griffin)};

			var points = new[]
			{
new Point3D(5393, 866, 45), //
new Point3D(5402, 866, 45), //
new Point3D(5401, 856, 45), //
new Point3D(5393, 855, 45), //
new Point3D(5391, 848, 47), //
new Point3D(5386, 851, 55), //
new Point3D(5405, 840, 45), //
new Point3D(5394, 830, 60), //
new Point3D(5392, 824, 60), //
new Point3D(5400, 822, 60), //
new Point3D(5388, 817, 60), //
new Point3D(5407, 818, 60), //
new Point3D(5413, 813, 60), //
new Point3D(5408, 823, 60), //
new Point3D(5395, 795, 65), //
new Point3D(5394, 784, 65), //
new Point3D(5405, 777, 75), //
new Point3D(5408, 788, 65), //
new Point3D(5427, 778, 60), //
new Point3D(5419, 777, 60), //
new Point3D(5425, 792, 60), //
new Point3D(5440, 779, 60), //
new Point3D(5446, 787, 60), //
new Point3D(5458, 783, 60), //
new Point3D(5458, 804, 60), //
new Point3D(5456, 821, 60), //
new Point3D(5461, 818, 60), //
new Point3D(5469, 809, 60), //
new Point3D(5477, 795, 67), //
new Point3D(5488, 794, 60), //
new Point3D(5493, 784, 70), //
new Point3D(5499, 777, 70), //
new Point3D(5483, 826, 60), //
new Point3D(5480, 832, 60), //
new Point3D(5464, 839, 45), //
new Point3D(5447, 843, 45), //
new Point3D(5471, 856, 45), //
new Point3D(5474, 869, 45), //
new Point3D(5461, 880, 30), //
new Point3D(5451, 876, 30), //
new Point3D(5448, 869, 45), //
new Point3D(5469, 888, 30), //
new Point3D(5502, 841, 45), //
new Point3D(5506, 849, 45), //
new Point3D(5515, 858, 45), //
new Point3D(5525, 848, 45), //
new Point3D(5522, 836, 50), //
new Point3D(5532, 867, 45), //
new Point3D(5542, 858, 45), //
new Point3D(5537, 862, 45), //
new Point3D(5546, 879, 30), //
new Point3D(5537, 882, 30), //
new Point3D(5530, 879, 30), //
new Point3D(5567, 858, 45), //
new Point3D(5571, 868, 45), //
new Point3D(5581, 858, 45), //
new Point3D(5596, 843, 45), //
new Point3D(5592, 835, 45), //
new Point3D(5611, 827, 60), //
new Point3D(5608, 817, 60), //
new Point3D(5603, 826, 60), //
new Point3D(5589, 817, 45), //
new Point3D(5577, 822, 45), //
new Point3D(5601, 807, 60), //
new Point3D(5589, 804, 45), //
new Point3D(5588, 790, 60), //
new Point3D(5605, 790, 60), //
new Point3D(5618, 783, 60), //
new Point3D(5585, 785, 60), //
new Point3D(5566, 800, 45), //
new Point3D(5571, 785, 60), //
new Point3D(5556, 799, 45), //
new Point3D(5547, 796, 45), //
new Point3D(5556, 777, 60), //
new Point3D(5554, 785, 60), //
new Point3D(5523, 785, 60), //
new Point3D(5532, 792, 60), //
new Point3D(5533, 799, 60), //
new Point3D(5520, 832, 60), //
new Point3D(5523, 875, 30), //
new Point3D(5532, 880, 30), //
new Point3D(5562, 896, 30), //
new Point3D(5568, 888, 30), //
new Point3D(5571, 891, 30), //
new Point3D(5552, 919, 30), //
new Point3D(5540, 909, 30), //
new Point3D(5514, 907, 30), //
new Point3D(5523, 889, 30), //
new Point3D(5588, 895, 30), //
new Point3D(5591, 885, 30), //
new Point3D(5598, 876, 30), //
new Point3D(5607, 882, 30), //
new Point3D(5604, 870, 45), //
new Point3D(5481, 949, 20), //
new Point3D(5485, 938, 20), //
new Point3D(5471, 938, 21), //
new Point3D(5460, 929, 20), //
new Point3D(5443, 921, 20), //
new Point3D(5433, 918, 20), //
new Point3D(5426, 922, 20), //
new Point3D(5429, 914, 20), //
new Point3D(5428, 941, 20), //
new Point3D(5421, 945, 20), //
new Point3D(5410, 943, 20), //
new Point3D(5413, 935, 20), //
new Point3D(5437, 963, 15), //
new Point3D(5443, 980, 15), //
new Point3D(5450, 974, 15), //
new Point3D(5450, 1000, 5), //



			};

			foreach (var p in points)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}

			// Water

			types = new[] {typeof(SludgeElemental), typeof(MutatedTentacles)};
			points = new[]
			{
				new Point3D(6051, 1465, 0), new Point3D(6037, 1458, 0), new Point3D(6078, 1493, 0), new Point3D(6087, 1446, 0),
				new Point3D(6087, 1457, 0), new Point3D(6087, 1476, 0), new Point3D(6090, 1485, 0), new Point3D(6072, 1451, 0),
				new Point3D(6091, 1493, 0), new Point3D(6111, 1484, -20), new Point3D(6117, 1452, 0)
			};

			foreach (var p in points)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}
		}

		private void GenerateHardSpawn()
		{
			var types = new[] {typeof(SewerMutant), typeof(SewerFiend), typeof(FecalConstruct)};
			var points = new[]
			{
				new Point3D(6092, 1447, 5), new Point3D(6090, 1460, 5), new Point3D(6090, 1463, 5), new Point3D(6090, 1467, 6),
				new Point3D(6107, 1452, 25), new Point3D(6107, 1454, 25), new Point3D(6103, 1469, 5), new Point3D(6098, 1469, 5),
				new Point3D(6098, 1478, 5), new Point3D(6083, 1485, 5), new Point3D(6107, 1454, 25), new Point3D(6107, 1452, 25),
				new Point3D(6080, 1448, 5), new Point3D(6077, 1445, 20), new Point3D(6067, 1455, 20), new Point3D(6062, 1458, 5),
				new Point3D(6067, 1445, 20), new Point3D(6083, 1491, 5), new Point3D(6092, 1491, 10), new Point3D(6097, 1491, 5),
				new Point3D(6102, 1491, 5), new Point3D(6107, 1491, 5), new Point3D(6107, 1483, 5), new Point3D(6115, 1491, -15),
				new Point3D(6115, 1481, -10), new Point3D(6115, 1471, 5), new Point3D(6115, 1463, 5), new Point3D(6112, 1448, 5)
			};

			foreach (var p in points)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}

			types = new[] {typeof(SewerRatman), typeof(SewerRatmanArcher), typeof(SewerRatmanMage)};
			points = new[]
			{
				//Control Room
				new Point3D(6046, 1456, 4), new Point3D(6049, 1456, 4), new Point3D(6052, 1456, 4), new Point3D(6051, 1447, 5),
				new Point3D(6051, 1445, 5), new Point3D(6051, 1443, 5), new Point3D(6051, 1441, 5), new Point3D(6047, 1441, 5),
				new Point3D(6047, 1443, 5), new Point3D(6047, 1445, 5), new Point3D(6047, 1447, 5), new Point3D(6060, 1441, 4),
				new Point3D(6060, 1438, 4), new Point3D(6038, 1438, 4), new Point3D(6038, 1441, 4),
				//Cave
				new Point3D(6121, 1451, 5), new Point3D(6123, 1451, 5), new Point3D(6121, 1440, 5), new Point3D(6123, 1440, 5),
				new Point3D(6129, 1441, 4)
			};

			foreach (var p in points)
			{
				CreateMobile(types.GetRandom(), p, false, true);
			}
		}

		private void GenerateBossSpawn()
		{
			// Cave
			Boss1 = CreateMobile<Hephastos>(new Point3D(5557, 824, 45), true, true);

			// Sewage Intake
			Boss2 = CreateMobile<RidableAncientHellHound>(new Point3D(5511, 942, 20), true, true);

			// Control Room
			Boss3 = CreateMobile<GuardianWolfEvo>(new Point3D(5464, 988, 5), true, true);
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

			// Boss 3 (Control Room)
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
		}

		protected override void OnAfterDelete()
		{
			base.OnAfterDelete();

			Boss1 = Boss2 = Boss3 = null;
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