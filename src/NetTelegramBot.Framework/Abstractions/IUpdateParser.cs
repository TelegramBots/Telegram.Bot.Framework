using System.Collections.Generic;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface IUpdateParser<TBot>
        where TBot : class, IBot
    {
        void SetBot(IBot bot);

        IEnumerable<IUpdateHandler> FindHandlersFor(Update update);
    }
}