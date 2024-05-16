using System;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Gumps
{
    public class MountIDGump : Gump
    {
        private SkillMountChimera m_Mount;

        public MountIDGump(SkillMountChimera mount, Mobile from) : base(50, 50)
        {
            m_Mount = mount;

			Closable = false;

            AddPage(0);
            AddBackground(0, 0, 650, 200, 0x13BE); // Background ID may vary

            // Add buttons
            int startX = 50;
            int startY = 50;
            int xOffset = 110;
            int yOffset = 30;
            string[] buttonNames = { "Horse", "Polar Bear", "Unicorn", "Ostard", "CuSidHe", "Beetle", "SerpentineDragon", "Tarantula", "Tiger", "HellHound", "Boura", "Charger" };

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    int buttonIndex = (row * 5) + col;

                    if (buttonIndex >= buttonNames.Length)
                        break;

                    AddButton(startX + (col * xOffset), startY + (row * yOffset), 0xFA5, 0xFA7, buttonIndex + 1, GumpButtonType.Reply, 0);
                    AddLabel(startX + (col * xOffset) + 35, startY + (row * yOffset), 0x481, buttonNames[buttonIndex]);
                }
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            // Handle button clicks
            int buttonID = info.ButtonID;
            string[] buttonNames = { "Horse", "Polar Bear", "Unicorn", "Ostard", "CuSidHe", "Beetle", "SerpentineDragon", "Tarantula", "Tiger", "HellHound", "Boura", "Charger" };

            if (buttonID >= 1 && buttonID <= buttonNames.Length)
            {
                string selectedButtonName = buttonNames[buttonID - 1];

                // Adjust mount's properties based on selected button
                switch (selectedButtonName)
                {
                    case "Horse":
                        m_Mount.TransparentMountedID = 0x3EAA; // Change to desired Mounted ID for Horse
                        m_Mount.NonTransparentMountedID = 0x3EA0; // Change to desired Non-Transparent Mounted ID for Horse
                        m_Mount.StatueID = 8413; // Change to desired Statue ID for Horse
                        break;
                    case "Polar Bear":
                        m_Mount.TransparentMountedID = 0x3EC5;
                        m_Mount.NonTransparentMountedID = 0x3EC5;
                        m_Mount.StatueID = 0x20E1;
                        break;
                    case "Unicorn":
                        m_Mount.TransparentMountedID = 0x3E9B;
                        m_Mount.NonTransparentMountedID = 0x3EB4;
                        m_Mount.StatueID = 0x25CE;
                        break;
                    case "Ostard":
                        m_Mount.TransparentMountedID = 0x3EAC;
                        m_Mount.NonTransparentMountedID = 0x3EA5;
                        m_Mount.StatueID = 0x2135;
                        break;
                    case "CuSidHe":
                        m_Mount.TransparentMountedID = 0x3E91;
                        m_Mount.NonTransparentMountedID = 0x3E91;
                        m_Mount.StatueID = 0x2D96;
                        break;
                    case "Beetle":
                        m_Mount.TransparentMountedID = 0x3E97;
                        m_Mount.NonTransparentMountedID = 0x3EBC;
                        m_Mount.StatueID = 0x260F;
                        break;
                    case "SerpentineDragon":
                        m_Mount.TransparentMountedID = 0x3ECE;
                        m_Mount.NonTransparentMountedID = 0x3ECE;
                        m_Mount.StatueID = 0xA010;
                        break;
                    case "Tarantula":
                        m_Mount.TransparentMountedID = 0x3ECA;
                        m_Mount.NonTransparentMountedID = 0x3ECA;
                        m_Mount.StatueID = 0x9DD6;
                        break;
                    case "Tiger":
                        m_Mount.TransparentMountedID = 0x3EC7;
                        m_Mount.NonTransparentMountedID = 0x3EC8;
                        m_Mount.StatueID = 0x9844;
                        break;
                    case "HellHound":
                        m_Mount.TransparentMountedID = 0x3EC9;
                        m_Mount.NonTransparentMountedID = 0x3EC9;
                        m_Mount.StatueID = 0x3FFD;
                        break;
                    case "Boura":
                        m_Mount.TransparentMountedID = 0x3EC6;
                        m_Mount.NonTransparentMountedID = 0x3EC6;
                        m_Mount.StatueID = 0x46F8;
                        break;
                    case "Charger":
                        m_Mount.TransparentMountedID = 0x3E92;
                        m_Mount.NonTransparentMountedID = 0x3E92;
                        m_Mount.StatueID = 0x2D9C;
                        break;
                }

                from.SendMessage($"You changed the mount's appearance to {selectedButtonName}.");
            }
        }
    }
}