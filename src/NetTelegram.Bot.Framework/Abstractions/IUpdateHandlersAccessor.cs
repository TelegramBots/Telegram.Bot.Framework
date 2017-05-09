using System.Collections.Generic;

namespace NetTelegram.Bot.Framework.Abstractions
{
    public interface IUpdateHandlersAccessor<TBot>
        where TBot : class, IBot
    {
        IEnumerable<IUpdateHandler> UpdateHandlers { get; }
    }
}
