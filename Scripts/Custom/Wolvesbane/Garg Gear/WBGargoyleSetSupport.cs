using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    /*
     * Shared support for the individually split Wolvesbane Gargoyle gear files.
     * Keep this file installed alongside the individual gear scripts.
     * It contains set detection and shared per-piece stat helpers only.
     */

    public static class WBGargoyleSetHelper
    {
        public static void QueueCheck(Mobile m)
        {
            if (m == null || m.Deleted)
                return;

            Timer.DelayCall(TimeSpan.Zero, delegate { Check(m); });
        }

        public static void Check(Mobile m)
        {
            if (m == null || m.Deleted)
                return;

            CheckObsidian(m);
            CheckAeternum(m);
        }

        private static bool Has<T>(Mobile m) where T : Item
        {
            if (m == null)
                return false;

            for (int i = 0; i < m.Items.Count; ++i)
            {
                if (m.Items[i] is T)
                    return true;
            }

            return false;
        }

        private static T Find<T>(Mobile m) where T : Item
        {
            if (m == null)
                return null;

            for (int i = 0; i < m.Items.Count; ++i)
            {
                T item = m.Items[i] as T;

                if (item != null)
                    return item;
            }

            return null;
        }

        private static void CheckObsidian(Mobile m)
        {
            ObsidianWingChest chest = Find<ObsidianWingChest>(m);

            bool full = chest != null &&
                        Has<ObsidianWingArms>(m) &&
                        Has<ObsidianWingLegs>(m) &&
                        Has<ObsidianWingKilt>(m) &&
                        Has<ObsidianWingArmor>(m);

            if (chest == null)
                return;

            if (full)
            {
                if (!chest.FullSetBonusActive)
                    chest.ApplyFullSetBonus(m);
                else
                    chest.EnsureSkillMods(m);
            }
            else if (chest.FullSetBonusActive)
            {
                chest.RemoveFullSetBonus(m);
            }
        }

        private static void CheckAeternum(Mobile m)
        {
            AeternumStoneChest chest = Find<AeternumStoneChest>(m);

            bool full = chest != null &&
                        Has<AeternumStoneArms>(m) &&
                        Has<AeternumStoneLegs>(m) &&
                        Has<AeternumStoneKilt>(m) &&
                        Has<AeternumStoneWingArmor>(m);

            if (chest == null)
                return;

            if (full)
            {
                if (!chest.FullSetBonusActive)
                    chest.ApplyFullSetBonus(m);
                else
                {
                    chest.EnsureSkillMods(m);
                    chest.EnsureProcTimer();
                }
            }
            else if (chest.FullSetBonusActive)
            {
                chest.RemoveFullSetBonus(m);
            }
        }
    }

    public static class WBObsidianPieceStats
    {
        public static void Apply(BaseArmor armor)
        {
            armor.Attributes.BonusStr = 111;
            armor.Attributes.BonusDex = 111;
            armor.Attributes.BonusInt = 111;
            armor.Attributes.NightSight = 1;
            armor.Attributes.RegenHits = 3;
            armor.Attributes.RegenMana = 3;
            armor.Attributes.RegenStam = 3;
            armor.Attributes.SpellDamage = 839;
            armor.Attributes.ReflectPhysical = 360;
            armor.Attributes.AttackChance = 360;
            armor.Attributes.DefendChance = 360;
            armor.Attributes.BonusHits = 360;
            armor.Attributes.BonusMana = 360;
            armor.Attributes.BonusStam = 360;
            armor.Attributes.Luck = 839;
            armor.Attributes.CastRecovery = 3;
            armor.Attributes.CastSpeed = 3;
            armor.Attributes.WeaponDamage = 360;
            armor.SkillBonuses.SetValues(0, SkillName.Throwing, 10);
            armor.SkillBonuses.SetValues(1, SkillName.Chivalry, 10);
            armor.SkillBonuses.SetValues(2, SkillName.Tactics, 10);
        }
    }

    public static class WBAeternumPieceStats
    {
        public static void Apply(BaseArmor armor)
        {
            armor.Attributes.BonusStr = 158;
            armor.Attributes.BonusDex = 158;
            armor.Attributes.BonusInt = 158;
            armor.Attributes.NightSight = 1;
            armor.Attributes.RegenHits = 4;
            armor.Attributes.RegenMana = 4;
            armor.Attributes.RegenStam = 4;
            armor.Attributes.SpellDamage = 1189;
            armor.Attributes.ReflectPhysical = 510;
            armor.Attributes.AttackChance = 510;
            armor.Attributes.DefendChance = 510;
            armor.Attributes.BonusHits = 510;
            armor.Attributes.BonusMana = 510;
            armor.Attributes.BonusStam = 510;
            armor.Attributes.Luck = 1189;
            armor.Attributes.CastRecovery = 4;
            armor.Attributes.CastSpeed = 4;
            armor.Attributes.WeaponDamage = 510;
            armor.SkillBonuses.SetValues(0, SkillName.Throwing, 15);
            armor.SkillBonuses.SetValues(1, SkillName.Chivalry, 15);
            armor.SkillBonuses.SetValues(2, SkillName.Tactics, 15);
        }
    }
}
