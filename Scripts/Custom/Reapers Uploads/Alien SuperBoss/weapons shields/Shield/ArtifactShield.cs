using System;
using Server;

namespace Server.Items
{
    public class ArtifactShield : HeaterShield
	{
        public override int ArtifactRarity { get { return Utility.RandomMinMax(1, 100); } }

	 	public override int InitMinHits{ get{ return 255; } }
	 	public override int InitMaxHits{ get{ return 255; } }

         private static string[] m_Names = new string[]
		{
            "Test 061","Test 062","Test 003","Test 004","Test 005","Test 006","Test 007","Test 008","Test 009","Test 010",
			"Test 011","Test 012","Test 013","Test 014","Test 015","Test 016","Test 017","Test 018","Test 019","Test 020",
			"Test 021","Test 072","Test 073","Test 024","Test 025","Test 096","Test 027","Test 028","Test 099","Test 030",
			"Test 031","Test 032","Test 033","Test 074","Test 095","Test 036","Test 037","Test 038","Test 039","Test 040",
			"Test 041","Test 042","Test 043","Test 044","Test 075","Test 046","Test 047","Test 048","Test 049","Test 090"			

		};

        [Constructable]
        public ArtifactShield()
        {
           Name = m_Names[Utility.Random(m_Names.Length)]; //Want Random names? 
           // Name = "Shield from another world";
            Hue = Utility.RandomMinMax(1, 3000);

            // random chance to get diffent resist %
            PhysicalBonus = Utility.RandomMinMax(5, 15);
            FireBonus = Utility.RandomMinMax(5, 15);
            ColdBonus = Utility.RandomMinMax(5, 15);
            PoisonBonus = Utility.RandomMinMax(5, 15);
            EnergyBonus = Utility.RandomMinMax(5, 15);

            // id it shows item as
            switch (Utility.Random(6))
            {
                case 0: ItemID = 7032; break;//arcaneshield
                case 1: ItemID = 7028; break;//metalshield
                case 2: ItemID = 7026; break;//bronzeshield
                case 3: ItemID = 7107; break;//chaosshield
                case 4: ItemID = 7108; break;//ordershield
                case 5: ItemID = 7030; break;//heatershield
            }
            // random chance to get these stats added to item ,chance of one stat per switch
            switch (Utility.Random(5))
            {
                case 0: Attributes.RegenHits = Utility.RandomMinMax(1, 30); break;
                case 1: Attributes.RegenStam = Utility.RandomMinMax(1, 30); break;
                case 2: Attributes.RegenStam = Utility.RandomMinMax(1, 30); break;
                case 3: Attributes.DefendChance = Utility.RandomMinMax(10, 40); break;
                case 4: Attributes.AttackChance = Utility.RandomMinMax(10, 40); break;
            }
			switch (Utility.Random(2))
            {
			    case 0: Attributes.DefendChance = Utility.RandomMinMax(10, 40); break;
                case 1: Attributes.AttackChance = Utility.RandomMinMax(10, 40); break;
            }
			 switch (Utility.Random(2))
            {
			    case 0: Attributes.DefendChance = Utility.RandomMinMax(10, 40); break;
                case 1: Attributes.AttackChance = Utility.RandomMinMax(10, 40); break;
            }
            switch (Utility.Random(6))
            {
                case 0: Attributes.BonusStr = Utility.RandomMinMax(1, 100); break;
                case 1: Attributes.BonusDex = Utility.RandomMinMax(1, 100); break;
                case 2: Attributes.BonusInt = Utility.RandomMinMax(1, 100); break;
                case 3: Attributes.BonusHits = Utility.RandomMinMax(10, 200); break;
                case 4: Attributes.BonusStam = Utility.RandomMinMax(10, 500); break;
                case 5: Attributes.BonusMana = Utility.RandomMinMax(10, 500); break;
            }
		    switch (Utility.Random(6))
            {
                case 0: Attributes.BonusStr = Utility.RandomMinMax(1, 100); break;
                case 1: Attributes.BonusDex = Utility.RandomMinMax(1, 100); break;
                case 2: Attributes.BonusInt = Utility.RandomMinMax(1, 100); break;
			}
			switch (Utility.Random(6))
            {
                case 0: Attributes.BonusStr = Utility.RandomMinMax(1, 100); break;
                case 1: Attributes.BonusDex = Utility.RandomMinMax(1, 100); break;
                case 2: Attributes.BonusInt = Utility.RandomMinMax(1, 100); break;
			}
			 switch (Utility.Random(4))
            {
                case 0: Attributes.BonusHits = Utility.RandomMinMax(10, 150); break;
                case 1: Attributes.BonusStam = Utility.RandomMinMax(10, 150); break;
                case 2: Attributes.BonusMana = Utility.RandomMinMax(10, 150); break;
			}
			switch (Utility.Random(4))
            {
                case 0: Attributes.BonusHits = Utility.RandomMinMax(10, 150); break;
                case 1: Attributes.BonusStam = Utility.RandomMinMax(10, 150); break;
                case 2: Attributes.BonusMana = Utility.RandomMinMax(10, 150); break;
			}
            switch (Utility.Random(5))
            {
                case 0: Attributes.WeaponDamage = Utility.RandomMinMax(10, 75); break;
                case 1: Attributes.WeaponSpeed = Utility.RandomMinMax(10, 75); break;
                case 2: Attributes.SpellDamage = Utility.RandomMinMax(10, 150); break;
                case 3: Attributes.CastRecovery = Utility.RandomMinMax(1, 10); break;
                case 4: Attributes.CastSpeed = Utility.RandomMinMax(1, 10); break;
            }
			 switch (Utility.Random(2))
            {
                case 0: Attributes.WeaponDamage = Utility.RandomMinMax(10, 75); break;
                case 1: Attributes.WeaponSpeed = Utility.RandomMinMax(10, 75); break;
			}
            switch (Utility.Random(6))
            {
                case 0: Attributes.LowerManaCost = Utility.RandomMinMax(5, 40); break;
                case 1: Attributes.LowerRegCost = Utility.RandomMinMax(10, 100); break;
                case 2: Attributes.ReflectPhysical = Utility.RandomMinMax(5, 200); break;
                case 3: Attributes.EnhancePotions = Utility.RandomMinMax(10, 20); break;
                case 4: Attributes.Luck = Utility.RandomMinMax(100, 2000); break;
                case 5: Attributes.NightSight = 1; break;
            }
            switch (Utility.Random(5))
            {
                case 0: Attributes.WeaponDamage = Utility.RandomMinMax(10, 75); break;
                case 1: Attributes.WeaponSpeed = Utility.RandomMinMax(10, 75); break;
                case 2: Attributes.SpellDamage = Utility.RandomMinMax(5, 125); break;
                case 3: Attributes.CastRecovery = Utility.RandomMinMax(1, 10); break;
                case 4: Attributes.CastSpeed = Utility.RandomMinMax(1, 10); break;
            }
			switch (Utility.Random(5))
            {
			    case 0: Attributes.SpellDamage = Utility.RandomMinMax(5, 125); break;
                case 1: Attributes.CastRecovery = Utility.RandomMinMax(1, 10); break;
                case 2: Attributes.CastSpeed = Utility.RandomMinMax(1, 10); break;
            }
			switch (Utility.Random(5))
            {
			    case 0: Attributes.SpellDamage = Utility.RandomMinMax(5, 125); break;
                case 1: Attributes.CastRecovery = Utility.RandomMinMax(1, 10); break;
                case 2: Attributes.CastSpeed = Utility.RandomMinMax(1, 10); break;
            }
// chance of being spellchanneling 
            switch (Utility.Random(2))
            {
             case 0: Attributes.SpellChanneling = 1; break;
            }
            //Disadvantages
            // can be brittle 
            switch (Utility.Random(5))
            {
                case 0: Attributes.Brittle = 1; break;
            }
            // can be cursed 
            switch (Utility.Random(2)) { case 0: LootType = LootType.Cursed; break; }

            // can be unlucky
            switch (Utility.Random(4))
            {
                case 0: Attributes.Luck = -10000; break;
            }
        }

	 	public ArtifactShield(Serial serial) : base( serial )
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
