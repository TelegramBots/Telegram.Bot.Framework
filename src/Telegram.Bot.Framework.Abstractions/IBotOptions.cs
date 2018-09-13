namespace Telegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// Configurations for the bot
    /// </summary>
    public interface IBotOptions
    {
        /// <summary>
        /// Optional if client not needed. Telegram API token
        /// </summary>
        string ApiToken { get; }

        string Url { get; }

        /// <summary>
        /// Path to TLS certificate file. The .pem public key file used for encrypting and authenticating webhooks
        /// </summary>
        string Certificate { get; }
    }
}