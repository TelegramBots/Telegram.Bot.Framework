using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// Configurations for the bot
    /// </summary>
    /// <typeparam name="TBot">Type of Bot</typeparam>
    public class BotOptions<TBot>
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
        /// Url to be used for webhook
        /// </summary>
        public string WebhookUrl { get; set; }

        /// <summary>
        /// Path to TLS certificate file. The .pem public key file used for encrypting and authenticating webhooks
        /// </summary>
        public string PathToCertificate { get; set; }

        /// <summary>
        /// Array of options for this bot's games
        /// </summary>
        public BotGameOption[] GameOptions { get; set; }
    }
}
