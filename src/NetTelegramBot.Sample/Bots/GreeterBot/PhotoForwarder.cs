using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Sample.Bots.GreeterBot
{
    public class PhotoForwarder : UpdateHandlerBase
    {
        public override bool CanHandle(Update update)
        {
            return update.Message.Photo != null;
        }

        public override async Task HandleUpdateAsync(Update update)
        {
            var req = new ForwardMessage(update.Message.Chat.Id, update.Message.Chat.Id, update.Message.MessageId);
            await Bot.MakeRequestAsync(req);
        }
    }
}
