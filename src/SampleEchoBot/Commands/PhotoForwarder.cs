using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;
using SampleEchoBot.Services;

namespace SampleEchoBot.Commands
{
    public interface IPhotoForwarder : IMessageHandler<IEchoBot>
    {

    }

    public class PhotoForwarder : MessageHandlerBase<IEchoBot>, IPhotoForwarder
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
