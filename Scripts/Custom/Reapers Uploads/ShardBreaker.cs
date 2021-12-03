// By Nerun

using System;

namespace Server.Items
{
    public class ShardBreaker : BaseWeapon
    {
        [Constructable]
        public ShardBreaker()
            : base(0xF4B)
        {
            Name = "Shard Breaker";
			WeaponAttributes.HitLightning = 150;
            WeaponAttributes.HitFireball = 150;
			WeaponAttributes.HitHarm = 150;
            WeaponAttributes.HitMagicArrow = 150;	
			WeaponAttributes.UseBestSkill = 1;
            WeaponAttributes.SelfRepair = 100;
			ExtendedWeaponAttributes.HitSwarm = 150;
			ExtendedWeaponAttributes.Bane = 1;
			
			Attributes.BonusStr = 250;
			Attributes.BonusInt = 250;
			Attributes.BonusDex = 250;
			Attributes.BonusHits = 1000;
			Attributes.BonusMana = 1000;
			Attributes.BonusStam = 1000;
			Attributes.Luck = 100000;
			Attributes.SpellChanneling = 1;
			Attributes.WeaponSpeed = 5000;
            Attributes.WeaponDamage = 5000;
            Attributes.NightSight = 1;
            Attributes.AttackChance = 250;
            Attributes.LowerRegCost = 100;
            Attributes.LowerManaCost = 100;
            Attributes.RegenHits = 50;
            Attributes.RegenStam = 50;
            Attributes.RegenMana = 50;
            Attributes.SpellDamage = 500;
            Attributes.CastRecovery = 50;
            Attributes.CastSpeed = 50;
            LootType = LootType.Cursed;
        }

        public ShardBreaker(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.IsPlayer())
            {
                from.SendMessage("The Axe seems to be glowing with Mystic Energy!"); 
            }
        }

        public override bool OnEquip(Mobile from)
        {
			Item itemEquipped;
			
            if (from.IsPlayer())
            {
                from.SendMessage("You are not Godly enough to wield this weapon"); 
                this.Delete();
				from.AddToBackpack( new ShardBreaker() );
				//from.Backpack.DropItem( itemEquipped );
            }
            return true;
        }
        public override int StrengthReq { get { return 500; }}
        public override int MinDamage { get { return 500; }}
        public override int MaxDamage { get { return 500; }}

        public override int InitMinHits { get { return 255; }}
        public override int InitMaxHits { get { return 255; }}
		
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