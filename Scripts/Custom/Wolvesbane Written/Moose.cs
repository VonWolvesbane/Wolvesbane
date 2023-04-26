using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a moose corpse")]
    public class Moose : BaseMount
    {
    	private static readonly Hashtable m_Table = new Hashtable();
    	
        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.ConcussionBlow;
        }


        [Constructable]
        public Moose()
            : this("a Britanian Moose")
        {
        }

        [Constructable]
        public Moose(string name) : base(name, 0xDA, 0x3EA4, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
        	
        	
            SetStr(1500, 3555);
            SetDex(185, 225);
            SetInt(185, 565);

            SetHits(7450, 9575);

            SetDamage(270, 430);

            SetDamageType(ResistanceType.Physical, 120);
            SetDamageType(ResistanceType.Cold, 200);

            SetResistance(ResistanceType.Physical, 105, 165);
            SetResistance(ResistanceType.Fire, 90, 104);
            SetResistance(ResistanceType.Cold, 95, 155);
            SetResistance(ResistanceType.Poison, 105, 130);
            SetResistance(ResistanceType.Energy, 95, 105);

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
            ControlSlots = 2;
            MinTameSkill = 145.1;
        }

        public Moose(Serial serial)
            : base(serial)
        {
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.FilthyRich, 4);
            this.AddLoot(LootPack.Gems, 4);
        }
        
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override int Meat { get { return 16; } }
        public override Poison HitPoison{ get{ return Poison.Lethal; } }
        public override Poison PoisonImmune{ get{ return Poison.Lethal; } }

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
