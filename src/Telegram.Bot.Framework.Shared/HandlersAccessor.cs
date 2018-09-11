using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Abstractions;

namespace Telegram.Bot.Framework
{
    public class HandlersAccessor<TBot> : IHandlersAccessor<TBot>
        where TBot : IBot
    {
        public Type[] HandlerTypes { get; }

        /// <summary>
        /// Initializes the accessor with a list of update handlers
        /// </summary>
        /// <param name="handlers">List of update handlers for the bot</param>
        public HandlersAccessor(IEnumerable<Type> handlers)
        {
            HandlerTypes = handlers.ToArray();
        }

        public HandlersAccessor(params Type[] handlers)
        {
            HandlerTypes = handlers;
        }
    }
}