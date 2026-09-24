using System;
using System.Collections.Generic;
using System.IO;
using Server;
using Server.Commands;

namespace Server.Mobiles
{
    public static class WolvesbaneGreeterConfig
    {
        public class GreeterPage
        {
            public string Title { get; set; }
            public List<string> Lines { get; set; }

            public GreeterPage(string title)
            {
                Title = title;
                Lines = new List<string>();
            }
        }

        public static readonly List<string> SpeechTips = new List<string>();
        public static readonly List<GreeterPage> Pages = new List<GreeterPage>();
        public static string AnnouncementText = "Double-click me for more information about Wolvesbane!";

        public static string ConfigPath
        {
            get { return Path.Combine(Core.BaseDirectory, "Data", "WolvesbaneGreeter.cfg"); }
        }

        public static void Initialize()
        {
            Load();
            CommandSystem.Register("ReloadGreeter", AccessLevel.GameMaster, new CommandEventHandler(ReloadGreeter_OnCommand));
        }

        private static void ReloadGreeter_OnCommand(CommandEventArgs e)
        {
            Load();
            e.Mobile.SendMessage(68, "Wolvesbane Greeter configuration reloaded. {0} speech tips and {1} help pages loaded.", SpeechTips.Count, Pages.Count);
        }

        public static void Load()
        {
            SpeechTips.Clear();
            Pages.Clear();
            AnnouncementText = "Double-click me for more information about Wolvesbane!";

            try
            {
                if (!File.Exists(ConfigPath))
                {
                    CreateDefaultConfig();
                }

                string[] lines = File.ReadAllLines(ConfigPath);
                GreeterPage currentPage = null;
                string section = String.Empty;

                for (int i = 0; i < lines.Length; i++)
                {
                    string raw = lines[i];
                    string line = raw.Trim();

                    if (line.Length == 0 || line.StartsWith("#") || line.StartsWith("//"))
                        continue;

                    if (line.Equals("[Announcement]", StringComparison.OrdinalIgnoreCase))
                    {
                        section = "announcement";
                        currentPage = null;
                        continue;
                    }

                    if (line.Equals("[SpeechTips]", StringComparison.OrdinalIgnoreCase))
                    {
                        section = "tips";
                        currentPage = null;
                        continue;
                    }

                    if (line.StartsWith("[Page:", StringComparison.OrdinalIgnoreCase) && line.EndsWith("]"))
                    {
                        string title = line.Substring(6, line.Length - 7).Trim();

                        if (title.Length == 0)
                            title = "Wolvesbane Help";

                        currentPage = new GreeterPage(title);
                        Pages.Add(currentPage);
                        section = "page";
                        continue;
                    }

                    if (section == "announcement")
                    {
                        AnnouncementText = line;
                        section = String.Empty; // only the first line after [Announcement] is used
                    }
                    else if (section == "tips")
                    {
                        SpeechTips.Add(line);
                    }
                    else if (section == "page" && currentPage != null)
                    {
                        currentPage.Lines.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[WolvesbaneGreeter] Error loading configuration: {0}", ex);
            }

            EnsureFallbacks();
        }

        private static void EnsureFallbacks()
        {
            if (SpeechTips.Count == 0)
            {
                SpeechTips.Add("Welcome to Wolvesbane! Double-click me if you need help getting started.");
                SpeechTips.Add("New here? I can show you commands, travel tips, custom systems, Discord information, and more.");
                SpeechTips.Add("Remember to use [vote to support Wolvesbane and earn vote rewards!");
            }

            if (Pages.Count == 0)
            {
                GreeterPage page = new GreeterPage("Welcome to Wolvesbane");
                page.Lines.Add("Welcome adventurer! Wolvesbane is a heavily customized Ultima Online shard with years of custom content.");
                page.Lines.Add("Use the buttons on the left to explore newcomer information and useful commands.");
                Pages.Add(page);
            }
        }

        private static void CreateDefaultConfig()
        {
            try
            {
                string directory = Path.GetDirectoryName(ConfigPath);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                File.WriteAllText(ConfigPath, DefaultConfigText);
                Console.WriteLine("[WolvesbaneGreeter] Created default config: {0}", ConfigPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[WolvesbaneGreeter] Could not create default configuration: {0}", ex);
            }
        }

        private const string DefaultConfigText = @"# Wolvesbane Greeter configuration
# Edit this file while the server is running, then use [ReloadGreeter in game.
#
# [Announcement] contains the recurring prompt telling players to double-click the Greeter.
# [SpeechTips] contains phrases the Greeter periodically says aloud.
# [Page:PAGE TITLE] creates a new button/page in the double-click help gump.
# Every non-comment line below that header becomes text on that page.

[Announcement]
Double-click me for more information about Wolvesbane!

[SpeechTips]
Welcome to Wolvesbane! Double-click me if you need help getting started.
New to Wolvesbane? Ask me about commands, travel, custom systems, voting, Discord, housing, and pets!
Use [vote to support Wolvesbane and earn vote rewards from supported voting sites.
Wolvesbane has many custom systems. Double-click me for a quick tour.
Need other players or staff? Join the Wolvesbane Discord community.
Don't be afraid to explore - Wolvesbane rewards discovery.

[Page:Welcome]
Welcome to Wolvesbane UO!
This guide is meant to help new and returning players quickly find the systems and information they need.
Wolvesbane contains a large amount of custom content, so take your time exploring.

[Page:Getting Started]
Begin by setting up your character, checking your backpack, and exploring the starting area.
Wolvesbane has an unlimited skill cap and a 1000 stat cap, so your character is not locked into a traditional seven-skill template.
Talk to NPCs, explore custom areas, and use this guide whenever you need a reminder.

[Page:Important Commands]
[vote - Opens the Wolvesbane voting system.
[myhouses - Displays information about your houses.
Additional commands can be added to this page at any time by editing Data/WolvesbaneGreeter.cfg.

[Page:Travel]
Public gates are available for many important destinations.
Wolvesbane also supports traditional Ultima Online travel such as recall and runebooks where permitted.
Explore carefully - some custom destinations contain much stronger creatures than standard areas.

[Page:Custom Systems]
Wolvesbane contains many custom systems, bosses, items, skills, treasure content, invasions, storage systems, and progression mechanics.
If something looks unfamiliar, it may be unique to Wolvesbane rather than standard Ultima Online.
Watch world messages and Discord announcements for events and new content.

[Page:Skills]
Wolvesbane allows unlimited total skills and has a 1000 stat cap.
Custom content may make use of skills in ways that differ from standard Ultima Online.
Gardening is a separate custom skill/system from Lumberjacking.

[Page:Voting]
Use [vote to open the Wolvesbane voting gump.
Several voting sites can reward vote tokens automatically when their callback system is supported.
Voting helps new players discover Wolvesbane and directly supports the shard.

[Page:Discord]
Wolvesbane Discord: https://discord.gg/sH6wgbSywK
Discord is a good place to ask questions, see announcements, discuss updates, and talk with other players.
Wolvesbane also supports communication between Discord and in-game world chat.

[Page:Housing]
Housing is available, but houses belonging to accounts inactive for an extended period may eventually enter the abandoned-property process.
Use [myhouses to review your houses.
If you plan to be away for a long period, check current Wolvesbane housing rules.

[Page:Pets & Taming]
Wolvesbane has extensive custom pet and creature content.
Some pets use custom progression or leveling systems beyond standard Ultima Online taming.
Experiment with different creatures and ask veteran tamers about unusual pet abilities.

[Page:Treasure Maps]
Wolvesbane treasure maps can lead to several facets, including custom locations.
Higher-level treasure maps can contain custom creatures and rare custom rewards.
Be prepared before attempting the highest treasure-map levels.

[Page:Events]
Watch for invasions, staff events, boss encounters, and other shard-wide activities.
Announcements may appear in game and on Discord.
Some events are intended for groups, so don't hesitate to team up with other players.
";
    }
}
