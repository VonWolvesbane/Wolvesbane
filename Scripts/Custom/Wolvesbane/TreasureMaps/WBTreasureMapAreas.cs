using System;
using System.Collections.Generic;
using System.IO;

using Server;
using Server.Commands;

namespace Wolvesbane.TreasureMaps
{
    /// <summary>
    /// Persistent whitelist of player-accessible treasure hunting zones on
    /// Wolvesbane's custom NewWolvesbane facet.
    ///
    /// We intentionally do not allow TreasureMap.cs to sample the whole custom
    /// facet. A walkable land tile is not proof that players can actually reach
    /// that location.
    /// </summary>
    public static class WBTreasureMapAreas
    {
        private static readonly List<Rectangle2D> m_Areas = new List<Rectangle2D>();

        private static readonly string m_Directory =
            Path.Combine(Core.BaseDirectory, "Data", "Wolvesbane", "TreasureMaps");

        private static readonly string m_File =
            Path.Combine(m_Directory, "NewWolvesbaneAreas.cfg");

        public static void Initialize()
        {
            Load();

            CommandSystem.Register("WBTMapAreaAdd", AccessLevel.Administrator, OnAreaAdd);
            CommandSystem.Register("WBTMapAreaAddRect", AccessLevel.Administrator, OnAreaAddRect);
            CommandSystem.Register("WBTMapAreaList", AccessLevel.GameMaster, OnAreaList);
            CommandSystem.Register("WBTMapAreaRemove", AccessLevel.Administrator, OnAreaRemove);
            CommandSystem.Register("WBTMapAreaClear", AccessLevel.Administrator, OnAreaClear);
        }

        public static Rectangle2D[] GetAreas()
        {
            return m_Areas.ToArray();
        }

        private static void OnAreaAdd(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from.Map != Map.NewWolvesbane)
            {
                from.SendMessage(33, "You must be standing on NewWolvesbane.");
                return;
            }

            int radius = 250;

            if (e.Arguments.Length > 0 &&
                (!Int32.TryParse(e.Arguments[0], out radius) || radius < 25 || radius > 1500))
            {
                from.SendMessage("Usage: [WBTMapAreaAdd <radius>");
                from.SendMessage("Radius must be between 25 and 1500 tiles.");
                return;
            }

            Rectangle2D rect = BuildCenteredArea(from.X, from.Y, radius);

            if (rect.Width <= 0 || rect.Height <= 0)
            {
                from.SendMessage(33, "Could not build a valid area at your location.");
                return;
            }

            m_Areas.Add(rect);
            Save();

            from.SendMessage(
                68,
                "Added NewWolvesbane treasure area #{0}: X={1}, Y={2}, W={3}, H={4}.",
                m_Areas.Count - 1,
                rect.X,
                rect.Y,
                rect.Width,
                rect.Height);
        }

        private static void OnAreaAddRect(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (e.Arguments.Length != 4)
            {
                from.SendMessage("Usage: [WBTMapAreaAddRect <x1> <y1> <x2> <y2>");
                return;
            }

            int x1, y1, x2, y2;

            if (!Int32.TryParse(e.Arguments[0], out x1) ||
                !Int32.TryParse(e.Arguments[1], out y1) ||
                !Int32.TryParse(e.Arguments[2], out x2) ||
                !Int32.TryParse(e.Arguments[3], out y2))
            {
                from.SendMessage(33, "All four coordinates must be numbers.");
                return;
            }

            int left = Math.Max(0, Math.Min(x1, x2));
            int top = Math.Max(0, Math.Min(y1, y2));
            int right = Math.Min(Map.NewWolvesbane.Width - 1, Math.Max(x1, x2));
            int bottom = Math.Min(Map.NewWolvesbane.Height - 1, Math.Max(y1, y2));

            Rectangle2D rect = new Rectangle2D(
                left,
                top,
                Math.Max(1, right - left + 1),
                Math.Max(1, bottom - top + 1));

            m_Areas.Add(rect);
            Save();

            from.SendMessage(
                68,
                "Added NewWolvesbane treasure area #{0}: X={1}, Y={2}, W={3}, H={4}.",
                m_Areas.Count - 1,
                rect.X,
                rect.Y,
                rect.Width,
                rect.Height);
        }

