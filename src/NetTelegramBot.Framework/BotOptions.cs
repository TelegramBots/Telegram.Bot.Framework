using NetTelegramBot.Framework.Abstractions;

namespace NetTelegramBot.Framework
{
    public class BotOptions<TIBot> : IBotOptions<TIBot>
        where TIBot : IBot
    {
        public string ApiToken { get; set; }

        public string BotName { get; set; }

        public bool? UseWebhook { get; set; }

        public string WebhookUrl { get; set; }
    }
}
