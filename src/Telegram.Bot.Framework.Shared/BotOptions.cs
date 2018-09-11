using Telegram.Bot.Abstractions;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// Configurations for the bot
    /// </summary>
    /// <typeparam name="TBot">Type of Bot</typeparam>
    public class BotOptions<TBot> : IBotOptions
        where TBot : IBot
    {
        /// <summary>
        /// Optional if client not needed. Telegram API token
        /// </summary>
        public string ApiToken { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// Path to TLS certificate file. The .pem public key file used for encrypting and authenticating webhooks
        /// </summary>
        public string Certificate { get; set; }
    }
}