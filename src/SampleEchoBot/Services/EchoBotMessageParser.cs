using System;
using System.Collections.Generic;
using NetTelegramBot.Framework;
using NetTelegramBotApi.Types;

namespace SampleEchoBot.Services
{
    public class EchoBotMessageParser : MessageParserBase<EchoBot>
    {
        public EchoBotMessageParser(IMessageHandlersAccessor<EchoBot> handlersAccessor)
            : base(handlersAccessor)
        {

        }

        public override IEnumerable<IMessageHandler<EchoBot>> FindMessageHandlers(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
