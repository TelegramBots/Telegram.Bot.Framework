using System.Collections.Generic;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework.Abstractions
{
    public interface IUpdateParser<TBot>
        where TBot : class, IBot
    {
        IEnumerable<IUpdateHandler> FindHandlersForUpdate(IBot bot, Update update);
    }
}