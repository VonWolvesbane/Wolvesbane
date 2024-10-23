using System;
using Server;
using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
	public class SkillKatana : Katana
	{
		private SkillName m_DefSkill = SkillName.Swords; // Default skill to Swords
		private DateTime m_SkillChangeTime;
		private Timer m_SkillTimer;
		private TimeSpan m_SkillDuration = TimeSpan.FromDays(7); // 7-day duration for the skill

		[CommandProperty(AccessLevel.GameMaster)]
		public SkillName DefSkill { get { return m_DefSkill; } set { m_DefSkill = value; UpdateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public DateTime SkillChangeTime { get { return m_SkillChangeTime; } set { m_SkillChangeTime = value; UpdateProperties(); } }

		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan SkillDuration
		{
			get { return m_SkillDuration; }
			set
			{
				m_SkillDuration = value;
				if (m_DefSkill != SkillName.Swords)
				{
					m_SkillChangeTime = DateTime.UtcNow.Add(m_SkillDuration);
					StartSkillTimer(m_SkillDuration); // Restart the timer
				}
				UpdateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public TimeSpan RemainingTime
		{
			get { return m_SkillChangeTime > DateTime.UtcNow ? m_SkillChangeTime - DateTime.UtcNow : TimeSpan.Zero; }
			set
			{
				m_SkillChangeTime = DateTime.UtcNow.Add(value);
				StartSkillTimer(value); // Restart the timer
				UpdateProperties();
			}
		}

		public override int InitMinHits { get { return 2600; } }
		public override int InitMaxHits { get { return 2600; } }
		public override int AosMinDamage { get { return 1; } }
		public override int AosMaxDamage { get { return 1; } }
		public override int AosSpeed { get { return 46; } }
		public override int DefHitSound { get { return 0x23B; } }
		public override int DefMissSound { get { return 0x23A; } }

		[Constructable]
		public SkillKatana()
		{
			Name = "A Skill Katana";
			Hue = 1164;
			Attributes.WeaponSpeed = 100;
			Attributes.AttackChance = 100;
			WeaponAttributes.SelfRepair = 100;
			SkillKatanaManager.Add(this);
		}

		public override void OnDelete()
		{
			SkillKatanaManager.Remove(this);
			StopSkillTimer();
			base.OnDelete();
		}

		public SkillKatana(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from is PlayerMobile player)
			{
				TimeSpan remainingTime = m_SkillChangeTime - DateTime.UtcNow;
				if (remainingTime > TimeSpan.Zero)
				{
					player.SendMessage($"You cannot change the skill for another {remainingTime.Days} days {remainingTime.Hours} hours and {remainingTime.Minutes} minutes.");
				}
				else
				{
					player.SendGump(new SkillSelectionGump(player, this, 0));
				}
			}
		}

		public void SetDefSkill(SkillName skill)
		{
			if (RemainingTime > TimeSpan.Zero)
			{
				Console.WriteLine("Cannot change skill until the timer has expired.");
				return;
			}

			m_DefSkill = skill;
			if (m_DefSkill != SkillName.Swords)
			{
				m_SkillChangeTime = DateTime.UtcNow.Add(m_SkillDuration);
				StartSkillTimer(m_SkillDuration); // Restart the timer
			}
			UpdateProperties(); // Explicitly update properties
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
			writer.Write((int)m_DefSkill); // Save the skill
			writer.Write(m_SkillChangeTime);
			writer.Write(m_SkillDuration);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_DefSkill = (SkillName)reader.ReadInt(); // Load the skill
			m_SkillChangeTime = reader.ReadDateTime();
			m_SkillDuration = reader.ReadTimeSpan();

			// Start timer if skill change time is in the future
			if (m_SkillChangeTime > DateTime.UtcNow)
			{
				StartSkillTimer(m_SkillChangeTime - DateTime.UtcNow);
			}
		}

		private void StartSkillTimer(TimeSpan delay)
		{
			StopSkillTimer();
			m_SkillTimer = new SkillKatanaTimer(this, delay);
			m_SkillTimer.Start();
		}

		private void StopSkillTimer()
		{
			if (m_SkillTimer != null)
			{
				m_SkillTimer.Stop();
				m_SkillTimer = null;
			}
		}

		public void UpdateProperties()
		{
			InvalidateProperties(); // Call this method to refresh properties
		}

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			string skillName = m_DefSkill.ToString().Replace('_', ' ');
			list.Add("Skill: {0}", skillName);
			if (m_DefSkill != SkillName.Swords)
			{
				TimeSpan remainingTime = m_SkillChangeTime - DateTime.UtcNow;
				if (remainingTime > TimeSpan.Zero)
				{
					list.Add("Time left: {0:D2}:{1:D2}:{2:D2}", (int)remainingTime.TotalHours, remainingTime.Minutes, remainingTime.Seconds);
				}
				else
				{
					list.Add("Time left: 00:00:00");
				}
			}
		}

		private string FormatTime(TimeSpan time)
		{
			return string.Format("{0}:{1:D2}:{2:D2}", (int)time.TotalHours, time.Minutes, time.Seconds);
		}
	}

	public class SkillKatanaTimer : Timer
	{
		private SkillKatana m_Katana;

		public SkillKatanaTimer(SkillKatana katana, TimeSpan delay) : base(delay)
		{
			m_Katana = katana;
			Priority = TimerPriority.OneMinute;
		}

		protected override void OnTick()
		{
			if (m_Katana != null && !m_Katana.Deleted)
			{
				m_Katana.SetDefSkill(SkillName.Swords); // Reset skill to default after timer ends
				m_Katana.UpdateProperties(); // Explicitly update properties
			}
			Stop();
		}
	}

	public static class SkillKatanaManager
	{
		private static List<SkillKatana> katanas = new List<SkillKatana>();

		public static void Add(SkillKatana katana)
		{
			if (!katanas.Contains(katana)) katanas.Add(katana);
		}

		public static void Remove(SkillKatana katana)
		
			{ katanas.Remove(katana); }
	}

	// Gump class for selecting skills
	public class SkillSelectionGump : Gump
	{
		private const int SkillsPerPage = 14;
		private PlayerMobile m_Mobile;
		private SkillKatana m_Weapon;
		private int m_Page;

		public SkillSelectionGump(PlayerMobile mobile, SkillKatana weapon, int page) : base(50, 50)
		{
			m_Mobile = mobile;
			m_Weapon = weapon;
			m_Page = page;
			AddPage(0);
			AddBackground(0, 0, 350, 480, 0x13EC);
			AddLabel(100, 10, 2100, "Made By: Phoenix for WolvesbaneUO");
			AddImageTiled(10, 10, 330, 30, 0x243A);
			AddLabel(125, 15, 2100, "Select Weapon Skill");
			int totalSkills = mobile.Skills.Length;
			int startSkill = m_Page * SkillsPerPage;
			int endSkill = Math.Min(startSkill + SkillsPerPage, totalSkills);
			int y = 60;
			for (int i = startSkill; i < endSkill; i++)
			{
				Skill skill = mobile.Skills[i];
				AddButton(50, y, 0x4B9, 0x4BA, i + 1, GumpButtonType.Reply, 0);
				AddLabel(90, y, 2100, skill.Info.Name);
				y += 25;
			}
			if (m_Page > 0)
			{
				AddButton(50, 410, 0xFAE, 0xFAF, 10000, GumpButtonType.Reply, 0);
				AddLabel(90, 410, 2100, "Previous");
			}
			if (endSkill < totalSkills)
			{
				AddButton(200, 410, 0xFA5, 0xFA6, 10001, GumpButtonType.Reply, 0);
				AddLabel(240, 410, 2100, "Next");
			}
			AddLabel(100, 440, 2100, "Made By: Phoenix for WolvesbaneUO");
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			PlayerMobile from = state.Mobile as PlayerMobile;
			if (info.ButtonID > 0 && info.ButtonID < 10000)
			{
				SkillName selectedSkill = (SkillName)(info.ButtonID - 1);
				Item wDollar = from.Backpack.FindItemByType<WDollar>();
				if (wDollar != null)
				{
					from.SendGump(new ConfirmationGump(from, selectedSkill, m_Weapon));
				}
				else
				{
					from.SendMessage("You need at least 1 W Dollar to change the skill.");
				}
			}
			else if (info.ButtonID == 10000)
			{
				from.SendGump(new SkillSelectionGump(from, m_Weapon, m_Page - 1));
			}
			else if (info.ButtonID == 10001)
			{
				from.SendGump(new SkillSelectionGump(from, m_Weapon, m_Page + 1));
			}
		}
	}

	// Gump class for confirmation
	public class ConfirmationGump : Gump
	{
		private PlayerMobile m_Mobile;
		private SkillName m_SelectedSkill;
		private SkillKatana m_Weapon;

		public ConfirmationGump(PlayerMobile mobile, SkillName selectedSkill, SkillKatana weapon) : base(50, 50)
		{
			m_Mobile = mobile;
			m_SelectedSkill = selectedSkill;
			m_Weapon = weapon;
			AddPage(0);
			AddBackground(0, 0, 550, 150, 0x13EC);
			AddLabel(100, 30, 2100, string.Format("Consume 1 W Dollar to change skill to {0}?", m_SelectedSkill.ToString().Replace('_', ' ')));
			AddButton(75, 100, 0xFA6, 0xFA5, 1, GumpButtonType.Reply, 0); // Yes
			AddLabel(110, 100, 2100, "Yes");
			AddButton(410, 100, 0xFA6, 0xFA5, 2, GumpButtonType.Reply, 0); // No
			AddLabel(445, 100, 2100, "No");
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (info.ButtonID == 1) // Yes
			{
				Item wDollar = m_Mobile.Backpack.FindItemByType<WDollar>();
				if (wDollar != null && wDollar.Amount > 0)
				{
					wDollar.Amount -= 1; 
					if (wDollar.Amount <= 0) 
					{
						wDollar.Delete(); 
					}
					
					m_Weapon.SetDefSkill(m_SelectedSkill); 
					m_Mobile.SendMessage("You have set the skill to " + m_SelectedSkill.ToString().Replace('_', ' '));
				}
				else
				{
					m_Mobile.SendMessage("You need at least 1 W Dollar to change the skill.");
				}
			}
		}
	}
}
