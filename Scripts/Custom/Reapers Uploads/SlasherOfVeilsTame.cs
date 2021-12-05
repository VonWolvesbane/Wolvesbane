using System;
using System.Collections;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a slasher of veils corpse")]
    public class SlasherOfVeilsTame : BaseCreature
    {
        [Constructable]
        public SlasherOfVeilsTame()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "The Skull Crusher";
            Body = 741;

            SetStr(2000, 2010);
            SetDex(250, 275);
            SetInt(350, 370);

            SetHits(5000, 5000);
            SetMana(650);
			SetStam(650);
			
            SetDamage(225, 300);

            SetDamageType(ResistanceType.Physical, 110);
            SetDamageType(ResistanceType.Fire, 0);
            SetDamageType(ResistanceType.Cold, 0);
            SetDamageType(ResistanceType.Poison, 0);
            SetDamageType(ResistanceType.Energy, 0);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 90 , 97);
            SetResistance(ResistanceType.Cold, 90, 98);
            SetResistance(ResistanceType.Poison, 90, 98);
            SetResistance(ResistanceType.Energy, 90, 97);

            SetSkill(SkillName.Anatomy, 110.8, 129.7);
            SetSkill(SkillName.Focus, 111.1, 125);
            SetSkill(SkillName.Meditation, 113.5, 129.9);
            SetSkill(SkillName.MagicResist, 100, 100.8);
            SetSkill(SkillName.Tactics, 190.5, 200);
            SetSkill(SkillName.Wrestling, 180.1, 200);
			
			Skills[SkillName.Anatomy].Cap = 200;
			Skills[SkillName.MagicResist].Cap = 200;
			Skills[SkillName.Tactics].Cap = 200;
			Skills[SkillName.Wrestling].Cap = 200;
			Skills[SkillName.Meditation].Cap = 200;
			Skills[SkillName.Focus].Cap = 200;

			ControlSlots = 7;
			Tamable = true;
			MinTameSkill = 148;
			
            Fame = 35000;
            Karma = -35000;
			
			SetWeaponAbility(WeaponAbility.ArmorIgnore);
			
            SetSpecialAbility(SpecialAbility.ManaDrain);

        }

        public SlasherOfVeilsTame(Serial serial)
            : base(serial)
        {
        }

        public override bool Unprovokable
        {
            get
            {
                return false;
            }
        }
        public override bool BardImmune
        {
            get
            {
                return false;
            }
        }
        public override int GetIdleSound()
        {
            return 1589;
        }

        public override int GetAngerSound()
        {
            return 1586;
        }

        public override int GetHurtSound()
        {
            return 1588;
        }

        public override int GetDeathSound()
        {
            return 1587;
        }

		public override bool AlwaysMurderer { get { return true; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.AosSuperBoss, 4);
            AddLoot(LootPack.Gems, 8);
        }
		
		 public override void AlterMeleeDamageTo(Mobile to, ref int damage)
        {
			
		
			if (to is BaseCreature )
            {
				if ( damage >= 400)
					
                damage = 400;
			}
		}
		
		public override int DefaultManaRegen
        {
            get
            {
                int regen = 40;
				return regen;
            }
        }
		
         public override void OnDamagedBySpell(Mobile caster)
        {
            if (0.5 > Utility.RandomDouble() && caster.InRange(Location, 10) && Map != null && caster.Alive && caster != this && caster.Map == Map)
            {
                MoveToWorld(caster.Location, Map);

                Timer.DelayCall(() =>
                {
                    Combatant = caster;
                });

                Effects.PlaySound(Location, Map, 0x1FE);
            }

            base.OnDamagedBySpell(caster);
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
