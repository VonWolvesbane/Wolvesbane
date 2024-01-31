using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;

public static class DiscordWebhook
{
	public static async Task SendWebhookMessage(string webhookUrl, string message)
	{
		using (WebClient client = new WebClient())
		{
			var data = new NameValueCollection
			{
				{ "content", message }
			};

			// Set the Content-Type header
			client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

			// Convert data to bytes
			byte[] responseBytes = await client.UploadValuesTaskAsync(webhookUrl, "POST", data);

			// Optionally, you can handle the response if needed
			string response = Encoding.UTF8.GetString(responseBytes);
		}
	}
}
