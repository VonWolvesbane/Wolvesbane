using System;
using Server;

namespace Server.Items
{
    public class PerfectedGloves : LeatherGloves
	{
        public override int ArtifactRarity{ get{ return 3000; } }
				public override bool IsArtifact { get { return true; } }

        public override int BasePhysicalResistance{ get{ return 15; } }
		public override int BaseFireResistance{ get{ return 15; } } 
		public override int BaseColdResistance{ get{ return 15; } }
		public override int BasePoisonResistance{ get{ return 15; } }
		public override int BaseEnergyResistance{ get{ return 15; } }

        public override bool AllowMaleWearer { get { return true; } }
	 	public override int InitMinHits{ get{ return 255; } }
	 	public override int InitMaxHits{ get{ return 255; } }

        /* private static string[] m_Names = new string[]
		{
            "Armor of Power", //Want random names? 
			"Armor crafted by the Godly Von Wolvesbane"  

		};*/

	 	[Constructable]
	 	public PerfectedGloves()
	 	{
            //Name = m_Names[Utility.Random(m_Names.Length)];
            Hue = Utility.RandomMinMax(5, 3000);
			this.SetHue = 2498;
			this.ItemID = 9795;
            Name = "Perfected Alien Gloves";

                this.SetAttributes.RegenHits = 55;
                this.SetAttributes.RegenStam = 55;
                this.SetAttributes.RegenMana = 55;
                this.SetAttributes.BonusStr = 850;
                this.SetAttributes.BonusDex = 850;
                this.SetAttributes.BonusInt = 850;
                this.SetAttributes.ReflectPhysical = 200;
                this.SetAttributes.Luck = 5000;
                this.SetAttributes.BonusHits = 1500;
                this.SetAttributes.BonusStam = 1500;
                this.SetAttributes.BonusMana = 1500;
				this.SetAttributes.SpellDamage = 500;
				SetAttributes.NightSight = 1;
                SetAttributes.EnhancePotions = 100;
				SetAttributes.CastRecovery = 10;
                SetAttributes.CastSpeed = 10;
                SetAttributes.LowerManaCost = 40;
                SetAttributes.LowerRegCost = 100;
                SetAttributes.WeaponDamage = 300;
                SetAttributes.WeaponSpeed = 75;
				SetAttributes.DefendChance = 40;
                SetAttributes.AttackChance = 40;
		}
	 	public PerfectedGloves(Serial serial) : base( serial )
	 	{
	 	}
		
		public override SetItem SetID { get { return SetItem.Alien; } }
        public override int Pieces { get { return 6; } }
		
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
