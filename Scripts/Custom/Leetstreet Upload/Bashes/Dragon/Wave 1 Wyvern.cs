using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crinkle dragon corpse")]
    public class CrinkleDragon : BaseCreature
    {
        [Constructable]
        public CrinkleDragon()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a crinkle dragon";
            this.Body = 62;
            this.BaseSoundID = 362;
            this.Hue = 2400;

            this.SetStr(350);
            this.SetDex(350); 
            this.SetInt(350); 

            this.SetHits(40000);
            this.SetStam(40000);

            this.SetDamage(16, 38);

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Fire, 50);

            
            this.SetResistance(ResistanceType.Physical, 60, 80);
            this.SetResistance(ResistanceType.Fire, 50, 70);
            this.SetResistance(ResistanceType.Cold, 30, 50);
            this.SetResistance(ResistanceType.Poison, 40, 60);
            this.SetResistance(ResistanceType.Energy, 30, 50);

            this.SetSkill(SkillName.Poisoning, 100);
            this.SetSkill(SkillName.MagicResist, 100);
            this.SetSkill(SkillName.Tactics, 100);
            this.SetSkill(SkillName.Wrestling, 100);

            this.Fame = 5000;
            this.Karma = -5000;

            this.VirtualArmor = 50;
        }

        public CrinkleDragon(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement
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
                return Poison.Deadly;
            }
        }

        public override Poison HitPoison
        {
            get
            {
                return Poison.Deadly;
            }
        }

        public override bool CanFly
        {
            get
            {
                return true;
            }
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Average);
            this.AddLoot(LootPack.Meager);
        }

        public override int GetAttackSound()
        {
            return 713;
        }

        public override int GetAngerSound()
        {
            return 718;
        }

        public override int GetDeathSound()
        {
            return 716;
        }

        public override int GetHurtSound()
        {
            return 721;
        }

        public override int GetIdleSound()
        {
            return 725;
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