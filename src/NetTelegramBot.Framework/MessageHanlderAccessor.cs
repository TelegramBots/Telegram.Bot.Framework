using System.Collections.Generic;
using NetTelegramBot.Framework.Abstractions;

namespace NetTelegramBot.Framework
{
    public class MessageHanlderAccessor<TIBot> : IMessageHandlersAccessor<TIBot>
        where TIBot : IBot
    {
        public IEnumerable<IMessageHandler<TIBot>> MessageHandlers { get; }

        public MessageHanlderAccessor(IEnumerable<IMessageHandler<TIBot>> handlers)
        {
            MessageHandlers = handlers;
        }
    }
}
