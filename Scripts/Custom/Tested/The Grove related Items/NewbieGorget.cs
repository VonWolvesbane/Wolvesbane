//=================================================
//This script was created by Gizmo's Uo Quest Maker
//This script was created on 2/22/2015 3:02:41 PM
//=================================================

using System;
using Server;

namespace Server.Items
{
	public class NewbieGorget : LeatherGorget
	{

		[Constructable]
		public NewbieGorget()
		{
			Name = "Newbie Gorget";
			Hue = 1910;
			LootType = LootType.Newbied;
			Attributes.DefendChance = 5;
			Attributes.BonusMana = 5;
			Attributes.BonusStam = 5;
			//Attributes.BonusHits = 5;
			//Attributes.Luck = 100;
			//Attributes.WeaponDamage = 20;
			//Attributes.SpellDamage = 20;
			Attributes.LowerRegCost = 15;
			ArmorAttributes.SelfRepair = 25;
			//MaxHitPoints = 200;
			//HitPoints = 200;
			PhysicalBonus = 2;
			FireBonus = 2;
			ColdBonus = 1;
			PoisonBonus = 2;
			EnergyBonus = 1;
		}

		public NewbieGorget( Serial serial ) : base( serial )
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
