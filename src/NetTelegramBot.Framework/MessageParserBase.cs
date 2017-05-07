using System;
using System.Collections.Generic;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public abstract class MessageParserBase<TBot> : IMessageParser<TBot>
        where TBot : BotBase<TBot>
    {
        protected IEnumerable<IMessageHandler<TBot>> MessageHandlers => _handlersAccessor.MessageHandlers;

        private readonly IMessageHandlersAccessor<TBot> _handlersAccessor;

        protected MessageParserBase(IMessageHandlersAccessor<TBot> handlersAccessor)
        {
            _handlersAccessor = handlersAccessor;
        }

        public virtual MessageType FindMessageType(Message message)
        {
            throw new NotImplementedException();
        }

        public abstract IEnumerable<IMessageHandler<TBot>> FindMessageHandlers(Message message);
    }
}
