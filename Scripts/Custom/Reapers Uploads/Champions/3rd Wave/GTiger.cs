using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a Tiger corpse")]
    public class GTiger : BaseCreature
    {
        public override double HealChance { get { return 0.2; } }


        [Constructable]
        public GTiger(string name)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a  Tiger";
            Hue = 2058;
            Body = 1255;

            SetStr(1200, 1225);
            SetDex(350, 370);
            SetInt(250, 285);

            SetHits(1310, 1475);

            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 45, 55);
            SetResistance(ResistanceType.Cold, 75, 85);
            SetResistance(ResistanceType.Poison, 40, 70);
            SetResistance(ResistanceType.Energy, 40, 65);

            SetSkill(SkillName.Wrestling, 90.1, 96.8);
            SetSkill(SkillName.Tactics, 90.3, 99.3);
            SetSkill(SkillName.MagicResist, 75.3, 90.0);
            SetSkill(SkillName.Anatomy, 65.5, 69.4);
            SetSkill(SkillName.Healing, 72.2, 98.9);

            Fame = 5000;  //Guessing here
            Karma = 5000;  //Guessing here

            Tamable = false;

            PackGold(500, 800);

            SetWeaponAbility(WeaponAbility.BleedAttack);
        }

        public GTiger(Serial serial)
            : base(serial)
        {
        }

        public override int TreasureMapLevel
        {
            get { return 5; }
        }

        public override bool CanHealOwner
        {
            get
            {
                return true;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.FruitsAndVegies;
            }
        }
        public override bool CanAngerOnTame
        {
            get
            {
                return true;
            }
        }
		public override void AlterMeleeDamageFrom( Mobile from, ref int damage )
        {
            if ( from is BaseCreature )
            {
				if ( damage >= 700) //After recieving more than 300 damage from a basecreature (not a player)
					
                damage = 700; //Change damage to 
            }
         }
		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			attacker.Damage(Utility.Random(15, 35), this); 
		}
        public override bool StatLossAfterTame
        {
            get
            {
                return true;
            }
        }
        public override int Hides
        {
            get
            {
                return 10;
            }
        }
        public override int Meat
        {
            get
            {
                return 3;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.AosFilthyRich, 5);
        }

        public override void OnAfterTame(Mobile tamer)
        {
            if (PetTrainingHelper.Enabled)
            {
                RawStr = (int)Math.Max(1, RawStr * 0.5);
                RawDex = (int)Math.Max(1, RawDex * 0.5);

                HitsMaxSeed = RawStr;
                Hits = RawStr;

                StamMaxSeed = RawDex;
                Stam = RawDex;
            }
            else
            {
                base.OnAfterTame(tamer);
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.Race != Race.Elf && from == ControlMaster && from.IsPlayer())
            {
                Item pads = from.FindItemOnLayer(Layer.Shoes);

                if (pads is PadsOfTheCuSidhe)
                    from.SendLocalizedMessage(1071981); // Your boots allow you to mount the Cu Sidhe.
                else
                {
                    from.SendLocalizedMessage(1072203); // Only Elves may use 
                    return;
                }
            }

            base.OnDoubleClick(from);
        }

        public override int GetIdleSound()
        {
            return 0x577;
        }

        public override int GetAttackSound()
        {
            return 0x576;
        }

        public override int GetAngerSound()
        {
            return 0x578;
        }

        public override int GetHurtSound()
        {
            return 0x576;
        }

        public override int GetDeathSound()
        {
            return 0x579;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)3); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version < 3 && Controlled && RawStr >= 1200 && ControlSlots == ControlSlotsMin)
            {
                Server.SkillHandlers.AnimalTaming.ScaleStats(this, 0.5);
            }

            if (version < 1 && Name == "a Cu Sidhe")
                Name = "a cu sidhe";

            if (version == 1)
            {
                SetWeaponAbility(WeaponAbility.BleedAttack);
            }
        }
    }
}
