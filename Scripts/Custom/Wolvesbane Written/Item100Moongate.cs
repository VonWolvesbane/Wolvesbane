#region References
using System;

using Server.Factions;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Regions;
using Server.Spells;
using VitaNex.Items;
#endregion

namespace Server.Items
{
    [DispellableField]
    public class Item100Moongate : Item
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Target { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map TargetMap { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Dispellable { get; set; }


        public virtual bool TeleportPets { get { return true; } }

        [Constructable]
        public Item100Moongate()
            : this(Point3D.Zero, null)
        {
            Dispellable = false;
        }

        [Constructable]
        public Item100Moongate(bool bDispellable)
            : this(Point3D.Zero, null)
        {
            Dispellable = bDispellable;
        }

        [Constructable]
        public Item100Moongate(Point3D target, Map targetMap)
            : base(0xF6C)
        {
            Movable = false;
            Light = LightType.Circle300;

            Target = target;
            TargetMap = targetMap;
        }

        public Item100Moongate(Serial serial)
            : base(serial)
        { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.Player)
                return;

            if (from.InRange(GetWorldLocation(), 1))
                CheckGate(from, 1);
            else
                from.SendLocalizedMessage(500446); // That is too far away.
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m.Player)
                CheckGate(m, 0);

            return true;
        }

        public virtual void CheckGate(Mobile m, int range)
        {
            #region Mondain's Legacy
            if (m.Hidden && m.IsPlayer() && Core.ML)
                m.RevealingAction();
            #endregion

            new DelayTimer(m, this, range).Start();
        }

        public virtual void OnGateUsed(Mobile m)
        {
            if (TargetMap == null || TargetMap == Map.Internal)
                return;

            if (TeleportPets)
                BaseCreature.TeleportPets(m, Target, TargetMap);

            m.MoveToWorld(Target, TargetMap);

            if (m is PlayerMobile)
            {
				SkillsCodex codex = new SkillsCodex(7, 100, true, SkillCodexMode.Fixed, SkillCodexFlags.Both);
				codex.Movable = false;
				m.AddToBackpack(codex);
			}

            if (m.IsPlayer() || !m.Hidden)
                m.PlaySound(0x1FE);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            writer.Write(Target);
            writer.Write(TargetMap);

            // Version 1
            writer.Write(Dispellable);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();

            Target = reader.ReadPoint3D();
            TargetMap = reader.ReadMap();

            if (version >= 1)
                Dispellable = reader.ReadBool();
        }

        public virtual bool ValidateUse(Mobile from, bool message)
        {
            if (from.Deleted || Deleted)
                return false;

            if (from.Map != Map || !from.InRange(this, 1))
            {
                if (message)
                    from.SendLocalizedMessage(500446); // That is too far away.

                return false;
            }

            return true;
        }

        public virtual void DelayCallback(Mobile from, int range)
        {
            if (!ValidateUse(from, false) || !from.InRange(this, range))
                return;

            if (TargetMap != null)
                OnGateUsed(from);
            else
                from.SendMessage("This moongate does not seem to go anywhere.");
        }

        private class DelayTimer : Timer
        {
            private readonly Mobile m_From;
            private readonly Item100Moongate m_Gate;
            private readonly int m_Range;

            public DelayTimer(Mobile from, Item100Moongate gate, int range)
                : base(TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_Gate = gate;
                m_Range = range;
            }

            protected override void OnTick()
            {
                m_Gate.DelayCallback(m_From, m_Range);
            }
        }
    }
}
