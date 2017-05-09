using System.Collections.Generic;
using NetTelegram.Bot.Framework.Abstractions;

namespace NetTelegram.Bot.Framework
{
    public class UpdateHanldersAccessor<TBot> : IUpdateHandlersAccessor<TBot>
        where TBot : class, IBot
    {
        public IEnumerable<IUpdateHandler> UpdateHandlers { get; }

        public UpdateHanldersAccessor(IEnumerable<IUpdateHandler> handlers)
        {
            UpdateHandlers = handlers;
        }
    }
}
