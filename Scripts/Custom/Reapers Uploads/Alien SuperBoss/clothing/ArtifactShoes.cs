using System;
using Server;

namespace Server.Items
{
    public class ArtifactShoes : Shoes
	{
        public override int ArtifactRarity { get { return Utility.RandomMinMax(1, 100); } }
				public override bool IsArtifact { get { return true; } }

         private static string[] m_Names = new string[]
		{
            "Test 111","Test 112","Test 113","Test 114","Test 115","Test 116","Test 117","Test 118","Test 119","Test 120",
			"Test 121","Test 122","Test 123","Test 124","Test 125","Test 126","Test 127","Test 128","Test 129","Test 130",
			"Test 131","Test 132","Test 133","Test 134","Test 135","Test 136","Test 137","Test 138","Test 139","Test 140",
			"Test 141","Test 142","Test 143","Test 144","Test 145","Test 146","Test 147","Test 148","Test 149","Test 150",
			"Test 151","Test 152","Test 153","Test 154","Test 155","Test 156","Test 157","Test 158","Test 159","Test 160"			

		};

        [Constructable]
        public ArtifactShoes()
        {
            Name = m_Names[Utility.Random(m_Names.Length)];
            Hue = Utility.RandomMinMax(5, 3000);
            //Name = "Shoes from another world";
			
            switch (Utility.Random(5))
            {
                case 0: ItemID = 5899; break;//boots
                case 1: ItemID = 5901; break;//sandals
                case 2: ItemID = 12228; break;//elvenboots
                case 3: ItemID = 5905; break;//thighboots
                case 4: ItemID = 5903; break;//shoes
             
            }
            // random chance to get these stats added to item ,chance of one stat per switch
            switch (Utility.Random(5))
            {
                case 0: Attributes.RegenHits = Utility.RandomMinMax(5, 20); break;
                case 1: Attributes.RegenStam = Utility.RandomMinMax(5, 20); break;
                case 2: Attributes.RegenStam = Utility.RandomMinMax(5, 20); break;
                case 3: Attributes.DefendChance = Utility.RandomMinMax(25, 35); break;
                case 4: Attributes.AttackChance = Utility.RandomMinMax(25, 35); break;
            }
			switch (Utility.Random(3))
            {
                case 0: Attributes.RegenHits = Utility.RandomMinMax(5, 20); break;
                case 1: Attributes.RegenStam = Utility.RandomMinMax(5, 20); break;
                case 2: Attributes.RegenStam = Utility.RandomMinMax(5, 20); break;
			}
			switch (Utility.Random(3))
            {
                case 0: Attributes.RegenHits = Utility.RandomMinMax(5, 20); break;
                case 1: Attributes.RegenStam = Utility.RandomMinMax(5, 20); break;
                case 2: Attributes.RegenStam = Utility.RandomMinMax(5, 20); break;
			}
			switch (Utility.Random(3))
            {
                case 0: Attributes.RegenHits = Utility.RandomMinMax(5, 20); break;
                case 1: Attributes.RegenStam = Utility.RandomMinMax(5, 20); break;
                case 2: Attributes.RegenStam = Utility.RandomMinMax(5, 20); break;
			}
            switch (Utility.Random(3))
            {
                case 0: Attributes.BonusStr = Utility.RandomMinMax(10, 50); break;
                case 1: Attributes.BonusDex = Utility.RandomMinMax(10, 50); break;
                case 2: Attributes.BonusInt = Utility.RandomMinMax(10, 50); break;
            }
			 switch (Utility.Random(3))
            {
                case 0: Attributes.BonusStr = Utility.RandomMinMax(50, 50); break;
                case 1: Attributes.BonusDex = Utility.RandomMinMax(50, 50); break;
                case 2: Attributes.BonusInt = Utility.RandomMinMax(50, 50); break;
            }
			switch (Utility.Random(4))
            {
                case 0: Attributes.BonusStr = Utility.RandomMinMax(10, 50); break;
                case 1: Attributes.BonusDex = Utility.RandomMinMax(10, 50); break;
                case 2: Attributes.BonusInt = Utility.RandomMinMax(10, 50); break;
            }
            switch (Utility.Random(5))
            {
                case 0: Attributes.WeaponDamage = Utility.RandomMinMax(10, 75); break;
                case 1: Attributes.WeaponSpeed = Utility.RandomMinMax(10, 75); break;
                case 2: Attributes.SpellDamage = Utility.RandomMinMax(10, 150); break;
                case 3: Attributes.CastRecovery = Utility.RandomMinMax(2, 10); break;
                case 4: Attributes.CastSpeed = Utility.RandomMinMax(2, 10); break;
            }
		    switch (Utility.Random(5))
            {
                case 0: Attributes.WeaponDamage = Utility.RandomMinMax(10, 75); break;
                case 1: Attributes.WeaponSpeed = Utility.RandomMinMax(10, 75); break;
                case 2: Attributes.SpellDamage = Utility.RandomMinMax(10, 150); break;
                case 3: Attributes.CastRecovery = Utility.RandomMinMax(2, 10); break;
                case 4: Attributes.CastSpeed = Utility.RandomMinMax(2, 10); break;
            }
            switch (Utility.Random(20))
            {
                case 0: Attributes.LowerManaCost = Utility.RandomMinMax(5, 50); break;
                case 1: Attributes.LowerRegCost = Utility.RandomMinMax(1, 100); break;
                case 2: Attributes.ReflectPhysical = Utility.RandomMinMax(5, 200); break;
                case 3: Attributes.EnhancePotions = Utility.RandomMinMax(10, 20); break;
                case 4: Attributes.Luck = Utility.RandomMinMax(1, 2000); break;
                case 5: Attributes.NightSight = 1; break;
            //    case 6: Attributes.SpellChanneling = 1; break;
            }
            switch (Utility.Random(5))
            {
                case 0: Attributes.WeaponDamage = Utility.RandomMinMax(10, 75); break;
                case 1: Attributes.WeaponSpeed = Utility.RandomMinMax(10, 75); break;
                case 2: Attributes.SpellDamage = Utility.RandomMinMax(5, 150); break;
                case 3: Attributes.CastRecovery = Utility.RandomMinMax(1, 10); break;
                case 4: Attributes.CastSpeed = Utility.RandomMinMax(1, 10); break;
            }


            // this will add 1 of these to every shoe sandal or boot
            switch (Utility.Random(21))
            {
                case 0: Attributes.WeaponDamage = Utility.RandomMinMax(10, 75); break;
                case 1: Attributes.WeaponSpeed = Utility.RandomMinMax(10, 75); break;
                case 2: Attributes.SpellDamage = Utility.RandomMinMax(5, 150); break;
                case 3: Attributes.CastRecovery = Utility.RandomMinMax(1, 10); break;
                case 4: Attributes.CastSpeed = Utility.RandomMinMax(1, 10); break;
                case 5: Attributes.LowerManaCost = Utility.RandomMinMax(5, 50); break;
                case 6: Attributes.LowerRegCost = Utility.RandomMinMax(10, 100); break;
                case 7: Attributes.ReflectPhysical = Utility.RandomMinMax(5, 200); break;
                case 8: Attributes.EnhancePotions = Utility.RandomMinMax(10, 20); break;
                case 9: Attributes.Luck = Utility.RandomMinMax(100, 2000); break;
                case 10: Attributes.BonusStr = Utility.RandomMinMax(10, 75); break;
                case 11: Attributes.BonusDex = Utility.RandomMinMax(10, 75); break;
                case 12: Attributes.BonusInt = Utility.RandomMinMax(10, 75); break;
                case 13: Attributes.BonusHits = Utility.RandomMinMax(10, 250); break;
                case 14: Attributes.BonusStam = Utility.RandomMinMax(10, 250); break;
                case 15: Attributes.BonusMana = Utility.RandomMinMax(10, 500); break;
                case 16: Attributes.RegenHits = Utility.RandomMinMax(1, 10); break;
                case 17: Attributes.RegenStam = Utility.RandomMinMax(1, 10); break;
                case 18: Attributes.RegenStam = Utility.RandomMinMax(1, 10); break;
                case 19: Attributes.DefendChance = Utility.RandomMinMax(10, 40); break;
                case 20: Attributes.AttackChance = Utility.RandomMinMax(10, 40); break;
            }
// random skill bonus
                      switch ( Utility.Random( 54 ))
            {
            case 0: SkillBonuses.SetValues( 1, SkillName.Alchemy, 10.0 ); break;
            case 1: SkillBonuses.SetValues( 1, SkillName.Anatomy, 10.0 ); break;
            case 2: SkillBonuses.SetValues( 1, SkillName.AnimalLore, 10.0 ); break;
            case 3: SkillBonuses.SetValues( 1, SkillName.ItemID, 10.0 ); break;
            case 4: SkillBonuses.SetValues( 1, SkillName.ArmsLore, 10.0 ); break;
            case 5: SkillBonuses.SetValues( 1, SkillName.Parry, 10.0 ); break;
            case 6: SkillBonuses.SetValues( 1, SkillName.Begging, 10.0 ); break;
            case 7: SkillBonuses.SetValues( 1, SkillName.Blacksmith, 10.0 ); break;
            case 8: SkillBonuses.SetValues( 1, SkillName.Fletching, 10.0 ); break;
            case 9: SkillBonuses.SetValues( 1, SkillName.Peacemaking, 10.0 ); break;
            case 10: SkillBonuses.SetValues( 1, SkillName.Camping, 10.0 ); break;
            case 11: SkillBonuses.SetValues( 1, SkillName.Carpentry, 10.0 ); break;
            case 12: SkillBonuses.SetValues( 1, SkillName.Cartography, 10.0 ); break;
            case 13: SkillBonuses.SetValues( 1, SkillName.Cooking, 10.0 ); break;
            case 14: SkillBonuses.SetValues( 1, SkillName.DetectHidden, 10.0 ); break;
            case 15: SkillBonuses.SetValues( 1, SkillName.Discordance, 10.0 ); break;
            case 16: SkillBonuses.SetValues( 1, SkillName.EvalInt, 10.0 ); break;
            case 17: SkillBonuses.SetValues( 1, SkillName.Healing, 10.0 ); break;
            case 18: SkillBonuses.SetValues( 1, SkillName.Fishing, 10.0 ); break;
            case 19: SkillBonuses.SetValues( 1, SkillName.Forensics, 10.0 ); break;
            case 20: SkillBonuses.SetValues(1, SkillName.Herding, 10.0); break;// lol has anyone ever used this skill before 
            case 21: SkillBonuses.SetValues( 1, SkillName.Hiding, 10.0 ); break;
            case 22: SkillBonuses.SetValues( 1, SkillName.Provocation, 10.0 ); break;
            case 23: SkillBonuses.SetValues( 1, SkillName.Inscribe, 10.0 ); break;
            case 24: SkillBonuses.SetValues( 1, SkillName.Lockpicking, 10.0 ); break;
            case 25: SkillBonuses.SetValues( 1, SkillName.Magery, 10.0 ); break;
            case 26: SkillBonuses.SetValues( 1, SkillName.MagicResist, 10.0 ); break;
            case 27: SkillBonuses.SetValues( 1, SkillName.Tactics, 10.0 ); break;
            case 28: SkillBonuses.SetValues( 0, SkillName.Snooping, 10.0 ); break;
            case 29: SkillBonuses.SetValues( 0, SkillName.Musicianship, 10.0 ); break;
            case 30: SkillBonuses.SetValues( 0, SkillName.Poisoning, 10.0 ); break;
            case 31: SkillBonuses.SetValues( 0, SkillName.Archery, 10.0 ); break;
            case 32: SkillBonuses.SetValues( 0, SkillName.SpiritSpeak, 10.0 ); break;
            case 33: SkillBonuses.SetValues( 0, SkillName.Stealing, 10.0 ); break;
            case 34: SkillBonuses.SetValues( 0, SkillName.Tailoring, 10.0 ); break;
            case 35: SkillBonuses.SetValues( 0, SkillName.AnimalTaming, 10.0 ); break;
            case 36: SkillBonuses.SetValues(0, SkillName.TasteID, 10.0); break;
            case 37: SkillBonuses.SetValues( 0, SkillName.Tinkering, 10.0 ); break;
            case 38: SkillBonuses.SetValues( 0, SkillName.Tracking, 10.0 ); break;
            case 39: SkillBonuses.SetValues( 0, SkillName.Veterinary, 10.0 ); break;
            case 40: SkillBonuses.SetValues( 0, SkillName.Swords, 10.0 ); break;
            case 41: SkillBonuses.SetValues( 0, SkillName.Macing, 10.0 ); break;
            case 42: SkillBonuses.SetValues( 0, SkillName.Fencing, 10.0 ); break;
            case 43: SkillBonuses.SetValues( 0, SkillName.Wrestling, 10.0 ); break;
            case 44: SkillBonuses.SetValues( 0, SkillName.Lumberjacking, 10.0 ); break;
            case 45: SkillBonuses.SetValues( 0, SkillName.Mining, 10.0 ); break;
            case 46: SkillBonuses.SetValues( 0, SkillName.Meditation, 10.0 ); break;
            case 47: SkillBonuses.SetValues( 0, SkillName.Stealth, 10.0 ); break;
            case 48: SkillBonuses.SetValues( 0, SkillName.RemoveTrap, 10.0 ); break;
            case 49: SkillBonuses.SetValues( 0, SkillName.Necromancy, 10.0 ); break;
            case 50: SkillBonuses.SetValues( 0, SkillName.Focus, 10.0 ); break;
            case 51: SkillBonuses.SetValues( 0, SkillName.Chivalry, 10.0 ); break;
            case 52: SkillBonuses.SetValues( 0, SkillName.Bushido, 10.0 ); break;
            case 53: SkillBonuses.SetValues( 0, SkillName.Ninjitsu, 10.0 ); break;
        }
		                      switch ( Utility.Random( 54 ))
            {
            case 0: SkillBonuses.SetValues( 0, SkillName.Alchemy, 10.0 ); break;
            case 1: SkillBonuses.SetValues( 0, SkillName.Anatomy, 10.0 ); break;
            case 2: SkillBonuses.SetValues( 0, SkillName.AnimalLore, 10.0 ); break;
            case 3: SkillBonuses.SetValues( 0, SkillName.ItemID, 10.0 ); break;
            case 4: SkillBonuses.SetValues( 0, SkillName.ArmsLore, 10.0 ); break;
            case 5: SkillBonuses.SetValues( 0, SkillName.Parry, 10.0 ); break;
            case 6: SkillBonuses.SetValues( 0, SkillName.Begging, 10.0 ); break;
            case 7: SkillBonuses.SetValues( 0, SkillName.Blacksmith, 10.0 ); break;
            case 8: SkillBonuses.SetValues( 0, SkillName.Fletching, 10.0 ); break;
            case 9: SkillBonuses.SetValues( 0, SkillName.Peacemaking, 10.0 ); break;
            case 10: SkillBonuses.SetValues( 0, SkillName.Camping, 10.0 ); break;
            case 11: SkillBonuses.SetValues( 0, SkillName.Carpentry, 10.0 ); break;
            case 12: SkillBonuses.SetValues( 0, SkillName.Cartography, 10.0 ); break;
            case 13: SkillBonuses.SetValues( 0, SkillName.Cooking, 10.0 ); break;
            case 14: SkillBonuses.SetValues( 0, SkillName.DetectHidden, 10.0 ); break;
            case 15: SkillBonuses.SetValues( 0, SkillName.Discordance, 10.0 ); break;
            case 16: SkillBonuses.SetValues( 0, SkillName.EvalInt, 10.0 ); break;
            case 17: SkillBonuses.SetValues( 0, SkillName.Healing, 10.0 ); break;
            case 18: SkillBonuses.SetValues( 0, SkillName.Fishing, 10.0 ); break;
            case 19: SkillBonuses.SetValues( 0, SkillName.Forensics, 10.0 ); break;
            case 20: SkillBonuses.SetValues(0, SkillName.Herding, 10.0); break;// lol has anyone ever used this skill before 
            case 21: SkillBonuses.SetValues( 0, SkillName.Hiding, 10.0 ); break;
            case 22: SkillBonuses.SetValues( 0, SkillName.Provocation, 10.0 ); break;
            case 23: SkillBonuses.SetValues( 0, SkillName.Inscribe, 10.0 ); break;
            case 24: SkillBonuses.SetValues( 0, SkillName.Lockpicking, 10.0 ); break;
            case 25: SkillBonuses.SetValues( 0, SkillName.Magery, 10.0 ); break;
            case 26: SkillBonuses.SetValues( 0, SkillName.MagicResist, 10.0 ); break;
            case 27: SkillBonuses.SetValues( 0, SkillName.Tactics, 10.0 ); break;
            case 28: SkillBonuses.SetValues( 1, SkillName.Snooping, 10.0 ); break;
            case 29: SkillBonuses.SetValues( 1, SkillName.Musicianship, 10.0 ); break;
            case 30: SkillBonuses.SetValues( 1, SkillName.Poisoning, 10.0 ); break;
            case 31: SkillBonuses.SetValues( 1, SkillName.Archery, 10.0 ); break;
            case 32: SkillBonuses.SetValues( 1, SkillName.SpiritSpeak, 10.0 ); break;
            case 33: SkillBonuses.SetValues( 1, SkillName.Stealing, 10.0 ); break;
            case 34: SkillBonuses.SetValues( 1, SkillName.Tailoring, 10.0 ); break;
            case 35: SkillBonuses.SetValues( 1, SkillName.AnimalTaming, 10.0 ); break;
            case 36: SkillBonuses.SetValues(1, SkillName.TasteID, 10.0); break;
            case 37: SkillBonuses.SetValues( 1, SkillName.Tinkering, 10.0 ); break;
            case 38: SkillBonuses.SetValues( 1, SkillName.Tracking, 10.0 ); break;
            case 39: SkillBonuses.SetValues( 1, SkillName.Veterinary, 10.0 ); break;
            case 40: SkillBonuses.SetValues( 1, SkillName.Swords, 10.0 ); break;
            case 41: SkillBonuses.SetValues( 1, SkillName.Macing, 10.0 ); break;
            case 42: SkillBonuses.SetValues( 1, SkillName.Fencing, 10.0 ); break;
            case 43: SkillBonuses.SetValues( 1, SkillName.Wrestling, 10.0 ); break;
            case 44: SkillBonuses.SetValues( 1, SkillName.Lumberjacking, 10.0 ); break;
            case 45: SkillBonuses.SetValues( 1, SkillName.Mining, 10.0 ); break;
            case 46: SkillBonuses.SetValues( 1, SkillName.Meditation, 10.0 ); break;
            case 47: SkillBonuses.SetValues( 1, SkillName.Stealth, 10.0 ); break;
            case 48: SkillBonuses.SetValues( 1, SkillName.RemoveTrap, 10.0 ); break;
            case 49: SkillBonuses.SetValues( 1, SkillName.Necromancy, 10.0 ); break;
            case 50: SkillBonuses.SetValues( 1, SkillName.Focus, 10.0 ); break;
            case 51: SkillBonuses.SetValues( 1, SkillName.Chivalry, 10.0 ); break;
            case 52: SkillBonuses.SetValues( 1, SkillName.Bushido, 10.0 ); break;
            case 53: SkillBonuses.SetValues( 1, SkillName.Ninjitsu, 10.0 ); break;
        }
// END random skill bonus
     //Disadvantages
                      // LRC BONUS
                      switch (Utility.Random(2))
                      {
                          case 0: Attributes.LowerRegCost = Utility.RandomMinMax(10, 100); break;
                      }
                      // can be brittle 
                      switch (Utility.Random(2))
                      {
                          case 0: Attributes.Brittle = 1; break;
                      }
                      // can be cursed 
                      switch (Utility.Random(2)) { case 0: LootType = LootType.Cursed; break; }

                      // can be unlucky
                      switch (Utility.Random(2))
                      {
                          case 0: Attributes.Luck = -10000; break;
                      }
        }

	 	public ArtifactShoes(Serial serial) : base( serial )
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

