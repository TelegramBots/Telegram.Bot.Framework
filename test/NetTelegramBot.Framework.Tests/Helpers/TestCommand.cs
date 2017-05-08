using System.Threading.Tasks;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Tests.Helpers
{
    public class TestCommandArgs : IBotCommandArgs<ITestBot>
    {
        public string RawInput { get; set; }
    }

    public interface ITestCommand : IBotCommand<ITestBot>
    {

    }

    public class TestCommand : BotCommandBase<ITestBot, TestCommandArgs>, ITestCommand
    {
        public TestCommand(string name)
            : base(name)
        {
            
        }

        public override Task ProcessCommand(Update update, TestCommandArgs args)
        {
            throw new System.NotImplementedException();
        }
    }
}
