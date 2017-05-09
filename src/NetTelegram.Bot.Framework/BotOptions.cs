using NetTelegram.Bot.Framework.Abstractions;

namespace NetTelegram.Bot.Framework
{
    public class BotOptions<TBot> : IBotOptions<TBot>
        where TBot : class, IBot
    {
        public string ApiToken { get; set; }

        public string BotName { get; set; }

        public string WebhookRoute { get; set; } 
    }
}
