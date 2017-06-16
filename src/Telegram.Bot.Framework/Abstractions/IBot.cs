using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// A wrapper around TelegramBot class. Used to make calls to the Bot API
    /// </summary>
    public interface IBot
    {
        /// <summary>
        /// Gets this Bot's user information
        /// </summary>
        User BotUserInfo { get; }

        /// <summary>
        /// Instance of Telegram bot client
        /// </summary>
        ITelegramBotClient Client { get; }
    }
}
