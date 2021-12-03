//Crafted By ReApEr
using System;
using System.Collections.Generic;
using Server.Engines.CannedEvil;
using Server.Items;
using Server.Mobiles;
using Server.Services.Virtues;

namespace Server.Mobiles
{
    public abstract class BaseMiniChampion : BaseCreature
    {
        public BaseMiniChampion(AIType aiType)
            : this(aiType, FightMode.Closest)
        {
        }

        public BaseMiniChampion(AIType aiType, FightMode mode)
            : base(aiType, mode, 18, 1, 0.1, 0.2)
        {
        }

        public BaseMiniChampion(Serial serial)
            : base(serial)
        {
        }
		public override bool CanBeParagon { get { return false; } }
        public virtual bool NoGoodies
        {
            get
            {
                return false;
            }
        }
        public static void GivePowerScrollTo(Mobile m)
        {
            if (m == null)	//sanity
                return;

			PowerScroll ps = CreateRandomPowerScroll();

            m.SendLocalizedMessage(1049524); // You have received a scroll of power!

            if (!Core.SE || m.Alive)
                m.AddToBackpack(ps);
            else
            {
                if (m.Corpse != null && !m.Corpse.Deleted)
                    m.Corpse.DropItem(ps);
                else
                    m.AddToBackpack(ps);
            }

            if (m is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)m;

                for (int j = 0; j < pm.JusticeProtectors.Count; ++j)
                {
                    Mobile prot = pm.JusticeProtectors[j];

                    if (prot.Map != m.Map || prot.Kills >= 5 || prot.Criminal || !JusticeVirtue.CheckMapRegion(m, prot))
                        continue;

                    int chance = 0;

                    switch( VirtueHelper.GetLevel(prot, VirtueName.Justice) )
                    {
                        case VirtueLevel.Seeker:
                            chance = 60;
                            break;
                        case VirtueLevel.Follower:
                            chance = 80;
                            break;
                        case VirtueLevel.Knight:
                            chance = 100;
                            break;
                    }

                    if (chance > Utility.Random(100))
                    {
						PowerScroll powerScroll = CreateRandomPowerScroll();

                        prot.SendLocalizedMessage(1049368); // You have been rewarded for your dedication to Justice!

                        if (!Core.SE || prot.Alive)
                            prot.AddToBackpack(CreateRandomPowerScroll());
                        else
                        {
                            if (prot.Corpse != null && !prot.Corpse.Deleted)
                                prot.Corpse.DropItem(powerScroll);
                            else
                                prot.AddToBackpack(CreateRandomPowerScroll());
                        }
                    }
                }
            }
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

        public void GivePowerScrolls()
        {
            if (this.Map != Map.Trammel)
                return;

            List<Mobile> toGive = new List<Mobile>();
            List<DamageStore> rights = GetLootingRights();

            for (int i = rights.Count - 1; i >= 0; --i)
            {
                DamageStore ds = rights[i];

                if (ds.m_HasRight)
                    toGive.Add(ds.m_Mobile);
            }

            if (toGive.Count == 0)
                return;

            for (int i = 0; i < toGive.Count; i++)
            {
                Mobile m = toGive[i];

                if (!(m is PlayerMobile))
                    continue;

                bool gainedPath = false;

                int pointsToGain = 400;

                if (VirtueHelper.Award(m, VirtueName.Valor, pointsToGain, ref gainedPath))
                {
                    if (gainedPath)
                        m.SendLocalizedMessage(1054032); // You have gained a path in Valor!
                    else
                        m.SendLocalizedMessage(1054030); // You have gained in Valor!
                    //No delay on Valor gains
                }
            }

            // Randomize
            for (int i = 0; i < toGive.Count; ++i)
            {
                int rand = Utility.Random(toGive.Count);
                Mobile hold = toGive[i];
                toGive[i] = toGive[rand];
                toGive[rand] = hold;
            }

            for (int i = 0; i < MiniChampionSystem.PowerScrollAmount; ++i)
            {
                Mobile m = toGive[i % toGive.Count];

                GivePowerScrollTo(m);
            }
        }

        public override bool OnBeforeDeath()
        {
            if (!this.NoKillAwards)
            {
                this.GivePowerScrolls();

                if (this.NoGoodies)
                    return base.OnBeforeDeath();

				GoldShower.DoForChamp(Location, Map);
            }

            return base.OnBeforeDeath();
        }

        private static PowerScroll CreateRandomPowerScroll()
        {
            int level;
            double random = Utility.RandomDouble();


            if (0.1 >= random)
                level = 10;
            else if (0.2 >= random)
                level = 10;
            else if (0.4 >= random)
                level = 5;
            else
                level = 5;

            return PowerScroll.CreateRandom(level, level);
        }

    }
}