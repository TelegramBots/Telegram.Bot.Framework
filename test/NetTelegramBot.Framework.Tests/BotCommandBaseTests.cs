using NetTelegramBot.Framework.Tests.Helpers;
using NetTelegramBotApi.Types;
using Xunit;

namespace NetTelegramBot.Framework.Tests
{
    public class BotCommandBaseTests
    {
        [Theory(DisplayName = "Accept handling specific commands")]
        [InlineData("/test")]
        [InlineData("/test    ")]
        [InlineData("/test abc")]
        [InlineData("/test@test_bot")]
        [InlineData("/test@test_bot ")]
        [InlineData("/test@test_bot  !")]
        public void ShouldAcceptHandlingAll(string text)
        {
            const bool Expected = true;
            var sut = new TestCommand("test");
            var update = new Update
            {
                Message = new Message
                {
                    Text = text,
                }
            };
            var actual = sut.CanHandle(update);
            Assert.Equal(Expected, actual);
        }
        /*
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("aasdasdsd")]
        [InlineData("/")]
        [InlineData("/@")]
        public void NotACommand(string value)
        {
            var parser = new CommandParser();
            var command = parser.TryParse(value);
            Assert.Null(command);
        }

        [Theory]
        [InlineData("/cmd arg1 arg2")]
        [InlineData("/cmd    arg1    arg2   ")]
        public void SimpleCommand(string value)
        {
            var parser = new CommandParser();
            var command = parser.TryParse(value);

            Assert.NotNull(command);
            Assert.Equal("cmd", command.Name);
            Assert.Null(command.BotUserName);
            Assert.Equal(new[] { "arg1", "arg2" }, command.Args);
        }

        [Theory]
        [InlineData("/cmd@bot1")]
        [InlineData("/cmd@bot1      ")]
        public void EmptyCommandWithBotname(string value)
        {
            var parser = new CommandParser();
            var command = parser.TryParse(value);

            Assert.NotNull(command);
            Assert.Equal("cmd", command.Name);
            Assert.Equal("bot1", command.BotUserName);
            Assert.Equal(null, command.Args);
        }

        [Theory]
        [InlineData("/cmd@bot1 arg1 arg2")]
        public void CommandWithBotname(string value)
        {
            var parser = new CommandParser();
            var command = parser.TryParse(value);

            Assert.NotNull(command);
            Assert.Equal("cmd", command.Name);
            Assert.Equal("bot1", command.BotUserName);
            Assert.Equal(new[] { "arg1", "arg2" }, command.Args);
        }

        [Theory]
        [InlineData("/cmd_arg1 arg2 arg3")]
        [InlineData("/cmd arg1_arg2 arg3")]
        [InlineData("/cmd_arg1@bot1 arg2 arg3")]
        public void AdditionalDelimiters(string value)
        {
            var parser = new CommandParser('_');
            var command = parser.TryParse(value);

            Assert.NotNull(command);
            Assert.Equal("cmd", command.Name);
            Assert.Equal(new[] { "arg1", "arg2", "arg3" }, command.Args);
        }
        */
    }
}
