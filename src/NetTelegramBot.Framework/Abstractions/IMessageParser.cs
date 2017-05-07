using System.Collections.Generic;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Abstractions
{
    public interface IMessageParser<TBot>
        where TBot : IBot
    {
        void SetBot(IBot bot);

        IEnumerable<IMessageHandler<TBot>> FindMessageHandlers(Update update);
    }
}