using System;
using Server;

namespace Server.Items
{
    public class ArtifactHelm : LeatherCap
	{
        public override int ArtifactRarity { get { return Utility.RandomMinMax(1, 100); } }

        public override int BasePhysicalResistance { get { return 0; } }
        public override int BaseFireResistance { get { return 0; } }
        public override int BaseColdResistance { get { return 0; } }
        public override int BasePoisonResistance { get { return 0; } }
        public override int BaseEnergyResistance { get { return 0; } }

        public override bool AllowMaleWearer { get { return true; } }
	 	public override int InitMinHits{ get{ return 255; } }
	 	public override int InitMaxHits{ get{ return 255; } }

         private static string[] m_Names = new string[]
		{
            "Test 001","Test 002","Test 003","Test 004","Test 005","Test 006","Test 007","Test 008","Test 009","Test 010",
			"Test 011","Test 012","Test 013","Test 014","Test 015","Test 016","Test 017","Test 018","Test 019","Test 020",
			"Test 021","Test 022","Test 023","Test 024","Test 025","Test 026","Test 027","Test 028","Test 029","Test 030",
			"Test 031","Test 032","Test 033","Test 034","Test 035","Test 036","Test 037","Test 038","Test 039","Test 040",
			"Test 041","Test 042","Test 043","Test 044","Test 045","Test 046","Test 047","Test 048","Test 049","Test 050"			

		};

