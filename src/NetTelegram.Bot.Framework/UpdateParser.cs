using System.Collections.Generic;
using System.Linq;
using NetTelegram.Bot.Framework.Abstractions;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework
{
    public class UpdateParser<TBot> : IUpdateParser<TBot>
        where TBot : BotBase<TBot>
    {
        public IBot Bot { get; set; }

        protected IEnumerable<IUpdateHandler> MessageHandlers => _handlersAccessor.UpdateHandlers;

        private readonly IUpdateHandlersAccessor<TBot> _handlersAccessor;

        public UpdateParser(IUpdateHandlersAccessor<TBot> handlersAccessor)
        {
            _handlersAccessor = handlersAccessor;
        }

        public virtual IEnumerable<IUpdateHandler> FindHandlersForUpdate(IBot bot, Update update)
        {
            return MessageHandlers
                .Where(x => x.CanHandleUpdate(bot, update));
        }
    }
}
