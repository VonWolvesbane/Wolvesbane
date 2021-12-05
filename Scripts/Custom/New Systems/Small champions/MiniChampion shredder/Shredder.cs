// Created by ReApEr
using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of Shredder")]
    public class Shredder : BaseMiniChampion
    {
        [Constructable]
        public Shredder()
            : base(AIType.AI_Melee)
        {

            Name = "Shredder";
            Body = 400;
            Female = false;

            SetStr(1350, 2400);
            SetDex(1150, 1200);
            SetInt(1150, 1200);

            SetHits(75000, 125000);

            SetDamage(35, 45);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 0, 65);
            SetResistance(ResistanceType.Fire, 0, 65);
            SetResistance(ResistanceType.Poison, 0, 65);
            SetResistance(ResistanceType.Energy, 0, 60);

            SetSkill(SkillName.Tactics, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 97.5);
            SetSkill(SkillName.Anatomy, 40, 60);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Focus, 120.0);
            SetSkill(SkillName.Fencing, 190.0, 200.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 40;

			PackGold( 30000, 50000 );
			
			
			AssassinChest Chest = new AssassinChest();
			Chest.Hue = 1;
			Chest.Movable = false;
			AddItem(Chest);
			
			AssassinArms Arms = new AssassinArms();
			Arms.Hue = 1;
			Arms.Movable = false;
			AddItem(Arms);
			
			AssassinLegs Legs = new AssassinLegs();
			Legs.Hue = 1;
			Legs.Movable = false;
			AddItem(Legs);

			AssassinGloves Gloves = new AssassinGloves();
			Gloves.Hue = 1154;
			Gloves.Movable = false;
			AddItem(Gloves);
			
			LeatherNinjaBelt Belt = new LeatherNinjaBelt();
			Belt.Hue = 1154;
            Belt.Movable = false;
            AddItem(Belt);
			
			PlateMempo Neck = new PlateMempo();
			Neck.Hue = 1109;
            Neck.Movable = false;
            AddItem(Neck);
			
			PlateBattleKabuto Hat = new PlateBattleKabuto();
			Hat.Hue = 1109;
            Hat.Movable = false;
            AddItem(Hat);			
			
			Tekagi Weapon = new Tekagi();
			Weapon.Hue = 1154;
            Weapon.Movable = false;
            AddItem(Weapon);	
			
			Boots Feet = new Boots();
			Feet.Hue = 1154;
            Feet.Movable = false;
            AddItem(Feet);			
			

	  }
		public override bool CanBeParagon 
		{ 
			get 
		{ 
			return false; 
			} 
		}
		
        public override void GenerateLoot()
        {
			if ( Utility.RandomDouble() < 0.5 )
            switch (Utility.Random(4))
            {
                case 0: PackItem(new FootSoldiersArms()); break;
				case 1: PackItem(new FootSoldiersChest()); break;
                case 2: PackItem(new FootSoldiersLegs()); break;
                case 3: PackItem(new FootSoldiersGloves()); break;
                    }
            }
        public Shredder(Serial serial)
            : base(serial)
        {
        }
		public override bool AlwaysMurderer { get { return true; } }
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
