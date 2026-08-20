using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Server;
using Server.Commands;

namespace Wolvesbane.DiscordBridge
{
	public static class WBDiscordInboundBridge
	{
		private const int BridgePort = 8091;
		private const string BridgeSecret = "69e6a9813ef51c5233aaa6cc337d115f52af7ff4f5af1102";
		private const int MessageHue = 0x58;
		private const int MaxMessageLength = 220;

		private static readonly object m_Sync = new object();
		private static readonly Queue<string> m_RecentMessageIds = new Queue<string>();
		private static readonly HashSet<string> m_RecentMessageIdSet = new HashSet<string>();

		private static TcpListener m_Listener;
		private static Thread m_Thread;
		private static volatile bool m_Running;
		private static long m_Accepted;
		private static long m_Rejected;
		private static string m_LastError = "(none)";
		private static DateTime m_LastMessageUtc = DateTime.MinValue;

		public static void Initialize()
		{
			CommandSystem.Register("WBDiscordBridge", AccessLevel.Administrator, new CommandEventHandler(OnCommand));
			Start();
		}

		private static void OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;
			string action = e.Arguments != null && e.Arguments.Length > 0 ? e.Arguments[0].ToLower() : "status";

			if (action == "status")
			{
				from.SendMessage(88, "Wolvesbane Discord -> World Chat Bridge");
				from.SendMessage("Running: {0}", m_Running ? "yes" : "no");
				from.SendMessage("Listener: 127.0.0.1:{0}", BridgePort);
				from.SendMessage("Accepted messages: {0:N0}", m_Accepted);
				from.SendMessage("Rejected messages: {0:N0}", m_Rejected);
				from.SendMessage("Last accepted message: {0}", m_LastMessageUtc == DateTime.MinValue ? "(none)" : m_LastMessageUtc.ToString("u"));
				from.SendMessage("Last error: {0}", m_LastError);
				return;
			}

			if (action == "start")
			{
				Start();
				from.SendMessage(m_Running ? 68 : 33, m_Running ? "Discord inbound bridge is running." : "Discord inbound bridge failed to start; check the console.");
				return;
			}

			if (action == "stop")
			{
				Stop();
				from.SendMessage(68, "Discord inbound bridge stopped.");
				return;
			}

			if (action == "test")
			{
				QueueWorldMessage("BridgeTest", "Discord inbound bridge test message.");
				from.SendMessage(68, "Queued a local Discord bridge test message.");
				return;
			}

			from.SendMessage("Usage: [WBDiscordBridge status|start|stop|test");
		}

		private static void Start()
		{
			lock (m_Sync)
			{
				if (m_Running)
					return;

				try
				{
					m_Listener = new TcpListener(IPAddress.Loopback, BridgePort);
					m_Listener.Start();
					m_Running = true;
					m_LastError = "(none)";

					m_Thread = new Thread(new ThreadStart(ListenLoop));
					m_Thread.IsBackground = true;
					m_Thread.Name = "WB Discord Inbound Bridge";
					m_Thread.Start();

					Console.WriteLine("WB Discord Bridge: listening on 127.0.0.1:{0}", BridgePort);
				}
				catch (Exception ex)
				{
					m_Running = false;
					m_LastError = ex.GetType().Name + ": " + ex.Message;
					Console.WriteLine("WB Discord Bridge ERROR: {0}", ex);
				}
			}
		}

		private static void Stop()
		{
			lock (m_Sync)
			{
				m_Running = false;
				try { if (m_Listener != null) m_Listener.Stop(); } catch { }
				m_Listener = null;
				m_Thread = null;
			}
		}

		private static void ListenLoop()
		{
			while (m_Running)
			{
				TcpClient client = null;
				try
				{
					client = m_Listener.AcceptTcpClient();
					client.ReceiveTimeout = 5000;
					ProcessClient(client);
				}
				catch (SocketException)
				{
					if (m_Running)
						RegisterError("SocketException while accepting bridge connection.");
				}
				catch (ObjectDisposedException) { }
				catch (Exception ex)
				{
					RegisterError(ex.GetType().Name + ": " + ex.Message);
				}
				finally
				{
					if (client != null) { try { client.Close(); } catch { } }
				}
			}
		}

