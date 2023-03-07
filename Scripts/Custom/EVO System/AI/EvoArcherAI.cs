#region AuthorHeader
//
//	EvoSystem version 2.1, by Xanthos
//
//
#endregion AuthorHeader
using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;

namespace Xanthos.Evo
{
	class EvoArcherAI : ArcherAI
	{
		private bool m_CanAttackPlayers;

		public EvoArcherAI( BaseCreature m, bool canAttackPlayers ) : base( m )
		{
			m_CanAttackPlayers = canAttackPlayers;
		}


        public override void EndPickTarget( Mobile from, IDamageable target, OrderType order )
		{
			Mobile oldTarget = m_Mobile.ControlTarget as Mobile;
			OrderType oldOrder = order;

			base.EndPickTarget( from, target, order );

			if ( OrderType.Attack == order && target is PlayerMobile && !m_CanAttackPlayers )
			{
				// Not allowed to attack players so reset what was changed by EndPickTarget
				m_Mobile.ControlTarget = oldTarget;
				m_Mobile.ControlOrder = oldOrder;
			}
		}

		public override bool DoOrderGuard()
		{
			if (m_Mobile.IsDeadPet)
			{
				return true;
			}

			var controlMaster = m_Mobile.ControlMaster;

			if (controlMaster == null || controlMaster.Deleted)
			{
				return true;
			}

			var combatant = m_Mobile.Combatant as Mobile;

            if (combatant != null && !ValidGuardTarget(combatant))
                combatant = null;

            Mobile closestMob = combatant;
            var closestDist = combatant == null ? m_Mobile.RangePerception : combatant.GetDistanceToSqrt(controlMaster);

            foreach (var aggressor in controlMaster.Aggressors.Select(x => x.Attacker).Where(m => ValidGuardTarget(m)))
            {
                var dist = aggressor.GetDistanceToSqrt(controlMaster);

                if (closestMob == null || dist < closestDist)
                {
                    closestMob = aggressor;
                    closestDist = dist;
                }
            }

            foreach (var aggressed in controlMaster.Aggressed.Select(x => x.Defender).Where(m => ValidGuardTarget(m)))
            {
                var dist = aggressed.GetDistanceToSqrt(controlMaster);

                if (closestMob == null || dist < closestDist)
                {
                    closestMob = aggressed;
                    closestDist = dist;
                }
            }

            if (closestMob != null)
            {
                if (m_Mobile.Debug && closestMob != null && combatant != closestMob)
                {
                    m_Mobile.DebugSay("Crap, my master has been attacked! I will attack one of those bastards!");
                }

                combatant = closestMob;
            }        

            if (combatant != null)
			{
				m_Mobile.DebugSay("Guarding from target...");

				m_Mobile.Combatant = combatant;
				m_Mobile.FocusMob = combatant;
				Action = ActionType.Combat;
                m_Mobile.Direction = m_Mobile.GetDirectionTo(combatant);

                /*
                * We need to call Think() here or spell casting monsters will not use
                * spells when guarding because their target is never processed.
                */
                Think();
			}
			else
			{
				m_Mobile.DebugSay("Nothing to guard from");

				m_Mobile.Warmode = false;

				if (Core.AOS)
				{
					m_Mobile.CurrentSpeed = m_Mobile.ActiveSpeed;
                }

				WalkMobileRange(controlMaster, 1, false, 0, 1);
			}

			return true;
		}

        public bool ValidGuardTarget(Mobile combatant)
        {
            return combatant != null && combatant != m_Mobile && combatant != m_Mobile.ControlMaster && !combatant.Deleted && m_Mobile.CanSee(combatant) &&
                combatant.Alive && (!(combatant is Mobile) || !combatant.IsDeadBondedPet) &&
                m_Mobile.CanBeHarmful(combatant, false) && combatant.Map == m_Mobile.Map && combatant.GetDistanceToSqrt(m_Mobile) <= m_Mobile.RangePerception;
        }

		public virtual bool DoOrderAttack()
		{
			if (m_Mobile.IsDeadPet)
			{
				return true;
			}

			if (m_Mobile.ControlTarget == null || m_Mobile.ControlTarget.Deleted || m_Mobile.ControlTarget.Map != m_Mobile.Map ||
				!m_Mobile.ControlTarget.Alive ||
				(m_Mobile.ControlTarget is Mobile && ((Mobile)m_Mobile.ControlTarget).IsDeadBondedPet))
			{
				m_Mobile.DebugSay(
					"I think he might be dead. He's not anywhere around here at least. That's cool. I'm glad he's dead.");

				if (Core.AOS)
				{
					m_Mobile.ControlTarget = m_Mobile.ControlMaster;
					m_Mobile.ControlOrder = OrderType.Follow;
				}
				else
				{
					m_Mobile.ControlTarget = null;
					m_Mobile.ControlOrder = OrderType.None;
				}

				if (m_Mobile.FightMode == FightMode.Closest || m_Mobile.FightMode == FightMode.Aggressor)
				{
					Mobile newCombatant = null;
					var newScore = 0.0;

					var eable = m_Mobile.GetMobilesInRange(m_Mobile.RangePerception);

					foreach (var aggr in eable)
					{
						if (!m_Mobile.CanSee(aggr) || aggr.Combatant != m_Mobile)
						{
							continue;
						}

						if (aggr.IsDeadBondedPet || !aggr.Alive)
						{
							continue;
						}

						var aggrScore = m_Mobile.GetFightModeRanking(aggr, FightMode.Closest, false);

						if ((newCombatant == null || aggrScore > newScore || (aggrScore == newScore && !aggr.Player && newCombatant.Player)) && m_Mobile.InLOS(aggr))
						{
							newCombatant = aggr;
							newScore = aggrScore;
						}
					}

					eable.Free();

					if (newCombatant != null)
					{
						m_Mobile.ControlTarget = newCombatant;
						m_Mobile.ControlOrder = OrderType.Attack;
						m_Mobile.Combatant = newCombatant;
						m_Mobile.DebugSay("But -that- is not dead. Here we go again...");
						Think();
					}
				}
			}
			else
			{
				m_Mobile.DebugSay("Attacking target...");

                if (m_Mobile.Combatant != null)
                    m_Mobile.Direction = m_Mobile.GetDirectionTo(m_Mobile.Combatant);

                Think();
			}

			return true;
		}
	}
}
