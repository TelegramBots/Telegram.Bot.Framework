using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;

namespace SampleGames.Bots.CrazyCircle
{
    public class CrazyCircleBot : BotBase<CrazyCircleBot>
    {
        private readonly ILogger<CrazyCircleBot> _logger;

        public CrazyCircleBot(IOptions<BotOptions<CrazyCircleBot>> botOptions,
            ILogger<CrazyCircleBot> logger)
            : base(botOptions)
        {
            _logger = logger;
        }

        public override Task HandleUnknownUpdate(Update update)
        {
            _logger.LogWarning("Don't know how to handle update of type `{0}`", update.Type);
            return Task.CompletedTask;
        }

        public override Task HandleFaultedUpdate(Update update, Exception e)
        {
            _logger.LogError("Error in handling update of type `{0}`.{1}{2}",
                update.Type, Environment.NewLine, e);
            return Task.CompletedTask;
        }
    }
}