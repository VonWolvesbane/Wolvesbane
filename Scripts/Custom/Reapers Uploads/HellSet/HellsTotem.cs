using System;

namespace Server.Items
{
    public class HellsTotem : BaseTalisman
    {
		public override bool IsArtifact { get { return true; } }
        public override bool ForceShowName { get { return true; } }
				public override int ArtifactRarity{ get{ return 666; } }
		
        [Constructable]
        public HellsTotem()
            : base(0x2F5A)
        {
			Name = "Totem of HELL";
            Hue = 0x27;
			
			switch (Utility.Random(3))
            {
                case 0:
				ItemID = 0x2F5B;break;
				case 1:
				ItemID = 0x2F59;break;

            }
			
            MaxChargeTime = 300;
            Removal = TalismanRemoval.Damage;
            Protection = GetRandomProtection(false);
            Skill = BaseTalisman.GetRandomSkill();
            ExceptionalBonus = BaseTalisman.GetRandomExceptional();
            SuccessBonus = BaseTalisman.GetRandomSuccessful();
			
			Attributes.BonusHits = Utility.RandomMinMax(10, 30);
			Attributes.BonusMana = Utility.RandomMinMax(10, 30);
			Attributes.BonusStam = Utility.RandomMinMax(10, 30);
			Attributes.BonusStr = Utility.RandomMinMax(10, 20); 
            Attributes.BonusDex = Utility.RandomMinMax(10, 20); 
            Attributes.BonusInt = Utility.RandomMinMax(10, 20); 
			Attributes.Luck = Utility.RandomMinMax(10, 200);
			this.Attributes.WeaponDamage = 25;
			this.Attributes.DefendChance = 25;
			this.Attributes.CastRecovery = 3;
			this.Attributes.CastSpeed = 3;
			this.Attributes.LowerManaCost = 10;
			this.Attributes.LowerRegCost = 20;
			
        }

        public HellsTotem(Serial serial)
            : base(serial)
        {
        }
        
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}