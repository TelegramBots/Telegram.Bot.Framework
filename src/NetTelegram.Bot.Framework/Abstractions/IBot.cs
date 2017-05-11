using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework.Abstractions
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
        /// Gets this Bot's webhook information set on Telegram
        /// </summary>
        WebhookInfo WebhookInfo { get; }

        /// <summary>
        /// Sends a HTTPS request to Telegram bot API
        /// </summary>
        /// <typeparam name="T">Type of expected response from Telegram Bot API</typeparam>
        /// <param name="request">Telegram API request call to be sent</param>
        /// <returns>Response from Telegram API</returns>
        Task<T> MakeRequestAsync<T>(NetTelegramBotApi.Requests.RequestBase<T> request);
    }
}
