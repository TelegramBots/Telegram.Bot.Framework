using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework
{
    public class BotWebhookOptions<TBot>
        where TBot : IBot
    {
        public string Url { get; set; }

        /// <summary>
        /// Path to TLS certificate file. The .pem public key file used for encrypting and authenticating webhooks
        /// </summary>
        public string Certificate { get; set; }
    }
}