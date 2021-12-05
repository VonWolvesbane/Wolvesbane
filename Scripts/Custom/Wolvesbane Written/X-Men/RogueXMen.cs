// Created by Nept
using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of Rogue")]
    public class RogueXMen : BaseCreature
    {
        [Constructable]
        public RogueXMen()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            Name = "Rogue";
	        Title = "Southern Belle of the X-Men";
            Body = 401;
            Female = true;
            Hue = 0;
            HairItemID = 8252;
            HairHue = 338;
            

            SetStr(2350, 5400);
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
            SetSkill(SkillName.Wrestling, 160.2, 195.0);
                 
            SetSkill(SkillName.Healing, 100.0, 105.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 35;

			PackGold( 5000, 20000 );

			TunicofXMenUniform Chest = new TunicofXMenUniform();
			Chest.Movable = false;
            Chest.ItemID = 10132;
            Chest.Hue = 472;
            AddItem(Chest);
            				
			LegsofXMenUniform Legs = new LegsofXMenUniform();
			Legs.Movable = false;
            Legs.Hue = 1957;
            AddItem(Legs);
			
			GlovesofXMenUniform Gloves = new GlovesofXMenUniform();
			Gloves.Movable = false;
            Gloves.Hue = 1986;
            AddItem(Gloves);

            BootsofXMenUniform Boots = new BootsofXMenUniform();
            Boots.Movable = false;
            Boots.Hue = 1986;
            AddItem(Boots);

            ArmsofXMenUniform Arms = new ArmsofXMenUniform();
            Arms.Movable = false;
            Arms.Hue = 1957;
            AddItem(Arms);

            BeltofXMenUniform Belt = new BeltofXMenUniform();
            Belt.Movable = false;
            Belt.Hue = 1986;
            AddItem(Belt);

            CircletofXMenUniform Head = new CircletofXMenUniform();
            Head.Movable = false;
            Head.Hue = 1457;
            AddItem(Head);
        }

        public override void GenerateLoot()
        {
            
                AddLoot(LootPack.FilthyRich);
            

            if (Utility.RandomDouble() < 0.15)
                switch (Utility.Random(1))
                {
                    case 0: PackItem(new Cloak()); break;
                }
        }

        public RogueXMen(Serial serial)
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
