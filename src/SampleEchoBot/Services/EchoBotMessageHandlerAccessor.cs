using System.Collections.Generic;
using NetTelegramBot.Framework.Abstractions;

namespace SampleEchoBot.Services
{
    public class EchoBotMessageHandlerAccessor : IMessageHandlersAccessor<IEchoBot>
    {
        public IEnumerable<IMessageHandler<IEchoBot>> MessageHandlers { get; }

        public EchoBotMessageHandlerAccessor(IMessageHandler<IEchoBot>[] handlers)
        {
            MessageHandlers = handlers;
        }
    }
}
