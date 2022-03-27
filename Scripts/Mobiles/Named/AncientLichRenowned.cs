using System;
using Server;

namespace Server.Items
{
	public class LichBrain : Item
	{
		[Constructable]
		public LichBrain()
			: base(0x1CF0)
		{
			Name = "Ancient Lich Brain";
			this.LootType = LootType.Blessed;
			this.Weight = 1;
			this.Hue = 0xD7;
		}

		public LichBrain(Serial serial)
			: base(serial)
		{
		}

		public override int LabelNumber
		{
			get
			{
				return 1074612;
			}
		}// Lich Brain

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}


namespace Server.Mobiles
{
    [CorpseName("Ancient Lich [Renowned] corpse")]  
    public class AncientLichRenowned : BaseRenowned
    {
        [Constructable]
        public AncientLichRenowned()
            : base(AIType.AI_NecroMage)
        {
            this.Name = "Ancient Lich";
            this.Title = "[Renowned]";
            this.Body = 78;
            this.BaseSoundID = 412;

            this.SetStr(250, 305);
            this.SetDex(96, 115);
            this.SetInt(966, 1045);

            this.SetHits(2000, 2500);

            this.SetDamage(15, 27);

            this.SetDamageType(ResistanceType.Physical, 20);
            this.SetDamageType(ResistanceType.Cold, 40);
            this.SetDamageType(ResistanceType.Energy, 40);

            this.SetResistance(ResistanceType.Physical, 55, 65);
            this.SetResistance(ResistanceType.Fire, 25, 30);
            this.SetResistance(ResistanceType.Cold, 50, 60);
            this.SetResistance(ResistanceType.Poison, 50, 60);
            this.SetResistance(ResistanceType.Energy, 25, 30);

            this.SetSkill(SkillName.EvalInt, 120.1, 130.0);
            this.SetSkill(SkillName.Magery, 120.1, 130.0);
            this.SetSkill(SkillName.Meditation, 100.1, 101.0);
            this.SetSkill(SkillName.MagicResist, 175.2, 200.0);
            this.SetSkill(SkillName.Tactics, 90.1, 100.0);
            this.SetSkill(SkillName.Wrestling, 75.1, 100.0);

            this.Fame = 23000;
            this.Karma = -23000;

            this.VirtualArmor = 60;

            this.PackNecroReg(30, 275);
        }

		public AncientLichRenowned(Serial serial)
            : base(serial)
        {
        }
		public override void OnCarve(Mobile from, Items.Corpse corpse, Item with)
		{
			PlayerMobile player = from as PlayerMobile;

			double baseProbability = 0.20;
			if (player != null && player.Skills.Forensics.Value >= 120)
				baseProbability += 0.10;
			if (IsParagon)
				baseProbability += 0.20;
			Items.BaseWeapon cutter = with as Items.BaseWeapon;
			if (cutter != null && cutter.GetLuckBonus() > 2000)
				baseProbability += 0.10;		

			if (baseProbability > Utility.RandomDouble())
			{
				corpse.DropItem(new Server.Items.LichBrain());
			}

			corpse.Carved = true;
		}

		
		public override Type[] UniqueSAList
        {
            get
            {
                return new Type[] { typeof(Items.SpinedBloodwormBracers), typeof(Items.DefenderOfTheMagus) };
            }
        }
        public override Type[] SharedSAList
        {
            get
            {
                return new Type[] { typeof(Items.SummonersKilt) };
            }
        }
        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }
        public override bool Unprovokable
        {
            get
            {
                return true;
            }
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
                return Poison.Lethal;
            }
        }
        public override int GetIdleSound()
        {
            return 0x19D;
        }

        public override int GetAngerSound()
        {
            return 0x175;
        }

        public override int GetDeathSound()
        {
            return 0x108;
        }

        public override int GetAttackSound()
        {
            return 0xE2;
        }

        public override int GetHurtSound()
        {
            return 0x28B;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);

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