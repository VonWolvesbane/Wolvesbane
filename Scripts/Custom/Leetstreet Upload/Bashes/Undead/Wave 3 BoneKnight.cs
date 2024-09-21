using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cute bone knight corpse")]
    public class CuteBoneKnight : BaseCreature
    {
        private Timer m_PoisonTimer;

        [Constructable]
        public CuteBoneKnight() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Cute Bone Knight";
            Body = 0x39; // Body ID for a lich-like appearance
            Hue = 2253; // Custom hue

            SetStr(800);
            SetDex(900);
            SetHits(80000); // Hits
            SetStam(900); // Stamina

            SetDamage(60, 90); // Min and Max damage

            SetSkill(SkillName.Wrestling, 175.0);
            SetSkill(SkillName.Tactics, 175.0);
            SetSkill(SkillName.Parry, 175.0);
            SetSkill(SkillName.Anatomy, 175.0);
            SetSkill(SkillName.Magery, 175.0);
            SetSkill(SkillName.EvalInt, 175.0);

            Fame = 3000; // Optional: Adjust based on desired difficulty
            Karma = -3000; // Optional: Adjust as needed

            VirtualArmor = 70; // Adjust based on desired difficulty

            m_PoisonTimer = new PoisonTimer(this);
            m_PoisonTimer.Start();
        }

        public CuteBoneKnight(Serial serial) : base(serial)
        {
        }

        private class PoisonTimer : Timer
        {
            private CuteBoneKnight m_BoneKnight;

            public PoisonTimer(CuteBoneKnight boneKnight) : base(TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15))
            {
                m_BoneKnight = boneKnight;
            }

            protected override void OnTick()
            {
                Map map = m_BoneKnight.Map;

                if (map == null)
                    return;

                // Find targets within range
                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in m_BoneKnight.GetMobilesInRange(10))
                {
                    if (m != m_BoneKnight && m.Player && m.Alive)
                    {
                        targets.Add(m);
                    }
                }

                // Apply poison effect to targets
                foreach (Mobile target in targets)
                {
                    if (target.Alive && target.Poison == null) // Check if the target is not already poisoned
                    {
                        m_BoneKnight.DoHarmful(target);
                        target.SendMessage("You have been poisoned by the Cute Bone Knight!");
                        Poison poison = Poison.Lethal; // Use a high-level poison (adjust as needed)
                        target.ApplyPoison(m_BoneKnight, poison);
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
    }
}
