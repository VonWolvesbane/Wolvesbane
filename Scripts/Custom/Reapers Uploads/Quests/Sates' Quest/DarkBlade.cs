using System;
using Server;

namespace Server.Items
{
	public class DarkBlade : DoubleBladedStaff  
	{
		public override int ArtifactRarity{ get{ return 1773; } } 

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.BleedAttack; } }

		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.InfectiousStrike; } }

		public override float Speed{ get{ return 3.50f; } }
		public override int MinDamage{ get{ return 25; } }
		public override int MaxDamage{ get{ return 35; } }
		
		public override int InitMinHits{ get{ return 255; } } 
		public override int InitMaxHits{ get{ return 255; } } 

		[Constructable]
		public DarkBlade() 
		{
			Weight = 15.0; 
            		Name = "Dark Blade";  
            		Hue = 1175;     

			WeaponAttributes.HitEnergyArea = 55;                                  
			WeaponAttributes.HitLightning = 75;
			WeaponAttributes.SelfRepair = 5;

			Attributes.CastRecovery = 2;
			Attributes.CastSpeed = 2;
			Attributes.SpellChanneling = 1;
			Attributes.SpellDamage = 10;
			Attributes.WeaponDamage = 50;
			Attributes.WeaponSpeed = 50;

			StrRequirement = 50;

			LootType = LootType.Regular;
		}

		public DarkBlade( Serial serial ) : base( serial ) 
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