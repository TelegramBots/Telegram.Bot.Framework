using System.Threading.Tasks;

namespace Telegram.Bot.Abstractions
{
    /// <summary>
    /// Processes an update
    /// </summary>
    public interface IUpdateHandler
    {
        /// <summary>
        /// Handles the update for bot. This method will be called only if CanHandleUpdate returns <value>true</value>
        /// </summary>
        /// <param name="bot">Instance of the bot this command is operating for</param>
        /// <param name="update">The update to be handled</param>
        /// <returns>Result of handling this update</returns>
        Task HandleAsync(IBot bot, IUpdateContext context, UpdateDelegate next);
    }
}