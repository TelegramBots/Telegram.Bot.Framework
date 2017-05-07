using System.Collections.Generic;
using NetTelegramBot.Framework;

namespace SampleEchoBot.Services
{
    public class EchoBotMessageHandlerAccessor : IMessageHandlersAccessor<EchoBot>
    {
        public IEnumerable<IMessageHandler<EchoBot>> MessageHandlers { get; }

        public EchoBotMessageHandlerAccessor(IMessageHandler<EchoBot>[] handlers)
        {
            MessageHandlers = handlers;
        }
    }
}
