//Crafter By ReApEr
using System;

namespace Server.Items
{
    public class DonatellosBo : GnarledStaff
    {
        [Constructable]
        public DonatellosBo()
        {
			this.Name ="Donatellos Bo";
			this.Slayer = SlayerName.Repond;
			this.WeaponAttributes.HitLeechHits = 40;    
			this.WeaponAttributes.HitLowerAttack = 25;
			this.WeaponAttributes.HitLowerDefend = 25;      
			this.WeaponAttributes.SelfRepair = 5;
			this.Attributes.SpellChanneling = 1;
			this.Attributes.AttackChance = 55;
            this.Attributes.RegenHits = 10;
            this.Attributes.BonusHits = 10;
            this.Attributes.BonusStr = 25;
            this.Attributes.RegenStam = 10;
			this.Attributes.DefendChance = 25;
			this.Attributes.WeaponDamage = 50;
			this.Attributes.WeaponSpeed = 25;
			this.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
			this.SkillBonuses.SetValues(1, SkillName.Parry, 20.0);
            this.Weight = 6.0;
        }

        public DonatellosBo(Serial serial)
            : base(serial)
        {
        }
        public override int MinDamage
		{
            get
            {
                return 20;
            }
        }
        public override int MaxDamage
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