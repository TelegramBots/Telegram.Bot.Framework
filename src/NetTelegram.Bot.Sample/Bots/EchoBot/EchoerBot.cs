using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTelegram.Bot.Framework;
using Telegram.Bot.Types;

namespace NetTelegram.Bot.Sample.Bots.EchoBot
{
    public class EchoerBot : BotBase<EchoerBot>
    {
        private readonly ILogger<EchoerBot> _logger;

        public EchoerBot(IOptions<BotOptions<EchoerBot>> botOptions, ILogger<EchoerBot> logger)
            : base(botOptions)
        {
            _logger = logger;
        }

        public override async Task HandleUnknownMessage(Update update)
        {
            _logger.LogWarning("Unable to handle an update");

            const string unknownUpdateText = "Sorry! I don't know what to do with this message";

            await Client.SendTextMessageAsync(update.Message.Chat.Id,
                unknownUpdateText,
                replyToMessageId: update.Message.MessageId);
        }

        public override Task HandleFaultedUpdate(Update update, Exception exception)
        {
            _logger.LogCritical("Exception thrown while handling an update");
            return Task.CompletedTask;
        }
    }
}
