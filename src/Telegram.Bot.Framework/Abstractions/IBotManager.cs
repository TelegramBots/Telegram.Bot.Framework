using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// Manages bot and sends updates to handlers
    /// </summary>
    /// <typeparam name="TBot">Type of bot</typeparam>
    public interface IBotManager<TBot>
        where TBot : class, IBot
    {
        /// <summary>
        /// Gets webhook's url from bot options provided
        /// </summary>
        string WebhookUrl { get; }
        
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
        /// Enables or disables the webhook for this bot
        /// </summary>
        /// <param name="enabled">Whether webhook should be set or deleted</param>
        /// <remarks>
        /// Webhook url will be retrieved from bot's <see cref="BotOptions{TBot}">options</see>.
        /// Disabling webhook means user wants to use long polling method to get updates.
        /// </remarks>
        Task SetWebhookStateAsync(bool enabled);
    }
}
