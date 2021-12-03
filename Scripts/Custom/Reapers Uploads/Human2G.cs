// By SHAMBAMPOW
using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server;
//using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class H2GTarget : Target 
	{
		private Human2G m_Deed;

		public H2GTarget( Human2G deed ) : base( 1, false, TargetFlags.None )
		{
			m_Deed = deed;
		}

		protected override void OnTarget( Mobile from, object target )
		{
		
		 if ( target is BaseArmor || target is BaseClothing )
			{
				Item item = (Item)target;

					if( item.Layer == Layer.Arms ) // Make sure its in their pack or they are wearing it
					{
						//Item newitem = typeof(GargishLeatherArms));
						Item newitem = Activator.CreateInstance(typeof(GargishLeatherArms)) as Item;
						
					BaseArmor oldarmor = (BaseArmor)item;
                    BaseArmor newarmor = (BaseArmor)newitem;

						if (oldarmor.PlayerConstructed)
						{
                        newarmor.PlayerConstructed = true;
                        newarmor.Crafter = oldarmor.Crafter;
                        newarmor.Quality = oldarmor.Quality;
						}
						newarmor.Name = oldarmor.Name;
						newarmor.Resource = oldarmor.Resource;

						newarmor.PhysicalBonus = oldarmor.PhysicalBonus;
						newarmor.FireBonus = oldarmor.FireBonus;
						newarmor.ColdBonus = oldarmor.ColdBonus;
						newarmor.PoisonBonus = oldarmor.PoisonBonus;
						newarmor.EnergyBonus = oldarmor.EnergyBonus;
						newarmor.Hue = oldarmor.Hue;
						newarmor.LootType = oldarmor.LootType;
						newarmor.Insured = oldarmor.Insured;

						oldarmor.OnAfterDuped(newarmor);
						newarmor.Parent = null;
						
						newarmor.Altered = true;
						
						if (oldarmor is IDurability && newarmor is IDurability)
                {
                    ((IDurability)newarmor).MaxHitPoints = ((IDurability)oldarmor).MaxHitPoints;
                    ((IDurability)newarmor).HitPoints = ((IDurability)oldarmor).HitPoints;
                }

                if (from.Backpack == null)
                    newarmor.MoveToWorld(from.Location, from.Map);
                else
                    from.Backpack.DropItem(newarmor);

                newarmor.InvalidateProperties();
				
						oldarmor.Delete();
						m_Deed.Delete();
						
					}
					
					else if ( item.Layer == Layer.Neck ) // Make sure its in their pack or they are wearing it
					{
						//Item newitem = typeof(GargishNecklace));
						Item newitem = Activator.CreateInstance(typeof(GargishNecklace)) as Item;
						
					BaseArmor oldarmor = (BaseArmor)item;
                    BaseArmor newarmor = (BaseArmor)newitem;

						if (oldarmor.PlayerConstructed)
						{
                        newarmor.PlayerConstructed = true;
                        newarmor.Crafter = oldarmor.Crafter;
                        newarmor.Quality = oldarmor.Quality;
						}
						newarmor.Name = oldarmor.Name;
						newarmor.Resource = oldarmor.Resource;

						newarmor.PhysicalBonus = oldarmor.PhysicalBonus;
						newarmor.FireBonus = oldarmor.FireBonus;
						newarmor.ColdBonus = oldarmor.ColdBonus;
						newarmor.PoisonBonus = oldarmor.PoisonBonus;
						newarmor.EnergyBonus = oldarmor.EnergyBonus;
						newarmor.Hue = oldarmor.Hue;
						newarmor.LootType = oldarmor.LootType;
						newarmor.Insured = oldarmor.Insured;

						oldarmor.OnAfterDuped(newarmor);
						newarmor.Parent = null;
						
						newarmor.Altered = true;
						
						if (oldarmor is IDurability && newarmor is IDurability)
                {
                    ((IDurability)newarmor).MaxHitPoints = ((IDurability)oldarmor).MaxHitPoints;
                    ((IDurability)newarmor).HitPoints = ((IDurability)oldarmor).HitPoints;
                }

                if (from.Backpack == null)
                    newarmor.MoveToWorld(from.Location, from.Map);
                else
                    from.Backpack.DropItem(newarmor);

                newarmor.InvalidateProperties();
				
						oldarmor.Delete();
						m_Deed.Delete();
						
					}

					else if ( item.Layer == Layer.InnerTorso ) // Make sure its in their pack or they are wearing it
					{
						//Item newitem = typeof(GargishLeatherChest));
						Item newitem = Activator.CreateInstance(typeof(GargishLeatherChest)) as Item;
						
					BaseArmor oldarmor = (BaseArmor)item;
                    BaseArmor newarmor = (BaseArmor)newitem;

						if (oldarmor.PlayerConstructed)
						{
                        newarmor.PlayerConstructed = true;
                        newarmor.Crafter = oldarmor.Crafter;
                        newarmor.Quality = oldarmor.Quality;
						}
						newarmor.Name = oldarmor.Name;
						newarmor.Resource = oldarmor.Resource;

						newarmor.PhysicalBonus = oldarmor.PhysicalBonus;
						newarmor.FireBonus = oldarmor.FireBonus;
						newarmor.ColdBonus = oldarmor.ColdBonus;
						newarmor.PoisonBonus = oldarmor.PoisonBonus;
						newarmor.EnergyBonus = oldarmor.EnergyBonus;
						newarmor.Hue = oldarmor.Hue;
						newarmor.LootType = oldarmor.LootType;
						newarmor.Insured = oldarmor.Insured;

						oldarmor.OnAfterDuped(newarmor);
						newarmor.Parent = null;
						
						newarmor.Altered = true;
						
						if (oldarmor is IDurability && newarmor is IDurability)
                {
                    ((IDurability)newarmor).MaxHitPoints = ((IDurability)oldarmor).MaxHitPoints;
                    ((IDurability)newarmor).HitPoints = ((IDurability)oldarmor).HitPoints;
                }

                if (from.Backpack == null)
                    newarmor.MoveToWorld(from.Location, from.Map);
                else
                    from.Backpack.DropItem(newarmor);

                newarmor.InvalidateProperties();
				
						oldarmor.Delete();
						m_Deed.Delete();
						
					}

					else if ( item.Layer == Layer.Pants ) // Make sure its in their pack or they are wearing it
					{
						//Item newitem = typeof(GargishLeatherLegs));
						Item newitem = Activator.CreateInstance(typeof(GargishLeatherLegs)) as Item;
						
					BaseArmor oldarmor = (BaseArmor)item;
                    BaseArmor newarmor = (BaseArmor)newitem;

						if (oldarmor.PlayerConstructed)
						{
                        newarmor.PlayerConstructed = true;
                        newarmor.Crafter = oldarmor.Crafter;
                        newarmor.Quality = oldarmor.Quality;
						}
						newarmor.Name = oldarmor.Name;
						newarmor.Resource = oldarmor.Resource;

						newarmor.PhysicalBonus = oldarmor.PhysicalBonus;
						newarmor.FireBonus = oldarmor.FireBonus;
						newarmor.ColdBonus = oldarmor.ColdBonus;
						newarmor.PoisonBonus = oldarmor.PoisonBonus;
						newarmor.EnergyBonus = oldarmor.EnergyBonus;
						newarmor.Hue = oldarmor.Hue;
						newarmor.LootType = oldarmor.LootType;
						newarmor.Insured = oldarmor.Insured;

						oldarmor.OnAfterDuped(newarmor);
						newarmor.Parent = null;
						
						newarmor.Altered = true;
						
						if (oldarmor is IDurability && newarmor is IDurability)
                {
                    ((IDurability)newarmor).MaxHitPoints = ((IDurability)oldarmor).MaxHitPoints;
                    ((IDurability)newarmor).HitPoints = ((IDurability)oldarmor).HitPoints;
                }

                if (from.Backpack == null)
                    newarmor.MoveToWorld(from.Location, from.Map);
                else
                    from.Backpack.DropItem(newarmor);

                newarmor.InvalidateProperties();
				
						oldarmor.Delete();
						m_Deed.Delete();
						
					}
				
				else if ( item.Layer == Layer.Gloves ) // Make sure its in their pack or they are wearing it
					{
						//Item newitem = typeof(GargishLeatherKilt));
						Item newitem = Activator.CreateInstance(typeof(GargishLeatherKilt)) as Item;
						
					BaseArmor oldarmor = (BaseArmor)item;
                    BaseArmor newarmor = (BaseArmor)newitem;

						if (oldarmor.PlayerConstructed)
						{
                        newarmor.PlayerConstructed = true;
                        newarmor.Crafter = oldarmor.Crafter;
                        newarmor.Quality = oldarmor.Quality;
						}
						newarmor.Name = oldarmor.Name;
						newarmor.Resource = oldarmor.Resource;

						newarmor.PhysicalBonus = oldarmor.PhysicalBonus;
						newarmor.FireBonus = oldarmor.FireBonus;
						newarmor.ColdBonus = oldarmor.ColdBonus;
						newarmor.PoisonBonus = oldarmor.PoisonBonus;
						newarmor.EnergyBonus = oldarmor.EnergyBonus;
						newarmor.Hue = oldarmor.Hue;
						newarmor.LootType = oldarmor.LootType;
						newarmor.Insured = oldarmor.Insured;

						oldarmor.OnAfterDuped(newarmor);
						newarmor.Parent = null;
						
						newarmor.Altered = true;
						
						if (oldarmor is IDurability && newarmor is IDurability)
                {
                    ((IDurability)newarmor).MaxHitPoints = ((IDurability)oldarmor).MaxHitPoints;
                    ((IDurability)newarmor).HitPoints = ((IDurability)oldarmor).HitPoints;
                }

                if (from.Backpack == null)
                    newarmor.MoveToWorld(from.Location, from.Map);
                else
                    from.Backpack.DropItem(newarmor);

                newarmor.InvalidateProperties();
				
						oldarmor.Delete();
						m_Deed.Delete();
						
					}

			else 
			{
				from.SendMessage( "You cannot Change that" );
			}
		}
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