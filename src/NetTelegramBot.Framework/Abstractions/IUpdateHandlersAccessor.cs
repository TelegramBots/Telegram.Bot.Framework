using System.Collections.Generic;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface IUpdateHandlersAccessor<TBot>
        where TBot : class, IBot
    {
        IEnumerable<IUpdateHandler> UpdateHandlers { get; }
    }
}
