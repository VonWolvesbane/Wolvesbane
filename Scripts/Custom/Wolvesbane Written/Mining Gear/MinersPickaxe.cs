using System;
using Server.Engines.Harvest;

namespace Server.Items
{
    [FlipableAttribute(0xE86, 0xE85)]
    public class MinersPickaxe : BaseAxe, IUsesRemaining, IHarvestTool
    {
        [Constructable]
        public MinersPickaxe()
            : base(0xE86)
        {
            this.Weight = 11.0;
            Name = "Miner's Pickaxe";
            Hue = 1289;
            SkillBonuses.SetValues(0, SkillName.Mining, 20.0);
            SkillBonuses.SetValues(1, SkillName.Blacksmith, 20.0);
            WeaponAttributes.SelfRepair = 100;
            //this.UsesRemaining = 50;
            //this.ShowUsesRemaining = true;
        }

        public MinersPickaxe(Serial serial)
            : base(serial)
        {
        }

        public override HarvestSystem HarvestSystem
        {
            get
            {
                return Mining.System;
            }
        }
        public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.DoubleStrike;
            }
        }
        public override WeaponAbility SecondaryAbility
        {
            get
            {
                return WeaponAbility.Disarm;
            }
        }
        public override int StrengthReq
        {
            get
            {
                return 50;
            }
        }
        public override int MinDamage
        {
            get
            {
                return 35;
            }
        }
        public override int MaxDamage
        {
            get
            {
                return 55;
            }
        }
        public override float Speed
        {
            get
            {
                return 3.00f;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 31;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 60;
            }
        }

        public override bool CanBeWornByGargoyles { get { return true; } }

        public override WeaponAnimation DefAnimation
        {
            get
            {
                return WeaponAnimation.Slash1H;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            this.ShowUsesRemaining = true;
        }
    }
}
