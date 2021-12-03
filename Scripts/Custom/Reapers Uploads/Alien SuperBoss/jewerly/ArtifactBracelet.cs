using System;
using Server;

namespace Server.Items
{
    public class ArtifactBracelet : GoldBracelet
	{

        public override int ArtifactRarity { get { return Utility.RandomMinMax(1, 100); } }
		public override bool IsArtifact { get { return true; } }
	 	public override int InitMinHits{ get{ return 255; } }
	 	public override int InitMaxHits{ get{ return 255; } }

         private static string[] m_Names = new string[]
		{
            "Test 999","Test 950","Test 961","Test 874","Test 864","Test 833","Test 748","Test 732","Test 741","Test 478",
			"Test 998","Test 951","Test 963","Test 875","Test 866","Test 835","Test 741","Test 763","Test 748","Test 482",
			"Test 997","Test 957","Test 969","Test 873","Test 868","Test 836","Test 745","Test 767","Test 742","Test 477",
			"Test 995","Test 956","Test 967","Test 870","Test 861","Test 839","Test 749","Test 766","Test 744","Test 498",
			"Test 994","Test 958","Test 948","Test 888","Test 867","Test 834","Test 743","Test 769","Test 747","Test 458"			

		};

        [Constructable]
        public ArtifactBracelet()
        {
            Name = m_Names[Utility.Random(m_Names.Length)];
            Hue = Utility.RandomMinMax(5, 3000);
			//Name = "EarRing from another world";

            // random chance to get diffent resist %
            Resistances.Physical = Utility.RandomMinMax(0, 15);
            Resistances.Fire = Utility.RandomMinMax(0, 15);
            Resistances.Cold = Utility.RandomMinMax(0, 15);
            Resistances.Poison = Utility.RandomMinMax(0, 15);
            Resistances.Energy = Utility.RandomMinMax(0, 15);
          
            // id it shows item as
            switch (Utility.Random(2))
            {
                case 0: ItemID = 4230; break;//gold
                case 1: ItemID = 7942; break;// silver
            }
            // random chance to get these stats added to item ,chance of one stat per switch
            switch (Utility.Random(5))
            {
                case 0: Attributes.RegenHits = Utility.RandomMinMax(5, 25); break;
                case 1: Attributes.RegenStam = Utility.RandomMinMax(5, 25); break;
                case 2: Attributes.RegenStam = Utility.RandomMinMax(5, 25); break;
                case 3: Attributes.DefendChance = Utility.RandomMinMax(25, 35); break;
                case 4: Attributes.AttackChance = Utility.RandomMinMax(25, 35); break;
            }
			 switch (Utility.Random(5))
            {
                case 0: Attributes.RegenHits = Utility.RandomMinMax(5, 25); break;
                case 1: Attributes.RegenStam = Utility.RandomMinMax(5, 25); break;
                case 2: Attributes.RegenStam = Utility.RandomMinMax(5, 25); break;
			}
				 switch (Utility.Random(5))
            {
                case 0: Attributes.RegenHits = Utility.RandomMinMax(5, 25); break;
                case 1: Attributes.RegenStam = Utility.RandomMinMax(5, 25); break;
                case 2: Attributes.RegenStam = Utility.RandomMinMax(5, 25); break;
			}
			            switch (Utility.Random(3))
            {
                case 0: Attributes.RegenHits = Utility.RandomMinMax(5, 10); break;
                case 1: Attributes.RegenStam = Utility.RandomMinMax(5, 10); break;
                case 2: Attributes.RegenStam = Utility.RandomMinMax(5, 10); break;
			}
			            switch (Utility.Random(3))
            {
                case 0: Attributes.RegenHits = Utility.RandomMinMax(5, 10); break;
                case 1: Attributes.RegenStam = Utility.RandomMinMax(5, 10); break;
                case 2: Attributes.RegenStam = Utility.RandomMinMax(5, 10); break;
			}
            switch (Utility.Random(3))
            {
                case 0: Attributes.BonusStr = Utility.RandomMinMax(10, 50); break;
                case 1: Attributes.BonusDex = Utility.RandomMinMax(10, 50); break;
                case 2: Attributes.BonusInt = Utility.RandomMinMax(10, 50); break;
            }
			 switch (Utility.Random(3))
            {
                case 0: Attributes.BonusStr = Utility.RandomMinMax(10, 50); break;
                case 1: Attributes.BonusDex = Utility.RandomMinMax(10, 50); break;
                case 2: Attributes.BonusInt = Utility.RandomMinMax(10, 50); break;
            }
			 switch (Utility.Random(3))
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
                case 2: Attributes.SpellDamage = Utility.RandomMinMax(5, 150); break;
                case 3: Attributes.CastRecovery = Utility.RandomMinMax(1, 10); break;
                case 4: Attributes.CastSpeed = Utility.RandomMinMax(1, 10); break;
            }
			switch (Utility.Random(3))
            {
                case 0: Attributes.SpellDamage = Utility.RandomMinMax(5, 150); break;
                case 1: Attributes.CastRecovery = Utility.RandomMinMax(1, 10); break;
                case 2: Attributes.CastSpeed = Utility.RandomMinMax(1, 10); break;
			}
			switch (Utility.Random(3))
            {
                case 0: Attributes.BonusHits = Utility.RandomMinMax(1, 150); break;
                case 1: Attributes.BonusStam = Utility.RandomMinMax(1, 150); break;
                case 2: Attributes.BonusMana = Utility.RandomMinMax(1, 150); break;
			}
			switch (Utility.Random(3))
            {
                case 0: Attributes.BonusHits = Utility.RandomMinMax(1, 100); break;
                case 1: Attributes.BonusStam = Utility.RandomMinMax(1, 100); break;
                case 2: Attributes.BonusMana = Utility.RandomMinMax(1, 100); break;
			}
            // this will add 1 of these
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
                case 10: Attributes.BonusStr = Utility.RandomMinMax(1, 75); break;
                case 11: Attributes.BonusDex = Utility.RandomMinMax(1, 75); break;
                case 12: Attributes.BonusInt = Utility.RandomMinMax(1, 75); break;
                case 13: Attributes.BonusHits = Utility.RandomMinMax(1, 100); break;
                case 14: Attributes.BonusStam = Utility.RandomMinMax(1, 100); break;
                case 15: Attributes.BonusMana = Utility.RandomMinMax(1, 100); break;
                case 16: Attributes.RegenHits = Utility.RandomMinMax(1, 10); break;
                case 17: Attributes.RegenStam = Utility.RandomMinMax(1, 10); break;
                case 18: Attributes.RegenStam = Utility.RandomMinMax(1, 10); break;
                case 19: Attributes.DefendChance = Utility.RandomMinMax(10, 40); break;
                case 20: Attributes.AttackChance = Utility.RandomMinMax(10, 40); break;
            }
// random skill bonus
                      switch ( Utility.Random( 54 ))
            {
            case 0: SkillBonuses.SetValues( 0, SkillName.Alchemy, 15.0 ); break;
            case 1: SkillBonuses.SetValues( 0, SkillName.Anatomy, 15.0 ); break;
            case 2: SkillBonuses.SetValues( 0, SkillName.AnimalLore, 15.0 ); break;
            case 3: SkillBonuses.SetValues( 0, SkillName.ItemID, 15.0 ); break;
            case 4: SkillBonuses.SetValues( 0, SkillName.ArmsLore, 15.0 ); break;
            case 5: SkillBonuses.SetValues( 0, SkillName.Parry, 15.0 ); break;
            case 6: SkillBonuses.SetValues( 0, SkillName.Begging, 15.0 ); break;
            case 7: SkillBonuses.SetValues( 0, SkillName.Blacksmith, 15.0 ); break;
            case 8: SkillBonuses.SetValues( 0, SkillName.Fletching, 15.0 ); break;
            case 9: SkillBonuses.SetValues( 0, SkillName.Peacemaking, 15.0 ); break;
            case 10: SkillBonuses.SetValues( 0, SkillName.Camping, 15.0 ); break;
            case 11: SkillBonuses.SetValues( 0, SkillName.Carpentry, 15.0 ); break;
            case 12: SkillBonuses.SetValues( 0, SkillName.Cartography, 15.0 ); break;
            case 13: SkillBonuses.SetValues( 0, SkillName.Cooking, 15.0 ); break;
            case 14: SkillBonuses.SetValues( 0, SkillName.DetectHidden, 15.0 ); break;
            case 15: SkillBonuses.SetValues( 0, SkillName.Discordance, 15.0 ); break;
            case 16: SkillBonuses.SetValues( 0, SkillName.EvalInt, 15.0 ); break;
            case 17: SkillBonuses.SetValues( 0, SkillName.Healing, 15.0 ); break;
            case 18: SkillBonuses.SetValues( 0, SkillName.Fishing, 15.0 ); break;
            case 19: SkillBonuses.SetValues( 0, SkillName.Forensics, 15.0 ); break;
            case 20: SkillBonuses.SetValues(0, SkillName.Herding, 15.0); break;
            case 21: SkillBonuses.SetValues( 0, SkillName.Hiding, 15.0 ); break;
            case 22: SkillBonuses.SetValues( 0, SkillName.Provocation, 15.0 ); break;
            case 23: SkillBonuses.SetValues( 0, SkillName.Inscribe, 15.0 ); break;
            case 24: SkillBonuses.SetValues( 0, SkillName.Lockpicking, 15.0 ); break;
            case 25: SkillBonuses.SetValues( 0, SkillName.Magery, 15.0 ); break;
            case 26: SkillBonuses.SetValues( 0, SkillName.MagicResist, 15.0 ); break;
            case 27: SkillBonuses.SetValues( 0, SkillName.Tactics, 15.0 ); break;
            case 28: SkillBonuses.SetValues( 0, SkillName.Snooping, 15.0 ); break;
            case 29: SkillBonuses.SetValues( 0, SkillName.Musicianship, 15.0 ); break;
            case 30: SkillBonuses.SetValues( 0, SkillName.Poisoning, 15.0 ); break;
            case 31: SkillBonuses.SetValues( 0, SkillName.Archery, 15.0 ); break;
            case 32: SkillBonuses.SetValues( 0, SkillName.SpiritSpeak, 15.0 ); break;
            case 33: SkillBonuses.SetValues( 0, SkillName.Stealing, 15.0 ); break;
            case 34: SkillBonuses.SetValues( 0, SkillName.Tailoring, 15.0 ); break;
            case 35: SkillBonuses.SetValues( 0, SkillName.AnimalTaming, 15.0 ); break;
            case 36: SkillBonuses.SetValues(0, SkillName.TasteID, 15.0); break;
            case 37: SkillBonuses.SetValues( 0, SkillName.Tinkering, 15.0 ); break;
            case 38: SkillBonuses.SetValues( 0, SkillName.Tracking, 15.0 ); break;
            case 39: SkillBonuses.SetValues( 0, SkillName.Veterinary, 15.0 ); break;
            case 40: SkillBonuses.SetValues( 0, SkillName.Swords, 15.0 ); break;
            case 41: SkillBonuses.SetValues( 0, SkillName.Macing, 15.0 ); break;
            case 42: SkillBonuses.SetValues( 0, SkillName.Fencing, 15.0 ); break;
            case 43: SkillBonuses.SetValues( 0, SkillName.Wrestling, 15.0 ); break;
            case 44: SkillBonuses.SetValues( 0, SkillName.Lumberjacking, 15.0 ); break;
            case 45: SkillBonuses.SetValues( 0, SkillName.Mining, 15.0 ); break;
            case 46: SkillBonuses.SetValues( 0, SkillName.Meditation, 15.0 ); break;
            case 47: SkillBonuses.SetValues( 0, SkillName.Stealth, 15.0 ); break;
            case 48: SkillBonuses.SetValues( 0, SkillName.RemoveTrap, 15.0 ); break;
            case 49: SkillBonuses.SetValues( 0, SkillName.Necromancy, 15.0 ); break;
            case 50: SkillBonuses.SetValues( 0, SkillName.Focus, 15.0 ); break;
            case 51: SkillBonuses.SetValues( 0, SkillName.Chivalry, 15.0 ); break;
            case 52: SkillBonuses.SetValues( 0, SkillName.Bushido, 15.0 ); break;
            case 53: SkillBonuses.SetValues( 0, SkillName.Ninjitsu, 15.0 ); break;
			}
		    switch ( Utility.Random( 42 ))
            {
            case 0: SkillBonuses.SetValues( 1, SkillName.Anatomy, 10.0 ); break;
            case 1: SkillBonuses.SetValues( 1, SkillName.AnimalLore, 10.0 ); break;
            case 2: SkillBonuses.SetValues( 1, SkillName.ArmsLore, 10.0 ); break;
            case 3: SkillBonuses.SetValues( 1, SkillName.Parry, 10.0 ); break;
            case 4: SkillBonuses.SetValues( 1, SkillName.Blacksmith, 10.0 ); break;
            case 5: SkillBonuses.SetValues( 1, SkillName.Fletching, 10.0 ); break;
            case 6: SkillBonuses.SetValues( 1, SkillName.Peacemaking, 10.0 ); break;
            case 7: SkillBonuses.SetValues( 1, SkillName.Carpentry, 10.0 ); break;
            case 8: SkillBonuses.SetValues( 1, SkillName.Cartography, 10.0 ); break;
            case 9: SkillBonuses.SetValues( 1, SkillName.Discordance, 10.0 ); break;
            case 10: SkillBonuses.SetValues( 1, SkillName.EvalInt, 10.0 ); break;
            case 11: SkillBonuses.SetValues( 1, SkillName.Healing, 10.0 ); break;
            case 12: SkillBonuses.SetValues( 1, SkillName.Fishing, 10.0 ); break;
            case 13: SkillBonuses.SetValues( 1, SkillName.Hiding, 10.0 ); break;
            case 14: SkillBonuses.SetValues( 1, SkillName.Provocation, 10.0 ); break;
            case 15: SkillBonuses.SetValues( 1, SkillName.Inscribe, 10.0 ); break;
            case 16: SkillBonuses.SetValues( 1, SkillName.Lockpicking, 10.0 ); break;
            case 17: SkillBonuses.SetValues( 1, SkillName.Magery, 10.0 ); break;
            case 18: SkillBonuses.SetValues( 1, SkillName.MagicResist, 10.0 ); break;
            case 19: SkillBonuses.SetValues( 1, SkillName.Tactics, 10.0 ); break;
            case 20: SkillBonuses.SetValues( 1, SkillName.Musicianship, 10.0 ); break;
            case 21: SkillBonuses.SetValues( 1, SkillName.Poisoning, 10.0 ); break;
            case 22: SkillBonuses.SetValues( 1, SkillName.Archery, 10.0 ); break;
            case 23: SkillBonuses.SetValues( 1, SkillName.SpiritSpeak, 10.0 ); break;
            case 24: SkillBonuses.SetValues( 1, SkillName.Stealing, 10.0 ); break;
            case 25: SkillBonuses.SetValues( 1, SkillName.Tailoring, 10.0 ); break;
            case 26: SkillBonuses.SetValues( 1, SkillName.AnimalTaming, 10.0 ); break;
            case 27: SkillBonuses.SetValues( 1, SkillName.Tinkering, 10.0 ); break;
            case 28: SkillBonuses.SetValues( 1, SkillName.Veterinary, 10.0 ); break;
            case 29: SkillBonuses.SetValues( 1, SkillName.Swords, 10.0 ); break;
            case 30: SkillBonuses.SetValues( 1, SkillName.Macing, 10.0 ); break;
            case 31: SkillBonuses.SetValues( 1, SkillName.Fencing, 10.0 ); break;
            case 32: SkillBonuses.SetValues( 1, SkillName.Wrestling, 10.0 ); break;
            case 33: SkillBonuses.SetValues( 1, SkillName.Lumberjacking, 10.0 ); break;
            case 34: SkillBonuses.SetValues( 1, SkillName.Mining, 10.0 ); break;
            case 35: SkillBonuses.SetValues( 1, SkillName.Meditation, 10.0 ); break;
            case 36: SkillBonuses.SetValues( 1, SkillName.Stealth, 10.0 ); break;
            case 37: SkillBonuses.SetValues( 1, SkillName.Necromancy, 10.0 ); break;
            case 38: SkillBonuses.SetValues( 1, SkillName.Focus, 10.0 ); break;
            case 39: SkillBonuses.SetValues( 1, SkillName.Chivalry, 10.0 ); break;
            case 40: SkillBonuses.SetValues( 1, SkillName.Bushido, 10.0 ); break;
            case 41: SkillBonuses.SetValues( 1, SkillName.Ninjitsu, 10.0 ); break;
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

	 	public ArtifactBracelet(Serial serial) : base( serial )
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

