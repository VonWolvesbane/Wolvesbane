using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Items;
using Server.Network;

public class SkillSelectGump : Gump
{
	private const int skillsPerPage = 10;

	private readonly Mobile _from;
	private readonly SkillKatana _katana;
	private readonly Item _token;
	private readonly int _page;

	private static readonly SkillName[] _skills = (SkillName[])Enum.GetValues(typeof(SkillName));

	public SkillSelectGump(Mobile from, SkillKatana katana, Item token, int page = 0) : base(50, 50)
	{
		_from = from;
		_katana = katana;
		_token = token;
		_page = page;

		int totalPages = (_skills.Length + skillsPerPage - 1) / skillsPerPage;
		int start = page * skillsPerPage;
		int end = Math.Min(start + skillsPerPage, _skills.Length);

		AddPage(0);
		AddBackground(0, 0, 300, 75 + (skillsPerPage * 25), 9270);

		AddLabel(100, 10, 1152, "Select a Skill");

		int y = 40;
		for (int i = start; i < end; i++)
		{
			SkillName skill = _skills[i];
			AddButton(20, y, 4005, 4007, i + 1, GumpButtonType.Reply, 0);
			AddLabel(60, y, 1152, skill.ToString());
			y += 25;
		}

		if (page > 0)
			AddButton(200, y, 4014, 4016, 1000, GumpButtonType.Reply, 0); // Prev

		if (page < totalPages - 1)
			AddButton(240, y, 4005, 4007, 1001, GumpButtonType.Reply, 0); // Next
	}

	public override void OnResponse(NetState sender, RelayInfo info)
	{
		if (_katana.Deleted || _token.Deleted)
			return;

		if (info.ButtonID == 1000)
		{
			_from.SendGump(new SkillSelectGump(_from, _katana, _token, _page - 1));
		}
		else if (info.ButtonID == 1001)
		{
			_from.SendGump(new SkillSelectGump(_from, _katana, _token, _page + 1));
		}
		else if (info.ButtonID >= 1 && info.ButtonID <= _skills.Length)
		{
			SkillName Skill = _skills[info.ButtonID - 1];
			_katana.SetSkill(_from, Skill, _token);
		}
	}
}
