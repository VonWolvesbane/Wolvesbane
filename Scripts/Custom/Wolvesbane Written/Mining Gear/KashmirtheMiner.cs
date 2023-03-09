// Created by Nept
using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Master of Mining")]
    public class Kashmir : BaseCreature
    {
        [Constructable]
        public Kashmir()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            Name = "Kashmir";
	    Title = "The Master of Mining";
            Body = 400;
            Female = false;
            Hue = 0;
            

            SetStr(2350, 2400);
            SetDex(2150, 2200);
            SetInt(2150, 2200);

            SetHits(100000, 150000);

            SetDamage(25, 45);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 0, 1);
            SetResistance(ResistanceType.Fire, 0, 1);
            SetResistance(ResistanceType.Poison, 0, 1);
            SetResistance(ResistanceType.Energy, 0, 1);

            SetSkill(SkillName.EvalInt, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 97.5);
            SetSkill(SkillName.Wrestling, 100.2, 105.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Focus, 120.0);
            SetSkill(SkillName.Swords, 210.0, 220.0);
            SetSkill(SkillName.Mining, 300.0, 450.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 35;

			PackGold( 20000, 30000 );

			TunicofExpertMining Chest = new TunicofExpertMining();
			Chest.Movable = false;
            
			AddItem(Chest);

			GorgetofExpertMining Neck = new GorgetofExpertMining();
			Neck.Movable = false;
            AddItem(Neck);
			
			ArmsofExpertMining Arms = new ArmsofExpertMining();
			Arms.Movable = false;
			AddItem(Arms);
			
			LegsofExpertMining Legs = new LegsofExpertMining();
			Legs.Movable = false;
			AddItem(Legs);
			
			GlovesofExpertMining Gloves = new GlovesofExpertMining();
			Gloves.Movable = false;
			AddItem(Gloves);
			
			CapofExpertMining Helm = new CapofExpertMining();
			Helm.Movable = false;
			AddItem(Helm);

			MinersPickaxe Weapon = new MinersPickaxe();
			Weapon.Movable = false;
			AddItem(Weapon);

	  }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            switch (Utility.Random(65))
            {
                    case 0: c.DropItem(new MinersPickaxe()); break;
                case 1: c.DropItem(new LegsofExpertMining()); break;
                case 2: c.DropItem(new ArmsofExpertMining()); break;
                case 3: c.DropItem(new GlovesofExpertMining()); break;
                case 4: c.DropItem(new CapofExpertMining()); break;
		case 5: c.DropItem(new TunicofExpertMining()); break;
		case 6: c.DropItem(new GorgetofExpertMining()); break;
                   
            }
        }

        public Kashmir(Serial serial)
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
