namespace NetTelegramBot.Framework
{
    public interface IBotOptions<TBot>
        where TBot : IBot
    {
        string ApiToken { get; set; }

        bool? UseWebhook { get; set; }

        string WebhookUrl { get; set; }
    }
}
