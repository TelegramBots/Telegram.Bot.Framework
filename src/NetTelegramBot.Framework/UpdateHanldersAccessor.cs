using System.Collections.Generic;
using NetTelegramBot.Framework.Abstractions;

namespace NetTelegramBot.Framework
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
