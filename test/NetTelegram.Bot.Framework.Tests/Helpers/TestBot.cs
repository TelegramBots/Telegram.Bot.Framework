using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework.Tests.Helpers
{
    public class TestBot : BotBase<TestBot>
    {
        public TestBot(IOptions<BotOptions<TestBot>> botOptions)
            : base(botOptions)
        {

        }

        public override Task HandleUnknownMessageAsync(Update update)
        {
            throw new System.NotImplementedException();
        }
    }
}
