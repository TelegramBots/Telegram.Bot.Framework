using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Tests.Helpers
{
    public class TestBot : BotBase<TestBot>
    {
        public TestBot(IOptions<BotOptions<TestBot>> botOptions)
            : base(botOptions)
        {

        }

        public override Task HandleUnknownMessage(Update update)
        {
            throw new NotImplementedException();
        }

        public override Task HandleFaultedUpdate(Update update, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
