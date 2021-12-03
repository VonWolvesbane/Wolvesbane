using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.Harvest
{
    public class FireRockMining : HarvestSystem
    {
        private static FireRockMining m_System;

        public static FireRockMining System
        {
            get
            {
                if (m_System == null)
                    m_System = new FireRockMining();

                return m_System;
            }
        }

        private HarvestDefinition m_FireRock;

        public HarvestDefinition FireRock
        {
            get { return m_FireRock; }
        }

        private FireRockMining()
        {
            HarvestResource[] res;
            HarvestVein[] veins;

            #region Mining for FireRock
            HarvestDefinition FireRock = m_FireRock = new HarvestDefinition();

            // Resource banks are every 8x8 tiles
            FireRock.BankWidth = 8;
            FireRock.BankHeight = 8;

            // Every bank holds from 6 to 12 FireRock
            FireRock.MinTotal = 5;
            FireRock.MaxTotal = 8;

            // A resource bank will respawn its content every 10 to 20 minutes
            FireRock.MinRespawn = TimeSpan.FromMinutes(10.0);
            FireRock.MaxRespawn = TimeSpan.FromMinutes(20.0);

            // Skill checking is done on the Mining skill
            FireRock.Skill = SkillName.Mining;

            // Set the list of harvestable tiles
            FireRock.Tiles = m_LavaTiles;

            // Players must be within 2 tiles to harvest
            FireRock.MaxRange = 2;

            // One FireRock per harvest action
            FireRock.ConsumedPerHarvest = 1;
            FireRock.ConsumedPerFeluccaHarvest = 2;
            //FireRock.ConsumedPerIlshenarHarvest = 2;
            //FireRock.ConsumedPerTerMurHarvest = 3;

            // The digging effect
            FireRock.EffectActions = new int[] { 11 };
            FireRock.EffectSounds = new int[] { 0x125, 0x126 };
            FireRock.EffectCounts = new int[] { 1 };
            FireRock.EffectDelay = TimeSpan.FromSeconds(1.6);
            FireRock.EffectSoundDelay = TimeSpan.FromSeconds(0.9);

            FireRock.NoResourcesMessage = "There is no FireRock here to mine."; // There is no FireRock here to mine.
            FireRock.DoubleHarvestMessage = "There is no FireRock here to mine."; // There is no FireRock here to mine.
            FireRock.TimedOutOfRangeMessage = "You have moved too far away to continue mining."; // You have moved too far away to continue mining.
            FireRock.OutOfRangeMessage = "That is too far away."; // That is too far away.
            FireRock.FailMessage = "You dig for a while but fail to find any FireRock."; // You dig for a while but fail to find any FireRock.
            FireRock.PackFullMessage = "Your backpack can't hold the FireRock, and it is lost!"; // Your backpack can't hold the FireRock, and it is lost!
            FireRock.ToolBrokeMessage = 1044038; // You have worn out your tool!

            res = new HarvestResource[]
				{
                    new HarvestResource( 79.0, 59.0, 100.0, "You dig a SmallFireRock and put it in your backpack", typeof (SmallFireRock)),
                    new HarvestResource( 89.0, 69.0, 105.0, "You dig some LargeFireRock and put it in your backpack", typeof (LargeFireRock)),
                    new HarvestResource( 99.0, 79.0, 115.0, "You dig some CrystalineFire and put it in your backpack", typeof (CrystalineFire), typeof( FireRockElemental))

				};

            veins = new HarvestVein[]
				{
					new HarvestVein( 74.7, 0.0, res[0], null   ), // SmallFireRock
                    new HarvestVein( 15.3, 0.5, res[1], res[0] ), // LargeFireRock
					new HarvestVein( 10.0, 0.5, res[2], res[0] ), // CrystalineFire

				};

            FireRock.Resources = res;
            FireRock.Veins = veins;

            Definitions.Add(FireRock);
            #endregion
        }

        public override Type GetResourceType(Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource)
        {
            if (def == this.m_FireRock)
            {				
                //PlayerMobile pm = from as PlayerMobile;
				PlayerMobile pm = null;

					if ( from is PlayerMobile )
						pm = from as PlayerMobile;
					else if ( from is BaseCreature )
						pm = ( (BaseCreature) from ).ControlMaster as PlayerMobile;

                return resource.Types[0];
            }

            return base.GetResourceType(from, tool, def, map, loc, resource);
        }

        public override bool CheckResources(Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, bool timed)
        {
            if (HarvestMap.CheckMapOnHarvest(from, loc, def) == null)
                return base.CheckResources(from, tool, def, map, loc, timed);

            return true;
        }

        public override bool CheckHarvest(Mobile from, Item tool)
        {
            if (!base.CheckHarvest(from, tool))
                return false;

            if (from.Mounted)
            {
                from.SendLocalizedMessage(501864); // You can't mine while riding.
                return false;
            }
            else if (from.IsBodyMod && !from.Body.IsHuman)
            {
                from.SendLocalizedMessage(501865); // You can't mine while polymorphed.
                return false;
            }

            return true;
        }

        public override bool CheckHarvest(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            if (!base.CheckHarvest(from, tool, def, toHarvest))
                return false;

            if (def == this.m_FireRock && !(from is PlayerMobile && from.Skills[SkillName.Mining].Base >= 100.0 ))
            {
                this.OnBadHarvestTarget(from, tool, toHarvest);
                return false;
            }
            else if (from.Mounted)
            {
                from.SendLocalizedMessage(501864); // You can't mine while riding.
                return false;
            }
            else if (from.IsBodyMod && !from.Body.IsHuman)
            {
                from.SendLocalizedMessage(501865); // You can't mine while polymorphed.
                return false;
            }

            return true;
        }

        public override HarvestVein MutateVein(Mobile from, Item tool, HarvestDefinition def, HarvestBank bank, object toHarvest, HarvestVein vein)
        {
            if (tool is GargoyleFirePick && def == this.m_FireRock)
            {
                int veinIndex = Array.IndexOf(def.Veins, vein);

                if (veinIndex >= 0 && veinIndex < (def.Veins.Length - 1))
                    return def.Veins[veinIndex + 1];
            }

            return base.MutateVein(from, tool, def, bank, toHarvest, vein);
        }

        private static readonly int[] m_Offsets = new int[]
        {
            -1, -1,
            -1, 0,
            -1, 1,
            0, -1,
            0, 1,
            1, -1,
            1, 0,
            1, 1
        };

        private static Type[] m_Types = new Type[]
		{
			typeof(FireRockElemental ),  // put your critters in there
            typeof(FireRockElemental )

		};
        public override void OnHarvestFinished(Mobile from, Item tool, HarvestDefinition def, HarvestVein vein, HarvestBank bank, HarvestResource resource, object harvested)
        {
            if (tool is GargoyleFirePick && def == m_FireRock && 0.1 > Utility.RandomDouble() && HarvestMap.CheckMapOnHarvest(from, harvested, def) == null)
            {
                try
                {
                    Type tospawn = (m_Types[Utility.Random(m_Types.Length)]);
                    Map map = from.Map;
                    if (map == null) return;
                    BaseCreature spawned = (FireRockElemental)Activator.CreateInstance(tospawn); // put your critter in there
                    if (spawned != null)
                    {
                        int offset = Utility.Random(8) * 2;
                        for (int i = 0; i < m_Offsets.Length; i += 2)
                        {
                            int x = from.X + m_Offsets[(offset + i) % m_Offsets.Length];
                            int y = from.Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];
                            if (map.CanSpawnMobile(x, y, from.Z))
                            {
                                spawned.MoveToWorld(new Point3D(x, y, from.Z), map);
                                spawned.Combatant = from;
                                return;
                            }
                            else
                            {
                                int z = map.GetAverageZ(x, y);
                                if (map.CanSpawnMobile(x, y, z))
                                {
                                    spawned.MoveToWorld(new Point3D(x, y, z), map);
                                    spawned.Combatant = from;
                                    return;
                                }
                            }
                        }
                        spawned.MoveToWorld(from.Location, from.Map);
                        spawned.Combatant = from;
                    }
                }
                catch { }
            }
        }

        public override bool BeginHarvesting(Mobile from, Item tool)
        {
            if (!base.BeginHarvesting(from, tool))
                return false;

            from.SendLocalizedMessage(503033); // Where do you wish to dig?
            return true;
        }

        public override void OnHarvestStarted(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            base.OnHarvestStarted(from, tool, def, toHarvest);

            if (Core.ML)
                from.RevealingAction();
        }

        public override void OnBadHarvestTarget(Mobile from, Item tool, object toHarvest)
        {
            if (toHarvest is LandTarget)
                from.SendLocalizedMessage(501862); // You can't mine there.
            else
                from.SendLocalizedMessage(501863); // You can't mine that.
        }

        #region Tile lists
        public static int[] LavaTiles { get { return m_LavaTiles; } }
        private static int[] m_LavaTiles = new int[]
                    {                
                 0x1F4, 0x1F5, 0x1F6, 0x1F7, 

                 0x12EE+0x3FFF, 0x12EF+0x3FFF, 0x12F0+0x3FFF, 0x12F1+0x3FFF, 0x12F2+0x3FFF, 0x12F4+0x3FFF, 0x12F5+0x3FFF, 
                 0x12F6+0x3FFF, 0x12F7+0x3FFF, 0x12F8+0x3FFF, 0x12F9+0x3FFF, 0x12FA+0x3FFF, 0x12FB+0x3FFF, 0x12FC+0x3FFF,
                 0x12FD+0x3FFF, 0x12FE+0x3FFF, 0x1300+0x3FFF, 0x1301+0x3FFF, 0x1302+0x3FFF, 0x1304+0x3FFF, 0x1306+0x3FFF,
                 0x1307+0x3FFF, 0x1308+0x3FFF, 0x1309+0x3FFF, 0x130A+0x3FFF, 0x130C+0x3FFF, 0x130D+0x3FFF, 0x130E+0x3FFF,
                 0x130F+0x3FFF, 0x1310+0x3FFF, 0x1312+0x3FFF, 0x1313+0x3FFF, 0x1314+0x3FFF, 0x1315+0x3FFF, 0x1316+0x3FFF,
                 0x1318+0x3FFF, 0x1319+0x3FFF, 0x131A+0x3FFF, 0x131B+0x3FFF, 0x131C+0x3FFF, 0x131E+0x3FFF, 0x131F+0x3FFF,
                 0x1320+0x3FFF, 0x1321+0x3FFF, 0x1322+0x3FFF, 0x1323+0x3FFF, 0x1324+0x3FFF, 0x1325+0x3FFF, 0x1326+0x3FFF,
                 0x1327+0x3FFF, 0x1328+0x3FFF, 0x1329+0x3FFF, 0x132A+0x3FFF, 0x132B+0x3FFF, 0x132C+0x3FFF, 0x132D+0x3FFF,
                 0x312E+0x3FFF, 0x132F+0x3FFF, 0x1330+0x3FFF, 0x1331+0x3FFF, 0x1332+0x3FFF, 0x1333+0x3FFF, 0x1334+0x3FFF,
                 0x1335+0x3FFF, 0x1336+0x3FFF, 0x1337+0x3FFF, 0x1338+0x3FFF, 0x1339+0x3FFF, 0x133A+0x3FFF, 0x133B+0x3FFF,
                 0x133C+0x3FFF, 0x133D+0x3FFF, 0x133E+0x3FFF, 0x133F+0x3FFF, 0x1340+0x3FFF, 0x1341+0x3FFF, 0x1342+0x3FFF,
                 0x1343+0x3FFF, 0x1344+0x3FFF, 0x1345+0x3FFF, 0x1346+0x3FFF, 0x1347+0x3FFF, 0x1348+0x3FFF, 0x1349+0x3FFF,
                 0x134A+0x3FFF, 0x134B+0x3FFF, 0x134C+0x3FFF, 0x134D+0x3FFF, 0x136E+0x3FFF, 0x137E+0x3FFF, 0x1380+0x3FFF,
                 0x1382+0x3FFF, 0x3286+0x3FFF, 0x3287+0x3FFF, 0x3288+0x3FFF, 0x3289+0x3FFF, 0x328B+0x3FFF, 0x328C+0x3FFF,
                 0x328D+0x3FFF, 0x328E+0x3FFF, 0x328F+0x3FFF, 0x3290+0x3FFF, 0x3291+0x3FFF, 0x3292+0x3FFF, 0x3293+0x3FFF,
                 0x3294+0x3FFF, 0x3295+0x3FFF, 0x3296+0x3FFF, 0x3297+0x3FFF, 0x3298+0x3FFF, 0x3299+0x3FFF, 0x329A+0x3FFF,
                 0x329B+0x3FFF, 0x329C+0x3FFF, 0x329D+0x3FFF, 0x329E+0x3FFF, 0x329F+0x3FFF, 0x32A0+0x3FFF, 0x32A1+0x3FFF,
                 0x32A2+0x3FFF, 0x32A3+0x3FFF, 0x32A4+0x3FFF, 0x32A5+0x3FFF, 0x32A6+0x3FFF, 0x32A7+0x3FFF, 0x32A8+0x3FFF,
                 0x32A9+0x3FFF, 0x32AA+0x3FFF, 0x32AB+0x3FFF, 0x32AC+0x3FFF, 0x32AD+0x3FFF, 0x32AE+0x3FFF, 0x32AF+0x3FFF,
                 0x32B0+0x3FFF, 0x32B1+0x3FFF, 0x343B+0x3FFF, 0x343C+0x3FFF, 0x343D+0x3FFF, 0x343E+0x3FFF, 0x343F+0x3FFF,
                 0x3440+0x3FFF, 0x3441+0x3FFF, 0x3442+0x3FFF, 0x3443+0x3FFF, 0x3444+0x3FFF, 0x3445+0x3FFF, 0x3446+0x3FFF,
                 0x3447+0x3FFF, 0x3448+0x3FFF, 0x3449+0x3FFF, 0x344A+0x3FFF, 0x344B+0x3FFF, 0x344C+0x3FFF, 0x344D+0x3FFF,
                 0x344E+0x3FFF, 0x344F+0x3FFF, 0x3450+0x3FFF, 0x3451+0x3FFF, 0x3452+0x3FFF, 0x3453+0x3FFF, 0x3454+0x3FFF,
                 0x3455+0x3FFF, 0x3456+0x3FFF, 0x3457+0x3FFF, 0x3462+0x3FFF, 0x3463+0x3FFF, 0x3464+0x3FFF, 0x3465+0x3FFF,
                 0x3466+0x3FFF, 0x3468+0x3FFF, 0x3469+0x3FFF, 0x346A+0x3FFF, 0x346B+0x3FFF, 0x346C+0x3FFF, 0x3547+0x3FFF,
                 0x3548+0x3FFF, 0x3549+0x3FFF, 0x354A+0x3FFF, 0x354B+0x3FFF, 0x354C+0x3FFF 
            };
        #endregion
    }
}
