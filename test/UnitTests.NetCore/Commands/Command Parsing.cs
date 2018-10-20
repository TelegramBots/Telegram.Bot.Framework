using Telegram.Bot.Framework.Abstractions;
using Xunit;

namespace UnitTests.NetCore
{
    public class CommandParsing
    {
        [Fact]
        public void Should_Parse_Valid_Commands()
        {
            IUpdateHandler command = new TestCommand(null);

            //command.HandleAsync()



            //Mock<IBot> mockBot = new Mock<IBot>();
            //mockBot.SetupGet(x => x.Username).Returns("Test_Bot");

            //IBot bot = mockBot.Object;
            //Message message = new Message
            //{
            //    Text = text,
            //    Entities = new[]
            //    {
            //        new MessageEntity
            //        {
            //            Type = MessageEntityType.BotCommand,
            //            Offset = text.IndexOf(commandValue, StringComparison.OrdinalIgnoreCase),
            //            Length = commandValue.Length
            //        },
            //    },
            //};

            //bool result = bot.CanHandleCommand("test", message);

            //Assert.True(result);
        }
    }
}