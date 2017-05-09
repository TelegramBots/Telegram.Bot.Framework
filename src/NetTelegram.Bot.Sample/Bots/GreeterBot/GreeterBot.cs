using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTelegram.Bot.Framework;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Sample.Bots.GreeterBot
{
    public class GreeterBot : BotBase<GreeterBot>
    {
        private readonly ILogger _logger;

        public GreeterBot(IOptions<BotOptions<GreeterBot>> botOptions, ILogger<GreeterBot> logger)
            : base(botOptions)
        {
            _logger = logger;
        }

        public override async Task HandleUnknownMessageAsync(Update update)
        {
            _logger.LogWarning($"Unable to handle an update sent by {update.Message.From.FirstName}");
            var req = new SendMessage(update.Message.Chat.Id, "Sorry! I don't know what to do with this message")
            {
                ReplyToMessageId = update.Message.MessageId,
                ParseMode = SendMessage.ParseModeEnum.Markdown,
            };
            await Bot.MakeRequestAsync(req);
        }
    }
}
