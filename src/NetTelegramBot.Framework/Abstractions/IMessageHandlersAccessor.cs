using System.Collections.Generic;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface IMessageHandlersAccessor<TBot>
        where TBot : IBot
    {
        IEnumerable<IMessageHandler<TBot>> MessageHandlers { get; }
    }
}
