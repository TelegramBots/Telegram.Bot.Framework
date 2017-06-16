using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstractions
{
    /// <summary>
    /// Responsible for quickly parsing the updates and finding the handlers for them
    /// </summary>
    /// <typeparam name="TBot">Type of bot</typeparam>
    public interface IUpdateParser<TBot>
        where TBot : class, IBot
    {
        /// <summary>
        /// Finds update handlers for this specific bot update
        /// </summary>
        /// <param name="bot">Instance of bot</param>
        /// <param name="update">Update to be checked for handling</param>
        /// <returns>List of update handlers for the bot able to handle that update</returns>
        IEnumerable<IUpdateHandler> FindHandlersForUpdate(IBot bot, Update update);
    }
}