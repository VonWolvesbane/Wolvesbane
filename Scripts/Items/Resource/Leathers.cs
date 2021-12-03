namespace Server.Items
{
    public abstract class BaseLeather : Item, ICommodity
    {
        protected virtual CraftResource DefaultResource => CraftResource.RegularLeather;

        private CraftResource m_Resource;
        public BaseLeather(CraftResource resource)
            : this(resource, 1)
        {
        }

        public BaseLeather(CraftResource resource, int amount)
            : base(0x1081)
        {
            Stackable = true;
            Weight = 1.0;
            Amount = amount;
            Hue = CraftResources.GetHue(resource);

            m_Resource = resource;
        }

        public BaseLeather(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get
            {
                return m_Resource;
            }
            set
            {
                m_Resource = value;
                InvalidateProperties();
            }
        }
        public override int LabelNumber
        {
            get
            {
                if (m_Resource >= CraftResource.SpinedLeather && m_Resource <= CraftResource.BarbedLeather)
                    return 1049684 + (m_Resource - CraftResource.SpinedLeather);

                return 1047022;
            }
        }
        TextDefinition ICommodity.Description => LabelNumber;
        bool ICommodity.IsDeedable => true;
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            writer.Write((int)m_Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 2: // Reset from Resource System
                    m_Resource = DefaultResource;
                    reader.ReadString();
                    break;
                case 1:
                    {
                        m_Resource = (CraftResource)reader.ReadInt();
                        break;
                    }
                case 0:
                    {
                        OreInfo info = new OreInfo(reader.ReadInt(), reader.ReadInt(), reader.ReadString());

                        m_Resource = CraftResources.GetFromOreInfo(info);
                        break;
                    }
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            if (Amount > 1)
                list.Add(1050039, "{0}\t#{1}", Amount, 1024199); // ~1_NUMBER~ ~2_ITEMNAME~
            else
                list.Add(1024199); // cut leather
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (!CraftResources.IsStandard(m_Resource))
            {
                int num = CraftResources.GetLocalizationNumber(m_Resource);

                if (num > 0)
                    list.Add(num);
                else
                    list.Add(CraftResources.GetName(m_Resource));
            }
        }
    }

    [Flipable(0x1081, 0x1082)]
    public class Leather : BaseLeather
    {
        [Constructable]
        public Leather()
            : this(1)
        {
        }

        [Constructable]
        public Leather(int amount)
            : base(CraftResource.RegularLeather, amount)
        {
        }

        public Leather(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x1081, 0x1082)]
    public class SpinedLeather : BaseLeather
    {
        protected override CraftResource DefaultResource => CraftResource.SpinedLeather;

        [Constructable]
        public SpinedLeather()
            : this(1)
        {
        }

        [Constructable]
        public SpinedLeather(int amount)
            : base(CraftResource.SpinedLeather, amount)
        {
        }

        public SpinedLeather(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x1081, 0x1082)]
    public class HornedLeather : BaseLeather
    {
        protected override CraftResource DefaultResource => CraftResource.HornedLeather;

        [Constructable]
        public HornedLeather()
            : this(1)
        {
        }

        [Constructable]
        public HornedLeather(int amount)
            : base(CraftResource.HornedLeather, amount)
        {
        }

        public HornedLeather(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x1081, 0x1082)]
    public class BarbedLeather : BaseLeather
    {
        protected override CraftResource DefaultResource => CraftResource.BarbedLeather;

        [Constructable]
        public BarbedLeather()
            : this(1)
        {
        }

        [Constructable]
        public BarbedLeather(int amount)
            : base(CraftResource.BarbedLeather, amount)
        {
        }

        public BarbedLeather(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
	[FlipableAttribute(0x1081, 0x1082)]
	public class PolarLeather : BaseLeather
	{
		[Constructable]
		public PolarLeather()
			: this(1)
		{
		}

		[Constructable]
		public PolarLeather(int amount)
			: base(CraftResource.PolarLeather, amount)
		{
			Name = "Polar Leather"; //daat99 OWLTR - resource name
		}

		public PolarLeather(Serial serial)
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

	[FlipableAttribute(0x1081, 0x1082)]
	public class SyntheticLeather : BaseLeather
	{
		[Constructable]
		public SyntheticLeather()
			: this(1)
		{
		}

		[Constructable]
		public SyntheticLeather(int amount)
			: base(CraftResource.SyntheticLeather, amount)
		{
			Name = "Synthetic Leather"; //daat99 OWLTR - resource name
		}

		public SyntheticLeather(Serial serial)
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

	[FlipableAttribute(0x1081, 0x1082)]
	public class BlazeLeather : BaseLeather
	{
		[Constructable]
		public BlazeLeather()
			: this(1)
		{
		}

		[Constructable]
		public BlazeLeather(int amount)
			: base(CraftResource.BlazeLeather, amount)
		{
			Name = "Blaze Leather"; //daat99 OWLTR - resource name
		}

		public BlazeLeather(Serial serial)
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

	[FlipableAttribute(0x1081, 0x1082)]
	public class DaemonicLeather : BaseLeather
	{
		[Constructable]
		public DaemonicLeather()
			: this(1)
		{
		}

		[Constructable]
		public DaemonicLeather(int amount)
			: base(CraftResource.DaemonicLeather, amount)
		{
			Name = "Daemonic Leather"; //daat99 OWLTR - resource name
		}

		public DaemonicLeather(Serial serial)
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

	[FlipableAttribute(0x1081, 0x1082)]
	public class ShadowLeather : BaseLeather
	{
		[Constructable]
		public ShadowLeather()
			: this(1)
		{
		}

		[Constructable]
		public ShadowLeather(int amount)
			: base(CraftResource.ShadowLeather, amount)
		{
			Name = "Shadow Leather"; //daat99 OWLTR - resource name
		}

		public ShadowLeather(Serial serial)
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

	[FlipableAttribute(0x1081, 0x1082)]
	public class FrostLeather : BaseLeather
	{
		[Constructable]
		public FrostLeather()
			: this(1)
		{
		}

		[Constructable]
		public FrostLeather(int amount)
			: base(CraftResource.FrostLeather, amount)
		{
			Name = "Frost Leather"; //daat99 OWLTR - resource name
		}

		public FrostLeather(Serial serial)
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

	[FlipableAttribute(0x1081, 0x1082)]
	public class EtherealLeather : BaseLeather
	{
		[Constructable]
		public EtherealLeather()
			: this(1)
		{
		}

		[Constructable]
		public EtherealLeather(int amount)
			: base(CraftResource.EtherealLeather, amount)
		{
			Name = "Ethereal Leather"; //daat99 OWLTR - resource name
		}

		public EtherealLeather(Serial serial)
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
	// dat stuff
}