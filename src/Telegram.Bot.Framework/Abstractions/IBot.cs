namespace Telegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// A wrapper around TelegramBot class. Used to make calls to the Bot API
    /// </summary>
    public interface IBot
    {
        /// <summary>
        /// Gets Bot's user name
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Instance of Telegram bot client
        /// </summary>
        ITelegramBotClient Client { get; }
    }
}
