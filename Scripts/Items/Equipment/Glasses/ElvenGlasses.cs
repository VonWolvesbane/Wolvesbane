using Server.Engines.Craft;

namespace Server.Items
{
    [Alterable(typeof(DefTinkering), typeof(GargishGlasses), true)]
    public class ElvenGlasses : BaseArmor, IRepairable, ICanBeElfOrHuman
    {
        public override int LabelNumber => 1032216;  // elven glasses
        public CraftSystem RepairSystem => DefTinkering.CraftSystem;


		public AosWeaponAttributes m_AosWeaponAttributes;
		[CommandProperty(AccessLevel.GameMaster)]
		public AosWeaponAttributes WeaponAttributes => this.m_AosWeaponAttributes;


		[Constructable]
		public ElvenGlasses()
			: base(0x2FB8)
		{
			this.Weight = 2;
			this.m_AosWeaponAttributes = new AosWeaponAttributes(this);
		}

		private bool _ElvesOnly;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ElfOnly { get { return _ElvesOnly; } set { _ElvesOnly = value; } }

        public ElvenGlasses(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance => 2;
        public override int BaseFireResistance => 4;
        public override int BaseColdResistance => 4;
        public override int BasePoisonResistance => 3;
        public override int BaseEnergyResistance => 2;
        public override int InitMinHits => 36;
        public override int InitMaxHits => 48;
        public override int StrReq => 45;
        public override ArmorMaterialType MaterialType => ArmorMaterialType.Leather;
        public override CraftResource DefaultResource => CraftResource.RegularLeather;
        public override ArmorMeditationAllowance DefMedAllowance => ArmorMeditationAllowance.All;



		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			SaveFlag flags = SaveFlag.None;

			SetSaveFlag(ref flags, SaveFlag.WeaponAttributes, !this.WeaponAttributes.IsEmpty);

			writer.Write((int)flags);

			if (GetSaveFlag(flags, SaveFlag.WeaponAttributes))
				this.WeaponAttributes.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			SaveFlag flags = (SaveFlag)reader.ReadInt();

			if (GetSaveFlag(flags, SaveFlag.WeaponAttributes))
				this.m_AosWeaponAttributes = new AosWeaponAttributes(this, reader);
			else
				this.m_AosWeaponAttributes = new AosWeaponAttributes(this);
		}

		private static void SetSaveFlag(ref SaveFlag flags, SaveFlag toSet, bool setIf)
		{
			if (setIf)
				flags |= toSet;
		}

		private static bool GetSaveFlag(SaveFlag flags, SaveFlag toGet)
		{
			return ((flags & toGet) != 0);
		}
	}
}
