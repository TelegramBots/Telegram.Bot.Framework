using Xunit;

namespace NetTelegramBot.Framework.Tests
{
    public class CommandParserTests
    {
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
            Assert.Null(command.BotName);
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
            Assert.Equal("bot1", command.BotName);
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
            Assert.Equal("bot1", command.BotName);
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
    }
}
