using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Sample.Bots.GreeterBot
{
    public class GreeterBot : BotBase<GreeterBot>
    {
        public GreeterBot(IBotOptions<GreeterBot> botOptions, IUpdateParser<GreeterBot> updateParser)
            : base(botOptions, updateParser)
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
