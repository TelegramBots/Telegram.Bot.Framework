using System.Collections.Generic;

namespace Telegram.Bot.Abstractions
{
    /// <summary>
    /// Provides a list of update handlers for the bot
    /// </summary>
    /// <typeparam name="TBot">Type of bot</typeparam>
    public interface IHandlersCollection<TBot> : IEnumerable<IHandlerPredicate>
        where TBot : IBot
    {
    }
}