// Created by Nept
using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of Gambit")]
    public class GambitXMen : BaseCreature
    {
        [Constructable]
        public GambitXMen()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            Name = "Gambit";
	        Title = "Ragin' Cajun of the X-Men";
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
            SetSkill(SkillName.Macing, 120.0, 145.0);         
            SetSkill(SkillName.Healing, 100.0, 105.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 35;

			PackGold( 5000, 20000 );

			TunicofXMenUniform Chest = new TunicofXMenUniform();
			Chest.Movable = false;
            Chest.Hue = 36;
            Chest.ItemID = 11111;
            AddItem(Chest);
            				
			LegsofXMenUniform Legs = new LegsofXMenUniform();
			Legs.Movable = false;
            Legs.Hue = 2922;
            AddItem(Legs);
			
			GlovesofXMenUniform Gloves = new GlovesofXMenUniform();
			Gloves.Movable = false;
            Gloves.Hue = 2922;
            AddItem(Gloves);

            BootsofXMenUniform Boots = new BootsofXMenUniform();
            Boots.Movable = false;
            Boots.Hue = 1264;
            AddItem(Boots);

            CircletofXMenUniform Head = new CircletofXMenUniform();
            Head.Movable = false;
            Head.Hue = 1457;
            AddItem(Head);

            ArmsofXMenUniform Arms = new ArmsofXMenUniform();
            Arms.Movable = false;
            Arms.Hue = 2922;
            AddItem(Arms);

            GorgetofXMenUniform Gorget = new GorgetofXMenUniform();
            Gorget.Movable = false;
            Gorget.Hue = 1264;
            AddItem(Gorget);

            StaffofGambit Weapon = new StaffofGambit();
            Weapon.Movable = false;
            AddItem(Weapon);


            

	  }

        public override void GenerateLoot()
        {
            
                AddLoot(LootPack.FilthyRich);
            

            if (Utility.RandomDouble() < 0.15)
                switch (Utility.Random(8))
                {
                    case 0: PackItem(new StaffofGambit()); break;
                    


                }
        }

        public GambitXMen(Serial serial)
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
