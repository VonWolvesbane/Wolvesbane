using System;
using Server.Engines.Craft;

namespace Server.Items
{
    public class WitchesHat : LeatherCap
	{
		public override int ArtifactRarity{ get{ return 100; } }
		public override bool IsArtifact { get { return true; } }

		
        [Constructable]
        public WitchesHat() 
		: base ()
        {


			//this.SkillBonuses.SetValues(0, SkillName.Archery, 10);
			ItemID = 5912;
			Hue = 2342; 
            Name = "<Body bgcolor=Black; text=#9400D3><Big><center>Savage Witches Hat</Body>";

			Attributes.BonusHits = Utility.RandomMinMax(10, 20);
			Attributes.BonusMana = Utility.RandomMinMax(50, 100);
			Attributes.BonusStr = Utility.RandomMinMax(10, 20); 
            Attributes.BonusDex = Utility.RandomMinMax(10, 20); 
            Attributes.BonusInt = Utility.RandomMinMax(50, 75); 
			//Attributes.RegenStam = Utility.RandomMinMax(10, 30);
			Attributes.RegenMana = Utility.RandomMinMax(20, 40);
			//Attributes.RegenHits = Utility.RandomMinMax(10, 30);
			Attributes.Luck = Utility.RandomMinMax(10, 200);
			this.Attributes.SpellDamage = 100;
			this.Attributes.WeaponDamage = 10;
			this.Attributes.DefendChance = 10;
			this.Attributes.CastRecovery = 3;
			this.Attributes.CastSpeed = 3;
			this.Attributes.LowerManaCost = 20;
			this.Attributes.LowerRegCost = 30;
			FireBonus = 17;
			ColdBonus = 18;
            PoisonBonus = 16;
            PhysicalBonus = 12;
            EnergyBonus = 19;
			
		}

        public WitchesHat(Serial serial)
            : base(serial)
        {
        }
		
		public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Leather;
            }
        }

        public override int LabelNumber
        {
            get
            {
                return 1094911;
            }
        }// Captain John's Hat [Replica]
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
        public override bool CanFortify
        {
            get
            {
                return true;
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
