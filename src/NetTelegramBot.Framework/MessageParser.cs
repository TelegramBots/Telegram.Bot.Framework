using System.Collections.Generic;
using System.Linq;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public class MessageParser<TBot> : IMessageParser<TBot>
        where TBot : IBot
    {
        protected IEnumerable<IMessageHandler<TBot>> MessageHandlers => _handlersAccessor.MessageHandlers;

        private readonly IMessageHandlersAccessor<TBot> _handlersAccessor;

        public MessageParser(IMessageHandlersAccessor<TBot> handlersAccessor)
        {
            _handlersAccessor = handlersAccessor;
        }

        public virtual IEnumerable<IMessageHandler<TBot>> FindMessageHandlers(Update update)
        {
            return MessageHandlers
                .Where(x => x.CanHandle(update));
        }
    }
}