		private static void ProcessClient(TcpClient client)
		{
			using (NetworkStream stream = client.GetStream())
			using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
			using (StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(false)))
			{
				writer.AutoFlush = true;
				string line = reader.ReadLine();

				if (String.IsNullOrEmpty(line)) { Reject(writer, "empty"); return; }
				string[] parts = line.Split('|');
				if (parts.Length != 5 || parts[0] != "WB1") { Reject(writer, "protocol"); return; }
				if (!FixedTimeEquals(parts[1], BridgeSecret)) { Reject(writer, "auth"); return; }

				string author, content;
				try
				{
					author = DecodeBase64(parts[2]);
					content = DecodeBase64(parts[3]);
				}
				catch { Reject(writer, "encoding"); return; }

				string messageId = parts[4];
				if (String.IsNullOrEmpty(messageId)) { Reject(writer, "id"); return; }

				lock (m_Sync)
				{
					if (m_RecentMessageIdSet.Contains(messageId))
					{
						writer.WriteLine("OK|duplicate");
						return;
					}
					m_RecentMessageIds.Enqueue(messageId);
					m_RecentMessageIdSet.Add(messageId);
					while (m_RecentMessageIds.Count > 200)
					{
						string oldId = m_RecentMessageIds.Dequeue();
						m_RecentMessageIdSet.Remove(oldId);
					}
				}

				author = Sanitize(author, 48);
				content = Sanitize(content, MaxMessageLength);

				if (String.IsNullOrWhiteSpace(author)) author = "Discord";
				if (String.IsNullOrWhiteSpace(content)) { Reject(writer, "content"); return; }

				Interlocked.Increment(ref m_Accepted);
				m_LastMessageUtc = DateTime.UtcNow;
				m_LastError = "(none)";
				QueueWorldMessage(author, content);
				writer.WriteLine("OK|accepted");
			}
		}

		private static void QueueWorldMessage(string author, string content)
		{
			string text = String.Format("[Discord] {0}: {1}", author, content);
			Server.Timer.DelayCall(TimeSpan.Zero, delegate
			{
				try { World.Broadcast(MessageHue, true, text); }
				catch (Exception ex) { RegisterError("Broadcast: " + ex.GetType().Name + ": " + ex.Message); }
			});
		}

		private static void Reject(StreamWriter writer, string reason)
		{
			Interlocked.Increment(ref m_Rejected);
			try { writer.WriteLine("ERR|" + reason); } catch { }
		}

		private static void RegisterError(string error)
		{
			m_LastError = error;
			Console.WriteLine("WB Discord Bridge ERROR: {0}", error);
		}

		private static string DecodeBase64(string value)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(value));
		}

		private static string Sanitize(string value, int maxLength)
		{
			if (String.IsNullOrEmpty(value)) return String.Empty;
			StringBuilder sb = new StringBuilder(value.Length);

			for (int i = 0; i < value.Length; ++i)
			{
				char c = value[i];
				if (c == '\r' || c == '\n' || c == '\t')
				{
					if (sb.Length == 0 || sb[sb.Length - 1] != ' ') sb.Append(' ');
					continue;
				}
				if (Char.IsControl(c)) continue;
				sb.Append(c);
				if (sb.Length >= maxLength) break;
			}
			return sb.ToString().Trim();
		}

		private static bool FixedTimeEquals(string a, string b)
		{
			if (a == null || b == null) return false;
			int diff = a.Length ^ b.Length;
			int max = Math.Max(a.Length, b.Length);

			for (int i = 0; i < max; ++i)
			{
				char ca = i < a.Length ? a[i] : (char)0;
				char cb = i < b.Length ? b[i] : (char)0;
				diff |= ca ^ cb;
			}
			return diff == 0;
		}
	}
}