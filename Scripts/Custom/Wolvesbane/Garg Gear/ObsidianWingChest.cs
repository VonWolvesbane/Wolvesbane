using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    // Split from Wolvesbane_Gargoyle_Gear_v3.cs: ObsidianWingChest

    public class ObsidianWingChest : GargishPlateChest
    {
        private bool m_FullSetBonusActive;
        private SkillMod m_ThrowingMod;
        private SkillMod m_ChivalryMod;
        private SkillMod m_TacticsMod;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool FullSetBonusActive { get { return m_FullSetBonusActive; } }

        public override int ArtifactRarity { get { return 500; } }
        public override bool IsArtifact { get { return true; } }
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public ObsidianWingChest()
        {
            Name = "Obsidian Wing Chest";
            Hue = 1175;

            ApplyBaseStats();
        }

        private void ApplyBaseStats()
        {
            Attributes.BonusStr = 111;
            Attributes.BonusDex = 111;
            Attributes.BonusInt = 111;
            Attributes.NightSight = 1;

            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.RegenStam = 3;
            Attributes.SpellDamage = 839;
            Attributes.ReflectPhysical = 360;
            Attributes.AttackChance = 360;
            Attributes.DefendChance = 360;
            Attributes.BonusHits = 360;
            Attributes.BonusMana = 360;
            Attributes.BonusStam = 360;
            Attributes.Luck = 839;
            Attributes.CastRecovery = 3;
            Attributes.CastSpeed = 3;
            Attributes.WeaponDamage = 360;

            PhysicalBonus = 6;
            ArmorAttributes.MageArmor = 1;
            ArmorAttributes.SelfRepair = 7;

            SkillBonuses.SetValues(0, SkillName.Throwing, 10);
            SkillBonuses.SetValues(1, SkillName.Chivalry, 10);
            SkillBonuses.SetValues(2, SkillName.Tactics, 10);
        }

        public void ApplyFullSetBonus(Mobile m)
        {
            if (m_FullSetBonusActive)
                return;

            m_FullSetBonusActive = true;

            // Completes the set to approximately 70% of Crusader wearable totals.
            Attributes.BonusStr += 94;
            Attributes.BonusDex += 94;
            Attributes.BonusInt += 94;

            Attributes.RegenHits += 4;
            Attributes.RegenMana += 4;
            Attributes.RegenStam += 4;

            Attributes.SpellDamage += 700;
            Attributes.ReflectPhysical += 298;
            Attributes.AttackChance += 298;
            Attributes.DefendChance += 298;
            Attributes.BonusHits += 298;
            Attributes.BonusMana += 298;
            Attributes.BonusStam += 298;
            Attributes.Luck += 700;
            Attributes.CastRecovery += 4;
            Attributes.CastSpeed += 4;
            Attributes.WeaponDamage += 298;

            PhysicalBonus += 11;

            EnsureSkillMods(m);
            InvalidateProperties();

            if (m != null)
            {
                m.SendMessage(0x48, "The power of the Obsidian Wing set surrounds you.");
                m.FixedEffect(0x376A, 9, 32);
                m.UpdateResistances();
            }
        }

        public void EnsureSkillMods(Mobile m)
        {
            if (!m_FullSetBonusActive || m == null || m.Deleted)
                return;

            // Five pieces provide +50; these mods bring the complete set to +63,
            // which is 70% of the Crusader set's +90 combat-skill total.
            if (m_ThrowingMod == null)
            {
                m_ThrowingMod = new DefaultSkillMod(SkillName.Throwing, true, 13.0);
                m.AddSkillMod(m_ThrowingMod);
            }

            if (m_ChivalryMod == null)
            {
                m_ChivalryMod = new DefaultSkillMod(SkillName.Chivalry, true, 13.0);
                m.AddSkillMod(m_ChivalryMod);
            }

            if (m_TacticsMod == null)
            {
                m_TacticsMod = new DefaultSkillMod(SkillName.Tactics, true, 13.0);
                m.AddSkillMod(m_TacticsMod);
            }
        }

        private void RemoveSkillMods(Mobile m)
        {
            if (m != null)
            {
                if (m_ThrowingMod != null) m.RemoveSkillMod(m_ThrowingMod);
                if (m_ChivalryMod != null) m.RemoveSkillMod(m_ChivalryMod);
                if (m_TacticsMod != null) m.RemoveSkillMod(m_TacticsMod);
            }

            m_ThrowingMod = null;
            m_ChivalryMod = null;
            m_TacticsMod = null;
        }

        public void RemoveFullSetBonus(Mobile m)
        {
            if (!m_FullSetBonusActive)
                return;

            m_FullSetBonusActive = false;

            Attributes.BonusStr -= 94;
            Attributes.BonusDex -= 94;
            Attributes.BonusInt -= 94;

            Attributes.RegenHits -= 4;
            Attributes.RegenMana -= 4;
            Attributes.RegenStam -= 4;

            Attributes.SpellDamage -= 700;
            Attributes.ReflectPhysical -= 298;
            Attributes.AttackChance -= 298;
            Attributes.DefendChance -= 298;
            Attributes.BonusHits -= 298;
            Attributes.BonusMana -= 298;
            Attributes.BonusStam -= 298;
            Attributes.Luck -= 700;
            Attributes.CastRecovery -= 4;
            Attributes.CastSpeed -= 4;
            Attributes.WeaponDamage -= 298;

            PhysicalBonus -= 11;

            RemoveSkillMods(m);
            InvalidateProperties();

            if (m != null)
            {
                m.SendMessage(0x22, "The power of the Obsidian Wing set fades.");
                m.UpdateResistances();
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("5-Piece Bonus: Obsidian Ascendance");

            if (m_FullSetBonusActive)
                list.Add("Full Set Bonus Active");
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);
            WBGargoyleSetHelper.QueueCheck(parent as Mobile);
        }

        public override void OnRemoved(object parent)
        {
            Mobile m = parent as Mobile;

            if (m_FullSetBonusActive)
                RemoveFullSetBonus(m);

            base.OnRemoved(parent);
            WBGargoyleSetHelper.QueueCheck(m);
        }

        public override void OnDelete()
        {
            RemoveSkillMods(Parent as Mobile);
            base.OnDelete();
        }

        public ObsidianWingChest(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);
            writer.Write(m_FullSetBonusActive);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            if (version >= 1)
                m_FullSetBonusActive = reader.ReadBool();

            Timer.DelayCall(TimeSpan.Zero, delegate
            {
                Mobile m = Parent as Mobile;
                if (m != null)
                    WBGargoyleSetHelper.Check(m);
            });
        }
    }
}
