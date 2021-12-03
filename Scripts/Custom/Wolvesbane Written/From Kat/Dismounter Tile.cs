using System;
using Server;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Items
{
    public class Dismounter : Item
    {
        #region Variables/Properties
        private bool m_Active;
        private newDirection m_Direction;
        private string m_Message;
        private bool m_MountOnly;
        private bool m_Silent;
        private double m_Chance;
        private string m_Emote;
        private bool m_StableMount;
        private bool m_SkillCheck;
        private string m_EthyMsg = "Your Ethereal Mount has been placed in your backpack";

        private char[] trimChar = { '*', ' ' };

        [CustomEnum(new string[] { "North", "Right", "East", "Down", "South", "Left", "West", "Up", "Random", "Opposite" })]
        public enum newDirection : byte
        {
            North = 0x0,
            Right = 0x1,
            East = 0x2,
            Down = 0x3,
            South = 0x4,
            Left = 0x5,
            West = 0x6,
            Up = 0x7,
            Random = 0x8,
            Opposite = 0x9
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public String Emote
        {
            get { return m_Emote; }
            set
            {
                m_Emote = value;
                m_Emote = '*' + m_Emote.TrimStart(trimChar);
                m_Emote = m_Emote.TrimEnd(trimChar) + '*';
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Chance
        {
            get { return (int)(m_Chance * 100); }
            set
            {
                int num = value;

                if (num > 100)
                    num = 100;
                else if (num < 1)
                {
                    m_Chance = 0;
                    m_Active = false;
                    return;
                }

                m_Chance = (double)num / 100;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Active
        {
            get { return m_Active; }
            set { m_Active = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public newDirection Facing
        {
            get { return m_Direction; }
            set { m_Direction = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public String Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool BlockMountOnly
        {
            get { return m_MountOnly; }
            set { m_MountOnly = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Silent
        {
            get { return m_Silent; }
            set { m_Silent = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool StableMount
        {
            get { return m_StableMount; }
            set { m_StableMount = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool UseSkillCheck
        {
            get { return m_SkillCheck; }
            set { m_SkillCheck = value; }
        }
        #endregion

        #region Constructors
        [Constructable]
        public Dismounter()
            : this(newDirection.Opposite, null, true)
        {
        }

        [Constructable]
        public Dismounter(newDirection dir)
            : this(dir, null, true)
        {
        }

        [Constructable]
        public Dismounter(newDirection dir, string msg)
            : this(dir, msg, true)
        {
        }

        [Constructable]
        public Dismounter(newDirection dir, bool active)
            : this(dir, null, active)
        {
        }

        [Constructable]
        public Dismounter(newDirection dir, string msg, bool active)
            : base(0x1B7A)
        {
            Movable = false;
            Visible = false;
            Name = "Dismounter";

            m_Active = active;
            m_Direction = dir;
            m_Message = msg;

            m_Chance = 1; // 100% Chance to dismount
            m_SkillCheck = true;
        }
        #endregion

        #region Methods
        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel < AccessLevel.GameMaster)
                return;

            from.SendGump(new PropertiesGump(from, this));
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(this.Name);

            if (m_Active)
                list.Add(1060742); // active
            else
                list.Add(1060743); // inactive

            list.Add(((newDirection)m_Direction).ToString());
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            if (m_Active)
                LabelTo(from, "Facing " + ((newDirection)m_Direction).ToString());
            else
                LabelTo(from, "(inactive)");
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m_Active)
            {
                bool rtn;
                int dir = (int)m_Direction;
                double throwChance = Utility.RandomDouble();
                double skillBuff = 1.0;

                if (m_SkillCheck)
                {
                    //100 in all 3 skills doubles the chance to remain
                    //mounted when dismount chance is set to less than 100%
                    double taming = m.Skills[SkillName.AnimalTaming].Value;
                    double anlore = m.Skills[SkillName.AnimalLore].Value;
                    double vetern = m.Skills[SkillName.Veterinary].Value;
                    skillBuff = (taming + anlore + vetern) / 150;
                }

                if (m_Chance < 1.0 && (throwChance * skillBuff) > m_Chance)
                {
                    return true;
                }

                if (m.Player && m.Mounted)
                {
                    IMount mount = (IMount)m.Mount;

                    if (mount is BaseMount)
                    {
                        if (m_StableMount)
                        {
                            rtn = DoStableMount((BaseMount)mount);
                            if (rtn == false)
                                return false;
                        }
                        else
                        {
                            mount.Rider = null;
                            DoSound((BaseMount)mount);

                            if (dir == (int)newDirection.Random)
                                dir = Utility.RandomMinMax(0, 7);
                            else if (dir == (int)newDirection.Opposite)
                                dir = ((int)((BaseMount)mount).Direction + 4) % 0x8;

                            ((BaseMount)mount).Direction = (Direction)dir;
                        }
                    }

                    else if (mount is EtherealMount)
                    {
                        ((EtherealMount)mount).UnmountMe();
                        ((EtherealMount)mount).Rider = null;
                        if (m_EthyMsg != null && m_EthyMsg != "")
                            m.SendMessage(m_EthyMsg);
                    }


                    if (m_Message != null)
                        m.SendMessage(m_Message);

                    if (m_Emote != null)
                        m.Emote(m_Emote);
                }

                else if (m_MountOnly && m is BaseMount)
                {
                    DoSound(m);

                    if (dir == (int)newDirection.Random)
                        dir = Utility.RandomMinMax(0, 7);
                    else if (dir == (int)newDirection.Opposite)
                        dir = ((int)((BaseMount)m).Direction + 4) % 0x8;

                    ((BaseMount)m).Direction = (Direction)dir;
                    return false;
                }
            }
            return true;
        }


        public bool DoStableMount(BaseMount mount)
        {
            Mobile m = mount.Rider;
            if (m == null)
                return false;

            BaseCreature bc = mount as BaseCreature;
            if (bc == null)
                return false;

            if (m.Stabled.Count >= AnimalTrainer.GetMaxStabled(m))
            {
                m.SendLocalizedMessage(1042565); // You have too many pets in the stables!
                return false;
            }

            //m.SendMessage("Stabled Prior: {0} of {1}", m.Stabled.Count, AnimalTrainer.GetMaxStabled(m));

            mount.Rider = null;

            bc.ControlTarget = null;
            bc.ControlOrder = OrderType.Stay;

            bc.Internalize();

            bc.SetControlMaster(null);
            bc.SummonMaster = null;

            bc.IsStabled = true;

            if (Core.SE)
                bc.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully happy

            m.Stabled.Add(bc);

            //m.SendMessage("Stabled After: {0} of {1}", m.Stabled.Count, AnimalTrainer.GetMaxStabled(m));

            return true;
        }

        private void DoSound(Mobile m)
        {
            if (m_Silent) return;
            Effects.PlaySound(m.Location, m.Map, m.BaseSoundID + Utility.Random(4));
        }
        #endregion

        #region Serialize/Deserialize
        public Dismounter(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)4);

            writer.Write(m_StableMount);
            writer.Write(m_SkillCheck);

            writer.Write(m_Chance);
            writer.Write(m_Emote);

            writer.Write(m_Silent);
            writer.Write(m_MountOnly);

            writer.Write(m_Message);
            writer.Write(m_Active);
            writer.Write((int)m_Direction);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 4:
                    {
                        m_StableMount = reader.ReadBool();
                        m_SkillCheck = reader.ReadBool();
                        goto case 3;
                    }
                case 3:
                    {
                        if (version < 4) m_SkillCheck = true;

                        m_Chance = reader.ReadDouble();
                        m_Emote = reader.ReadString();
                        goto case 2;
                    }
                case 2:
                    {
                        if (version < 3) m_Chance = 100;

                        m_Silent = reader.ReadBool();
                        m_MountOnly = reader.ReadBool();
                        goto case 1;
                    }
                case 1:
                    {
                        m_Message = reader.ReadString();
                        goto case 0;
                    }
                case 0:
                    {
                        m_Active = reader.ReadBool();
                        m_Direction = (newDirection)reader.ReadInt();
                        break;
                    }
            }
        }
        #endregion
    }
}