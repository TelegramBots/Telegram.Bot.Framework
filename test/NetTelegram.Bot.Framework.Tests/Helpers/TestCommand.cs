using System.Threading.Tasks;
using NetTelegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace NetTelegram.Bot.Framework.Tests.Helpers
{
    public class TestCommandArgs : ICommandArgs
    {
        public string RawInput { get; set; }

        public string ArgsInput { get; set; }
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
