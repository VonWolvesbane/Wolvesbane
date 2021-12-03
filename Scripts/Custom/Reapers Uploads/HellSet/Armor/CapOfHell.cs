using System;
using Server.Engines.Craft;

namespace Server.Items
{
    public class CapOfHell : LeatherCap
	{
		public override int ArtifactRarity{ get{ return 666; } }
		public override bool IsArtifact { get { return true; } }

		
        [Constructable]
        public CapOfHell() 
		: base ()
        {


			this.SkillBonuses.SetValues(0, SkillName.Archery, 10);

			Hue = 0x27; 
            Name = "Cap Of HELL";

			Attributes.BonusHits = Utility.RandomMinMax(20, 40);
			Attributes.BonusMana = Utility.RandomMinMax(20, 40);
			Attributes.BonusStam = Utility.RandomMinMax(20, 40);
			Attributes.BonusStr = Utility.RandomMinMax(10, 30); 
            Attributes.BonusDex = Utility.RandomMinMax(10, 30); 
            Attributes.BonusInt = Utility.RandomMinMax(10, 30); 
			Attributes.RegenStam = Utility.RandomMinMax(10, 30);
			Attributes.RegenMana = Utility.RandomMinMax(10, 30);
			Attributes.RegenHits = Utility.RandomMinMax(10, 30);
			Attributes.Luck = Utility.RandomMinMax(10, 200);
			this.Attributes.SpellDamage = 35;
			this.Attributes.WeaponDamage = 25;
			this.Attributes.DefendChance = 25;
			this.Attributes.CastRecovery = 3;
			this.Attributes.CastSpeed = 3;
			this.Attributes.LowerManaCost = 20;
			this.Attributes.LowerRegCost = 30;
			FireBonus = 25;
			ColdBonus = 25;
            PoisonBonus = 25;
            PhysicalBonus = 25;
            EnergyBonus = 15;

			
		}

        public CapOfHell(Serial serial)
            : base(serial)
        {
        }
		
		public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Cloth;
            }
        }

        public override int LabelNumber
        {
            get
            {
                return 1094911;
            }
        }// Captain John's Hat [Replica]
        public override int BasePhysicalResistance
        {
            get
            {
                return 4;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 6;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 9;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 7;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 8;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 150;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 150;
            }
        }
        public override bool CanFortify
        {
            get
            {
                return false;
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
