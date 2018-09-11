using System;

namespace Telegram.Bot.Abstractions
{
    /// <summary>
    /// Provides a list of update handlers for the bot
    /// </summary>
    /// <typeparam name="TBot">Type of bot</typeparam>
    public interface IHandlersAccessor<TBot>
        where TBot : IBot
    {
        Type[] HandlerTypes { get; }
    }
}