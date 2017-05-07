using System.Collections.Generic;

namespace NetTelegramBot.Framework
{
    public interface IMessageHandlersAccessor<TBot>
        where TBot : BotBase<TBot>
    {
        IEnumerable<IMessageHandler<TBot>> MessageHandlers { get; }
    }
}
