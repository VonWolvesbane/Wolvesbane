using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Gumps;

namespace Server.ACC.CSS.Systems.Rogue
{
    public class RogueIntimidationSpell : RogueSpell
    {
		        private static readonly Hashtable m_Registry = new Hashtable();
   private static SpellInfo m_Info = new SpellInfo(
                                                        "Intimidation", " ",
            //SpellCircle.Fourth,
                                                        212,
                                                        9041
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0; } }
        public override double RequiredSkill { get { return 0; } }
        public override int RequiredMana { get { return 0; } }
        private static readonly Hashtable m_Table = new Hashtable();
        public RogueIntimidationSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }
		        public static Hashtable Registry
        {
            get
            {
                return m_Registry;
            }
        }
        public static void Toggle(Mobile caster, Mobile target, bool archprotection)
        {
            /* Players under the protection spell effect can no longer have their spells "disrupted" when hit.
            * Players under the protection spell have decreased physical resistance stat value (-15 + (Inscription/20),
            * a decreased "resisting spells" skill value by -35 + (Inscription/20),
            * and a slower casting speed modifier (technically, a negative "faster cast speed") of 2 points.
            * The protection spell has an indefinite duration, becoming active when cast, and deactivated when re-cast.
            * Reactive Armor, Protection, and Magic Reflection will stay on—even after logging out,
            * even after dying—until you “turn them off” by casting them again.
            */
            object[] mods = (object[])m_Table[target];

            if (mods == null)
            {
                target.PlaySound(0x1E9);
                target.FixedParticles(0x375A, 9, 20, 5016, EffectLayer.Waist);

                mods = new object[5]
                {
					new DefaultSkillMod( SkillName.Hiding, true, -20 ),
					new DefaultSkillMod( SkillName.Stealth, true, -20 ),
					new DefaultSkillMod( SkillName.Swords, true, 50 ),
					new DefaultSkillMod( SkillName.Macing, true, 50 ),
					new DefaultSkillMod( SkillName.Fencing, true, 50 )               
			    };

                m_Table[target] = mods;
				Registry[target] = 100.0;

                target.AddSkillMod((SkillMod)mods[0]);
                target.AddSkillMod((SkillMod)mods[1]);
                target.AddSkillMod((SkillMod)mods[2]);
                target.AddSkillMod((SkillMod)mods[3]);
                target.AddSkillMod((SkillMod)mods[4]);
				}
            else
            {
                target.PlaySound(0x1ED);
                target.FixedParticles(0x375A, 9, 20, 5016, EffectLayer.Waist);

                m_Table.Remove(target);
				Registry.Remove(target);

                target.RemoveSkillMod((SkillMod)mods[0]);
                target.RemoveSkillMod((SkillMod)mods[1]);
                target.RemoveSkillMod((SkillMod)mods[2]);
                target.RemoveSkillMod((SkillMod)mods[3]);
                target.RemoveSkillMod((SkillMod)mods[4]);
            }
        }

        public static void EndIntimidation(Mobile m)
        {
            if (m_Table.Contains(m))
            {
                object[] mods = (object[])m_Table[m];

                m_Table.Remove(m);
                Registry.Remove(m);

                    m.RemoveSkillMod((SkillMod)mods[0]);
                    m.RemoveSkillMod((SkillMod)mods[1]);
                    m.RemoveSkillMod((SkillMod)mods[2]);
                    m.RemoveSkillMod((SkillMod)mods[3]);
                    m.RemoveSkillMod((SkillMod)mods[4]);
            }
        }

        public override bool CheckCast()
        {
            if (Core.AOS)
                return true;

            if (m_Registry.ContainsKey(this.Caster))
            {
                this.Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                return false;
            }
            else if (!this.Caster.CanBeginAction(typeof(DefensiveSpell)))
            {
                this.Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
                return false;
            }

            return true;
        }

        public override void OnCast()
        {
            if (Core.AOS)
            {
                if (this.CheckSequence())
                    Toggle(this.Caster, this.Caster, false);

                this.FinishSequence();
            }
            else
            {
                if (m_Registry.ContainsKey(this.Caster))
                {
                    this.Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                }
                else if (!this.Caster.CanBeginAction(typeof(DefensiveSpell)))
                {
                    this.Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
                }
                else if (this.CheckSequence())
                {
                    if (this.Caster.BeginAction(typeof(DefensiveSpell)))
                    {
                        double value = (int)(this.Caster.Skills[SkillName.Stealth].Value + this.Caster.Skills[SkillName.Hiding].Value + this.Caster.Skills[SkillName.Focus].Value);
                        value /= 4;

                        if (value < 0)
                            value = 0;
                        else if (value > 75)
                            value = 75.0;

                        Registry.Add(this.Caster, value);
                        new InternalTimer(this.Caster).Start();

                        this.Caster.FixedParticles(0x375A, 9, 20, 5016, EffectLayer.Waist);
                        this.Caster.PlaySound(0x1ED);
                    }
                    else
                    {
                        this.Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
                    }
                }

                this.FinishSequence();
            }
        }

        #region SA
        public static bool HasIntimidation(Mobile m)
        {
            return m_Table.ContainsKey(m);
        }
        #endregion

        private class InternalTimer : Timer
        {
            private readonly Mobile m_Caster;
            public InternalTimer(Mobile caster)
                : base(TimeSpan.FromSeconds(0))
            {
                double val = caster.Skills[SkillName.Magery].Value * 2.0;
                if (val < 15)
                    val = 15;
                else if (val > 240)
                    val = 240;

                this.m_Caster = caster;
                this.Delay = TimeSpan.FromSeconds(val);
                this.Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                RogueIntimidationSpell.Registry.Remove(this.m_Caster);
                DefensiveSpell.Nullify(this.m_Caster);
            }
        }
    }
}
