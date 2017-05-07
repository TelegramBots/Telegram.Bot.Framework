using System.Collections.Generic;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public interface IMessageParser<TBot>
        where TBot : IBot
    {
        MessageType FindMessageType(Message message);

        IEnumerable<IMessageHandler<TBot>> FindMessageHandlers(Message message);
    }
}