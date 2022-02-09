using System;
using Server;
using Server.Items;

namespace RuneTargetExtensions
{
	public class RuneEffect
	{

		public static bool ReduceDurability(BaseArmor item, Mobile from)
		{
			from.SendMessage("You have failed to enhance the armor!");
			if (item.MaxHitPoints <= 0)
				return true;

			from.PlaySound(42);

			item.HitPoints -= 1;
			if (item.HitPoints > 0)
			{
				from.SendMessage("The armor is damaged!");
			}
			else
			{
				from.SendMessage("The armor is damaged beyond repair!");
				item.Delete();
				return false;
			}
			return true;
		}

			public static bool ReduceDurability(BaseWeapon item, Mobile from)
		{
			from.SendMessage("You have failed to enhance the weapon!");
			if (item.MaxHitPoints <= 0)
				return true;

			from.PlaySound(42);

			item.HitPoints -= 1;
			if (item.HitPoints > 0)
			{
				from.SendMessage("The weapon is damaged!");
			}
			else
			{
				from.SendMessage("The weapon is damaged beyond repair!");
				item.Delete();
				return false;
			}
			return true;
		}


		public static bool ReduceDurability(BaseShield item, Mobile from)
		{
			from.SendMessage("You have failed to enhance the shield!");
			if (item.MaxHitPoints <= 0)
				return true;

			from.PlaySound(42);

			item.HitPoints -= 1;
			if (item.HitPoints > 0)
			{
				from.SendMessage("The shield is damaged!");
			}
			else
			{
				from.SendMessage("The shield is damaged beyond repair!");
				item.Delete();
				return false;
			}
			return true;
		}
		public static bool ReduceDurability(BaseClothing item, Mobile from)
		{
			from.SendMessage("You have failed to enhance the clothing!");
			from.PlaySound(88);
			return true;
		}

		public static bool ReduceDurability(BaseJewel item, Mobile from)
		{
			from.SendMessage("You have failed to enhance the jewelery!");
			if (item.MaxHitPoints <= 0)
				return true; 
			
			from.PlaySound(88);
			item.HitPoints -= 1;
			if (item.HitPoints > 0)
			{
				from.SendMessage("The jewelery is damaged!");
			}
			else
			{
				from.SendMessage("The jewelery is damaged beyond repair!");
				item.Delete();
				return false;
			}

			return true;
		}		

	}
}