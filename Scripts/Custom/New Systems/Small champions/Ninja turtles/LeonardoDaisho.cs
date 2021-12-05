//Crafter By ReApEr
using System;

namespace Server.Items
{
    public class LeonardoDaisho : Daisho
    {
        [Constructable]
        public LeonardoDaisho()
        {
			this.Name ="Leonardos ninjatōs";
			this.Slayer = SlayerName.Repond;
			this.WeaponAttributes.HitFireball = 75;
			this.WeaponAttributes.HitLightning = 75;
			this.WeaponAttributes.HitHarm = 100;
			this.WeaponAttributes.SelfRepair = 5;
			this.WeaponAttributes.ReactiveParalyze = 1;
			this.WeaponAttributes.HitLeechHits = 40;    
			this.WeaponAttributes.HitLowerAttack = 25;
			this.WeaponAttributes.HitLowerDefend = 25;      
			this.Attributes.SpellChanneling = 1;
			this.Attributes.AttackChance = 55;
            this.Attributes.BonusStr = 25;
			this.Attributes.WeaponDamage = 50;
			this.Attributes.WeaponSpeed = 25;
			this.SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
			this.SkillBonuses.SetValues(1, SkillName.Parry, 20.0);
            this.Weight = 6.0;
        }

        public LeonardoDaisho(Serial serial)
            : base(serial)
        {
        }
        public override int AosMinDamage
		{
            get
            {
                return 20;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 30;
            }
        }
        public override int OldMinDamage
        {
            get
            {
                return 20;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 30;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 55;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 155;
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