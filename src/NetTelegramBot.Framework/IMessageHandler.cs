using System.Threading.Tasks;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public interface IMessageHandler<TBot>
        where TBot : IBot
    {
        MessageType MessageType { get; }

        TBot Bot { get; set; }

        Task HandleMessageAsync(Message message);
    }
}
