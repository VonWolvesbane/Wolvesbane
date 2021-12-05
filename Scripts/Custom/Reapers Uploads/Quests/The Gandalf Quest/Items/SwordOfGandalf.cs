//Scripted By James4245
using System;
using Server;

namespace Server.Items
{
	public class Glamdring : Broadsword
	{

		public override int AosMinDamage{ get{ return 20; } }
		public override int AosMaxDamage{ get{ return 25; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public Glamdring()
		{
			Name = "Glamdring";
			Hue = 1153;
			
			DurabilityLevel = WeaponDurabilityLevel.Indestructible;

				WeaponAttributes.HitLightning = 100;
				WeaponAttributes.HitLeechMana = 50;
				Attributes.BonusMana = 25;
				Attributes.BonusInt = 15;
				WeaponAttributes.MageWeapon = 30;
				Attributes.WeaponDamage = 55;
				Attributes.RegenMana = 5;
				Attributes.SpellChanneling = 1;
				Attributes.CastSpeed = 2;
				Attributes.CastRecovery = 3;
				Attributes.WeaponSpeed = 45;
				WeaponAttributes.LowerStatReq = 100;
				Attributes.SpellDamage = 100;
				}



        public Glamdring( Serial serial ) : base( serial )
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