	 	[Constructable]
	 	public ArtifactHelm()
	 	{
            Name = m_Names[Utility.Random(m_Names.Length)];
            Hue = Utility.RandomMinMax(5, 3000);
            //Name = "Armor from another world";

            // random chance to get diffent resist %
            PhysicalBonus = Utility.RandomMinMax(4, 13);
            FireBonus = Utility.RandomMinMax(4, 13);
            ColdBonus = Utility.RandomMinMax(4, 13);
            PoisonBonus = Utility.RandomMinMax(4, 13);
            EnergyBonus = Utility.RandomMinMax(4, 13);

            // id it shows item as
            switch (Utility.Random(7))
            {
                case 0: ItemID = 0x140E; break;//norse
                case 1: ItemID = 0x1408; break;//close
                case 2: ItemID = 0x1718; break;//wizard
                case 3: ItemID = 0x141B; break;//orc
                case 4: ItemID = 0x1549; break;//trible
                case 5: ItemID = 5138; break;//plate
                case 6: ItemID = 5440; break;//badana
            }

           // random chance to get these stats added to item ,chance of one stat per switch
           switch (Utility.Random(5))
            {
                case 0: Attributes.RegenHits = Utility.RandomMinMax(1, 10); break;
                case 1: Attributes.RegenStam = Utility.RandomMinMax(1, 10); break;
                case 2: Attributes.RegenStam = Utility.RandomMinMax(1, 10); break;
                case 3: Attributes.DefendChance = Utility.RandomMinMax(1, 40); break;
                case 4: Attributes.AttackChance = Utility.RandomMinMax(1, 40); break;
            }
			switch (Utility.Random(3))
            {
                case 0: Attributes.RegenHits = Utility.RandomMinMax(1, 25); break;
                case 1: Attributes.RegenStam = Utility.RandomMinMax(1, 25); break;
                case 2: Attributes.RegenStam = Utility.RandomMinMax(1, 25); break;
			}
            switch (Utility.Random(3))
            {
                case 0: Attributes.BonusStr = Utility.RandomMinMax(1, 75); break;
                case 1: Attributes.BonusDex = Utility.RandomMinMax(1, 75); break;
                case 2: Attributes.BonusInt = Utility.RandomMinMax(1, 75); break;
            }
			 switch (Utility.Random(3))
            {
                case 0: Attributes.BonusStr = Utility.RandomMinMax(1, 75); break;
                case 1: Attributes.BonusDex = Utility.RandomMinMax(1, 75); break;
                case 2: Attributes.BonusInt = Utility.RandomMinMax(1, 75); break;
            }
			 switch (Utility.Random(3))
            {
                case 0: Attributes.BonusStr = Utility.RandomMinMax(1, 10); break;
                case 1: Attributes.BonusDex = Utility.RandomMinMax(1, 10); break;
                case 2: Attributes.BonusInt = Utility.RandomMinMax(1, 10); break;
            }
            switch (Utility.Random(5))
            {
                case 0: Attributes.WeaponDamage = Utility.RandomMinMax(1, 75); break;
                case 1: Attributes.WeaponSpeed = Utility.RandomMinMax(1, 75); break;
                case 2: Attributes.SpellDamage = Utility.RandomMinMax(1, 250); break;
                case 3: Attributes.CastRecovery = Utility.RandomMinMax(1, 10); break;
                case 4: Attributes.CastSpeed = Utility.RandomMinMax(1, 10); break;
            }
			switch (Utility.Random(3))
            {
                case 0: Attributes.WeaponDamage = Utility.RandomMinMax(1, 75); break;
                case 1: Attributes.WeaponSpeed = Utility.RandomMinMax(1, 75); break;
                case 2: Attributes.SpellDamage = Utility.RandomMinMax(1, 250); break;
			}
            switch (Utility.Random(6))
            {
                case 0: Attributes.LowerManaCost = Utility.RandomMinMax(1, 40); break;
                case 1: Attributes.LowerRegCost = Utility.RandomMinMax(1, 100); break;
                case 2: Attributes.ReflectPhysical = Utility.RandomMinMax(1, 200); break;
                case 3: Attributes.EnhancePotions = Utility.RandomMinMax(1, 100); break;
                case 4: Attributes.Luck = Utility.RandomMinMax(1, 2000); break;
                case 5: Attributes.NightSight = 1; break;
            }
            switch (Utility.Random(5))
            {
                case 0: Attributes.WeaponDamage = Utility.RandomMinMax(1, 75); break;
                case 1: Attributes.WeaponSpeed = Utility.RandomMinMax(1, 75); break;
                case 2: Attributes.SpellDamage = Utility.RandomMinMax(1, 150); break;
                case 3: Attributes.CastRecovery = Utility.RandomMinMax(1, 10); break;
                case 4: Attributes.CastSpeed = Utility.RandomMinMax(1, 10); break;
            }
			switch (Utility.Random(3))
            {
                case 0: Attributes.BonusHits = Utility.RandomMinMax(1, 100); break;
                case 1: Attributes.BonusStam = Utility.RandomMinMax(1, 100); break;
                case 2: Attributes.BonusMana = Utility.RandomMinMax(1, 100); break;
            }
			switch (Utility.Random(3))
            {
                case 0: Attributes.BonusHits = Utility.RandomMinMax(1, 200); break;
                case 1: Attributes.BonusStam = Utility.RandomMinMax(1, 200); break;
                case 2: Attributes.BonusMana = Utility.RandomMinMax(1, 200); break;
            }
			switch (Utility.Random(3))
            {
                case 0: Attributes.BonusHits = Utility.RandomMinMax(1, 150); break;
                case 1: Attributes.BonusStam = Utility.RandomMinMax(1, 150); break;
                case 2: Attributes.BonusMana = Utility.RandomMinMax(1, 150); break;
            }
            //Disadvantages
            // can be brittle 
            switch (Utility.Random(4))
            {
                case 0: Attributes.Brittle = 1; break;
            }
            // can be cursed 
            switch (Utility.Random(2)) { case 0: LootType = LootType.Cursed; break; }

            // can be unlucky
            switch (Utility.Random(2))
            {
                case 0: Attributes.Luck = Utility.RandomMinMax(-10000, -15000);break;
            }
        }
	 	public ArtifactHelm(Serial serial) : base( serial )
	 	{
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
