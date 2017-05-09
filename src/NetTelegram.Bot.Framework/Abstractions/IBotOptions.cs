namespace NetTelegram.Bot.Framework.Abstractions
{
    public interface IBotOptions<TBot>
        where TBot : class, IBot
    {
        string ApiToken { get; set; }

        string BotName { get; set; }

        bool UseWebhook { get; set; }

        string WebhookUrl { get; set; }
    }
}
