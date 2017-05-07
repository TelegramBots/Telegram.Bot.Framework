using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;
using SampleEchoBot.Services;

namespace SampleEchoBot.Commands
{
    public interface IMessageForwarder
    {

    }

    public class MessageForwarder : MessageHandlerBase<EchoBot>, IMessageForwarder
    {
        public override MessageType MessageType { get; } = MessageType.All;
        
        public override async Task HandleMessageAsync(Message message)
        {
            await Bot.MakeRequestAsync(new ForwardMessage(message.Chat.Id, message.Chat.Id, message.MessageId));
        }
    }
}
