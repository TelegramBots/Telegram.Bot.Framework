using System.Threading.Tasks;
using NetTelegram.Bot.Framework;
using NetTelegram.Bot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Sample.Bots.EchoBot
{
    public class TextMessageEchoer : UpdateHandlerBase
    {
        public override bool CanHandleUpdate(IBot bot, Update update)
        {
            return !string.IsNullOrEmpty(update.Message?.Text);
        }

        public override async Task<UpdateHandlingResult> HandleUpdateAsync(IBot bot, Update update)
        {
            var req = new SendMessage(update.Message.Chat.Id, $"You said:\n`{update.Message.Text.Replace("\n", "`\n`")}`")
            {
                ReplyToMessageId = update.Message.MessageId,
                ParseMode = SendMessage.ParseModeEnum.Markdown,
            };
            await bot.MakeRequestAsync(req);
            return UpdateHandlingResult.Continue;
        }
    }
}
