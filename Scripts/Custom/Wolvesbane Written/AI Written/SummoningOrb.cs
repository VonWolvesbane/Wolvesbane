using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Xanthos.Evo;

namespace Server.Items
{
	public class ExaltedSummoningOrb : Item
	{
		private List<Type> m_CreatureTypes; // List of creature types to choose from

		[Constructable]
		public ExaltedSummoningOrb() : base(0x573E)
		{
			Name = "Summoning Orb";
			Hue = 1109;
			Weight = 1.0;

			// Define the list of creature types here
			m_CreatureTypes = new List<Type> { typeof(Hephastos), typeof(RidableAncientHellHound), typeof(GuardianMercenary), typeof(Alien1), typeof(GuardianWolfEvo), typeof(Idium) };
		}

		public ExaltedSummoningOrb(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendMessage("You must have the orb in your backpack to use it.");
				return;
			}

			if (m_CreatureTypes != null && m_CreatureTypes.Count > 0)
			{
				try
				{
					Type randomCreatureType = m_CreatureTypes[Utility.Random(m_CreatureTypes.Count)];
					BaseCreature creature = (BaseCreature)Activator.CreateInstance(randomCreatureType);
					if (creature != null)
					{
						creature.MoveToWorld(from.Location, from.Map);
						from.SendMessage("You summon a {0}.", creature.Name);
						this.Delete();
					}
				}
				catch (Exception ex)
				{
					from.SendMessage("An error occurred while summoning the creature.");
					Console.WriteLine("Error summoning creature: " + ex.Message);
				}
			}
			else
			{
				from.SendMessage("No creature types specified for summoning.");
			}
		}
	

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version

			writer.Write(m_CreatureTypes.Count);
			foreach (Type creatureType in m_CreatureTypes)
			{
				writer.Write(creatureType.FullName);
			}
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			int count = reader.ReadInt();
			m_CreatureTypes = new List<Type>();
			for (int i = 0; i < count; i++)
			{
				string creatureTypeName = reader.ReadString();
				if (!String.IsNullOrEmpty(creatureTypeName))
				{
					Type creatureType = ScriptCompiler.FindTypeByFullName(creatureTypeName);
					if (creatureType != null)
						m_CreatureTypes.Add(creatureType);
				}
			}
		}
	}
}
