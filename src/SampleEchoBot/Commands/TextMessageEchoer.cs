using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;
using SampleEchoBot.Services;

namespace SampleEchoBot.Commands
{
    public interface ITextMessageEchoer : IMessageHandler<IEchoBot>
    {

    }

    public class TextMessageEchoer : MessageHandlerBase<IEchoBot>, ITextMessageEchoer
    {
        public override bool CanHandle(Update update)
        {
            return !string.IsNullOrEmpty(update.Message?.Text);
        }

        public override async Task HandleMessageAsync(Update update)
        {
            var req = new SendMessage(update.Message.Chat.Id, update.Message.Text)
            {
                ReplyToMessageId = update.Message.MessageId,
            };
            await Bot.MakeRequestAsync(req);
        }
    }
}
