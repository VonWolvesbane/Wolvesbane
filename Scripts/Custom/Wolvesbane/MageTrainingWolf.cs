using System;
using Server;
using Server.Mobiles;

namespace Server.Mobiles
{
    /// <summary>
    /// Wolvesbane training wolf.
    /// Uses Mage AI and is prevented from normal walking.
    /// </summary>
    public class MageTrainingWolf : TrainingElemental
    {
        [Constructable]
        public MageTrainingWolf()
            : base()
        {
            Name = "MAGE WOLF";
            BodyValue = 27;
            Hue = 2400;

            RawInt = 200;

            // Wolvesbane/ServUO fork exposes DisallowAllMoves as read-only.
            // CantWalk is the supported writable movement lock used by your shard.
            CantWalk = true;

            // Explicitly use mage AI.
            AI = AIType.AI_Mage;

            Skills[SkillName.Magery].Base = 65.0;
        }

        public MageTrainingWolf(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-assert the movement lock after load.
            CantWalk = true;
        }
    }
}
