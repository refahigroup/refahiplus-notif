using Refahi.Notif.Domain.Contract.Messaging;
using System.Web;

namespace Refahi.Notif.Infrastructure.Messaging.Telegram
{
    public class TelegramSender : ITelegramSender
    {
        private readonly TelegramConfiguration _config;
        public TelegramSender(TelegramConfiguration configuration)
        {
            _config = configuration;
        }
        public async Task<string> SendAsync(string chatId, string? body, string? fileName, Stream? fileData)
        {
            if (!string.IsNullOrEmpty(fileName) && fileData != null)
                return await SendFileAsync(chatId, body, fileName, fileData);
            if (!string.IsNullOrEmpty(body))
                return await SendMessageAsync(chatId, body);
            return "Empty";
        }
        private async Task<string> SendFileAsync(string chatId, string? body, string fileName, Stream fileData)
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_config.BaseUrl}/bot{_config.BotToken}/sendDocument?chat_id={chatId}");
            var content = new MultipartFormDataContent();
            fileData.Position = 0;
            content.Add(new StreamContent(fileData), "document", fileName);
            if (!string.IsNullOrEmpty(body))
                content.Add(new StringContent(body), "caption");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        private async Task<string> SendMessageAsync(string chatId, string body)
        {
            var url = $"{_config.BaseUrl}/bot{_config.BotToken}/sendMessage?chat_id={chatId}&text=";
            url += HttpUtility.HtmlEncode(body) + "&";
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
