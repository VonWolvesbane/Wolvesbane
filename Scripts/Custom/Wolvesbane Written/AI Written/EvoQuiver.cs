using System;

namespace Server.Items
{
	public class QuiverOfEvolution : BaseQuiver
	{
		private int mEvolutionPoints = 0;
		private bool mIsEquipped;

		[CommandProperty(AccessLevel.GameMaster)]
		public int EvolutionPoints
		{
			get { return mEvolutionPoints; }
			set { mEvolutionPoints = value; }
		}

		public override int ArtifactRarity { get { return 2023; } }

		// Constructor
		[Constructable]
		public QuiverOfEvolution() : base(0x2FB7)
		{
			// Set the display name of the quiver
			Name = "Elven Quiver of Evolution";

			// Make it movable
			Movable = true;
		}

		public QuiverOfEvolution(Serial serial)
			: base(serial)
		{
		}

		public override bool OnEquip(Mobile from)
		{
			mIsEquipped = base.OnEquip(from);

			// Start the evolution process when the quiver is equipped
			if (mIsEquipped && from != null && from.Player && !from.IsDeadBondedPet)
			{
				Timer.DelayCall(TimeSpan.FromMinutes(1), new TimerCallback(OnEvolutionTick));
			}

			return mIsEquipped;
		}

		// Event handler for the evolution process
		private void OnEvolutionTick()
		{
			// Check if the quiver is still equipped by a player
			if (mIsEquipped && Parent is Mobile mobile)
			{
				if (mobile.Player && !mobile.IsDeadBondedPet)
				{
					// Gain 1 evolution point per tick (1 minute interval)
					mEvolutionPoints++;
					ApplyGain();
				}
			}
		}

		public void ApplyGain()
		{
			mEvolutionPoints++;
			this.Name = "Elven Quiver of Evolution (" + mEvolutionPoints.ToString() + ")";

			// Calculate the number of points that can be applied to each attribute
			int pointsPerAttribute = mEvolutionPoints / 1000; // Gain 1 point for every 1000 evolution points

			if (pointsPerAttribute > 100) // Cap the attribute gain at 100 per attribute
				pointsPerAttribute = 100;

			// Set the attributes based on the evolution points
			this.Attributes.LowerAmmoCost = pointsPerAttribute;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version

			// Serialize the evolution points
			writer.Write(mEvolutionPoints);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			// Deserialize the evolution points
			mEvolutionPoints = reader.ReadInt();
		}
	}
}

