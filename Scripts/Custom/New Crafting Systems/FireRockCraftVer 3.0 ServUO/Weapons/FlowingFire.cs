/* Created by Hammerhand*/

using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class FlowingFire : BaseSword
	{
        public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ArmorIgnore; } }
        public override WeaponAbility SecondaryAbility { get { return WeaponAbility.InfectiousStrike; } }

        public override int Hue { get { return 1359; } }
		public override int StrengthReq{ get{ return 30; } }
		public override int MinDamage{ get{ return 12; } }
		public override int MaxDamage{ get{ return 15; } }
        public override float Speed { get { return 3.00f; } }

        public override int DefHitSound { get { return 0x23C; } }
        public override int DefMissSound { get { return 0x238; } }

		public override int InitMinHits{ get{ return 98; } }
		public override int InitMaxHits{ get{ return 180; } }

        public override SkillName DefSkill { get { return SkillName.Fencing; } }
        public override WeaponType DefType { get { return WeaponType.Piercing; } }
        public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Pierce1H; } }

		[Constructable]
        public FlowingFire() : base(0x1401)
		{
            Name = "Flowing Fire";
            Hue = 1359;
			Weight = 2.0;

            Attributes.AttackChance = Utility.RandomMinMax(7, 40);
            Attributes.BonusDex = Utility.RandomMinMax(3, 15);
            Attributes.WeaponSpeed = Utility.RandomMinMax(8, 20);
            Attributes.SpellChanneling = 1;
            Attributes.Luck = Utility.RandomMinMax(150, 250);
            Attributes.RegenStam = Utility.RandomMinMax(8, 11);
            Attributes.ReflectPhysical = Utility.RandomMinMax(12, 25);

            WeaponAttributes.HitFireArea = Utility.RandomMinMax(8, 35);
            WeaponAttributes.UseBestSkill = 1;
            WeaponAttributes.HitLowerDefend = 10;
            WeaponAttributes.SelfRepair = 5;

            LootType = LootType.Regular;
		}

        public FlowingFire(Serial serial) : base(serial)
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}