using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace SampleEchoBot.Bots.GreeterBot
{
    public interface IGreeterBot : IBot
    {

    }

    public class GreeterBot : BotBase<IGreeterBot>, IGreeterBot
    {
        public GreeterBot(IBotOptions<IGreeterBot> botOptions, IMessageParser<IGreeterBot> messageParser)
            : base(botOptions, messageParser)
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
