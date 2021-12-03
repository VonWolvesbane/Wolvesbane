//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 2/22/2015 3:02:41 PM
//=================================================

using System;
using Server;

namespace Server.Items
{
	public class NewbieGloves : LeatherGloves
	{

		[Constructable]
		public NewbieGloves()
		{
			Name = "Newbie Gloves";
			Hue = 706;
			LootType = LootType.Newbied;
			Attributes.AttackChance = 5;
			//Attributes.BonusMana = 5;
			//Attributes.BonusStam = 5;
			//Attributes.BonusHits = 5;
			Attributes.Luck = 10;
			//Attributes.WeaponDamage = 20;
			//Attributes.SpellDamage = 20;
			Attributes.LowerRegCost = 15;
			ArmorAttributes.SelfRepair = 25;
			//MaxHitPoints = 200;
			//HitPoints = 200;
			PhysicalBonus = 1;
			FireBonus = 2;
			ColdBonus = 2;
			PoisonBonus = 2;
			EnergyBonus = 3;
		}

		public NewbieGloves( Serial serial ) : base( serial )
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
