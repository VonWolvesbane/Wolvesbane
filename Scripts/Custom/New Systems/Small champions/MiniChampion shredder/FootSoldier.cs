// Created by ReApEr
using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of A Foot Soldier")]
    public class FootSoldier : BaseCreature
    {
        [Constructable]
        public FootSoldier()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            Name = "A Foot Soldier";
            Body = 400;
            Female = false;

            SetStr(500, 700);
            SetDex(215, 220);
            SetInt(215, 220);

            SetHits(1200, 1750);

            SetDamage(45, 55);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 0, 0);
            SetResistance(ResistanceType.Fire, 0, 0);
            SetResistance(ResistanceType.Poison, 0, 0);
            SetResistance(ResistanceType.Energy, 0, 0);

            SetSkill(SkillName.Tactics, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 97.5);
            SetSkill(SkillName.Anatomy, 40, 60);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Focus, 120.0);
            SetSkill(SkillName.Macing, 90.0, 100.0);

            Fame = 25000;
            Karma = 25000;

            VirtualArmor = 40;

			PackGold( 20000, 30000 );
			
			Bandana Bandana = new Bandana();
			Bandana.Hue = 1157;
			Bandana.Layer = Layer.Talisman;
			Bandana.Movable = false;
			AddItem(Bandana);
			
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
			
			LeatherNinjaHood Hat = new LeatherNinjaHood();
			Hat.Hue = 1109;
            Hat.Movable = false;
            AddItem(Hat);			
			
			Boots Feet = new Boots();
			Feet.Hue = 1154;
            Feet.Movable = false;
            AddItem(Feet);		

			Nunchaku Weapon = new Nunchaku();
            Weapon.Movable = false;
            AddItem(Weapon);				
			

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
			if ( Utility.RandomDouble() < 0.05 )
            switch (Utility.Random(5))
            {
                case 0: PackItem(new FootSoldiersArms()); break;
				case 1: PackItem(new FootSoldiersChest()); break;
                case 2: PackItem(new FootSoldiersLegs()); break;
                case 3: PackItem(new FootSoldiersGloves()); break;
                    }
            }
        public FootSoldier(Serial serial)
            : base(serial)
        {
        }
		        public override bool ShowFameTitle
        {
            get
            {
                return false;
            }
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
