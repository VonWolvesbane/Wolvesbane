using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a tarantula corpse")]
    public class TarantulaMount : BaseMount
    {
    	private static readonly Hashtable m_Table = new Hashtable();
    	
        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.BleedAttack;
        }


        [Constructable]
        public TarantulaMount()
            : this("a Tarantula")
        {
        }

        [Constructable]
        public TarantulaMount(string name) : base(name, 0x579, 0x3ECA, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
        	BaseSoundID = 389;
        	
            SetStr(500, 555);
            SetDex(85, 125);
            SetInt(85, 165);

            SetHits(450, 575);

            SetDamage(20, 30);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            SetResistance(ResistanceType.Physical, 55, 75);
            SetResistance(ResistanceType.Fire, 20, 40);
            SetResistance(ResistanceType.Cold, 55, 65);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 25, 45);

            SetSkill(SkillName.Anatomy, 0, 0);
            SetSkill(SkillName.MagicResist, 91.4, 101.4);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 97.3, 105.2);
            SetSkill(SkillName.Poisoning, 95.0, 120.0);
            SetSkill(SkillName.Parry, 95.0, 105.0);

            Fame = 14000;
            Karma = -14000;

            VirtualArmor = 60;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 115.1;
        }

        public TarantulaMount(Serial serial)
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