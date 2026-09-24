using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    // Split from Wolvesbane_Gargoyle_Gear_v3.cs: AeternumStoneChest

    public class AeternumStoneChest : GargishPlateChest
    {
        private bool m_FullSetBonusActive;
        private SkillMod m_ThrowingMod;
        private SkillMod m_ChivalryMod;
        private SkillMod m_TacticsMod;
        private Timer m_ProcTimer;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool FullSetBonusActive { get { return m_FullSetBonusActive; } }

        public override int ArtifactRarity { get { return 777; } }
        public override bool IsArtifact { get { return true; } }
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public AeternumStoneChest()
        {
            Name = "Aeternum Stone Chest";
            Hue = 2406;
            ApplyBaseStats();
        }

        private void ApplyBaseStats()
        {
            WBAeternumPieceStats.Apply(this);
            PhysicalBonus = 10;
            ArmorAttributes.MageArmor = 1;
            ArmorAttributes.SelfRepair = 10;
        }

        public void ApplyFullSetBonus(Mobile m)
        {
            if (m_FullSetBonusActive)
                return;

            m_FullSetBonusActive = true;

            // Five base pieces + these values match the Crusader wearable totals.
            Attributes.BonusStr += 137;
            Attributes.BonusDex += 137;
            Attributes.BonusInt += 137;

            Attributes.RegenHits += 7;
            Attributes.RegenMana += 7;
            Attributes.RegenStam += 7;

            Attributes.SpellDamage += 1048;
            Attributes.ReflectPhysical += 447;
            Attributes.AttackChance += 447;
            Attributes.DefendChance += 447;
            Attributes.BonusHits += 447;
            Attributes.BonusMana += 447;
            Attributes.BonusStam += 447;
            Attributes.Luck += 1048;
            Attributes.CastRecovery += 7;
            Attributes.CastSpeed += 7;
            Attributes.WeaponDamage += 447;

            PhysicalBonus += 10;

            EnsureSkillMods(m);
            EnsureProcTimer();
            InvalidateProperties();

            if (m != null)
            {
                m.SendMessage(0x55, "The Aeternum Stone set awakens around you.");
                m.FixedEffect(0x375A, 10, 40);
                m.PlaySound(0x1F7);
                m.UpdateResistances();
            }
        }

        public void EnsureSkillMods(Mobile m)
        {
            if (!m_FullSetBonusActive || m == null || m.Deleted)
                return;

            // Five pieces provide +75; full-set mods complete the +90 Crusader-equivalent total.
            if (m_ThrowingMod == null)
            {
                m_ThrowingMod = new DefaultSkillMod(SkillName.Throwing, true, 15.0);
                m.AddSkillMod(m_ThrowingMod);
            }

            if (m_ChivalryMod == null)
            {
                m_ChivalryMod = new DefaultSkillMod(SkillName.Chivalry, true, 15.0);
                m.AddSkillMod(m_ChivalryMod);
            }

            if (m_TacticsMod == null)
            {
                m_TacticsMod = new DefaultSkillMod(SkillName.Tactics, true, 15.0);
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

        public void EnsureProcTimer()
        {
            if (!m_FullSetBonusActive || Deleted)
            {
                StopProcTimer();
                return;
            }

            if (m_ProcTimer == null)
            {
                m_ProcTimer = new AeternumRenewalTimer(this);
                m_ProcTimer.Start();
            }
        }

        private void StopProcTimer()
        {
            if (m_ProcTimer != null)
            {
                m_ProcTimer.Stop();
                m_ProcTimer = null;
            }
        }

        private class AeternumRenewalTimer : Timer
        {
            private readonly AeternumStoneChest m_Chest;

            public AeternumRenewalTimer(AeternumStoneChest chest)
                : base(TimeSpan.FromSeconds(8.0), TimeSpan.FromSeconds(8.0))
            {
                m_Chest = chest;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Chest == null || m_Chest.Deleted || !m_Chest.m_FullSetBonusActive)
                {
                    Stop();
                    return;
                }

                Mobile m = m_Chest.Parent as Mobile;

                if (m == null || m.Deleted || !m.Alive || m.Combatant == null)
                    return;

                if (Utility.RandomDouble() < 0.25)
                {
                    int manaRestore = Math.Max(100, m.ManaMax / 20); // 5% of max, minimum 100
                    int stamRestore = Math.Max(100, m.StamMax / 20);

                    int oldMana = m.Mana;
                    int oldStam = m.Stam;

                    m.Mana = Math.Min(m.ManaMax, m.Mana + manaRestore);
                    m.Stam = Math.Min(m.StamMax, m.Stam + stamRestore);

                    if (m.Mana != oldMana || m.Stam != oldStam)
                    {
                        m.FixedEffect(0x376A, 9, 20);
                        m.PlaySound(0x1F2);
                        m.SendMessage(0x55, "Aeternum Renewal restores your mana and stamina.");
                    }
                }
            }
        }

        public void RemoveFullSetBonus(Mobile m)
        {
            if (!m_FullSetBonusActive)
                return;

            m_FullSetBonusActive = false;

            Attributes.BonusStr -= 137;
            Attributes.BonusDex -= 137;
            Attributes.BonusInt -= 137;

            Attributes.RegenHits -= 7;
            Attributes.RegenMana -= 7;
            Attributes.RegenStam -= 7;

            Attributes.SpellDamage -= 1048;
            Attributes.ReflectPhysical -= 447;
            Attributes.AttackChance -= 447;
            Attributes.DefendChance -= 447;
            Attributes.BonusHits -= 447;
            Attributes.BonusMana -= 447;
            Attributes.BonusStam -= 447;
            Attributes.Luck -= 1048;
            Attributes.CastRecovery -= 7;
            Attributes.CastSpeed -= 7;
            Attributes.WeaponDamage -= 447;

            PhysicalBonus -= 10;

            StopProcTimer();
            RemoveSkillMods(m);
            InvalidateProperties();

            if (m != null)
            {
                m.SendMessage(0x22, "The power of the Aeternum Stone set fades.");
                m.UpdateResistances();
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("5-Piece Bonus: Aeternum Ascendance");
            list.Add("Full Set Proc: Aeternum Renewal");

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
            StopProcTimer();
            RemoveSkillMods(Parent as Mobile);
            base.OnDelete();
        }

        public AeternumStoneChest(Serial s) : base(s) { }

        public override void Serialize(GenericWriter w)
        {
            base.Serialize(w);
            w.Write(1);
            w.Write(m_FullSetBonusActive);
        }

        public override void Deserialize(GenericReader r)
        {
            base.Deserialize(r);

            int version = r.ReadInt();
            if (version >= 1)
                m_FullSetBonusActive = r.ReadBool();

            Timer.DelayCall(TimeSpan.Zero, delegate
            {
                Mobile m = Parent as Mobile;

                if (m != null)
                    WBGargoyleSetHelper.Check(m);

                if (m_FullSetBonusActive)
                    EnsureProcTimer();
            });
        }
    }
}
