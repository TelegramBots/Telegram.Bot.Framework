using System.Collections.Generic;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface IMessageParser<TBot>
        where TBot : IBot
    {
        IEnumerable<IMessageHandler<TBot>> FindMessageHandlers(Update update);
    }
}