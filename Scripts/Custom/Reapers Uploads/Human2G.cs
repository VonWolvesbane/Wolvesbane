// By SHAMBAMPOW
using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class H2GTarget : Target 
	{
		private Human2G m_Deed;

		public H2GTarget( Human2G deed ) : base( 1, false, TargetFlags.None )
		{
			m_Deed = deed;
		}

		protected void CopyAttributes(BaseWeapon destWeapon, BaseWeapon sourceWeapon)
		{
			if (destWeapon == null || sourceWeapon == null)
				return;

			if (sourceWeapon.PlayerConstructed)
			{
				destWeapon.PlayerConstructed = true;
				destWeapon.Crafter = sourceWeapon.Crafter;
				destWeapon.Quality = sourceWeapon.Quality;
			}
			destWeapon.Name = sourceWeapon.Name;
			destWeapon.Resource = sourceWeapon.Resource;

			destWeapon.Hue = sourceWeapon.Hue;
			destWeapon.LootType = sourceWeapon.LootType;
			destWeapon.Insured = sourceWeapon.Insured;

			sourceWeapon.OnAfterDuped(destWeapon);

			if (sourceWeapon is IDurability && destWeapon is IDurability)
			{
				((IDurability)destWeapon).MaxHitPoints = ((IDurability)sourceWeapon).MaxHitPoints;
				((IDurability)destWeapon).HitPoints = ((IDurability)sourceWeapon).HitPoints;
			}

			destWeapon.Parent = null;
			destWeapon.Altered = true;
		}
		protected override void OnTarget(Mobile from, object target)
		{
			Item oldItem = target as Item;
			Item newItem = null;
			bool success = false;

			if (oldItem is BaseWeapon)
			{
				BaseWeapon source = (BaseWeapon)oldItem;
				if (source != null)
				{
					switch (target)
					{
						case HellSword sword:
							newItem = Activator.CreateInstance(typeof(DreadSword)) as BaseWeapon;
							break;
						case BloodyKatana katana:
							newItem = Activator.CreateInstance(typeof(GargishKatana)) as BaseWeapon;
							break;
						case WarHammer hammer:
							newItem = Activator.CreateInstance(typeof(GargishWarHammer)) as BaseWeapon;
							break;

					}
					if (newItem != null)
					{
						BaseWeapon newWeapon = newItem as BaseWeapon;
						newWeapon.Altered = true;

						CopyAttributes(newWeapon, source);

						newWeapon.InvalidateProperties();
						success = true;
					}
				}
			}
			else if (oldItem is BaseArmor || oldItem is BaseClothing)
			{
				if (oldItem is HellShield)
				{
					newItem = Activator.CreateInstance(typeof(SmallPlateShield)) as Item;
				}
				switch (oldItem.Layer)
				{
					case Layer.Arms:
						newItem = Activator.CreateInstance(typeof(GargishLeatherArms)) as Item;
						break;
					case Layer.Neck:
						newItem = Activator.CreateInstance(typeof(GargishNecklace)) as Item;
						break;
					case Layer.InnerTorso:
						newItem = Activator.CreateInstance(typeof(GargishLeatherChest)) as Item;
						break;
					case Layer.Pants:
						newItem = Activator.CreateInstance(typeof(GargishLeatherLegs)) as Item;
						break;
					case Layer.Gloves:
						newItem = Activator.CreateInstance(typeof(GargishLeatherKilt)) as Item;
						break;
					case Layer.Waist:
						newItem = Activator.CreateInstance(typeof(GargishApron)) as Item;
						break;
					case Layer.Helm:
						newItem = Activator.CreateInstance(typeof(GargishBandana)) as Item;
						break;
					case Layer.Shoes:
						newItem = Activator.CreateInstance(typeof(LeatherTalons)) as Item;
						break;
				}
				if (newItem != null)
				{
					if (newItem is BaseClothing)
					{
						BaseClothing newClothing = (BaseClothing)newItem;
						newClothing.Altered = true;
						if (oldItem is BaseArmor)
						{
							BaseArmor oldarmor = (BaseArmor)oldItem;

							if (oldarmor.PlayerConstructed)
							{
								newClothing.PlayerConstructed = true;
								newClothing.Crafter = oldarmor.Crafter;
								newClothing.Quality = oldarmor.Quality;
							}
							newClothing.Name = oldarmor.Name;
							newClothing.Resource = oldarmor.Resource;

							newClothing.Hue = oldarmor.Hue;
							newClothing.LootType = oldarmor.LootType;
							newClothing.Insured = oldarmor.Insured;

							newClothing.CopyFromArmor(oldarmor);
							if (oldarmor is IDurability && newClothing is IDurability)
							{
								((IDurability)newClothing).MaxHitPoints = ((IDurability)oldarmor).MaxHitPoints;
								((IDurability)newClothing).HitPoints = ((IDurability)oldarmor).HitPoints;
							}
						}
						if (oldItem is BaseClothing)
						{
							BaseClothing oldClothing = (BaseClothing)oldItem;

							if (oldClothing.PlayerConstructed)
							{
								newClothing.PlayerConstructed = true;
								newClothing.Crafter = oldClothing.Crafter;
								newClothing.Quality = oldClothing.Quality;
							}
							newClothing.Name = oldClothing.Name;
							newClothing.Resource = oldClothing.Resource;

							newClothing.Hue = oldClothing.Hue;
							newClothing.LootType = oldClothing.LootType;
							newClothing.Insured = oldClothing.Insured;

							oldClothing.OnAfterDuped(newClothing);
							if (oldClothing is IDurability && newClothing is IDurability)
							{
								((IDurability)newClothing).MaxHitPoints = ((IDurability)oldClothing).MaxHitPoints;
								((IDurability)newClothing).HitPoints = ((IDurability)oldClothing).HitPoints;
							}
						}
						success = true;
					}
					if (newItem is BaseArmor)
					{
						BaseArmor newArmor = (BaseArmor)newItem;
						newArmor.Altered = true;
						if (oldItem is BaseArmor)
						{
							BaseArmor oldArmor = (BaseArmor)oldItem;

							if (oldArmor.PlayerConstructed)
							{
								newArmor.PlayerConstructed = true;
								newArmor.Crafter = oldArmor.Crafter;
								newArmor.Quality = oldArmor.Quality;
							}
							newArmor.Name = oldArmor.Name;
							newArmor.Resource = oldArmor.Resource;

							newArmor.Hue = oldArmor.Hue;
							newArmor.LootType = oldArmor.LootType;
							newArmor.Insured = oldArmor.Insured;

							oldArmor.OnAfterDuped(newArmor);
							if (oldArmor is IDurability && newArmor is IDurability)
							{
								((IDurability)newArmor).MaxHitPoints = ((IDurability)oldArmor).MaxHitPoints;
								((IDurability)newArmor).HitPoints = ((IDurability)oldArmor).HitPoints;
							}
						}
						if (oldItem is BaseClothing)
						{
							BaseClothing oldClothing = (BaseClothing)oldItem;

							if (oldClothing.PlayerConstructed)
							{
								newArmor.PlayerConstructed = true;
								newArmor.Crafter = oldClothing.Crafter;
								newArmor.Quality = oldClothing.Quality;
							}
							newArmor.Name = oldClothing.Name;
							newArmor.Resource = oldClothing.Resource;

							newArmor.Hue = oldClothing.Hue;
							newArmor.LootType = oldClothing.LootType;
							newArmor.Insured = oldClothing.Insured;

							// This isn't going to work, need a cloth -> armor like above armor -> cloth
							oldClothing.OnAfterDuped(newArmor);
							if (oldClothing is IDurability && newArmor is IDurability)
							{
								((IDurability)newArmor).MaxHitPoints = ((IDurability)oldClothing).MaxHitPoints;
								((IDurability)newArmor).HitPoints = ((IDurability)oldClothing).HitPoints;
							}
						}
						success = true;
					}
				}
			}
			if (success)
			{
				XmlLevelItem xmlAttachment = (XmlLevelItem)XmlAttach.FindAttachment(oldItem, typeof(XmlLevelItem));
				if (xmlAttachment != null)
				{
					XmlAttach.AttachTo(newItem, xmlAttachment);
				}

				newItem.Parent = null;
				if (from.Backpack == null)
					newItem.MoveToWorld(from.Location, from.Map);
				else
					from.Backpack.DropItem(newItem);

				newItem.InvalidateProperties();

				from.Backpack.RemoveItem(oldItem);
				oldItem.Delete();
				m_Deed.Delete();
				return;
			}


			from.SendMessage("You cannot Change that");
		}
		
		public class Human2G : Item // Create the item class which is derived from the base item class
	{
		[Constructable]
		public Human2G() : base( 0x14F0 )
		{
			Weight = 1.0;
			Name = "Human to Garg deed";
			LootType = LootType.Blessed;
			Hue = 1147;
		}

		public Human2G( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			LootType = LootType.Blessed;

			int version = reader.ReadInt();
		}

		public override bool DisplayLootType{ get{ return false; } }

		public override void OnDoubleClick( Mobile from ) // Override double click of the deed to call our target
		{
			if ( !IsChildOf( from.Backpack ) ) // Make sure its in their pack
			{
				 from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendMessage("What item would you like to change to gargoyle"  );
				from.Target = new H2GTarget( this ); // Call our target
			}
		}	
	}
}
}