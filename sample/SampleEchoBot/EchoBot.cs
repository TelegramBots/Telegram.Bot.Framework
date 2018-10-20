using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SampleEchoBot
{
    public class EchoBot : BotBase<EchoBot>
    {
        private readonly ILogger<EchoBot> _logger;

        public EchoBot(IOptions<BotOptions<EchoBot>> botOptions, ILogger<EchoBot> logger)
            : base(botOptions)
        {
            _logger = logger;
        }

        public override async Task HandleUnknownUpdate(Update update)
        {
            _logger.LogWarning("Unable to handle update of type `{0}`", update.Type);

            string text;
            int replyToMesageId = default(int);

            switch (update.Type)
            {
                case UpdateType.Message when
                new[] {ChatType.Private, ChatType.Group, ChatType.Supergroup}.Contains(update.Message.Chat.Type):
                    text = $"Unable to handle message update of type `{update.Message.Type}`.";
                    replyToMesageId = update.Message.MessageId;
                    break;
                default:
                    text = null;
                    break;
            }

            if (text != null)
            {
                await Client.SendTextMessageAsync(update.Message.Chat.Id, text, ParseMode.Markdown,
                    replyToMessageId: replyToMesageId);
            }
        }

        public override Task HandleFaultedUpdate(Update update, Exception e)
        {
            _logger.LogError($"Exception occured in handling update of type `{0}`: {1}", update.Type, e.Message);

            return Task.CompletedTask;
        }
    }
}