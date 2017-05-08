using System.Threading.Tasks;
using NetTelegramBot.Framework;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Sample.Bots.EchoBot
{
    public class TextMessageEchoer : UpdateHandlerBase
    {
        public override bool CanHandle(Update update)
        {
            return !string.IsNullOrEmpty(update.Message?.Text);
        }

        public override async Task HandleUpdateAsync(Update update)
        {
            var req = new SendMessage(update.Message.Chat.Id, $"You said:\n`{update.Message.Text.Replace("\n", "`\n`")}`")
            {
                ReplyToMessageId = update.Message.MessageId,
                ParseMode = SendMessage.ParseModeEnum.Markdown,
            };
            await Bot.MakeRequestAsync(req);
        }
    }
}
