#region References
using Server.ContextMenus;
using Server.Engines.CannedEvil;
using Server.Engines.Harvest;
using Server.Gumps;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Regions;
using Server.Spells;
using Server.Targeting;
using System;
using System.Collections.Generic;
#endregion

namespace Server.Items
{
	public class TreasureMap : MapItem
	{
		public static bool NewSystem => false;

		public static double LootChance = Config.Get("TreasureMaps.LootChance", .01);
		private static TimeSpan ResetTime = TimeSpan.FromDays(Config.Get("TreasureMaps.ResetTime", 30.0));

		#region Forgotten Treasures
		private TreasurePackage _Package;

		[CommandProperty(AccessLevel.GameMaster)]
		public TreasureLevel TreasureLevel
		{
			get { return (TreasureLevel)m_Level; }
			set
			{
				if ((int)value != Level)
				{
					Level = (int)value;
				}
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public TreasurePackage Package
		{
			get { return _Package; }
			set { _Package = value; InvalidateProperties(); }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public TreasureFacet TreasureFacet => TreasureMapInfo.GetFacet(ChestLocation, Facet);

		protected void AssignRandomPackage()
		{
			Package = (TreasurePackage)Utility.Random(5);
		}

		public void AssignChestQuality(Mobile digger, TreasureMapChest chest)
		{
			double skill = digger.Skills[SkillName.Cartography].Value;

			int dif;

			switch (TreasureLevel)
			{
				default:
				case TreasureLevel.Stash: dif = 100; break;
				case TreasureLevel.Supply: dif = 200; break;
				case TreasureLevel.Cache: dif = 300; break;
				case TreasureLevel.Hoard: dif = 400; break;
				case TreasureLevel.Trove: dif = 500; break;
			}

			if (Utility.Random(dif) <= skill)
			{
				chest.ChestQuality = ChestQuality.Gold;
			}
			else if (Utility.Random(dif) <= skill * 2)
			{
				chest.ChestQuality = ChestQuality.Standard;
			}
			else
			{
				chest.ChestQuality = ChestQuality.Rusty;
			}
		}
		#endregion

		#region Spawn Types
		private static readonly Type[][] m_SpawnTypes = new Type[][]
		{
			new Type[]{ typeof( HeadlessOne ), typeof( Skeleton ) },
			new Type[]{ typeof( Mongbat ), typeof( Ratman ), typeof( HeadlessOne ), typeof( Skeleton ), typeof( Zombie ) },
			new Type[]{ typeof( OrcishMage ), typeof( Gargoyle ), typeof( Gazer ), typeof( HellHound ), typeof( EarthElemental ) },
			new Type[]{ typeof( Lich ), typeof( OgreLord ), typeof( DreadSpider ), typeof( AirElemental ), typeof( FireElemental ) },
			new Type[]{ typeof( DreadSpider ), typeof( LichLord ), typeof( Daemon ), typeof( ElderGazer ), typeof( OgreLord ) },
			new Type[]{ typeof( LichLord ), typeof( Daemon ), typeof( ElderGazer ), typeof( PoisonElemental ), typeof( BloodElemental ) },
			new Type[]{ typeof( AncientWyrm ), typeof( Balron ), typeof( BloodElemental ), typeof( PoisonElemental ), typeof( Titan ) },
			new Type[]{ typeof( BloodElemental), typeof(ColdDrake), typeof(FrostDragon), typeof(FrostDrake), typeof(GreaterDragon), typeof(PoisonElemental)}
		};

		// NewWolvesbane-specific guardian progression.
		private static readonly Type[][] m_NewWolvesbaneSpawnTypes = new Type[][]
		{
			new Type[]{ typeof( HeadlessOne ), typeof( Skeleton ) },
			new Type[]{ typeof( Mongbat ), typeof( Ratman ), typeof( HeadlessOne ), typeof( Skeleton ), typeof( Zombie ) },
			new Type[]{ typeof( OrcishMage ), typeof( Gargoyle ), typeof( Gazer ), typeof( HellHound ), typeof( EarthElemental ) },
			new Type[]{ typeof( Lich ), typeof( OgreLord ), typeof( DreadSpider ), typeof( AirElemental ), typeof( FireElemental ) },
			new Type[]{ typeof( DreadSpider ), typeof( LichLord ), typeof( Daemon ), typeof( ElderGazer ), typeof( OgreLord ) },
			new Type[]{ typeof( LichLord ), typeof( Daemon ), typeof( ElderGazer ), typeof( PoisonElemental ), typeof( BloodElemental ) },
			new Type[]{ typeof( AncientWyrm ), typeof( Balron ), typeof( BloodElemental ), typeof( PoisonElemental ), typeof( Titan ), typeof( Griffin ), typeof( Moose ), typeof( Panda ), typeof( WolvesbanianImp ) },
			new Type[]{ typeof( BloodElemental ), typeof( ColdDrake ), typeof( FrostDragon ), typeof( FrostDrake ), typeof( GreaterDragon ), typeof( PoisonElemental ), typeof( Griffin ), typeof( Moose ), typeof( Panda ), typeof( WolvesbanianImp ) }
		};

		private static readonly Type[][] m_TokunoSpawnTypes = new Type[][]
		{
			new Type[]{ typeof( HeadlessOne ), typeof( Skeleton ) },
			new Type[]{ typeof( HeadlessOne ), typeof( Mongbat ), typeof( Ratman ), typeof( Skeleton), typeof( Zombie ),  },
			new Type[]{ typeof( EarthElemental ), typeof( Gazer ), typeof( Gargoyle ), typeof( HellHound ), typeof( OrcishMage ), },
			new Type[]{ typeof( AirElemental ), typeof( DreadSpider ), typeof( FireElemental ), typeof( Lich ), typeof( OgreLord ), },
			new Type[]{ typeof( ElderGazer ), typeof( Daemon ), typeof( DreadSpider ), typeof( LichLord ), typeof( OgreLord ), },
			new Type[]{ typeof( FanDancer ), typeof( RevenantLion ), typeof( Ronin ), typeof( RuneBeetle ) },
			new Type[]{ typeof( Hiryu ), typeof( LadyOfTheSnow ), typeof( Oni ), typeof( RuneBeetle ), typeof( YomotsuWarrior ), typeof( YomotsuPriest ) },
			new Type[]{ typeof( Yamandon ), typeof( LadyOfTheSnow ), typeof( RuneBeetle ), typeof( YomotsuPriest ) }
		};

		private static readonly Type[][] m_MalasSpawnTypes = new Type[][]
		{
			new Type[]{ typeof( HeadlessOne ), typeof( Skeleton ) },
			new Type[]{ typeof( Mongbat ), typeof( Ratman ), typeof( HeadlessOne ), typeof( Skeleton ), typeof( Zombie ) },
			new Type[]{ typeof( OrcishMage ), typeof( Gargoyle ), typeof( Gazer ), typeof( HellHound ), typeof( EarthElemental ) },
			new Type[]{ typeof( Lich ), typeof( OgreLord ), typeof( DreadSpider ), typeof( AirElemental ), typeof( FireElemental ) },
			new Type[]{ typeof( DreadSpider ), typeof( LichLord ), typeof( Daemon ), typeof( ElderGazer ), typeof( OgreLord ) },
			new Type[]{ typeof( LichLord ), typeof( Ravager ), typeof( WandererOfTheVoid ), typeof( Minotaur ) },
			new Type[]{ typeof( Devourer ), typeof( MinotaurScout ), typeof( MinotaurCaptain ), typeof( RottingCorpse ), typeof( WandererOfTheVoid ) },
			new Type[]{ typeof( Devourer ), typeof( MinotaurGeneral ), typeof( MinotaurCaptain ), typeof( RottingCorpse ), typeof( WandererOfTheVoid ) }

		};

		private static readonly Type[][] m_IlshenarSpawnTypes = new Type[][]
		{
			new Type[]{ typeof( HeadlessOne ), typeof( Skeleton ) },
			new Type[]{ typeof( Mongbat ), typeof( Ratman ), typeof( HeadlessOne ), typeof( Skeleton ), typeof( Zombie ) },
			new Type[]{ typeof( OrcishMage ), typeof( Gargoyle ), typeof( Gazer ), typeof( HellHound ), typeof( EarthElemental ) },
			new Type[]{ typeof( Lich ), typeof( OgreLord ), typeof( DreadSpider ), typeof( AirElemental ), typeof( FireElemental ) },
			new Type[]{ typeof( DreadSpider ), typeof( LichLord ), typeof( Daemon ), typeof( ElderGazer ), typeof( OgreLord ) },
			new Type[]{ typeof( DarkGuardian ), typeof( ExodusOverseer ), typeof( GargoyleDestroyer ), typeof( GargoyleEnforcer ), typeof( PoisonElemental ) },
			new Type[]{ typeof( Changeling ), typeof( ExodusMinion ), typeof( GargoyleEnforcer ), typeof( GargoyleDestroyer ), typeof( Titan ) },
			new Type[]{ typeof( RenegadeChangeling ), typeof( ExodusMinion ), typeof( GargoyleEnforcer ), typeof( GargoyleDestroyer ), typeof( Titan ) }
		};

		private static readonly Type[][] m_TerMurSpawnTypes = new Type[][]
		{
			new Type[]{ typeof( HeadlessOne ), typeof( Skeleton ) },
			new Type[]{ typeof( ClockworkScorpion ), typeof( CorrosiveSlime ), typeof( GreaterMongbat ) },
			new Type[]{ typeof( AcidSlug ), typeof( FireElemental ), typeof( WaterElemental ) },
			new Type[]{ typeof( LeatherWolf ), typeof( StoneSlith ), typeof( ToxicSlith ) },
			new Type[]{ typeof( BloodWorm ), typeof( Kepetch ), typeof( StoneSlith ), typeof( ToxicSlith ) },
			new Type[]{ typeof( FireAnt ), typeof( LavaElemental ), typeof( MaddeningHorror ) },
			new Type[]{ typeof( EnragedEarthElemental ), typeof( FireDaemon ), typeof( GreaterPoisonElemental ), typeof( LavaElemental ), typeof( DragonWolf ) },
			new Type[]{ typeof( EnragedColossus ), typeof( EnragedEarthElemental ), typeof( FireDaemon ), typeof( GreaterPoisonElemental ), typeof( LavaElemental ) }
		};

		private static readonly Type[][] m_EodonSpawnTypes = new Type[][]
		{
			new Type[] { typeof(MyrmidexLarvae), typeof(SilverbackGorilla), typeof(Panther), typeof(WildTiger) },
			new Type[] { typeof(AcidElemental), typeof(SandVortex), typeof(Lion)/*, typeof(SabreToothedTiger)*/ },
			new Type[] { typeof(AcidElemental), typeof(SandVortex), typeof(Lion)/*, typeof(SabreToothedTiger)*/ },
			new Type[] { typeof(Infernus), typeof(FireElemental), typeof(Dimetrosaur), typeof(Saurosaurus) },
			new Type[] { typeof(Infernus), typeof(FireElemental), typeof(Dimetrosaur), typeof(Saurosaurus) },
			new Type[] { typeof(KotlAutomaton), typeof(MyrmidexDrone), typeof(Allosaurus), typeof(Triceratops) },
			new Type[] { typeof(Anchisaur), typeof(Allosaurus), typeof(SandVortex) }
		};
		#endregion

		#region Spawn Locations
		private static readonly Rectangle2D[] m_FelTramWrap = new Rectangle2D[]
		{
			new Rectangle2D(0, 0, 5119, 4095)
		};

		private static readonly Rectangle2D[] m_TokunoWrap = new Rectangle2D[]
		{
			new Rectangle2D(155, 207, 30, 40),
			new Rectangle2D(280, 230, 157, 45),
			new Rectangle2D(445, 215, 30, 35 ),
			new Rectangle2D(447, 53, 58, 40),
			new Rectangle2D(612, 240, 20, 17),
			new Rectangle2D(167, 275, 53, 60),
			new Rectangle2D(734, 407, 14, 22),
			new Rectangle2D(753, 489, 8, 30),
			new Rectangle2D(624, 619, 20, 24),
			new Rectangle2D(624, 725, 8, 8),
			new Rectangle2D(574, 734, 20, 16),
			new Rectangle2D(431, 752, 25, 27),
			new Rectangle2D(348, 968, 52, 135),
			new Rectangle2D(282, 1188, 90, 100),
			new Rectangle2D(348, 1335, 50, 50),

			new Rectangle2D(228, 284, 500, 316),
			new Rectangle2D(95, 600, 345, 243),
			new Rectangle2D(155, 842, 146, 1358),
			new Rectangle2D(495, 812, 435, 350),
			new Rectangle2D(501, 1156, 100, 150),
			new Rectangle2D(876, 1156, 90, 150),

			new Rectangle2D(970, 1159, 14, 25),
			new Rectangle2D(990, 1151, 5, 15),
			new Rectangle2D(1004, 1120, 16, 30),
			new Rectangle2D(1008, 1032, 12, 15),
			new Rectangle2D(1163, 383, 20, 20),

			new Rectangle2D(839, 30, 168, 120),
			new Rectangle2D(707, 150, 307, 250),
			new Rectangle2D(845, 397, 179, 75),
			new Rectangle2D(1068, 382, 60, 80),
			new Rectangle2D(787, 687, 60, 72),
			new Rectangle2D(848, 473, 557, 655),
		};

		private static readonly Rectangle2D[] m_MalasWrap = new Rectangle2D[]
		{
			new Rectangle2D(611, 67, 1862, 705),
			new Rectangle2D(1540, 852, 286, 182),
			new Rectangle2D(602, 784, 546, 746),
			new Rectangle2D(1160, 1035, 1299, 871)
		};

		private static readonly Rectangle2D[] m_IlshenarWrap = new Rectangle2D[]
		{
			new Rectangle2D(221, 314, 657, 286),
			new Rectangle2D(530, 600, 212, 205),
			new Rectangle2D(261, 805, 495, 655),
			new Rectangle2D(908, 925, 90, 170),
			new Rectangle2D(1031, 904, 730, 450),
			new Rectangle2D(1028, 630, 318, 161),
			new Rectangle2D(1205, 368, 265, 237),
			new Rectangle2D(1551, 516, 200, 130),
		};

		private static readonly Rectangle2D[] m_TerMurWrap = new Rectangle2D[]
		{
			new Rectangle2D(535, 2895, 85, 117),
			new Rectangle2D(525, 3085, 115, 70),
			new Rectangle2D(755, 2860, 400, 270),
			new Rectangle2D(1025, 3280, 190, 100 ),
			new Rectangle2D(305, 3445, 175, 255),
			new Rectangle2D(480, 3540, 90, 110),
			new Rectangle2D(605, 3880, 200, 170),
			new Rectangle2D(750, 3830, 80, 80),
		};

		private static readonly Rectangle2D[] m_EodonWrap = new Rectangle2D[]
		{
			new Rectangle2D(259, 1400, 354, 510),
			new Rectangle2D(259, 1400, 354, 510),
			new Rectangle2D(259, 1400, 354, 510),
			new Rectangle2D(688, 1440, 46, 88),
			new Rectangle2D(613, 1466, 65, 139),
			new Rectangle2D(678, 1568, 43, 40),
			new Rectangle2D(613, 1720, 91, 72),
			new Rectangle2D(618, 1792, 44, 273),
			new Rectangle2D(662, 1969, 84, 166),
			new Rectangle2D(754, 1963, 100, 65),
			new Rectangle2D(174, 1540, 85, 420),
		};
		#endregion
		public static bool IsInHavenIsland(IPoint2D loc)
		{
			return (loc.X >= 3314 && loc.X <= 3814 && loc.Y >= 2345 && loc.Y <= 3095);
		}
		private int m_Level;
		private bool m_Completed;
		private Mobile m_CompletedBy;
		private Mobile m_Decoder;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Level
		{
			get { return m_Level; }
			set
			{
				m_Level = Math.Min(value, TreasureMapInfo.NewSystem ? 4 : 7);

				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Completed
		{
			get { return m_Completed; }
			set
			{
				m_Completed = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile CompletedBy
		{
			get { return m_CompletedBy; }
			set
			{
				m_CompletedBy = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Decoder
		{
			get { return m_Decoder; }
			set
			{
				m_Decoder = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Point2D ChestLocation { get; set; }

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime NextReset { get; set; }

		public override int LabelNumber
		{
			get
			{
				if (m_Decoder != null)
				{
					if (m_Level == 6)
					{
						return 1063453;
					}
					else if (m_Level == 7)
					{
						return 1116773;
					}
					else
					{
						return 1041516 + m_Level;
					}
				}
				else if (m_Level == 6)
				{
					return 1063452;
				}
				else if (m_Level == 7)
				{
					return 1116790;
				}
				else
				{
					return 1041510 + m_Level;
				}
			}
		}

		public TreasureMap()
		{
		}

		// Wolvesbane treasure-map facet whitelist.
		// Eodon is NOT a separate server Map; it is an internal TreasureFacet
		// classification for part of Map.TerMur.
		public static bool IsAllowedTreasureMap(Map map)
		{
			return map == Map.Trammel ||
				map == Map.Felucca ||
				map == Map.Ilshenar ||
				map == Map.Malas ||
				map == Map.Tokuno ||
				map == Map.TerMur ||
				map == Map.NewWolvesbane;
		}

		private static Map NormalizeTreasureMap(Map map)
		{
			if (IsAllowedTreasureMap(map))
				return map;

			if (map != null && map != Map.Internal)
			{
				Console.WriteLine(
					"[TreasureMap] Unsupported facet '{0}' requested. Redirecting treasure map to Trammel.",
					map);
			}

			return Map.Trammel;
		}

		[Constructable]
		public TreasureMap(int level, Map map)
			: this(level, map, false)
		{
		}

		[Constructable]
		public TreasureMap(int level, Map map, bool eodon)
		{
			Level = level;
			bool newSystem = TreasureMapInfo.NewSystem;

			if (newSystem)
			{
				AssignRandomPackage();
			}

			if ((!newSystem && level == 7) || map == null || map == Map.Internal)
				map = GetRandomMap();
			else
				map = NormalizeTreasureMap(map);

			Facet = map;
			ChestLocation = GetRandomLocation(map, eodon);

			if (ChestLocation == Point2D.Zero)
			{
				Console.WriteLine(
					"[TreasureMap] WARNING: map {0} was created without a valid treasure location on {1}.",
					Serial,
					map);
			}

			Width = 300;
			Height = 300;
			int width, height;

			GetWidthAndHeight(map, out width, out height);

			int x1 = ChestLocation.X - Utility.RandomMinMax(width / 4, (width / 4) * 3);
			int y1 = ChestLocation.Y - Utility.RandomMinMax(height / 4, (height / 4) * 3);

			if (x1 < 0)
				x1 = 0;

			if (y1 < 0)
				y1 = 0;

			int x2;
			int y2;

			AdjustMap(map, out x2, out y2, x1, y1, width, height, eodon);

			x1 = x2 - width;
			y1 = y2 - height;

			Bounds = new Rectangle2D(x1, y1, width, height);
			Protected = true;

			AddWorldPin(ChestLocation.X, ChestLocation.Y);

			NextReset = DateTime.UtcNow + ResetTime;
		}

		public Map GetRandomMap()
		{
			switch (Utility.Random(7))
			{
				default:
				case 0: return Map.Trammel;
				case 1: return Map.Felucca;
				case 2: return Map.Ilshenar;
				case 3: return Map.Malas;
				case 4: return Map.Tokuno;
				case 5: return Map.TerMur;
				case 6:
					// Do not randomly create a NewWolvesbane map until staff have
					// configured at least one approved treasure area.
					return Wolvesbane.TreasureMaps.WBTreasureMapAreas.GetAreas().Length > 0
						? Map.NewWolvesbane
						: Map.Trammel;
			}
		}

		public static Point2D GetRandomLocation(Map map)
		{
			return GetRandomLocation(map, false);
		}

		private static Point2D GetRandomLocation(Map map, bool eodon)
		{
			if (!IsAllowedTreasureMap(map))
				return Point2D.Zero;

			Rectangle2D[] areas = GetTreasureAreas(map, eodon);

			// The script already contains carefully selected facet-specific treasure
			// regions. The previous implementation ignored them and sampled the
			// entire facet, which frequently selected void/water/invalid areas.
			if (areas != null && areas.Length > 0)
			{
				const int attempts = 100;

				for (int i = 0; i < attempts; ++i)
				{
					Rectangle2D area = areas[Utility.Random(areas.Length)];

					if (area.Width <= 0 || area.Height <= 0)
						continue;

					int x = area.X + Utility.Random(area.Width);
					int y = area.Y + Utility.Random(area.Height);

					if (ValidateLocation(x, y, map))
						return new Point2D(x, y);
				}

				// A deterministic second pass prevents random bad luck from causing
				// a false failure. Probe a grid inside every configured treasure area.
				for (int a = 0; a < areas.Length; ++a)
				{
					Rectangle2D area = areas[a];

					if (area.Width <= 0 || area.Height <= 0)
						continue;

					int stepX = Math.Max(1, area.Width / 12);
					int stepY = Math.Max(1, area.Height / 12);

					for (int x = area.X + (stepX / 2); x < area.X + area.Width; x += stepX)
					{
						for (int y = area.Y + (stepY / 2); y < area.Y + area.Height; y += stepY)
						{
							if (ValidateLocation(x, y, map))
								return new Point2D(x, y);
						}
					}
				}
			}

			// NewWolvesbane is a custom map. Its Scripts define the dimensions, but
			// the uploaded source does not contain enough terrain/travel information
			// to prove which parts of the facet are player-accessible. For this facet
			// we therefore REQUIRE staff-approved treasure areas instead of falling
			// back to random coordinates across the whole map.
			if (map == Map.NewWolvesbane)
			{
				Console.WriteLine(
					"[TreasureMap] NewWolvesbane has no usable approved treasure area. Use [WBTMapAreaAdd <radius> while standing in a safe hunting area.");

				return Point2D.Zero;
			}

			// Last-resort fallback: sample the stock facet itself. Do not silently return
			// map center unless it is actually a valid dig location.
			const int fallbackAttempts = 250;

			for (int i = 0; i < fallbackAttempts; ++i)
			{
				int x = Utility.Random(map.Width);
				int y = Utility.Random(map.Height);

				if (ValidateLocation(x, y, map))
					return new Point2D(x, y);
			}

			Point2D center = new Point2D(map.Width / 2, map.Height / 2);

			if (ValidateLocation(center.X, center.Y, map))
			{
				Console.WriteLine(
					"[TreasureMap] WARNING: no configured treasure-area location was valid for {0}; using validated map center {1},{2}.",
					map,
					center.X,
					center.Y);

				return center;
			}

			Console.WriteLine(
				"[TreasureMap] ERROR: unable to find any valid treasure location for facet {0} (Eodon={1}).",
				map,
				eodon);

			return Point2D.Zero;
		}

		private static Rectangle2D[] GetTreasureAreas(Map map, bool eodon)
		{
			if (map == Map.NewWolvesbane)
				return Wolvesbane.TreasureMaps.WBTreasureMapAreas.GetAreas();

			if (map == Map.Trammel || map == Map.Felucca)
				return m_FelTramWrap;

			if (map == Map.Ilshenar)
				return m_IlshenarWrap;

			if (map == Map.Malas)
				return m_MalasWrap;

			if (map == Map.Tokuno)
				return m_TokunoWrap;

			if (map == Map.TerMur)
				return eodon ? m_EodonWrap : m_TerMurWrap;

			return null;
		}


		public static bool ValidateLocation(int x, int y, Map map)
		{
			if (map == null || map == Map.Internal)
				return false;

			try
			{
				if (x < 0 || y < 0 || x >= map.Width || y >= map.Height)
					return false;

				LandTile landTile = map.Tiles.GetLandTile(x, y);

				if (landTile.ID <= 2)
					return false;

				int landID = landTile.ID & TileData.MaxLandValue;

				if (landID < 0 || landID >= TileData.LandTable.Length)
					return false;

				TileFlag flags = TileData.LandTable[landID].Flags;

				if ((flags & TileFlag.Impassable) != 0 || (flags & TileFlag.Wet) != 0)
					return false;

				// VitaNex knows the classic UO water/coastline tile IDs in addition
				// to the generic Wet flag. Custom facets often use water graphics
				// whose TileData flags alone are insufficient.
				if (landTile.IsWater() || landTile.IsCoastline())
					return false;

				StaticTile[] items = map.Tiles.GetStaticTiles(x, y, true);

				for (int i = 0; i < items.Length; ++i)
				{
					StaticTile tile = items[i];
					int id = tile.ID & TileData.MaxItemValue;

					if (id < 0 || id >= TileData.ItemTable.Length)
						continue;

					TileFlag f = TileData.ItemTable[id].Flags;

					if ((f & TileFlag.Impassable) != 0 ||
						(f & TileFlag.Wet) != 0 ||
						tile.IsWater())
					{
						return false;
					}
				}

				int z = map.GetAverageZ(x, y);

				// The chest and guardians need an actual standable tile, not merely
				// a non-wet land graphic.
				if (!map.CanSpawnMobile(x, y, z))
					return false;

				// NewWolvesbane gets an intentionally stronger dry-land test.
				// Reject shorelines, tiny islands, docks, and water-edge tiles by
				// requiring a dry/standable buffer around the treasure location.
				if (map == Map.NewWolvesbane && !HasDryTreasureBuffer(x, y, map, 5))
					return false;

				return true;
			}
			catch
			{
				return false;
			}
		}

		private static bool HasDryTreasureBuffer(int x, int y, Map map, int radius)
		{
			for (int dx = -radius; dx <= radius; ++dx)
			{
				for (int dy = -radius; dy <= radius; ++dy)
				{
					// Check a diamond-shaped neighborhood. This gives a meaningful
					// shoreline buffer without making every candidate excessively
					// expensive to validate.
					if (Math.Abs(dx) + Math.Abs(dy) > radius)
						continue;

					int px = x + dx;
					int py = y + dy;

					if (px < 0 || py < 0 || px >= map.Width || py >= map.Height)
						return false;

					LandTile land = map.Tiles.GetLandTile(px, py);

					if (land.ID <= 2 || land.IsWater() || land.IsCoastline())
						return false;

					int landID = land.ID & TileData.MaxLandValue;

					if (landID < 0 || landID >= TileData.LandTable.Length)
						return false;

					TileFlag flags = TileData.LandTable[landID].Flags;

					if ((flags & TileFlag.Wet) != 0 || (flags & TileFlag.Impassable) != 0)
						return false;

					StaticTile[] statics = map.Tiles.GetStaticTiles(px, py, true);

					for (int i = 0; i < statics.Length; ++i)
					{
						StaticTile tile = statics[i];
						int id = tile.ID & TileData.MaxItemValue;

						if (id < 0 || id >= TileData.ItemTable.Length)
							continue;

						TileFlag f = TileData.ItemTable[id].Flags;

						if ((f & TileFlag.Wet) != 0 || tile.IsWater())
							return false;
					}
				}
			}

			return true;
		}


		public void GetWidthAndHeight(Map map, out int width, out int height)
		{
			if (map == Map.Trammel || map == Map.Felucca || map == Map.NewWolvesbane)
			{
				width = 600;
				height = 600;
			}
			else if (map == Map.TerMur)
			{
				width = 200;
				height = 200;
			}
			else
			{
				width = 300;
				height = 300;
			}
		}

		public void AdjustMap(Map map, out int x2, out int y2, int x1, int y1, int width, int height)
		{
			AdjustMap(map, out x2, out y2, x1, y1, width, height, false);
		}

		public void AdjustMap(Map map, out int x2, out int y2, int x1, int y1, int width, int height, bool eodon)
		{
			x2 = x1 + width;
			y2 = y1 + height;

			if (map == Map.NewWolvesbane)
			{
				if (x2 >= map.Width)
					x2 = map.Width - 1;

				if (y2 >= map.Height)
					y2 = map.Height - 1;

				if (x2 < width)
					x2 = width;

				if (y2 < height)
					y2 = height;
			}
			else if (map == Map.Trammel || map == Map.Felucca)
			{
				if (x2 >= 5120)
					x2 = 5119;

				if (y2 >= 4096)
					y2 = 4095;
			}
			else if (map == Map.Ilshenar)
			{
				if (x2 >= 1890)
					x2 = 1889;

				if (x2 <= 120)
					x2 = 121;

				if (y2 >= 1465)
					y2 = 1464;

				if (y2 <= 105)
					y2 = 106;
			}
			else if (map == Map.Malas)
			{
				if (x2 >= 2522)
					x2 = 2521;

				if (x2 <= 515)
					x2 = 516;

				if (y2 >= 1990)
					y2 = 1989;

				if (y2 <= 0)
					y2 = 1;
			}
			else if (map == Map.Tokuno)
			{
				if (x2 >= 1428)
					x2 = 1427;

				if (x2 <= 0)
					x2 = 1;

				if (y2 >= 1420)
					y2 = 1419;

				if (y2 <= 0)
					y2 = 1;
			}
			else if (map == Map.TerMur)
			{
				if (eodon)
				{
					if (x2 <= 62)
						x2 = 63;

					if (x2 >= 960)
						x2 = 959;

					if (y2 <= 1343)
						y2 = 1344;

					if (y2 >= 2240)
						y2 = 2239;
				}
				else
				{
					if (x2 >= 1271)
						x2 = 1270;

					if (x2 <= 260)
						x2 = 261;

					if (y2 >= 4094)
						y2 = 4083;

					if (y2 <= 2760)
						y2 = 2761;
				}
			}
		}

		public virtual void OnMapComplete(Mobile from, TreasureMapChest chest)
		{
		}

		public virtual void OnChestOpened(Mobile from, TreasureMapChest chest)
		{
		}

		public TreasureMap(Serial serial)
			: base(serial)
		{ }

		public static BaseCreature Spawn(int level, Point3D p, bool guardian, Map map)
		{
			if (!IsAllowedTreasureMap(map))
			{
				Console.WriteLine(
					"[TreasureMap] Refusing guardian spawn on unsupported facet '{0}'.",
					map == null ? "(null)" : map.ToString());
				return null;
			}

			Type[][] spawns;

			if (map == Map.NewWolvesbane)
				spawns = m_NewWolvesbaneSpawnTypes;
			else if (map == Map.Trammel || map == Map.Felucca)
				spawns = m_SpawnTypes;
			else if (map == Map.Tokuno)
				spawns = m_TokunoSpawnTypes;
			else if (map == Map.Ilshenar)
				spawns = m_IlshenarSpawnTypes;
			else if (map == Map.Malas)
				spawns = m_MalasSpawnTypes;
			else // Map.TerMur (including the Eodon subregion)
			{
				if (SpellHelper.IsEodon(map, p))
					spawns = m_EodonSpawnTypes;
				else
					spawns = m_TerMurSpawnTypes;
			}

			if (level >= 0 && level < spawns.Length)
			{
				BaseCreature bc;
				Type[] list = GetSpawnList(spawns, level);

				try
				{
					bc = (BaseCreature)Activator.CreateInstance(list[Utility.Random(list.Length)]);
				}
				catch (Exception e)
				{
					Diagnostics.ExceptionLogging.LogException(e);
					return null;
				}

				bc.Home = p;
				bc.RangeHome = 5;

				if (guardian)
				{
					bc.Title = "(Guardian)";

					if (!TreasureMapInfo.NewSystem && level == 0)
					{
						bc.Name = "a chest guardian";
						bc.Hue = 0x835;
					}

					//if (BaseCreature.IsSoulboundEnemies && !bc.Tamable)
					//{
					//    bc.IsSoulBound = true;
					//}
				}

				return bc;
			}

			return null;
		}

		public static BaseCreature Spawn(int level, Point3D p, Map map, Mobile target, bool guardian)
		{
			if (map == null)
			{
				return null;
			}

			BaseCreature c = Spawn(level, p, guardian, map);

			if (c != null)
			{
				bool spawned = false;

				for (int i = 0; !spawned && i < 10; ++i)
				{
					int x = p.X - 3 + Utility.Random(7);
					int y = p.Y - 3 + Utility.Random(7);

					if (map.CanSpawnMobile(x, y, p.Z))
					{
						c.MoveToWorld(new Point3D(x, y, p.Z), map);
						spawned = true;
					}
					else
					{
						int z = map.GetAverageZ(x, y);

						if (map.CanSpawnMobile(x, y, z))
						{
							c.MoveToWorld(new Point3D(x, y, z), map);
							spawned = true;
						}
					}
				}

				if (!spawned)
				{
					c.Delete();
					return null;
				}

				if (target != null)
				{
					Timer.DelayCall(() => c.Combatant = target);
				}

				return c;
			}

			return null;
		}

		public static Type[] GetSpawnList(Type[][] table, int level)
		{
			Type[] array;

			if (TreasureMapInfo.NewSystem)
			{
				switch (level)
				{
					default: array = table[level + 1]; break;
					case 2:
						List<Type> list1 = new List<Type>();
						list1.AddRange(table[2]);
						list1.AddRange(table[3]);

						array = list1.ToArray();
						break;
					case 3:
						List<Type> list2 = new List<Type>();
						list2.AddRange(table[4]);
						list2.AddRange(table[5]);

						array = list2.ToArray();
						break;
					case 4: array = table[6]; break;
					case 5: array = table[7]; break;
				}
			}
			else
			{
				array = table[level];
			}

			return array;
		}

		public static bool HasDiggingTool(Mobile m)
		{
			if (m.Backpack == null)
			{
				return false;
			}

			List<BaseHarvestTool> items = m.Backpack.FindItemsByType<BaseHarvestTool>();

			foreach (BaseHarvestTool tool in items)
			{
				if (tool.HarvestSystem == Mining.System)
				{
					return true;
				}
			}

			return false;
		}

		public virtual void OnBeginDig(Mobile from)
		{
			if (m_Completed)
			{
				from.SendLocalizedMessage(503028); // The treasure for this map has already been found.
			}
			else if (m_Level == 0 && !CheckYoung(from))
			{
				from.SendLocalizedMessage(1046447); // Only a young player may use this treasure map.
			}
			/*
        else if ( from != m_Decoder )
        {
        from.SendLocalizedMessage( 503016 ); // Only the person who decoded this map may actually dig up the treasure.
        }
        */
			else if (m_Decoder != from && !HasRequiredSkill(from))
			{
				from.SendLocalizedMessage(503031); // You did not decode this map and have no clue where to look for the treasure.
			}
			else if (!from.CanBeginAction(typeof(TreasureMap)))
			{
				from.SendLocalizedMessage(503020); // You are already digging treasure.
			}
			else if (from.Map != Facet)
			{
				from.SendLocalizedMessage(1010479); // You seem to be in the right place, but may be on the wrong facet!
			}
			else
			{
				from.SendLocalizedMessage(503033); // Where do you wish to dig?
				from.Target = new DigTarget(this);
			}
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(GetWorldLocation(), 2))
			{
				from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
				return;
			}

			if (!m_Completed && m_Decoder == null)
			{
				Decode(from);
			}
			else
			{
				DisplayTo(from);
			}
		}

		public virtual void Decode(Mobile from)
		{
			if (m_Completed || m_Decoder != null)
			{
				return;
			}

			if (m_Level == 0)
			{
				if (!CheckYoung(from))
				{
					from.SendLocalizedMessage(1046447); // Only a young player may use this treasure map.
					return;
				}
			}
			else
			{
				double minSkill = GetMinSkillLevel();

				if (from.Skills[SkillName.Cartography].Value < minSkill)
				{
					if (m_Level == 1)
					{
						from.CheckSkill(SkillName.Cartography, 0, minSkill);
					}
					else
					{
						from.SendLocalizedMessage(503013); // The map is too difficult to attempt to decode.
					}
				}

				if (!from.CheckSkill(SkillName.Cartography, minSkill - 10, minSkill + 30))
				{
					from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 503018); // You fail to make anything of the map.
					return;
				}
			}

			from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 503019); // You successfully decode a treasure map!
			Decoder = from;

			LootType = LootType.Blessed;

			DisplayTo(from);
		}

		public void ResetLocation()
		{
			if (!m_Completed)
			{
				ClearPins();
				LootType = LootType.Regular;
				m_Decoder = null;

				// The previous implementation called GetRandomLocation(...) but discarded
				// its return value. That reset the decoder without actually moving the
				// treasure. Rebuild both ChestLocation and the displayed map bounds.
				ResetTreasureLocation(
					TreasureMapInfo.NewSystem && TreasureFacet == TreasureFacet.Eodon);

				InvalidateProperties();
				NextReset = DateTime.UtcNow + ResetTime;
			}
		}

		private void ResetTreasureLocation(bool eodon)
		{
			Map map = Facet;

			if (!IsAllowedTreasureMap(map))
				map = GetRandomMap();

			Facet = map;

			Point2D location = GetRandomLocation(map, eodon);

			if (location == Point2D.Zero)
			{
				Console.WriteLine(
					"[TreasureMap] Reset aborted for map {0}: no valid location could be selected on {1}.",
					Serial,
					map);

				NextReset = DateTime.UtcNow + TimeSpan.FromHours(6.0);
				return;
			}

			ChestLocation = location;

			int width, height;
			GetWidthAndHeight(map, out width, out height);

			int x1 = ChestLocation.X - Utility.RandomMinMax(width / 4, (width / 4) * 3);
			int y1 = ChestLocation.Y - Utility.RandomMinMax(height / 4, (height / 4) * 3);

			if (x1 < 0)
				x1 = 0;

			if (y1 < 0)
				y1 = 0;

			int x2, y2;
			AdjustMap(map, out x2, out y2, x1, y1, width, height, eodon);

			x1 = x2 - width;
			y1 = y2 - height;

			Bounds = new Rectangle2D(x1, y1, width, height);
		}

		public override void DisplayTo(Mobile from)
		{
			if (m_Completed)
			{
				SendLocalizedMessageTo(from, 503014); // This treasure hunt has already been completed.
			}
			else if (m_Level == 0 && !CheckYoung(from))
			{
				from.SendLocalizedMessage(1046447); // Only a young player may use this treasure map.
				return;
			}
			else if (m_Decoder != from && !HasRequiredSkill(from))
			{
				from.SendLocalizedMessage(503031); // You did not decode this map and have no clue where to look for the treasure.
				return;
			}
			else
			{
				SendLocalizedMessageTo(from, 503017); // The treasure is marked by the red pin. Grab a shovel and go dig it up!
			}

			if (Pins.Count == 0)
			{
				AddWorldPin(ChestLocation.X, ChestLocation.Y);
			}

			from.PlaySound(0x249);

			// Custom facet 6 does not have reliable stock cartography artwork in the
			// client. Keep the real ChestLocation/dig mechanics, but use a server-side
			// chart for NewWolvesbane instead of opening a blank client MapDisplay.
			if (Facet == Map.NewWolvesbane)
			{
				from.CloseGump(typeof(Wolvesbane.TreasureMaps.WBNewWolvesbaneTreasureChartGump));
				from.SendGump(new Wolvesbane.TreasureMaps.WBNewWolvesbaneTreasureChartGump(from, this));
				return;
			}

			base.DisplayTo(from);
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);

			if (!m_Completed)
			{
				if (m_Decoder == null)
				{
					list.Add(new DecodeMapEntry(this));
				}
				else
				{
					bool digTool = HasDiggingTool(from);

					list.Add(new OpenMapEntry(this));
					list.Add(new DigEntry(this, digTool));
				}
			}
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			if (TreasureMapInfo.NewSystem)
			{
				list.Add(m_Decoder != null ? 1158980 + (int)TreasureLevel : 1158975 + (int)TreasureLevel, "#" + TreasureMapInfo.PackageLocalization(Package).ToString());
			}
			else
			{
				base.AddNameProperty(list);
			}
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);

			TreasureFacet facet = TreasureMapInfo.GetFacet(ChestLocation, Facet);

			switch (facet)
			{
				case TreasureFacet.Trammel: list.Add(1041503); break;
				case TreasureFacet.Felucca: list.Add(1041502); break;
				case TreasureFacet.Ilshenar: list.Add(1060850); break;
				case TreasureFacet.Malas: list.Add(1060851); break;
				case TreasureFacet.Tokuno: list.Add(1115645); break;
				case TreasureFacet.TerMur: list.Add(1115646); break;
				case TreasureFacet.Eodon: list.Add(1158985); break;
				case TreasureFacet.NewWolvesbane: list.Add("For Somewhere In New Wolvesbane"); break;
			}

			if (m_Completed)
			{
				list.Add(1041507, m_CompletedBy == null ? "someone" : m_CompletedBy.Name); // completed by ~1_val~
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(3);

			writer.Write((int)Package);

			writer.Write(NextReset);

			writer.Write(m_CompletedBy);

			writer.Write(m_Level);
			writer.Write(m_Completed);
			writer.Write(m_Decoder);

			writer.Write(ChestLocation);

			// Never schedule gameplay work from Serialize().
			// World saves may occur repeatedly; the old code created a new reset timer
			// on every save for every expired map, causing reset storms.
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 3:
					{
						Package = (TreasurePackage)reader.ReadInt();
						goto case 2;
					}
				case 2:
					{
						NextReset = reader.ReadDateTime();
						goto case 1;
					}
				case 1:
					{
						m_CompletedBy = reader.ReadMobile();

						goto case 0;
					}
				case 0:
					{
						m_Level = reader.ReadInt();
						m_Completed = reader.ReadBool();
						m_Decoder = reader.ReadMobile();

						if (version == 1)
							Facet = reader.ReadMap();

						ChestLocation = reader.ReadPoint2D();

						if (version == 0 && m_Completed)
						{
							m_CompletedBy = m_Decoder;
						}

						break;
					}
			}

			if (version < 3 && TreasureMapInfo.NewSystem)
			{
				// Versions prior to the package-aware v3 format came from the legacy
				// level model. Convert old saved maps once as they are loaded.
				Level = TreasureMapInfo.ConvertLevel(m_Level);
				AssignRandomPackage();
			}

			if (!IsAllowedTreasureMap(Facet))
			{
				Map oldFacet = Facet;
				Facet = Map.Trammel;

				Console.WriteLine(
					"[TreasureMap] Saved map {0} referenced unsupported facet '{1}'. Migrating it to Trammel.",
					Serial,
					oldFacet == null ? "(null)" : oldFacet.ToString());

				if (!Completed)
					ResetTreasureLocation(false);
			}

			if (m_Decoder != null && LootType == LootType.Regular)
			{
				LootType = LootType.Blessed;
			}

			if (NextReset == DateTime.MinValue)
			{
				NextReset = DateTime.UtcNow + ResetTime;
			}

			if (!Completed && NextReset < DateTime.UtcNow)
			{
				// Schedule exactly once for this load and stagger old maps so a shard
				// with many expired maps does not relocate all of them in one tick.
				Timer.DelayCall(
					TimeSpan.FromSeconds(Utility.RandomMinMax(15, 180)),
					ResetLocation);
			}
		}

		private bool CheckYoung(Mobile from)
		{
			if (from.AccessLevel >= AccessLevel.GameMaster || TreasureMapInfo.NewSystem)
			{
				return true;
			}

			if (from is PlayerMobile && ((PlayerMobile)from).Young)
			{
				return true;
			}

			if (from == Decoder)
			{
				Level = 1;
				from.SendLocalizedMessage(1046446); // This is now a level one treasure map.
				return true;
			}

			return false;
		}

		private double GetMinSkillLevel()
		{
			switch (m_Level)
			{
				case 0:
					return 27;
				case 1:
					return 70;
				case 2:
					return 90;
				case 3:
				case 4:
					return 100.0;

				default:
					return 0.0;
			}
		}

		protected virtual bool HasRequiredSkill(Mobile from)
		{
			return (from.Skills[SkillName.Cartography].Value >= GetMinSkillLevel());
		}

		protected class DigTarget : Target
		{
			private readonly TreasureMap m_Map;

			public DigTarget(TreasureMap map)
				: base(6, true, TargetFlags.None)
			{
				m_Map = map;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (m_Map.Deleted)
				{
					return;
				}

				Map map = m_Map.Facet;

				if (m_Map.m_Completed)
				{
					from.SendLocalizedMessage(503028); // The treasure for this map has already been found.
				}
				/*
            else if ( from != m_Map.m_Decoder )
            {
            from.SendLocalizedMessage( 503016 ); // Only the person who decoded this map may actually dig up the treasure.
            }
            */
				else if (m_Map.m_Decoder != from && !m_Map.HasRequiredSkill(from))
				{
					from.SendLocalizedMessage(503031); // You did not decode this map and have no clue where to look for the treasure.
					return;
				}
				else if (!from.CanBeginAction(typeof(TreasureMap)))
				{
					from.SendLocalizedMessage(503020); // You are already digging treasure.
				}
				else if (!HasDiggingTool(from))
				{
					from.SendMessage("You must have a digging tool to dig for treasure.");
				}
				else if (from.Map != map)
				{
					from.SendLocalizedMessage(1010479); // You seem to be in the right place, but may be on the wrong facet!
				}
				else
				{
					IPoint3D p = targeted as IPoint3D;

					Point3D targ3D;
					if (p is Item)
					{
						targ3D = ((Item)p).GetWorldLocation();
					}
					else
					{
						targ3D = new Point3D(p);
					}

					int maxRange;
					double skillValue = TreasureMapInfo.NewSystem ? from.Skills[SkillName.Cartography].Value : from.Skills[SkillName.Mining].Value;

					if (skillValue >= 100.0)
					{
						maxRange = 4;
					}
					else if (skillValue >= 81.0)
					{
						maxRange = 3;
					}
					else if (skillValue >= 51.0)
					{
						maxRange = 2;
					}
					else
					{
						maxRange = 1;
					}

					Point2D loc = m_Map.ChestLocation;
					int x = loc.X, y = loc.Y;

					Point3D chest3D0 = new Point3D(loc, 0);

					if (Utility.InRange(targ3D, chest3D0, maxRange))
					{
						if (from.Location.X == x && from.Location.Y == y)
						{
							from.SendLocalizedMessage(503030); // The chest can't be dug up because you are standing on top of it.
						}
						else if (map != null)
						{
							int z = map.GetAverageZ(x, y);

							if (!map.CanFit(x, y, z, 16, true, true))
							{
								from.SendLocalizedMessage(503021);
								// You have found the treasure chest but something is keeping it from being dug up.
							}
							else if (from.BeginAction(typeof(TreasureMap)))
							{
								new DigTimer(from, m_Map, new Point3D(x, y, z), map).Start();
							}
							else
							{
								from.SendLocalizedMessage(503020); // You are already digging treasure.
							}
						}
					}
					else if (m_Map.Level > 0)
					{
						if (Utility.InRange(targ3D, chest3D0, 8)) // We're close, but not quite
						{
							from.SendLocalizedMessage(503032); // You dig and dig but no treasure seems to be here.
						}
						else
						{
							from.SendLocalizedMessage(503035); // You dig and dig but fail to find any treasure.
						}
					}
					else
					{
						if (Utility.InRange(targ3D, chest3D0, 8)) // We're close, but not quite
						{
							from.SendAsciiMessage(0x44, "The treasure chest is very close!");
						}
						else
						{
							Direction dir = Utility.GetDirection(targ3D, chest3D0);

							string sDir;
							switch (dir)
							{
								case Direction.North:
									sDir = "north";
									break;
								case Direction.Right:
									sDir = "northeast";
									break;
								case Direction.East:
									sDir = "east";
									break;
								case Direction.Down:
									sDir = "southeast";
									break;
								case Direction.South:
									sDir = "south";
									break;
								case Direction.Left:
									sDir = "southwest";
									break;
								case Direction.West:
									sDir = "west";
									break;
								default:
									sDir = "northwest";
									break;
							}

							from.SendAsciiMessage(0x44, "Try looking for the treasure chest more to the {0}.", sDir);
						}
					}
				}
			}
		}

		private class DigTimer : Timer
		{
			private readonly Mobile m_From;
			private readonly TreasureMap m_TreasureMap;
			private readonly Map m_Map;
			private readonly long m_NextSkillTime;
			private readonly long m_NextSpellTime;
			private readonly long m_NextActionTime;
			private readonly long m_LastMoveTime;
			private TreasureChestDirt m_Dirt1;
			private TreasureChestDirt m_Dirt2;
			private TreasureMapChest m_Chest;
			private int m_Count;

			public Point3D ChestLocation { get; private set; }

			public DigTimer(Mobile from, TreasureMap treasureMap, Point3D location, Map map)
				: base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
			{
				m_From = from;
				m_TreasureMap = treasureMap;

				ChestLocation = location;
				m_Map = map;

				m_NextSkillTime = from.NextSkillTime;
				m_NextSpellTime = from.NextSpellTime;
				m_NextActionTime = from.NextActionTime;
				m_LastMoveTime = from.LastMoveTime;

				Priority = TimerPriority.TenMS;
			}

			protected override void OnTick()
			{
				if (m_NextSkillTime != m_From.NextSkillTime || (!TreasureMapInfo.NewSystem && m_NextSpellTime != m_From.NextSpellTime) ||
					m_NextActionTime != m_From.NextActionTime)
				{
					Terminate();
					return;
				}

				if (m_LastMoveTime != m_From.LastMoveTime)
				{
					m_From.SendLocalizedMessage(503023);
					// You cannot move around while digging up treasure. You will need to start digging anew.
					Terminate();
					return;
				}

				int z = (m_Chest != null) ? m_Chest.Z + m_Chest.ItemData.Height : int.MinValue;
				int height = 16;

				if (z > ChestLocation.Z)
				{
					height -= (z - ChestLocation.Z);
				}
				else
				{
					z = ChestLocation.Z;
				}

				if (!m_Map.CanFit(ChestLocation.X, ChestLocation.Y, z, height, true, true, false))
				{
					m_From.SendLocalizedMessage(503024);
					// You stop digging because something is directly on top of the treasure chest.
					Terminate();
					return;
				}

				m_Count++;

				m_From.RevealingAction();
				m_From.Direction = m_From.GetDirectionTo(ChestLocation);

				if (m_Count > 1 && m_Dirt1 == null)
				{
					m_Dirt1 = new TreasureChestDirt();
					m_Dirt1.MoveToWorld(ChestLocation, m_Map);

					m_Dirt2 = new TreasureChestDirt();
					m_Dirt2.MoveToWorld(new Point3D(ChestLocation.X, ChestLocation.Y - 1, ChestLocation.Z), m_Map);
				}

				if (m_Count == 5)
				{
					m_Dirt1.Turn1();
				}
				else if (m_Count == 10)
				{
					m_Dirt1.Turn2();
					m_Dirt2.Turn2();
				}
				else if (m_Count > 10)
				{
					if (m_Chest == null)
					{
						m_Chest = new TreasureMapChest(m_From, m_TreasureMap.Level, true);

						if (TreasureMapInfo.NewSystem)
						{
							m_TreasureMap.AssignChestQuality(m_From, m_Chest);
						}

						m_Chest.MoveToWorld(new Point3D(ChestLocation.X, ChestLocation.Y, ChestLocation.Z - 15), m_Map);
					}
					else
					{
						m_Chest.Z++;
					}

					Effects.PlaySound(m_Chest, m_Map, 0x33B);
				}

				if (m_Chest != null && m_Chest.Location.Z >= ChestLocation.Z)
				{
					Stop();
					m_From.EndAction(typeof(TreasureMap));

					m_Chest.Temporary = false;
					m_Chest.TreasureMap = m_TreasureMap;
					m_Chest.DigTime = DateTime.UtcNow;
					m_TreasureMap.Completed = true;
					m_TreasureMap.CompletedBy = m_From;

					if (TreasureMapInfo.NewSystem)
					{
						TreasureMapInfo.Fill(m_From, m_Chest, m_TreasureMap);
					}
					//else
					//{
					//    TreasureMapChest.Fill(m_From, m_Chest, m_Chest.Level, false);
					//}

					m_TreasureMap.OnMapComplete(m_From, m_Chest);

					int spawns;
					switch (m_TreasureMap.Level)
					{
						case 0:
							spawns = TreasureMapInfo.NewSystem ? 4 : 3;
							break;
						case 1:
							spawns = TreasureMapInfo.NewSystem ? 4 : 0;
							break;
						default:
							spawns = 4;
							break;
					}

					for (int i = 0; i < spawns; ++i)
					{
						bool guardian = TreasureMapInfo.NewSystem ? Utility.RandomDouble() >= 0.3 : true;

						BaseCreature bc = Spawn(m_TreasureMap.Level, m_Chest.Location, m_Chest.Map, null, guardian);

						if (bc != null && guardian)
						{
							bc.Hue = 2725;
							m_Chest.Guardians.Add(bc);
						}
					}
				}
				else
				{
					if (m_From.Body.IsHuman && !m_From.Mounted)
					{
						m_From.Animate(AnimationType.Attack, 3);
					}

					new SoundTimer(m_From, 0x125 + (m_Count % 2)).Start();
				}
			}

			private void Terminate()
			{
				Stop();
				m_From.EndAction(typeof(TreasureMap));

				if (m_Chest != null)
				{
					m_Chest.Delete();
				}

				if (m_Dirt1 != null)
				{
					m_Dirt1.Delete();
					m_Dirt2.Delete();
				}
			}

			private class SoundTimer : Timer
			{
				private readonly Mobile m_From;
				private readonly int m_SoundID;

				public SoundTimer(Mobile from, int soundID)
					: base(TimeSpan.FromSeconds(0.9))
				{
					m_From = from;
					m_SoundID = soundID;

					Priority = TimerPriority.TenMS;
				}

				protected override void OnTick()
				{
					m_From.PlaySound(m_SoundID);
				}
			}
		}

		private class DecodeMapEntry : ContextMenuEntry
		{
			private readonly TreasureMap m_Map;

			public DecodeMapEntry(TreasureMap map)
				: base(6147, 2)
			{
				m_Map = map;
			}

			public override void OnClick()
			{
				if (!m_Map.Deleted)
				{
					m_Map.Decode(Owner.From);
				}
			}
		}

		private class OpenMapEntry : ContextMenuEntry
		{
			private readonly TreasureMap m_Map;

			public OpenMapEntry(TreasureMap map)
				: base(6150, 2)
			{
				m_Map = map;
			}

			public override void OnClick()
			{
				if (!m_Map.Deleted)
				{
					m_Map.DisplayTo(Owner.From);
				}
			}
		}

		private class DigEntry : ContextMenuEntry
		{
			private readonly TreasureMap m_Map;

			public DigEntry(TreasureMap map, bool enabled)
				: base(6148, 2)
			{
				m_Map = map;

				if (!enabled)
				{
					Flags |= CMEFlags.Disabled;
				}
			}

			public override void OnClick()
			{
				if (m_Map.Deleted)
				{
					return;
				}

				Mobile from = Owner.From;

				if (HasDiggingTool(from))
				{
					m_Map.OnBeginDig(from);
				}
				else
				{
					from.SendMessage("You must have a digging tool to dig for treasure.");
				}
			}
		}
	}

	public class TreasureChestDirt : Item
	{
		public TreasureChestDirt()
			: base(0x912)
		{
			Movable = false;

			Timer.DelayCall(TimeSpan.FromMinutes(2.0), Delete);
		}

		public TreasureChestDirt(Serial serial)
			: base(serial)
		{ }

		public void Turn1()
		{
			ItemID = 0x913;
		}

		public void Turn2()
		{
			ItemID = 0x914;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();

			Delete();
		}
	}
}
