using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Network;

namespace Server.Custom.Wolvesbane
{
    public static class WolvesbaneMOTD
    {
        private const string DefaultTitle = "Wolvesbane News & Updates";
        private const string WebsiteUrl = "https://wolvesbaneuo.com";
        private const string DiscordUrl = "https://discord.gg/sH6wgbSywK";

        private static readonly string SaveDirectory = Path.Combine(Core.BaseDirectory, "Saves", "Wolvesbane");
        private static readonly string SaveFile = Path.Combine(SaveDirectory, "MOTD.xml");

        private static string _Title = DefaultTitle;
        private static string[] _Lines = new string[10];
        private static int _Revision = 0;
        private static DateTime _Published = DateTime.MinValue;
        private static readonly Dictionary<int, int> _Seen = new Dictionary<int, int>();

        public static void Initialize()
        {
            CommandSystem.Register("MOTD", AccessLevel.GameMaster, OnMOTDCommand);
            CommandSystem.Register("News", AccessLevel.Player, OnNewsCommand);

            EventSink.Login += OnLogin;
            EventSink.WorldSave += OnWorldSave;
            EventSink.WorldLoad += OnWorldLoad;
        }

        private static void OnMOTDCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from == null)
                return;

            from.CloseGump(typeof(MOTDEditGump));
            from.SendGump(new MOTDEditGump());
        }

        private static void OnNewsCommand(CommandEventArgs e)
        {
            if (e.Mobile != null)
                ShowNews(e.Mobile);
        }

        private static void OnLogin(LoginEventArgs e)
        {
            Mobile m = e.Mobile;
            if (m == null || _Revision <= 0)
                return;

            int seen;
            if (!_Seen.TryGetValue(m.Serial.Value, out seen) || seen < _Revision)
            {
                Timer.DelayCall(TimeSpan.FromSeconds(2.0), delegate
                {
                    if (m != null && !m.Deleted && m.NetState != null)
                        ShowNews(m);
                });
            }
        }

        private static void ShowNews(Mobile from)
        {
            if (from == null)
                return;

            if (_Revision <= 0)
            {
                from.SendMessage(68, "There is no Wolvesbane news posted right now.");
                return;
            }

            _Seen[from.Serial.Value] = _Revision;
            from.CloseGump(typeof(MOTDViewGump));
            from.SendGump(new MOTDViewGump());
        }

        private static void Publish(string title, string[] lines)
        {
            _Title = String.IsNullOrWhiteSpace(title) ? DefaultTitle : title.Trim();
            _Lines = new string[10];

            for (int i = 0; i < _Lines.Length; i++)
                _Lines[i] = (lines != null && i < lines.Length && lines[i] != null) ? lines[i].Trim() : String.Empty;

            _Revision++;
            _Published = DateTime.UtcNow;
            Save();
        }

        private static void ForceShowAgain()
        {
            _Revision++;
            _Published = DateTime.UtcNow;
            Save();
        }

        private static void OnWorldSave(WorldSaveEventArgs e)
        {
            Save();
        }

        private static void OnWorldLoad()
        {
            Load();
        }

        private static void Save()
        {
            try
            {
                if (!Directory.Exists(SaveDirectory))
                    Directory.CreateDirectory(SaveDirectory);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(SaveFile, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("WolvesbaneMOTD");
                    writer.WriteAttributeString("version", "1");
                    writer.WriteElementString("Revision", _Revision.ToString());
                    writer.WriteElementString("Published", _Published.ToString("o"));
                    writer.WriteElementString("Title", _Title ?? DefaultTitle);

                    writer.WriteStartElement("Lines");
                    for (int i = 0; i < _Lines.Length; i++)
                    {
                        writer.WriteStartElement("Line");
                        writer.WriteAttributeString("index", i.ToString());
                        writer.WriteString(_Lines[i] ?? String.Empty);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.WriteStartElement("Seen");
                    foreach (KeyValuePair<int, int> kvp in _Seen)
                    {
                        writer.WriteStartElement("Player");
                        writer.WriteAttributeString("serial", kvp.Key.ToString());
                        writer.WriteAttributeString("revision", kvp.Value.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Wolvesbane MOTD] Save failed: {0}", ex);
            }
        }

        private static void Load()
        {
            if (!File.Exists(SaveFile))
                return;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(SaveFile);
                XmlElement root = doc["WolvesbaneMOTD"];
                if (root == null)
                    return;

                int revision;
                if (Int32.TryParse(GetNodeText(root, "Revision"), out revision))
                    _Revision = revision;

                DateTime published;
                if (DateTime.TryParse(GetNodeText(root, "Published"), out published))
                    _Published = published.ToUniversalTime();

                string title = GetNodeText(root, "Title");
                _Title = String.IsNullOrWhiteSpace(title) ? DefaultTitle : title;

                _Lines = new string[10];
                XmlElement linesNode = root["Lines"];
                if (linesNode != null)
                {
                    foreach (XmlNode node in linesNode.ChildNodes)
                    {
                        XmlElement line = node as XmlElement;
                        int index;
                        if (line != null && line.Name == "Line" && Int32.TryParse(line.GetAttribute("index"), out index) && index >= 0 && index < _Lines.Length)
                            _Lines[index] = line.InnerText ?? String.Empty;
                    }
                }

                _Seen.Clear();
                XmlElement seenNode = root["Seen"];
                if (seenNode != null)
                {
                    foreach (XmlNode node in seenNode.ChildNodes)
                    {
                        XmlElement player = node as XmlElement;
                        int serial, seenRevision;
                        if (player != null && player.Name == "Player" &&
                            Int32.TryParse(player.GetAttribute("serial"), out serial) &&
                            Int32.TryParse(player.GetAttribute("revision"), out seenRevision))
                        {
                            _Seen[serial] = seenRevision;
                        }
                    }
                }

                Console.WriteLine("[Wolvesbane MOTD] Loaded revision {0}.", _Revision);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Wolvesbane MOTD] Load failed: {0}", ex);
            }
        }

        private static string GetNodeText(XmlElement root, string name)
        {
            XmlElement node = root[name];
            return node == null ? String.Empty : node.InnerText;
        }

        private static string Escape(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        private static void OpenUrl(Mobile from, string url)
        {
            if (from != null && from.NetState != null)
                from.NetState.Send(new LaunchBrowser(url));
        }

        private class MOTDViewGump : Gump
        {
            public MOTDViewGump() : base(120, 80)
            {
                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddPage(0);
                AddBackground(0, 0, 560, 440, 9270);
                AddAlphaRegion(12, 12, 536, 416);

                AddHtml(25, 22, 510, 30,
                    "<CENTER><BASEFONT COLOR=#FFD57A><BIG>" + Escape(_Title) + "</BIG></BASEFONT></CENTER>", false, false);

                string publishedText = _Published == DateTime.MinValue ? String.Empty : _Published.ToLocalTime().ToString("MMMM d, yyyy");
                AddHtml(25, 55, 510, 22,
                    "<CENTER><BASEFONT COLOR=#B0B0B0>" + Escape(publishedText) + "</BASEFONT></CENTER>", false, false);

                System.Text.StringBuilder body = new System.Text.StringBuilder();
                body.Append("<BASEFONT COLOR=#FFFFFF>");
                bool any = false;

                for (int i = 0; i < _Lines.Length; i++)
                {
                    if (String.IsNullOrWhiteSpace(_Lines[i]))
                        continue;
                    if (any)
                        body.Append("<BR>");
                    body.Append(Escape(_Lines[i]));
                    any = true;
                }

                if (!any)
                    body.Append("No additional news has been posted.");

                body.Append("</BASEFONT>");
                AddHtml(35, 90, 490, 245, body.ToString(), true, true);

                AddButton(65, 365, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddLabel(100, 367, 1152, "Wolvesbane Website");
                AddButton(300, 365, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddLabel(335, 367, 1152, "Wolvesbane Discord");
                AddButton(225, 402, 4017, 4019, 0, GumpButtonType.Reply, 0);
                AddLabel(258, 404, 1152, "Close");
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;
                if (from == null)
                    return;

                if (info.ButtonID == 1)
                    OpenUrl(from, WebsiteUrl);
                else if (info.ButtonID == 2)
                    OpenUrl(from, DiscordUrl);
            }
        }

        private class MOTDEditGump : Gump
        {
            private const int TitleEntry = 100;
            private const int FirstLineEntry = 200;

            public MOTDEditGump() : base(80, 40)
            {
                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddPage(0);
                AddBackground(0, 0, 680, 610, 9270);
                AddAlphaRegion(12, 12, 656, 586);
                AddHtml(20, 18, 640, 30,
                    "<CENTER><BASEFONT COLOR=#FFD57A><BIG>Wolvesbane MOTD Editor</BIG></BASEFONT></CENTER>", false, false);

                AddLabel(30, 58, 1152, "Title:");
                AddBackground(95, 54, 550, 28, 9350);
                AddTextEntry(103, 59, 530, 20, 0x480, TitleEntry, _Title ?? DefaultTitle);

                AddHtml(30, 90, 610, 35,
                    "<BASEFONT COLOR=#B0B0B0>Enter up to 10 lines. Save & Publish creates a new revision so players see it on login.</BASEFONT>", false, false);

                int y = 135;
                for (int i = 0; i < 10; i++)
                {
                    AddLabel(30, y + 4, 1152, (i + 1).ToString() + ":");
                    AddBackground(58, y, 587, 28, 9350);
                    AddTextEntry(66, y + 5, 567, 20, 0x480, FirstLineEntry + i,
                        (_Lines != null && i < _Lines.Length) ? (_Lines[i] ?? String.Empty) : String.Empty);
                    y += 36;
                }

                AddButton(35, 520, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddLabel(70, 522, 68, "Save & Publish");
                AddButton(245, 520, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddLabel(280, 522, 1152, "Preview");
                AddButton(390, 520, 4005, 4007, 3, GumpButtonType.Reply, 0);
                AddLabel(425, 522, 33, "Force Show Again");
                AddButton(550, 560, 4017, 4019, 0, GumpButtonType.Reply, 0);
                AddLabel(583, 562, 1152, "Close");
                AddHtml(30, 565, 485, 22,
                    "<BASEFONT COLOR=#B0B0B0>Current revision: " + _Revision + "</BASEFONT>", false, false);
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;
                if (from == null || from.AccessLevel < AccessLevel.GameMaster || info.ButtonID == 0)
                    return;

                string title = GetEntry(info, TitleEntry);
                string[] lines = new string[10];
                for (int i = 0; i < lines.Length; i++)
                    lines[i] = GetEntry(info, FirstLineEntry + i);

                if (info.ButtonID == 1)
                {
                    Publish(title, lines);
                    from.SendMessage(68, "Wolvesbane MOTD revision {0} has been published.", _Revision);
                    ShowNews(from);
                }
                else if (info.ButtonID == 2)
                {
                    from.SendGump(new MOTDPreviewGump(title, lines));
                    from.SendGump(new MOTDEditValuesGump(title, lines));
                }
                else if (info.ButtonID == 3)
                {
                    ForceShowAgain();
                    from.SendMessage(68, "The current Wolvesbane MOTD will be shown to players again.");
                    from.SendGump(new MOTDEditGump());
                }
            }

            protected static string GetEntry(RelayInfo info, int id)
            {
                TextRelay relay = info.GetTextEntry(id);
                return relay == null ? String.Empty : (relay.Text ?? String.Empty);
            }
        }

        private class MOTDEditValuesGump : Gump
        {
            private const int TitleEntry = 100;
            private const int FirstLineEntry = 200;

            public MOTDEditValuesGump(string title, string[] lines) : base(80, 40)
            {
                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddPage(0);
                AddBackground(0, 0, 680, 610, 9270);
                AddAlphaRegion(12, 12, 656, 586);
                AddHtml(20, 18, 640, 30,
                    "<CENTER><BASEFONT COLOR=#FFD57A><BIG>Wolvesbane MOTD Editor</BIG></BASEFONT></CENTER>", false, false);

                AddLabel(30, 58, 1152, "Title:");
                AddBackground(95, 54, 550, 28, 9350);
                AddTextEntry(103, 59, 530, 20, 0x480, TitleEntry, title ?? DefaultTitle);
                AddHtml(30, 90, 610, 35,
                    "<BASEFONT COLOR=#B0B0B0>Preview is open. Continue editing here, then Save & Publish when ready.</BASEFONT>", false, false);

                int y = 135;
                for (int i = 0; i < 10; i++)
                {
                    AddLabel(30, y + 4, 1152, (i + 1).ToString() + ":");
                    AddBackground(58, y, 587, 28, 9350);
                    AddTextEntry(66, y + 5, 567, 20, 0x480, FirstLineEntry + i,
                        (lines != null && i < lines.Length) ? (lines[i] ?? String.Empty) : String.Empty);
                    y += 36;
                }

                AddButton(35, 520, 4005, 4007, 1, GumpButtonType.Reply, 0);
                AddLabel(70, 522, 68, "Save & Publish");
                AddButton(245, 520, 4005, 4007, 2, GumpButtonType.Reply, 0);
                AddLabel(280, 522, 1152, "Preview");
                AddButton(390, 520, 4005, 4007, 3, GumpButtonType.Reply, 0);
                AddLabel(425, 522, 33, "Force Show Again");
                AddButton(550, 560, 4017, 4019, 0, GumpButtonType.Reply, 0);
                AddLabel(583, 562, 1152, "Close");
            }

            public override void OnResponse(NetState sender, RelayInfo info)
            {
                Mobile from = sender.Mobile;
                if (from == null || from.AccessLevel < AccessLevel.GameMaster || info.ButtonID == 0)
                    return;

                string title = GetEntry(info, TitleEntry);
                string[] lines = new string[10];
                for (int i = 0; i < lines.Length; i++)
                    lines[i] = GetEntry(info, FirstLineEntry + i);

                if (info.ButtonID == 1)
                {
                    Publish(title, lines);
                    from.SendMessage(68, "Wolvesbane MOTD revision {0} has been published.", _Revision);
                    ShowNews(from);
                }
                else if (info.ButtonID == 2)
                {
                    from.SendGump(new MOTDPreviewGump(title, lines));
                    from.SendGump(new MOTDEditValuesGump(title, lines));
                }
                else if (info.ButtonID == 3)
                {
                    ForceShowAgain();
                    from.SendMessage(68, "The current Wolvesbane MOTD will be shown to players again.");
                    from.SendGump(new MOTDEditGump());
                }
            }

            private static string GetEntry(RelayInfo info, int id)
            {
                TextRelay relay = info.GetTextEntry(id);
                return relay == null ? String.Empty : (relay.Text ?? String.Empty);
            }
        }

        private class MOTDPreviewGump : Gump
        {
            public MOTDPreviewGump(string title, string[] lines) : base(780, 80)
            {
                Closable = true;
                Disposable = true;
                Dragable = true;
                Resizable = false;

                AddPage(0);
                AddBackground(0, 0, 560, 405, 9270);
                AddAlphaRegion(12, 12, 536, 381);
                AddHtml(25, 22, 510, 30,
                    "<CENTER><BASEFONT COLOR=#FFD57A><BIG>" + Escape(String.IsNullOrWhiteSpace(title) ? DefaultTitle : title) + "</BIG></BASEFONT></CENTER>", false, false);
                AddHtml(25, 55, 510, 22,
                    "<CENTER><BASEFONT COLOR=#B0B0B0>PREVIEW - NOT PUBLISHED</BASEFONT></CENTER>", false, false);

                System.Text.StringBuilder body = new System.Text.StringBuilder();
                body.Append("<BASEFONT COLOR=#FFFFFF>");
                bool any = false;

                for (int i = 0; i < 10; i++)
                {
                    string line = (lines != null && i < lines.Length) ? lines[i] : String.Empty;
                    if (String.IsNullOrWhiteSpace(line))
                        continue;
                    if (any)
                        body.Append("<BR>");
                    body.Append(Escape(line));
                    any = true;
                }

                if (!any)
                    body.Append("No additional news has been posted.");

                body.Append("</BASEFONT>");
                AddHtml(35, 90, 490, 245, body.ToString(), true, true);
                AddButton(225, 355, 4017, 4019, 0, GumpButtonType.Reply, 0);
                AddLabel(258, 357, 1152, "Close Preview");
            }
        }
    }
}
