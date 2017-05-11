namespace NetTelegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// Configurations for the bot
    /// </summary>
    /// <typeparam name="TBot">Type of Bot</typeparam>
    public interface IBotOptions<TBot>
        where TBot : class, IBot
    {
        /// <summary>
        /// Telegram API token
        /// </summary>
        string ApiToken { get; set; }

        /// <summary>
        /// User name of bot registered on Telegram
        /// </summary>
        string BotUserName { get; set; }

        /// <summary>
        /// Route to be used for webhook updates
        /// </summary>
        string WebhookRoute { get; set; }
    }
}
