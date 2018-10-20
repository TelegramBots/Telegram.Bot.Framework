using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SampleBots.Bots.GreeterBot
{
    public class GreeterBot : BotBase<GreeterBot>
    {
        private readonly ILogger _logger;

        public GreeterBot(IOptions<BotOptions<GreeterBot>> botOptions, ILogger<GreeterBot> logger)
            : base(botOptions)
        {
            _logger = logger;
        }

        public override async Task HandleUnknownUpdate(Update update)
        {
            _logger.LogWarning("Unable to handle an update");

            const string unknownUpdateText = "Sorry! I don't know what to do with this message";

            if (update.Type == UpdateType.Message)
            {
                await Client.SendTextMessageAsync(update.Message.Chat.Id,
                    unknownUpdateText,
                    replyToMessageId: update.Message.MessageId);
            }
            else
            {
                
            }
        }

        public override Task HandleFaultedUpdate(Update update, Exception e)
        {
            _logger.LogCritical("Exception thrown while handling an update");
            return Task.CompletedTask;
        }
    }
}
