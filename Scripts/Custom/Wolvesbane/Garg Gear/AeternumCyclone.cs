using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    // Split from Wolvesbane_Gargoyle_Gear_v3.cs: AeternumCyclone

    public class AeternumCyclone : Cyclone
    {
        public override int ArtifactRarity { get { return 777; } }
        public override bool IsArtifact { get { return true; } }
        public override int InitMinHits { get { return 255; } }
        public override int InitMaxHits { get { return 255; } }

        [Constructable]
        public AeternumCyclone()
            : base()
        {
            Name = "Aeternum Cyclone";
            Hue = 2406;

            Attributes.BonusStr = 103;
            Attributes.BonusInt = 103;
            Attributes.BonusDex = 103;
            Attributes.NightSight = 1;
            Attributes.Luck = 777;

            WeaponAttributes.HitDispel = 75;
            WeaponAttributes.HitLightning = 100;
            WeaponAttributes.HitPhysicalArea = 100;
            WeaponAttributes.HitEnergyArea = 100;
            WeaponAttributes.HitFatigue = 100;
            WeaponAttributes.HitLowerDefend = 50;
            WeaponAttributes.HitLowerAttack = 50;
            WeaponAttributes.HitLeechHits = 75;
            WeaponAttributes.HitLeechMana = 75;
            WeaponAttributes.SelfRepair = 100;
            WeaponAttributes.UseBestSkill = 1;
            WeaponAttributes.MageWeapon = 0;
            WeaponAttributes.ResistPhysicalBonus = 10;

            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.BonusHits = 333;
            Attributes.BonusMana = 333;
            Attributes.BonusStam = 333;
            Attributes.CastRecovery = 3;
            Attributes.ReflectPhysical = 333;
            Attributes.AttackChance = 333;
            Attributes.DefendChance = 333;
            Attributes.SpellDamage = 777;
            Attributes.SpellChanneling = 1;

            // Holy Avenger's script ends with these final values after overwriting earlier entries.
            Attributes.WeaponDamage = 50;
            Attributes.WeaponSpeed = 20;

            MaxRange = 9;
            MinDamage = 39;
            MaxDamage = 50;
        }

        public AeternumCyclone(Serial serial) : base(serial) { }

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
