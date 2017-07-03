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
        /// Base bot url to be used for webhook and game updates
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Path to TLS certificate file. The .pem public key file used for encrypting and authenticating webhooks
        /// </summary>
        public string PathToCertificate { get; set; }

        public BotGameOption[] GameOptions { get; set; }
    }
}
