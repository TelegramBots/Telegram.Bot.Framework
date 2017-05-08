using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace SampleEchoBot.Bots.GreeterBot
{
    public interface IPhotoForwarder : IMessageHandler<IGreeterBot>
    {

    }

    public class PhotoForwarder : MessageHandlerBase<IGreeterBot>, IPhotoForwarder
    {
        public override bool CanHandle(Update update)
        {
            return update.Message.Photo != null;
        }

        public override async Task HandleMessageAsync(Update update)
        {
            var req = new ForwardMessage(update.Message.Chat.Id, update.Message.Chat.Id, update.Message.MessageId);
            await Bot.MakeRequestAsync(req);
        }
    }
}
