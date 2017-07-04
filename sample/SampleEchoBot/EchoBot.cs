using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;

namespace SampleEchoBot
{
    public class EchoBot : BotBase<EchoBot>
    {
        public EchoBot(IOptions<BotOptions<EchoBot>> botOptions)
            : base(botOptions)
        {

        }

        public override Task HandleUnknownMessage(Update update)
        {
            return Task.CompletedTask;
        }

        public override Task HandleFaultedUpdate(Update update, Exception e)
        {
            return Task.CompletedTask;
        }
    }
}
