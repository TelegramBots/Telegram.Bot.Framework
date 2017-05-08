using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
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

        public override async Task<UpdateHandlingResult> HandleUpdateAsync(Update update)
        {
            var req = new ForwardMessage(update.Message.Chat.Id, update.Message.Chat.Id, update.Message.MessageId);
            await Bot.MakeRequestAsync(req);
            return UpdateHandlingResult.Handled;
        }
    }
}
