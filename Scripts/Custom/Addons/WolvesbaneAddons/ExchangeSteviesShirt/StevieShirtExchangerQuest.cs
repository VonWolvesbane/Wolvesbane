// Make an NPC that will exchange Stevies Shirt for a Garg Tamers Robe
// We'll make sure the player is a garg, and they must be at least Noble to the garg queen
using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Spells;
namespace Server.Mobiles
{
	[CorpseName("Gargoyle")]
	public class StevieShirtExchanger : Mobile
	{

		internal const int RequiredPoints = 5000;
		public virtual bool IsInvulnerable { get { return true; } }
		[Constructable]
		public StevieShirtExchanger()
		{

			///////////STR/DEX/INT
			InitStats(31, 41, 51);

			///////////name
			Name = "Steglf";

			///////////title
			Title = "[Noble]";

			///////////Garg.
			Body = 0x0004;

			///////////skincolor
			Hue = Utility.RandomSkinHue();

			///////////Random hair and haircolor
			Utility.AssignRandomHair(this);

			///////////clothing and hues
			AddItem(new Server.Items.FancyShirt(Utility.RandomBlueHue()));
			AddItem(new Server.Items.LongPants(Utility.RandomBlueHue()));
			AddItem(new Server.Items.Sandals(Utility.RandomBlueHue()));

			///////////immortal and frozen to-the-spot features below:
			Blessed = true;
			CantWalk = false;

			///////////Adding a backpack
			Container pack = new Backpack();
			pack.DropItem(new Gold(250, 300));
			pack.Movable = false;
			AddItem(pack);
		}

		public StevieShirtExchanger(Serial serial) : base(serial) { }
		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list); 
			list.Add(new StevieShirtExchangerEntry(from, this));
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
		public class StevieShirtExchangerEntry : ContextMenuEntry
		{
			private Mobile m_Mobile; private Mobile m_Giver;
			public StevieShirtExchangerEntry(Mobile from, Mobile giver) 
				: base(6146, 3) 
			{ 
				m_Mobile = from; 
				m_Giver = giver; 
			}
			public override void OnClick()
			{
				if (!(m_Mobile is PlayerMobile)) 
					return;
				PlayerMobile mobile = (PlayerMobile)m_Mobile;
				{
					if (mobile.Race != Race.Gargoyle)
					{
						if (!mobile.HasGump(typeof(StevieShirtExchangerQuestGump3)))
						{
							mobile.SendGump(new StevieShirtExchangerQuestGump3(mobile));
						}
						return;
					}

					double points = Server.Engines.Points.PointsSystem.QueensLoyalty.GetPoints(mobile);
					if (points > RequiredPoints)
					{
						if (!mobile.HasGump(typeof(StevieShirtExchangerQuestGump1)))
						{
							mobile.SendGump(new StevieShirtExchangerQuestGump1(mobile));
						}
					}
					else 
					{
						if (!mobile.HasGump(typeof(StevieShirtExchangerQuestGump2)))
						{
							mobile.SendGump(new StevieShirtExchangerQuestGump2(mobile));
						}
					}
				}
			}
		}
		public override bool OnDragDrop(Mobile from, Item dropped)
		{
			PlayerMobile mobile = from as PlayerMobile;
			if (mobile == null)
			{
				return false;
			}

			if (mobile.Race == Race.Gargoyle)
			{

				double points = Server.Engines.Points.PointsSystem.QueensLoyalty.GetPoints(from);
				if (points > RequiredPoints)
				{
					if (dropped is StevesShirt)
					{
						dropped.Delete();

						mobile.AddToBackpack(new RoyalGargishTamingSash());

						this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Your service to the queen is appreciated", mobile.NetState);

						return true;
					}
				}
			}
			this.PrivateOverheadMessage(MessageType.Regular, 1153, false, "Be gone!", mobile.NetState);
			return false;
		}
	}
}
