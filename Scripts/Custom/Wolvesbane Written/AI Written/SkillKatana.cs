using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;

public class SkillKatana : Katana
{
	public override int LabelNumber => 1076257; // Optional: custom name
	private DateTime _NextSkillChange;

	[CommandProperty(AccessLevel.GameMaster)]
	public DateTime NextSkillChange
	{
		get => _NextSkillChange;
		set => _NextSkillChange = value;
	}

	[Constructable]
	public SkillKatana()
	{
		Hue = 1150;
		LootType = LootType.Blessed;
		Weight = 6.0;
		Name = "Skill Katana";
	}

	public override void OnDoubleClick(Mobile from)
	{
		if (!IsChildOf(from.Backpack))
		{
			from.SendMessage("That must be in your backpack.");
			return;
		}

		from.SendMessage("This katana is currently set to train: " + this.Skill.ToString());
	}

	public void SetSkill(Mobile from, SkillName skill, Item token)
	{
		if (token == null || token.Deleted)
		{
			from.SendMessage("You need a WolvesbaneDollar to change the skill.");
			return;
		}

		if (_NextSkillChange > DateTime.UtcNow)
		{
			TimeSpan remaining = _NextSkillChange - DateTime.UtcNow;
			from.SendMessage($"You must wait {remaining.Days}d {remaining.Hours}h {remaining.Minutes}m before changing the skill again.");
			return;
		}

		token.Consume(); // Consume one WolvesbaneDollar

		this.Skill = skill; // ✅ Set actual weapon skill
		_NextSkillChange = DateTime.UtcNow + TimeSpan.FromDays(3);

		from.SendMessage($"Katana is now set to use skill: {this.Skill}");
		from.SendMessage($"[DEBUG] Next skill change set for: {_NextSkillChange} (UTC)");
	}

	public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
	{
		if (from.Alive && IsChildOf(from.Backpack))
		{
			list.Add(new ChangeSkillEntry(from, this));
		}
	}

	public override void AddNameProperties(ObjectPropertyList list)
	{
		base.AddNameProperties(list);

		list.Add(1070722, "Trains Skill: {0}", this.Skill.ToString());

		TimeSpan cooldown = _NextSkillChange - DateTime.UtcNow;
		if (cooldown > TimeSpan.Zero)
		{
			list.Add(1070722, "Change ready in: {0}d {1}h {2}m", cooldown.Days, cooldown.Hours, cooldown.Minutes);
		}
		else
		{
			list.Add(1070722, "Skill change is ready.");
		}
	}

	private class ChangeSkillEntry : ContextMenuEntry
	{
		private readonly Mobile _from;
		private readonly SkillKatana _katana;

		public ChangeSkillEntry(Mobile from, SkillKatana katana)
			: base(1078584, 12)
		{
			_from = from;
			_katana = katana;
		}

		public override void OnClick()
		{
			if (!_katana.IsChildOf(_from.Backpack))
			{
				_from.SendMessage("That must be in your backpack.");
				return;
			}

			if (_katana.NextSkillChange > DateTime.UtcNow)
			{
				TimeSpan remaining = _katana.NextSkillChange - DateTime.UtcNow;
				_from.SendMessage($"You must wait {remaining.Days}d {remaining.Hours}h {remaining.Minutes}m before changing the skill again.");
				return;
			}

			Item token = _from.Backpack.FindItemByType(typeof(WDollar));
			if (token == null)
			{
				_from.SendMessage("You need a Wolvesbane Dollar in your backpack to change the skill.");
				return;
			}

			_from.SendGump(new SkillSelectGump(_from, _katana, token));
		}

		public override string ToString()
		{
			return "Change Skill";
		}
	}

	public SkillKatana(Serial serial) : base(serial) { }

	public override void Serialize(GenericWriter writer)
	{
		base.Serialize(writer);
		writer.Write(0); // version
		writer.Write(_NextSkillChange);
	}

	public override void Deserialize(GenericReader reader)
	{
		base.Deserialize(reader);
		int version = reader.ReadInt();
		_NextSkillChange = reader.ReadDateTime();
	}
}
