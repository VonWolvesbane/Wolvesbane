// Created by Nept
using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Evil Shard Owner")]
    public class VonWolvesbane : BaseCreature
    {
        [Constructable]
        public VonWolvesbane()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            Name = "Von Wolvesbane";
	    Title = "The Evil Shard Owner";
            Body = 400;
            Female = false;
            Hue = 0;
            

            SetStr(2350, 2400);
            SetDex(2150, 2200);
            SetInt(2150, 2200);

            SetHits(100000, 150000);

            SetDamage(25, 45);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 75);
            SetResistance(ResistanceType.Fire, 30, 75);
            SetResistance(ResistanceType.Poison, 30, 75);
            SetResistance(ResistanceType.Energy, 30, 75);

            SetSkill(SkillName.EvalInt, 100.0, 150.0);
            SetSkill(SkillName.Tactics, 125.1, 160.0);
            SetSkill(SkillName.MagicResist, 175.0, 197.5);
            SetSkill(SkillName.Wrestling, 200.2, 205.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.Focus, 120.0);
            SetSkill(SkillName.Magery, 210.0, 220.0);
            SetSkill(SkillName.AnimalTaming, 300.0, 450.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 35;

			PackGold( 20000, 30000 );

			TunicofExpertAnimalTaming Chest = new TunicofExpertAnimalTaming();
			Chest.Movable = false;
            Chest.Hue = 1174;
			AddItem(Chest);

			GorgetofExpertAnimalTaming Neck = new GorgetofExpertAnimalTaming();
			Neck.Movable = false;
            Neck.Hue = 1174;
            AddItem(Neck);
			
			ArmsofExpertAnimalTaming Arms = new ArmsofExpertAnimalTaming();
			Arms.Movable = false;
            Arms.Hue = 1174;
			AddItem(Arms);
			
			LegsofExpertAnimalTaming Legs = new LegsofExpertAnimalTaming();
			Legs.Movable = false;
            Legs.Hue = 1174;
			AddItem(Legs);
			
			GlovesofExpertAnimalTaming Gloves = new GlovesofExpertAnimalTaming();
			Gloves.Movable = false;
            Gloves.Hue = 1174;
			AddItem(Gloves);
			
			CapofExpertAnimalTaming Helm = new CapofExpertAnimalTaming();
			Helm.Movable = false;
            Helm.Hue = 1174;
			AddItem(Helm);

            Boots Boots = new Boots();
            AddItem(Boots);

            Robe OuterTorso = new Robe();
            OuterTorso.Movable = false;
            OuterTorso.ItemID = 30742;
            OuterTorso.Hue = 1174;
            AddItem(OuterTorso);

        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            if (Utility.RandomDouble() < 0.05)
                switch (Utility.Random(25))
                {
                    case 0: c.DropItem(new InstaTameDeed()); break;
                case 1: c.DropItem(new LegsofExpertAnimalTaming()); break;
                case 2: c.DropItem(new ArmsofExpertAnimalTaming()); break;
                case 3: c.DropItem(new GlovesofExpertAnimalTaming()); break;
                case 4: c.DropItem(new CapofExpertAnimalTaming()); break;
		case 5: c.DropItem(new TunicofExpertAnimalTaming()); break;
		case 6: c.DropItem(new GorgetofExpertAnimalTaming()); break;
                    case 7:
                        c.DropItem(new EarringsofExpertAnimalTaming()); break;
                    case 8:
                        c.DropItem(new RingofExpertAnimalTaming()); break;
                    case 9:
                        c.DropItem(new PetMageAIDeed()); break;
                    case 10:
                        c.DropItem(new PetMysticAIDeed()); break;
                    case 11:
                        c.DropItem(new PetNecroAIDeed()); break;
                    case 12:
                        c.DropItem(new PetPaladinAIDeed()); break;
                    case 13:
                        c.DropItem(new PetSamuraiAIDeed()); break;
                    case 14:
                        c.DropItem(new PetSpellweavingAIDeed()); break;
                        { 

                    }
            }
        }

        public VonWolvesbane(Serial serial)
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
