using NetTelegramBot.Framework.Abstractions;

namespace SampleEchoBot.Services
{
    public class EchoBotOptions : IBotOptions<IEchoBot>
    {
        public string ApiToken { get; set; }

        public string BotName { get; set; }

        public bool? UseWebhook { get; set; }

        public string WebhookUrl { get; set; }
    }
}
