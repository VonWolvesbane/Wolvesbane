using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute(0xF43, 0xF44)]
	public class LumberjacksAxe : BaseAxe, IUsesRemaining
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

		public override int StrengthReq { get { return 20; } }
		public override int MinDamage { get { return 13; } }
		public override int MaxDamage { get { return 15; } }
		public override float Speed { get { return 41; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 80; } }

		[Constructable]
		public LumberjacksAxe() : this (50 ) { }
		[Constructable]
		public LumberjacksAxe( int uses )
			: base(0xF43)
		{
			ShowUsesRemaining = true;
			UsesRemaining = uses;
			Name = "Lumberjack's Axe";
			Weight = 4.0;
		}

		public LumberjacksAxe(Serial serial)
			: base(serial)
		{
		}
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}