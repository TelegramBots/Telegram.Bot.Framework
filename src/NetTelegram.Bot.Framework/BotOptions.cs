using NetTelegram.Bot.Framework.Abstractions;

namespace NetTelegram.Bot.Framework
{
    /// <summary>
    /// Configurations for the bot
    /// </summary>
    /// <typeparam name="TBot">Type of Bot</typeparam>
    public class BotOptions<TBot> : IBotOptions<TBot>
        where TBot : class, IBot
    {
        /// <summary>
        /// Telegram API token
        /// </summary>
        public string ApiToken { get; set; }

        /// <summary>
        /// User name of bot registered on Telegram
        /// </summary>
        public string BotUserName { get; set; }

        /// <summary>
        /// Route to be used for webhook updates
        /// </summary>
        public string WebhookRoute { get; set; }
    }
}
