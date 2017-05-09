using System.Collections.Generic;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface IUpdateParser<TBot>
        where TBot : class, IBot
    {
        IEnumerable<IUpdateHandler> FindHandlersForUpdate(IBot bot, Update update);
    }
}