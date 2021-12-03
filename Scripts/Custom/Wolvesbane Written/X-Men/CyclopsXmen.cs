// Created by Nept
using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of Cyclops")]
    public class CyclopsXMen : BaseCreature
    {
        [Constructable]
        public CyclopsXMen()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            Name = "Cyclops";
	        Title = "Leader of the X-Men";
            Body = 400;
            Female = false;
            HairItemID = 8251;
            HairHue = 1190;
            

            SetStr(1350, 3400);
            SetDex(1150, 1200);
            SetInt(1150, 1200);

            SetHits(100000, 150000);

            SetDamage(55, 75);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 0, 1);
            SetResistance(ResistanceType.Fire, 0, 1);
            SetResistance(ResistanceType.Poison, 0, 1);
            SetResistance(ResistanceType.Energy, 0, 1);

            SetSkill(SkillName.EvalInt, 85.0, 100.0);
            SetSkill(SkillName.Tactics, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 115.0, 117.5);
            SetSkill(SkillName.Wrestling, 130.2, 145.0);
                       
            SetSkill(SkillName.Healing, 100.0, 105.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 35;

			PackGold( 5000, 20000 );

			TunicofXMenUniform Chest = new TunicofXMenUniform();
			Chest.Movable = false;
            Chest.Hue = 1984;
            AddItem(Chest);
            				
			LegsofXMenUniform Legs = new LegsofXMenUniform();
			Legs.Movable = false;
            Legs.Hue = 1984;
            AddItem(Legs);
			
			GlovesofXMenUniform Gloves = new GlovesofXMenUniform();
			Gloves.Movable = false;
            Gloves.Hue = 1986;
            AddItem(Gloves);

            BootsofXMenUniform Boots = new BootsofXMenUniform();
            Boots.Movable = false;
            Boots.Hue = 1986;
            AddItem(Boots);

            BeltofXMenUniform Waist = new BeltofXMenUniform();
            Waist.Movable = false;
            Waist.Hue = 1986;
            AddItem(Waist);

            ArmsofXMenUniform Arms = new ArmsofXMenUniform();
            Arms.Movable = false;
            Arms.Hue = 1984;
            AddItem(Arms);

            CyclopsVisor Head = new CyclopsVisor();
            Head.Movable = false;
            Head.Hue = 1986;
            AddItem(Head);
            

	  }

        public override void GenerateLoot()
        {
            
                AddLoot(LootPack.FilthyRich);
            

            if (Utility.RandomDouble() < 0.15)
                switch (Utility.Random(8))
                {
                    case 0: PackItem(new CyclopsVisor()); break;
                    


                }
        }

        public CyclopsXMen(Serial serial)
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
