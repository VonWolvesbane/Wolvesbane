using Server.Targeting; 
using System; 
using Server; 
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Accounting;
using Server.Gumps; 
using Server.Multis;
using Server.Network;
using Server.Menus; 
using System.Linq;
using Server.Menus.Questions; 
using Server.Mobiles; 
using System.Collections;

namespace Server.Items
{
	public class PetLeash : Item
	{
		private int m_Charges = 100;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Charges
		{
			get { return m_Charges; }
			set { m_Charges = value; InvalidateProperties(); }
		}
		public int MaxCharges
		{
			get { return 250; }
		}
		public virtual bool DeleteWhenEmpty
		{ get { return false; } }

		[Constructable]
		public PetLeash() : base(0x1374)
		{
			Weight = 1.0;
			Movable = true;
			Hue = 1153;
			Name = "a pet leash";
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);

			list.Add(1060658, "Charges\t{0}", m_Charges.ToString());
		}

		public PetLeash(Serial serial) : base(serial)
		{
		}

		internal bool CheckDblClickCriteria(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else if (FSATS.EnableShrinkSystem == false)
			{
				from.SendMessage("The shrink system has been disabled. Contact your server administrator for details.");
			}
			else if (Charges <= 0)
			{
				from.SendMessage("TThe leash is out of charges.");
			}
			else if (from.Skills[SkillName.AnimalTaming].Value <= 75)
			{
				from.SendMessage("You must have 75 animal taming to use a hitching post.");
				from.SendMessage("Try using a pet shriking potion.");
			}
			else
			{
				// all good
				return true;
			}

			// fail
			return false;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (CheckDblClickCriteria(from))
			{
				from.BeginTarget(2, false, TargetFlags.None, new TargetCallback(OnTarget));
				from.SendMessage("What do you wish to shrink?");
			}

		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);

			writer.Write(m_Charges);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Charges = reader.ReadInt();
		}

		internal void OnTarget(Mobile from, object target)
		{
			if (target == from)
				from.SendMessage("You cant shrink yourself!");

			else if (target is PlayerMobile)
				from.SendMessage("That person gives you a dirty look.");

			else if (target is Item)
				from.SendMessage("You can only shrink pets that you own");

			else if (target is BaseBioCreature && FSATS.EnableBioShrink == false)
				from.SendMessage("Unnatural creatures cannot be shrunk");

			else if (Server.Spells.SpellHelper.CheckCombat(from))
				from.SendMessage("You cannot shrink your pet while your fighting.");

			else if (target is BaseCreature)
			{
				BaseCreature creature = (BaseCreature)target;

				bool packanimal = false;
				Type typ = creature.GetType();
				string nam = typ.Name;

				foreach (string ispack in FSATS.PackAnimals)
				{
					if (ispack == nam)
						packanimal = true;
				}

				if (creature.ControlMaster != from && creature.Controlled == false)
				{
					from.SendMessage("This is not your pet.");
				}
				else if (packanimal == true && (creature.Backpack != null && creature.Backpack.Items.Count > 0))
				{
					from.SendMessage("You must unload your pets backpack first.");
				}
				else if (creature.IsDeadPet)
				{
					from.SendMessage("You cannot shrink the dead.");
				}
				else if (creature.Summoned)
				{
					from.SendMessage("You cannot shrink a summoned creature.");
				}
				else if (creature.Combatant != null && creature.InRange(creature.Combatant, 12) && creature.Map == creature.Combatant.Map)
				{
					from.SendMessage("Your pet is fighting, You cannot shrink it yet.");
				}
				else if (creature.BodyMod != 0)
				{
					from.SendMessage("You cannot shrink your pet while its polymorphed.");
				}

				else if (creature.Controlled == true && creature.ControlMaster == from)
				{
					Type type = creature.GetType();
					ShrinkItem si = new ShrinkItem();
					// Can it be stored in the leashes container (must be PetBag)
					PetBag pack = this.Parent as PetBag;
					if (pack != null)
					{
						if (!pack.CheckHold(from, si, false, true, 0, 0))
							pack = null;
					}

					// if it cannot be stored in the container, find a pet bag somewhere
					if (pack == null)
					{
						pack = from.Backpack.FindItemByType(typeof(PetBag)) as PetBag;
						if (pack != null && !pack.CheckHold(from, si, false, true, 0, 0))
							pack = null;
					}

					// if no places to store it, dont shrink pet
					if (pack == null)
					{
						from.SendMessage("You need a Pet Bag with space to shrink a pet.");
						si.Delete();
						return;
					}

					si.MobType = type;
					si.Pet = creature;
					si.PetOwner = from;
					si.Name = creature.Name;

					if (creature is BaseMount)
					{
						BaseMount mount = (BaseMount)creature;
						si.MountID = mount.ItemID;
					}
					IEntity p1 = new Entity(Serial.Zero, new Point3D(from.X, from.Y, from.Z), from.Map);
					IEntity p2 = new Entity(Serial.Zero, new Point3D(from.X, from.Y, from.Z + 50), from.Map);

					Effects.SendMovingParticles(p2, p1, ShrinkTable.Lookup(creature), 1, 0, true, false, 0, 3, 1153, 1, 0, EffectLayer.Head, 0x100);
					from.PlaySound(492);

					creature.Controlled = true;
					creature.ControlMaster = null;
					creature.Internalize();

					creature.OwnerAbandonTime = DateTime.MinValue;

					creature.IsStabled = true;

					this.Charges -= 1;
					if (this.Charges == 0 && this.DeleteWhenEmpty)
						this.Delete();

					pack.DropItem(si);

				}
				else
				{
					from.SendMessage("Your pet bag is full.");
				}
				//end of new edit
			}
		}
	}
	public class AutoPetLeash : PetLeash
	{

		[Constructable]
		public AutoPetLeash() : base()
		{
			Hue = 1254;
			Weight = 1.0;
			Movable = true;
			Name = "Automatic PetLeash";
		}
		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
		}
		public AutoPetLeash(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (CheckDblClickCriteria(from))
			{
				PlayerMobile player = from as PlayerMobile;
				if (player != null)
				{
					System.Collections.Generic.List<Mobile> targets = new System.Collections.Generic.List<Mobile>();
					targets.AddRange(player.AllFollowers);
					foreach (Mobile pet in targets)
					{
						if (pet is BaseCreature && player.Mount != pet)
						{
							OnTarget(from, pet);
						}
					}
				}
			}
		}
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}

	}
}
