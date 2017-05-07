using NetTelegramBot.Framework;

namespace SampleEchoBot.Services
{
    public class EchoBotOptions : IBotOptions<EchoBot>
    {
        public string ApiToken { get; set; }

        public bool? UseWebhook { get; set; }

        public string WebhookUrl { get; set; }
    }
}
