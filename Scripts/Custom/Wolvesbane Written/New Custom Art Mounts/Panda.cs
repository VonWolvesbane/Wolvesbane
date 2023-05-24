using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a panda corpse")]
    public class Panda : BaseMount
    {
    	private static readonly Hashtable m_Table = new Hashtable();
    	
        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.ConcussionBlow;
        }
		
			
        [Constructable]
        public Panda()
            : this("a Wolvesbanian Panda")
        {
        }

        [Constructable]
        public Panda(string name) : base(name, 0xBB, 0x3EBA, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.05, 0.1)
        {
        	
        	
            SetStr(2500, 3555);
            SetDex(185, 225);
            SetInt(385, 765);

            SetHits(7450, 9575);

            SetDamage(1270, 1430);

            SetDamageType(ResistanceType.Physical, 120);
            SetDamageType(ResistanceType.Cold, 200);

            SetResistance(ResistanceType.Physical, 105, 165);
            SetResistance(ResistanceType.Fire, 90, 104);
            SetResistance(ResistanceType.Cold, 95, 155);
            SetResistance(ResistanceType.Poison, 105, 130);
            SetResistance(ResistanceType.Energy, 95, 200);

            SetSkill(SkillName.Anatomy, 100, 120);
            SetSkill(SkillName.MagicResist, 191.4, 201.4);
            SetSkill(SkillName.Tactics, 200.1, 210.0);
            SetSkill(SkillName.Wrestling, 197.3, 205.2);
			SetSkill(SkillName.Healing, 95.0, 150.0);
            SetSkill(SkillName.Parry, 195.0, 205.0);

            Fame = 14000;
            Karma = -14000;

            VirtualArmor = 60;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 125.1;
			
        }

        public Panda(Serial serial)
            : base(serial)
        {
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.FilthyRich, 4);
            this.AddLoot(LootPack.Gems, 4);
        }
        
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }
        public override int Meat { get { return 16; } }
		public override bool SubdueBeforeTame { get { return true; } }//Add or remove any other things you want the steed to do.
		public override bool CanHealOwner { get { return true; } }


		public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.1 > Utility.RandomDouble())
            {
             
                
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
