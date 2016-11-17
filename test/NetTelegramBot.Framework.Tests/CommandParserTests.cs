namespace NetTelegramBot.Framework.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

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
            Assert.Equal("cmd", command.Command);
            Assert.Null(command.Bot);
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
            Assert.Equal("cmd", command.Command);
            Assert.Equal("bot1", command.Bot);
            Assert.Equal(null, command.Args);
        }

        [Theory]
        [InlineData("/cmd@bot1 arg1 arg2")]
        public void CommandWithBotname(string value)
        {
            var parser = new CommandParser();
            var command = parser.TryParse(value);

            Assert.NotNull(command);
            Assert.Equal("cmd", command.Command);
            Assert.Equal("bot1", command.Bot);
            Assert.Equal(new[] { "arg1", "arg2" }, command.Args);
        }

        [Theory]
        [InlineData("/cMD")]
        [InlineData("/cMd@bot1")]
        [InlineData("/Cmd arg1 arg2")]
        [InlineData("/cMd@bot1 arg1 arg2")]
        public void CommandInUppercase(string value)
        {
            var parser = new CommandParser(true);
            var command = parser.TryParse(value);

            Assert.NotNull(command);
            Assert.Equal("CMD", command.Command);
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
            Assert.Equal("cmd", command.Command);
            Assert.Equal(new[] { "arg1", "arg2", "arg3" }, command.Args);
        }
    }
}
