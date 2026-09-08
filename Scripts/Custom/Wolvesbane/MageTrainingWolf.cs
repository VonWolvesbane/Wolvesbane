using System;
using Server;
using Server.Mobiles;

namespace Server.Mobiles
{
    /// <summary>
    /// Wolvesbane training wolf.
    /// Uses Mage AI but blocks MageAI movement/repositioning teleport behavior
    /// by setting DisallowAllMoves = true.
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

            // MageAI checks this before using TeleportSpell for movement/stuck logic.
            DisallowAllMoves = true;

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
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-assert behavior after load.
            DisallowAllMoves = true;
            AI = AIType.AI_Mage;
        }
    }
}
