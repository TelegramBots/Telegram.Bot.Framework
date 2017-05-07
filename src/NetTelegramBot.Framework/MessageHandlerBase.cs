using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public abstract class MessageHandlerBase<TBot> : IMessageHandler<TBot>
        where TBot : BotBase<TBot>
    {
        public IBot Bot { get; set; }

        public abstract bool CanHandle(Update update);

        public abstract Task HandleMessageAsync(Update update);
    }
}
