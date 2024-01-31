using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Commands;

namespace DiscordPlayerCount
{
	public class DiscordPlayerCountScript
	{
		public static void Initialize()
		{
			CommandSystem.Register("GetPlayerCount", AccessLevel.Player, new CommandEventHandler(GetPlayerCount_OnCommand));

			EventSink.Login += OnPlayerLogin;
			EventSink.Logout += OnPlayerLogout;
		}

		private static void GetPlayerCount_OnCommand(CommandEventArgs e)
		{
			UpdatePlayerCount();
		}

		private static void OnPlayerLogin(LoginEventArgs e)
		{
			UpdatePlayerCount();
		}

		private static void OnPlayerLogout(LogoutEventArgs e)
		{
			UpdatePlayerCount();
		}

		private static void UpdatePlayerCount()
		{
			int playerCount = CountOnlinePlayers();
			string message = $"Current player count: {playerCount}";

			// Replace 'YOUR_DISCORD_WEBHOOK_URL' with your actual Discord webhook URL
			string webhookUrl = "https://discord.com/api/webhooks/863596394271211520/ZFE72jXhjTujf_ZwA5PaCEcR4YQzkN_6ezYYIErskbkG2YeJiN-bFVzhDR3sY3QqUmdG";

			// Send the message to Discord
			DiscordWebhook.SendWebhookMessage(webhookUrl, message);
		}

		private static int CountOnlinePlayers()
		{
			int count = 0;

			foreach (Mobile m in World.Mobiles.Values)
			{
				if (m is PlayerMobile && m.NetState != null && m.NetState.Mobile == m)

				{
					count++;
				}
			}

			return count;
		}
	}
}
