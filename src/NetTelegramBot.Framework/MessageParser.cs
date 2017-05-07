using System.Collections.Generic;
using System.Linq;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public class MessageParser<TBot> : IMessageParser<TBot>
        where TBot : BotBase<TBot>
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
