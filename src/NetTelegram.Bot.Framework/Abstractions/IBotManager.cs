using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// Manages bot and sends updates to handlers
    /// </summary>
    /// <typeparam name="TBot">Type of bot</typeparam>
    public interface IBotManager<TBot>
        where TBot : class, IBot
    {
        /// <summary>
        /// Gets webhook's route from bot options provided
        /// </summary>
        string WebhookRoute { get; }

        /// <summary>
        /// Handle the update
        /// </summary>
        /// <param name="update">Update to be handled</param>
        /// <returns></returns>
        Task HandleUpdateAsync(Update update);

        /// <summary>
        /// Pulls the updates from Telegram if any and passes them to handlers
        /// </summary>
        /// <returns></returns>
        Task GetAndHandleNewUpdatesAsync();

        /// <summary>
        /// Sets webhook for this bot
        /// </summary>
        /// <param name="appBaseUrl">Applications's base url</param>
        /// <returns></returns>
        Task SetWebhook(string appBaseUrl);
    }
}
