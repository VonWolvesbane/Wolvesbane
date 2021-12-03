// Created with UO Weapon Generator
// Created On: 8/12/2007 10:06:14 PM
// By: Hammerhand

using System;
using Server;

namespace Server.Items
{
    public class LegendarySwordOfAmbrose : Longsword
    {
        public override int ArtifactRarity{ get{ return 31; } }
        public override int InitMinHits{ get{ return 255; } }
        public override int InitMaxHits{ get{ return 255; } }
		public override int MinDamage{ get{ return 25; } }
		public override int MaxDamage{ get{ return 25; } }

        [Constructable]
        public LegendarySwordOfAmbrose()
        {
            Name = "Legendary Sword Of Ambrose";
            Hue = 2401;
            Slayer = SlayerName.DaemonDismissal;
            Attributes.SpellChanneling = 1;
            Attributes.BonusHits = 20;
            Attributes.RegenHits = 2;
            Attributes.RegenStam = 2;
            WeaponAttributes.UseBestSkill = 1;
            WeaponAttributes.HitLeechStam = 100;
            Attributes.AttackChance = 25;
            Attributes.DefendChance = 15;
            Attributes.WeaponDamage = 45;
            Attributes.WeaponSpeed = 25;
            Attributes.ReflectPhysical = 15;
            WeaponAttributes.ResistPhysicalBonus = 5;
            WeaponAttributes.SelfRepair = 5;
            WeaponAttributes.HitLightning = 100;
        }

        public LegendarySwordOfAmbrose(Serial serial) : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    } // End Class
} // End Namespace
