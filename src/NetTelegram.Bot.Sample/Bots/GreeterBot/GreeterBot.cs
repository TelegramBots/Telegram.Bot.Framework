using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTelegram.Bot.Framework;
using Telegram.Bot.Types;

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
