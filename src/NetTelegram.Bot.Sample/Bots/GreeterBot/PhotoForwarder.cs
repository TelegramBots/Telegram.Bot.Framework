using System.Threading.Tasks;
using NetTelegram.Bot.Framework;
using NetTelegram.Bot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Sample.Bots.GreeterBot
{
    public class PhotoForwarder : UpdateHandlerBase
    {
        public override bool CanHandleUpdate(IBot bot, Update update)
        {
            return update.Message.Photo != null;
        }

        public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            var req = new ForwardMessage(update.Message.Chat.Id, update.Message.Chat.Id, update.Message.MessageId);
            await bot.MakeRequest(req);
            return UpdateHandlingResult.Handled;
        }
    }
}
