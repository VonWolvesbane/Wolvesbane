// Created by ReApEr
using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("corpse of A ninja turtle")]
    public class Michelangelo : BaseCreature
    {
        [Constructable]
        public Michelangelo()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {

            Name = "Michelangelo";
			Title = "The Ninja Turtle";
            Body = 400;
            Female = false;
            Hue = 458;

            SetStr(1350, 2400);
            SetDex(1150, 1200);
            SetInt(1150, 1200);

            SetHits(50000, 75000);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 90, 90);
            SetResistance(ResistanceType.Fire, 90, 90);
            SetResistance(ResistanceType.Poison, 90, 90);
            SetResistance(ResistanceType.Energy, 90, 90);
			
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
			
			TurtlesChest Chest = new TurtlesChest();
			Chest.Movable = false;
			Chest.Name = "Michelangelo";
			AddItem(Chest);
			
			Bandana Hat = new Bandana();
			Hat.Hue = 2932;
            Hat.Movable = false;
            AddItem(Hat);
			
			MichelangeloNunchuk Weapon = new MichelangeloNunchuk();
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
			if ( Utility.RandomDouble() < 0.20 )
            switch (Utility.Random(2))
            {
                case 0: PackItem(new MichelangeloNunchuk()); break;
                    }
            }
        public Michelangelo(Serial serial)
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
