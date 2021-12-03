using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a wolf spider corpse")]
    public class WolfSpiderMount : BaseMount
    {
    	private static readonly Hashtable m_Table = new Hashtable();
    	
        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.BleedAttack;
        }


        [Constructable]
        public WolfSpiderMount()
            : this("a Wolf Spider")
        {
        }

        [Constructable]
        public WolfSpiderMount(string name) : base(name, 0x579, 0x3ECA, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
        	BaseSoundID = 389;
            Hue = 2850;
        	
            SetStr(2500, 2555);
            SetDex(285, 325);
            SetInt(285, 365);

            SetHits(2450, 2575);

            SetDamage(270, 430);

            SetDamageType(ResistanceType.Physical, 120);
            SetDamageType(ResistanceType.Poison, 200);

            SetResistance(ResistanceType.Physical, 155, 175);
            SetResistance(ResistanceType.Fire, 120, 140);
            SetResistance(ResistanceType.Cold, 155, 165);
            SetResistance(ResistanceType.Poison, 175, 190);
            SetResistance(ResistanceType.Energy, 125, 145);

            SetSkill(SkillName.Anatomy, 100, 120);
            SetSkill(SkillName.MagicResist, 191.4, 201.4);
            SetSkill(SkillName.Tactics, 200.1, 210.0);
            SetSkill(SkillName.Wrestling, 197.3, 205.2);
            SetSkill(SkillName.Poisoning, 295.0, 320.0);
            SetSkill(SkillName.Parry, 195.0, 205.0);

            Fame = 14000;
            Karma = -14000;

            VirtualArmor = 60;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 145.1;
        }

        public WolfSpiderMount(Serial serial)
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
             
                ExpireTimer timer = (ExpireTimer)m_Table[defender];

                if (timer != null)
                {
                    timer.DoExpire();
                    defender.SendMessage("The spiders fangs pour more venom into your already weakend body."); 
                }
                else
                    defender.SendMessage("The spider sinks it's fangs into your flesh, releasing it's toxic venom!"); 

                int effect = -(defender.PhysicalResistance * 15 / 100);

                ResistanceMod mod = new ResistanceMod(ResistanceType.Physical, effect);

                defender.FixedEffect(0x37B9, 10, 5);
                defender.AddResistanceMod(mod);

                timer = new ExpireTimer(defender, mod, TimeSpan.FromSeconds(5.0));
                timer.Start();
                m_Table[defender] = timer;
            }
        }
        
        private class ExpireTimer : Timer
        {
            private readonly Mobile m_Mobile;
            private readonly ResistanceMod m_Mod;
            public ExpireTimer(Mobile m, ResistanceMod mod, TimeSpan delay)
                : base(delay)
            {
                this.m_Mobile = m;
                this.m_Mod = mod;
                this.Priority = TimerPriority.TwoFiftyMS;
            }

            public void DoExpire()
            {
                this.m_Mobile.RemoveResistanceMod(this.m_Mod);
                this.Stop();
                m_Table.Remove(this.m_Mobile);
            }

            protected override void OnTick()
            {
                this.m_Mobile.SendLocalizedMessage(1070838); // Your resistance to physical attacks has returned.
                this.DoExpire();
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
