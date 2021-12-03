//Crafted By ReApEr
using System;

namespace Server.Items
{
    public class FootSoldiersLegs : LeatherLegs
    {
        [Constructable]
        public FootSoldiersLegs()
            : base()
        {
            this.SetHue = 0x455;
			
            this.Attributes.BonusStam = 5;
            this.Attributes.WeaponSpeed = 15;		
			
            this.SetSkillBonuses.SetValues(0, SkillName.Stealth, 20);
			this.SetSkillBonuses.SetValues(1, SkillName.Hiding, 20);
			this.SetSkillBonuses.SetValues(0, SkillName.Ninjitsu, 20);
			
            this.SetSelfRepair = 5;
			
			this.SetAttributes.WeaponDamage = 55;
			this.SetAttributes.AttackChance = 45;
            this.SetAttributes.BonusDex = 55;
			this.SetAttributes.BonusStr = 55;
			this.SetAttributes.BonusInt = 55;
			this.SetAttributes.BonusStam = 100;
			this.SetAttributes.BonusMana = 100;
			this.SetAttributes.BonusHits = 100;
			
            this.SetPhysicalBonus = 20;
            this.SetFireBonus = 20;
            this.SetColdBonus = 20;
            this.SetPoisonBonus = 20;
            this.SetEnergyBonus = 20;
        }

        public FootSoldiersLegs(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1074304;
            }
        }// Assassin Armor
        public override SetItem SetID
        {
            get
            {
                return SetItem.Assassin;
            }
        }
        public override int Pieces
        {
            get
            {
                return 4;
            }
        }
        public override int BasePhysicalResistance
        {
            get
            {
                return 10;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 10;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 10;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 10;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 10;
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