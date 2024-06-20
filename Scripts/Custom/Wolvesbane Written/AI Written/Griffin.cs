using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a griffin corpse")]
	public class Griffin : BaseMount
	{
		public override bool IsBondable => true;
		

		[Constructable]
		public Griffin() : base("Griffin", 0xBC, 0x3EB8, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			

			// Set random attributes for the griffin
			int randomStr = Utility.RandomMinMax(1200, 5000);
			int randomDex = Utility.RandomMinMax(1200, 5000);
			int randomInt = Utility.RandomMinMax(1200, 5000);
			int randomHits = Utility.RandomMinMax(2300, 7500);
			int randomMinDamage = Utility.RandomMinMax(100, 250);
			int randomMaxDamage = Utility.RandomMinMax(250, 1000);

			// Set attributes and hit points
			SetStr(randomStr, 10000);
			SetDex(randomDex, 10000);
			SetInt(randomInt, 10000);
			SetHits(randomHits);
			SetStam(1000);

			// Set random mana value for Magery skill
			int randomMana = Utility.RandomMinMax(100, 500);
			SetMana(randomMana);

			// Set random damage range
			SetDamage(randomMinDamage, randomMaxDamage);

			// Set random resistances between 85 and 120
			int randomResist = Utility.RandomMinMax(85, 120);
			SetResistance(ResistanceType.Physical, randomResist);
			SetResistance(ResistanceType.Fire, randomResist);
			SetResistance(ResistanceType.Cold, randomResist);
			SetResistance(ResistanceType.Poison, randomResist);
			SetResistance(ResistanceType.Energy, randomResist);

			// Set initial skill values and maximum trainable values
			SetSkill(SkillName.Wrestling, 75.0, 120.0);
			SetSkill(SkillName.Tactics, 75.0, 120.0);
			SetSkill(SkillName.MagicResist, 75.0, 120.0);
			SetSkill(SkillName.Anatomy, 75.0, 120.0);
			SetSkill(SkillName.Magery, 75.0, 120.0);
			SetSkill(SkillName.Parry, 75.0, 120.0);
			SetSkill(SkillName.EvalInt, 75.0, 120.0);
			SetSkill(SkillName.Meditation, 75.0, 120.0);
			SetSkill(SkillName.Spellweaving, 75.0, 120.0);

			Tamable = true;
			
			MinTameSkill = 145.1;
			ControlSlots = 5;
		}

		public Griffin(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}

		// Implement special attacks here

		public void RippingTalons(Mobile target)
		{
			// Check if the target is valid and not already dead
			if (target != null && !target.Deleted)
			{
				// Calculate the amount of bleeding damage based on the Griffin's strength
				int bleedingDamage = Str / 10;

				// Apply the bleeding effect to the target
				target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
				target.SendLocalizedMessage(1060169); // You are bleeding!

				// Schedule a damage event to apply bleeding damage over time
				Timer.DelayCall(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(1.0), 10, () =>
				{
					if (target != null && !target.Deleted && target.Alive)
					{
						target.Damage(bleedingDamage, this);
					}
				});
			}
		}

		public void SwoopingGust()
		{
			// Get a list of nearby mobiles within a certain range (adjust the range as needed)
			var mobiles = GetMobilesInRange(4);

			// Check each mobile in the list
			foreach (Mobile mobile in mobiles)
			{
				if (mobile != null && !mobile.Deleted && mobile.Alive && mobile != this)
				{
					// Calculate the damage to apply based on the Griffin's dexterity
					int swoopingDamage = Dex / 8;

					// Apply the damage to the mobile
					mobile.Damage(swoopingDamage, this);

					// Apply knockback to the mobile
					ApplyKnockback(mobile, this, 1, true, true);
				}
			}
		}

		// Method to apply knockback to a target
		public static void ApplyKnockback(Mobile target, Mobile from, int distance, bool forwards, bool vertical)
		{
			if (target == null || target.Deleted || from == null || from.Deleted)
				return;

			if (target.Map == null || !target.InRange(from.Location, 12) || !from.InLOS(target))
				return;

			Map map = target.Map;

			int dx = target.X - from.X;
			int dy = target.Y - from.Y;

			int adx = Math.Abs(dx);
			int ady = Math.Abs(dy);

			if ((forwards && ((dx <= 0 && dy <= 0) || (dx >= 0 && dy >= 0)))
				|| (!forwards && ((dx <= 0 && dy >= 0) || (dx >= 0 && dy <= 0))))
			{
				adx += distance;
				ady += distance;
			}
			else
			{
				adx -= distance;
				ady -= distance;

				if (adx < 0)
					adx = 0;
				if (ady < 0)
					ady = 0;
			}

			if (adx > ady)
				ady = adx;
			else
				adx = ady;

			int x = target.X + (dx == 0 ? 0 : (dx < 0 ? -adx : adx));
			int y = target.Y + (dy == 0 ? 0 : (dy < 0 ? -ady : ady));

			if (!map.CanFit(x, y, target.Z, 16, false, false))
				return;

			target.Location = new Point3D(x, y, target.Z);
			target.ProcessDelta();

			if (vertical)
			{
				int val = Math.Abs(from.Z - target.Z);

				if (val >= 16)
					return;

				target.Z = from.Z;
				target.ProcessDelta();
			}
		}

		public void EaglesEye()
		{
			// Set the duration for the increased accuracy and critical hit chance (adjust as needed)
			TimeSpan duration = TimeSpan.FromSeconds(10.0);

			// Increase the Griffin's accuracy and critical hit chance
			this.Hits += 25;
			this.DamageMin += 10;
			this.DamageMax += 10;

			// Schedule a timer to revert the changes after the duration
			Timer.DelayCall(duration, () =>
			{
				if (this != null && !this.Deleted)
				{
					// Revert the changes after the duration
					this.Hits -= 25;
					this.DamageMin -= 10;
					this.DamageMax -= 10;
				}
			});
		}
	}
}
