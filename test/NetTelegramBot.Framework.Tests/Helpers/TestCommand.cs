using System.Threading.Tasks;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework.Tests.Helpers
{
    public class TestCommandArgs : ICommandArgs
    {
        public string RawInput { get; set; }
    }

    public class TestCommand : CommandBase<TestCommandArgs>
    {
        public TestCommand(string name)
            : base(name)
        {

        }

        public override Task<UpdateHandlingResult> HandleCommand(Update update, TestCommandArgs args)
        {
            throw new System.NotImplementedException();
        }
    }
}
