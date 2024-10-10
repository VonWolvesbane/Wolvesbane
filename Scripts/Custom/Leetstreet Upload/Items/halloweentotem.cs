using System;
using Server.Targeting;

namespace Server.Items
{
    public class HalloweenTotem : Item
    {
        public override int LabelNumber { get { return 1158780; } } // Mystical Polymorph Totem

        private int m_Hue = -1;

        private static readonly int[] HalloweenBodies = new int[]
        {
            0x13D, // Vampire
            0x2CF, // Werewolf
            0x32,  // Skeleton
            0x9A,  // Mummy
            0x99,  // Ghost
            0x136, // Banshee
            0x2E4  // Dream Wraith
        };

        [CommandProperty(AccessLevel.GameMaster)]
        public int Duration { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public string CostumeCreatureName { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Transformed { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int CostumeHue
        {
            get { return m_Hue; }
            set { m_Hue = value; }
        }

        [Constructable]
        public HalloweenTotem()
            : base(0xA276)
        {
            LootType = LootType.Blessed;
        }

        public HalloweenTotem(Serial serial)
            : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (CostumeCreatureName != null)
            {
                list.Add(1158707, String.Format("{0}", CostumeCreatureName)); // a ~1_name~ costume
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1060640); // The item must be in your backpack to use it.                
            }
            else
            {
                if (!Transformed)
                {
                    EnMask(from);
                }
                else
                {
                    DeMask(from);
                }
            }
        }

        public override bool DropToWorld(Mobile from, Point3D p)
        {
            bool drop = base.DropToWorld(from, p);

            if (Transformed)
            {
                DeMask(from);
            }

            return drop;
        }

        private Timer m_Timer;

        private void EnMask(Mobile from)
        {
            if (from.Mounted || from.Flying)
            {
                from.SendLocalizedMessage(1010097); // You cannot use this while mounted or flying. 
            }
            else if (from.IsBodyMod || from.HueMod > -1)
            {
                from.SendLocalizedMessage(1158010); // You cannot use that item in this form.
            }
            else
            {
                Duration = 28800;

                // Randomly select a body from HalloweenBodies
                int randomIndex = Utility.Random(HalloweenBodies.Length);
                from.BodyMod = HalloweenBodies[randomIndex];

                m_Hue = Utility.Random(0, 1000); // Optional: Randomize hue if needed

                if (m_Timer == null || !m_Timer.Running)
                
                    m_Timer = Timer.DelayCall(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1), delegate { Slice(from); });

                BuffInfo.AddBuff(from, new BuffInfo(BuffIcon.MysticalPolymorphTotem, 1158780, 1158017, TimeSpan.FromSeconds(Duration), from, CostumeCreatureName));

                ItemID = 0xA20B;
                from.HueMod = m_Hue;
                Transformed = true;
            }
        }

        public virtual void Slice(Mobile from)
        {
            if (Duration > 0)
                Duration--;
            else
            {
                DeMask(from);

                if (m_Timer != null)
                    m_Timer.Stop();

                m_Timer = null;
            }
        }

        private void DeMask(Mobile from)
        {
            ItemID = 0xA276;
            from.BodyMod = 0;
            from.HueMod = -1;
            Transformed = false;
            Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0x3728, 8, 20, 5042);
            from.PlaySound(250);
            BuffInfo.RemoveBuff(from, BuffIcon.MysticalPolymorphTotem);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(CostumeCreatureName);
            writer.Write((int)m_Hue);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            CostumeCreatureName = reader.ReadString();
            m_Hue = reader.ReadInt();
            ItemID = 0xA276;
        }
    }
}
