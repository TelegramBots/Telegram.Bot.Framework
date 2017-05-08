using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NetTelegramBot.Framework;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Sample.Bots.GreeterBot
{
    public class GreeterBot : BotBase<GreeterBot>
    {
        public GreeterBot(IOptions<BotOptions<GreeterBot>> botOptions)
            : base(botOptions)
        {

        }

        public override async Task HandleUnknownMessageAsync(Update update)
        {
            var req = new SendMessage(update.Message.Chat.Id, "Sorry! I don't know what to do with this message")
            {
                ReplyToMessageId = update.Message.MessageId,
                ParseMode = SendMessage.ParseModeEnum.Markdown,
            };
            await Bot.MakeRequestAsync(req);
        }
    }
}
