using System;
using System.IO;
using System.Threading;
using Server;
using Server.Commands;

namespace Server.Commands
{
    public class WBTwiceDailyBackup
    {
        private static readonly object m_Sync = new object();

        private static Timer m_Timer;
        private static DateTime m_NextRun;
        private static DateTime m_LastBackup = DateTime.MinValue;
        private static string m_LastBackupPath = "(none)";
        private static string m_LastError = "(none)";
        private static bool m_BackupRunning;

        private const string BackupRoot = "Backups/Wolvesbane";

        public static void Initialize()
        {
            CommandSystem.Register("WBBackup", AccessLevel.Administrator, new CommandEventHandler(OnCommand));

            ScheduleNext();
        }

        private static void OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            string action = (e.Arguments != null && e.Arguments.Length > 0)
                ? e.Arguments[0].ToLower()
                : "status";

            if (action == "status")
            {
                from.SendMessage(88, "Wolvesbane Twice-Daily Backup");
                from.SendMessage("Next scheduled backup: {0}", m_NextRun == DateTime.MinValue ? "(not scheduled)" : m_NextRun.ToString());
                from.SendMessage("Backup currently running: {0}", m_BackupRunning ? "yes" : "no");
                from.SendMessage("Last successful backup: {0}", m_LastBackup == DateTime.MinValue ? "(none)" : m_LastBackup.ToString());
                from.SendMessage("Last backup path: {0}", m_LastBackupPath);
                from.SendMessage("Last error: {0}", m_LastError);
                return;
            }

            if (action == "now")
            {
                from.SendMessage(68, "Starting a manual Wolvesbane save + backup test.");
                BeginScheduledBackup();
                return;
            }

            from.SendMessage("Usage: [WBBackup status");
            from.SendMessage("       [WBBackup now");
        }

        private static void ScheduleNext()
        {
            DateTime now = DateTime.Now;

            DateTime noon = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);
            DateTime midnightTomorrow = now.Date.AddDays(1);

            DateTime next;

            if (now < noon)
                next = noon;
            else
                next = midnightTomorrow;

            TimeSpan delay = next - now;

            if (delay < TimeSpan.FromSeconds(1.0))
                delay = TimeSpan.FromSeconds(1.0);

            m_NextRun = next;

            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer = null;
            }

            m_Timer = Timer.DelayCall(delay, new TimerCallback(OnScheduledTime));

            Console.WriteLine(
                "WB Backup: next scheduled backup is {0} (server local time).",
                m_NextRun);
        }

        private static void OnScheduledTime()
        {
            // Schedule the next noon/midnight first so an exception cannot disable future backups.
            ScheduleNext();

            BeginScheduledBackup();
        }

        private static void BeginScheduledBackup()
        {
            lock (m_Sync)
            {
                if (m_BackupRunning)
                {
                    Console.WriteLine("WB Backup: skipped because a backup is already running.");
                    return;
                }

                if (World.Saving)
                {
                    Console.WriteLine("WB Backup: world is already saving; retrying backup in one minute.");
                    Timer.DelayCall(TimeSpan.FromMinutes(1.0), new TimerCallback(BeginScheduledBackup));
                    return;
                }

                m_BackupRunning = true;
                m_LastError = "(none)";
            }

            try
            {
                Console.WriteLine("WB Backup: forcing a world save before backup.");

                // Normal foreground save. This preserves the normal player-facing save message.
                World.Save(true, false);

                ThreadPool.QueueUserWorkItem(new WaitCallback(CopySavedWorldInBackground));
            }
            catch (Exception ex)
            {
                lock (m_Sync)
                {
                    m_BackupRunning = false;
                    m_LastError = ex.GetType().Name + ": " + ex.Message;
                }

                Console.WriteLine("WB Backup ERROR before copy: {0}", ex);
            }
        }

        private static void CopySavedWorldInBackground(object state)
        {
            try
            {
                // Defensive wait: make sure every save strategy has finished any disk work.
                World.WaitForWriteCompletion();

                string source = Path.GetFullPath("Saves");

                if (!Directory.Exists(source))
                    throw new DirectoryNotFoundException("Saves directory was not found: " + source);

                DateTime stamp = DateTime.Now;
                string destination = Path.GetFullPath(
                    Path.Combine(
                        BackupRoot,
                        stamp.ToString("yyyy-MM-dd_HH-mm-ss")));

                destination = MakeUniqueDirectory(destination);

                Console.WriteLine("WB Backup: copying {0} -> {1}", source, destination);

                CopyDirectory(source, destination);

                lock (m_Sync)
                {
                    m_LastBackup = DateTime.Now;
                    m_LastBackupPath = destination;
                    m_LastError = "(none)";
                    m_BackupRunning = false;
                }

                Console.WriteLine(
                    "WB Backup: completed successfully at {0}.",
                    m_LastBackup);
            }
            catch (Exception ex)
            {
                lock (m_Sync)
                {
                    m_BackupRunning = false;
                    m_LastError = ex.GetType().Name + ": " + ex.Message;
                }

                Console.WriteLine("WB Backup ERROR: {0}", ex);
            }
        }

        private static string MakeUniqueDirectory(string path)
        {
            if (!Directory.Exists(path))
                return path;

            for (int i = 1; i < 1000; ++i)
            {
                string candidate = path + "_" + i.ToString("000");

                if (!Directory.Exists(candidate))
                    return candidate;
            }

            return path + "_" + Guid.NewGuid().ToString("N");
        }

        private static void CopyDirectory(string source, string destination)
        {
            Directory.CreateDirectory(destination);

            string[] files = Directory.GetFiles(source);

            for (int i = 0; i < files.Length; ++i)
            {
                string name = Path.GetFileName(files[i]);
                string destFile = Path.Combine(destination, name);
                File.Copy(files[i], destFile, true);
            }

            string[] directories = Directory.GetDirectories(source);

            for (int i = 0; i < directories.Length; ++i)
            {
                string name = Path.GetFileName(directories[i]);
                CopyDirectory(directories[i], Path.Combine(destination, name));
            }
        }
    }
}
