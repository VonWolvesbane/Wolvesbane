using System;

namespace Server.Items
{
    public class TheImpossible : WoodenKiteShield
    {
        public override bool IsArtifact { get { return true; } }
        [Constructable]
        public TheImpossible()
        {
            Name = "<Body bgcolor=Black; text=#87CEEB><Big><center>The Impossible</Body>";
            Hue = 0x556;
            Attributes.NightSight = 1;
            Attributes.SpellChanneling = 1;
            Attributes.DefendChance = 100;
            Attributes.CastSpeed = 4;
            Attributes.CastRecovery = 6;

            SkillBonuses.SetValues(0, SkillName.Parry, 10);
            SkillBonuses.SetValues(1, SkillName.Chivalry, 10);
            SkillBonuses.SetValues(2, SkillName.Tactics, 10);
            SkillBonuses.SetValues(3, SkillName.Magery, 25);
            SkillBonuses.SetValues(4, SkillName.Inscribe, 25);


            AbsorptionAttributes.EaterEnergy = 20;
            AbsorptionAttributes.EaterPoison = 20;
            AbsorptionAttributes.EaterCold = 20;
            AbsorptionAttributes.EaterFire = 20;

            ArmorAttributes.SelfRepair = 100;

            Attributes.ReflectPhysical = 100;
            Attributes.Luck = 2000;
            Attributes.BonusHits = 1000;
            Attributes.BonusStam = 1000;
            Attributes.BonusMana = 1000;
            Attributes.SpellDamage = 2000;
            Attributes.LowerManaCost = 100;
            Attributes.LowerRegCost = 100;

            
            
            

            PhysicalBonus = 100;
            FireBonus = 100;
            ColdBonus = 100;
            PoisonBonus = 100;
            EnergyBonus = 100;


        }

        public TheImpossible(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1061101;
            }
        }
        public override int ArtifactRarity
        {
            get
            {
                return 1000;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 255;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 255;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}