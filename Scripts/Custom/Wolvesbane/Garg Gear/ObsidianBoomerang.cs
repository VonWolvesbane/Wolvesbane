using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    // Split from Wolvesbane_Gargoyle_Gear_v3.cs: ObsidianBoomerang

    public class ObsidianBoomerang : Boomerang
    {
        public override int ArtifactRarity { get { return 544; } }
        public override bool IsArtifact { get { return true; } }
        public override int InitMinHits { get { return 200; } }
        public override int InitMaxHits { get { return 200; } }

        [Constructable]
        public ObsidianBoomerang()
            : base()
        {
            Name = "Obsidian Wing Boomerang";
            Hue = 1175;

            Attributes.BonusStr = 72;
            Attributes.BonusInt = 72;
            Attributes.BonusDex = 72;
            Attributes.NightSight = 1;
            Attributes.Luck = 544;

            WeaponAttributes.HitDispel = 53;
            WeaponAttributes.HitLightning = 70;
            WeaponAttributes.HitPhysicalArea = 70;
            WeaponAttributes.HitEnergyArea = 70;
            WeaponAttributes.HitFatigue = 70;
            WeaponAttributes.HitLowerDefend = 35;
            WeaponAttributes.HitLowerAttack = 35;
            WeaponAttributes.HitLeechHits = 53;
            WeaponAttributes.HitLeechMana = 53;
            WeaponAttributes.SelfRepair = 70;
            WeaponAttributes.UseBestSkill = 1;
            WeaponAttributes.MageWeapon = 0;
            WeaponAttributes.ResistPhysicalBonus = 7;

            Attributes.RegenHits = 2;
            Attributes.RegenMana = 2;
            Attributes.BonusHits = 233;
            Attributes.BonusMana = 233;
            Attributes.BonusStam = 233;
            Attributes.CastRecovery = 2;
            Attributes.ReflectPhysical = 233;
            Attributes.AttackChance = 233;
            Attributes.DefendChance = 233;
            Attributes.SpellDamage = 544;
            Attributes.SpellChanneling = 1;

            Attributes.WeaponDamage = 35;
            Attributes.WeaponSpeed = 14;

            MaxRange = 8;
            MinDamage = 27;
            MaxDamage = 35;
        }

        public ObsidianBoomerang(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
