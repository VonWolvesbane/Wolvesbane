using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a revenant lion corpse")]
    public class GRevenantLion : BaseCreature
    {
        [Constructable]
        public GRevenantLion()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Revenant Lion";
            Body = 251;

			Hue = 2058;

            SetStr(1200, 1225);
            SetDex(350, 370);
            SetInt(250, 285);

            SetHits(2010, 2275);

            SetDamage(28, 38);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 50, 65);
            SetResistance(ResistanceType.Fire, 25, 65);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 30, 70);
            SetResistance(ResistanceType.Energy, 70, 85);

            SetSkill(SkillName.Wrestling, 90.1, 96.8);
            SetSkill(SkillName.Tactics, 90.3, 99.3);
            SetSkill(SkillName.MagicResist, 75.3, 90.0);
            SetSkill(SkillName.Anatomy, 65.5, 69.4);
			SetSkill(SkillName.Magery, 110, 120);
			SetSkill(SkillName.EvalInt, 65.5, 69.4);
			

            Fame = 4000;
            Karma = -4000;
            PackNecroReg(6, 8);

            PackBodyPartOrBones();

            SetWeaponAbility(WeaponAbility.BleedAttack);
        }

        public GRevenantLion(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Greater;
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

			attacker.Damage(Utility.Random(15, 55), this); 
		}
        public override Poison HitPoison
        {
            get
            {
                return Poison.Greater;
            }
        }

        public override int GetAngerSound()
        {
            return 0x518;
        }

        public override int GetIdleSound()
        {
            return 0x517;
        }

        public override int GetAttackSound()
        {
            return 0x516;
        }

        public override int GetHurtSound()
        {
            return 0x519;
        }

        public override int GetDeathSound()
        {
            return 0x515;
        }
		
		public override int TreasureMapLevel { get { return 3; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.MedScrolls, 2);
            // TODO: Bone Pile
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