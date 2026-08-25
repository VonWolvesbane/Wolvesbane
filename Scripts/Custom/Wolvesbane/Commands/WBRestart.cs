using System;
using Server;
using Server.Commands;

namespace Wolvesbane.Commands
{
    public static class WBRestart
    {
        private static bool _Restarting;

        public static void Initialize()
        {
            CommandSystem.Register("Restart", AccessLevel.Administrator, new CommandEventHandler(OnRestart));
        }

        [Usage("Restart")]
        [Description("Saves the world and performs a clean server restart.")]
        private static void OnRestart(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (_Restarting)
            {
                from.SendMessage(33, "A server restart is already in progress.");
                return;
            }

            if (World.Saving)
            {
                from.SendMessage(33, "The world is already saving. Wait for the save to finish, then use [Restart again.");
                return;
            }

            _Restarting = true;

            World.Broadcast(0x35, true, "The server is being saved and restarted by an administrator.");
            Console.WriteLine("WB RESTART: Requested by {0}. Saving world before restart...",
                from != null ? from.Name : "(console)");

            try
            {
                // Wolvesbane already uses this save pattern in the backup system.
                World.Save(true, false);

                // Defensive: ensure asynchronous save writes have completed before
                // allowing the process to restart.
                World.WaitForWriteCompletion();

                Console.WriteLine("WB RESTART: Save completed. Restarting server.");

                // true = restart. This uses ServUO's normal restart mechanism,
                // equivalent to the restart path used by the server/admin controls.
                Core.Kill(true);
            }
            catch (Exception ex)
            {
                _Restarting = false;

                Console.WriteLine("WB RESTART ERROR: {0}", ex);

                if (from != null)
                    from.SendMessage(33, "Restart failed: {0}: {1}", ex.GetType().Name, ex.Message);
            }
        }
    }
}
