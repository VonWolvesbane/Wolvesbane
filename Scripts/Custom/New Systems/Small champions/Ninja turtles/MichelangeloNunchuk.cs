//Crafter By ReApEr
using System;

namespace Server.Items
{
    public class MichelangeloNunchuk : Nunchaku
    {
        [Constructable]
        public MichelangeloNunchuk()
        {
			this.Name ="Michelangelo Nunchaku";
			this.Slayer = SlayerName.Repond;
			this.WeaponAttributes.HitFireball = 75;
			this.WeaponAttributes.HitLightning = 75;
			this.WeaponAttributes.HitHarm = 100;
			this.WeaponAttributes.SelfRepair = 5;
			this.WeaponAttributes.ReactiveParalyze = 1;
			this.WeaponAttributes.HitLeechHits = 40;    
			this.WeaponAttributes.HitLowerAttack = 50;
			this.WeaponAttributes.HitLowerDefend = 50;      
			this.WeaponAttributes.SelfRepair = 5;
			this.Attributes.SpellChanneling = 1;
			this.Attributes.AttackChance = 55;
			this.SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
			this.SkillBonuses.SetValues(1, SkillName.Parry, 20.0);
            this.Weight = 6.0;
        }

        public MichelangeloNunchuk(Serial serial)
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