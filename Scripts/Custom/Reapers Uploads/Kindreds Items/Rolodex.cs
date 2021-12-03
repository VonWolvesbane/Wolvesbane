using System;

namespace Server.Items
{
    public class Rolodex : Spellbook
    {
        public override bool IsArtifact { get { return true; } }

        [Constructable]
        public Rolodex()
            : base(UInt64.MaxValue)
        {
			Name = "<Body bgcolor=Black; text=#ff0000><Big><center>Rolodex of souls</Body>";
            Hue = 0;
            Slayer = SlayerName.Silver;
			Slayer2 = SlayerName.Exorcism;
			
			//AbsorptionAttributes.CastingFocus = 10;
            Attributes.RegenMana = 25;
            Attributes.LowerRegCost = 100;
            Attributes.CastRecovery = 10;
            Attributes.CastSpeed = 10;
            Attributes.LowerManaCost = 50;
            Attributes.BonusInt = 150;
			Attributes.BonusMana = 500;
			Attributes.BonusHits = 125;
            Attributes.SpellDamage = 500;
            SkillBonuses.SetValues(0, SkillName.Magery, 25.0);
			SkillBonuses.SetValues(1, SkillName.EvalInt, 25.0);
			SkillBonuses.SetValues(2, SkillName.Inscribe, 25.0);
        }

        public Rolodex(Serial serial)
            : base(serial)
        {
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