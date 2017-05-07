using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public abstract class MessageHandlerBase<TBot> : IMessageHandler<TBot>
        where TBot : BotBase<TBot>
    {
        public abstract MessageType MessageType { get; }

        public TBot Bot { get; set; }

        public abstract Task HandleMessageAsync(Message message);
    }
}
