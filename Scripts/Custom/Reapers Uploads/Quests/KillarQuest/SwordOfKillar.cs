using System;
using Server;
using Server.Spells;
using Server.Mobiles;

namespace Server.Items
{
public class SwordOfKillar : PaladinSword
{
public override int ArtifactRarity{ get{ return 20; } }

		public override int MinDamage{ get{ return 23; } }
		public override int MaxDamage{ get{ return 30; } }
public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.Disarm; } }
public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.CrushingBlow; } }
public override float Speed{ get{ return 3.50f; } }

[Constructable]
public SwordOfKillar()
{
Hue = 1161;
Name = "Sword Of Killar";
Attributes.Luck = 250;
Attributes.AttackChance = 25;
Attributes.SpellChanneling = 1;
Attributes.WeaponSpeed = 100;
Attributes.WeaponDamage = 100;
Attributes.ReflectPhysical = 100;
//Attributes.CastRecovery = 2;
Attributes.CastSpeed = 2;
Attributes.RegenMana = 2;
Attributes.RegenStam = 2;
WeaponAttributes.ResistFireBonus = 10;
WeaponAttributes.UseBestSkill = 1;
WeaponAttributes.HitFireball = 100;
WeaponAttributes.SelfRepair = 100;

Slayer = SlayerName.Silver;

LootType = LootType.Blessed;

}

public override void GetDamageTypes( Mobile weilder, out int phys, out int fire, out int cold, out int pois, out int nrgy, out int chaos, out int direct )
{
phys = nrgy = cold = pois = chaos = direct = 0;
fire = 100;
}

		public override void OnHit(Mobile attacker, IDamageable defender, double Damagebonus) //On hit trigger
	{
	if (defender is BaseCreature)
            {
				if (0.3 > Utility.RandomDouble())//10% of the time this weapon will set a defenders hit points to 0 almost killing it
            {
                  defender.Hits -=250;
				  defender.FixedParticles( 0x3709, 10, 30, 5052, EffectLayer.Waist ); //FlameStrike Effect
				  defender.PlaySound( 0x208 );
				  attacker.Say( "Ancestors of Killar...Aide Me!!!" ); 
                        }
			}
         base.OnHit( attacker, defender, Damagebonus );
            }

public SwordOfKillar( Serial serial ) : base( serial )
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
}
}