        private static void OnAreaList(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            from.SendMessage(88, "NewWolvesbane approved treasure areas: {0}", m_Areas.Count);

            if (m_Areas.Count == 0)
            {
                from.SendMessage(33, "None configured. Use [WBTMapAreaAdd <radius>.");
                return;
            }

            for (int i = 0; i < m_Areas.Count; ++i)
            {
                Rectangle2D rect = m_Areas[i];

                from.SendMessage(
                    "#{0}: X={1}, Y={2}, W={3}, H={4}",
                    i,
                    rect.X,
                    rect.Y,
                    rect.Width,
                    rect.Height);
            }
        }

        private static void OnAreaRemove(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            int index;

            if (e.Arguments.Length != 1 ||
                !Int32.TryParse(e.Arguments[0], out index) ||
                index < 0 ||
                index >= m_Areas.Count)
            {
                from.SendMessage("Usage: [WBTMapAreaRemove <index>");
                return;
            }

            Rectangle2D rect = m_Areas[index];
            m_Areas.RemoveAt(index);
            Save();

            from.SendMessage(
                68,
                "Removed treasure area #{0} (X={1}, Y={2}, W={3}, H={4}).",
                index,
                rect.X,
                rect.Y,
                rect.Width,
                rect.Height);
        }

        private static void OnAreaClear(CommandEventArgs e)
        {
            m_Areas.Clear();
            Save();

            e.Mobile.SendMessage(68, "All NewWolvesbane treasure areas were cleared.");
        }

        private static Rectangle2D BuildCenteredArea(int x, int y, int radius)
        {
            int left = Math.Max(0, x - radius);
            int top = Math.Max(0, y - radius);
            int right = Math.Min(Map.NewWolvesbane.Width - 1, x + radius);
            int bottom = Math.Min(Map.NewWolvesbane.Height - 1, y + radius);

            return new Rectangle2D(
                left,
                top,
                Math.Max(1, right - left + 1),
                Math.Max(1, bottom - top + 1));
        }

        private static void Load()
        {
            m_Areas.Clear();

            try
            {
                if (!File.Exists(m_File))
                {
                    Console.WriteLine(
                        "WB Treasure Maps: no NewWolvesbane approved-area file exists yet.");

                    return;
                }

                string[] lines = File.ReadAllLines(m_File);

                for (int i = 0; i < lines.Length; ++i)
                {
                    string line = lines[i].Trim();

                    if (line.Length == 0 || line.StartsWith("#"))
                        continue;

                    string[] parts = line.Split(',');

                    if (parts.Length != 4)
                        continue;

                    int x, y, width, height;

                    if (!Int32.TryParse(parts[0], out x) ||
                        !Int32.TryParse(parts[1], out y) ||
                        !Int32.TryParse(parts[2], out width) ||
                        !Int32.TryParse(parts[3], out height))
                        continue;

                    if (width <= 0 || height <= 0)
                        continue;

                    m_Areas.Add(new Rectangle2D(x, y, width, height));
                }

                Console.WriteLine(
                    "WB Treasure Maps: loaded {0} NewWolvesbane approved treasure area(s).",
                    m_Areas.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "WB Treasure Maps ERROR loading NewWolvesbane areas: {0}",
                    ex);
            }
        }

        private static void Save()
        {
            try
            {
                Directory.CreateDirectory(m_Directory);

                using (StreamWriter writer = new StreamWriter(m_File, false))
                {
                    writer.WriteLine("# Wolvesbane NewWolvesbane treasure hunting areas");
                    writer.WriteLine("# x,y,width,height");

                    for (int i = 0; i < m_Areas.Count; ++i)
                    {
                        Rectangle2D rect = m_Areas[i];

                        writer.WriteLine(
                            "{0},{1},{2},{3}",
                            rect.X,
                            rect.Y,
                            rect.Width,
                            rect.Height);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "WB Treasure Maps ERROR saving NewWolvesbane areas: {0}",
                    ex);
            }
        }
    }
}
