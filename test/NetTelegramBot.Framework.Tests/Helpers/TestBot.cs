using System.Threading.Tasks;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Tests.Helpers
{
    public interface ITestBot : IBot
    {

    }

    public class TestBot : BotBase<ITestBot>, ITestBot
    {
        public TestBot(IBotOptions<ITestBot> botOptions, IMessageParser<ITestBot> messageParser)
            : base(botOptions, messageParser)
        {
            BotUserInfo = new User
            {
                Username = "Test_Bot",
            };
        }

        public override Task HandleUnknownMessageAsync(Update update)
        {
            throw new System.NotImplementedException();
        }
    }
}
