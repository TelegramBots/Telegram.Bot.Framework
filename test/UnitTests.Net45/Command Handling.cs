using Moq;
using System;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace UnitTests.Net45
{
    public class CommandHandling
    {
        [Theory(DisplayName = "Should accept handling valid \"/test\" commands for bot \"@Test_bot\"")]
        [InlineData("/test", "/test")]
        [InlineData("/test    ", "/test")]
        [InlineData("/test abc", "/test")]
        [InlineData("/TesT", "/tESt")]
        [InlineData("/test@test_bot", "/test@test_bot")]
        [InlineData("/test@test_bot ", "/test@test_bot")]
        [InlineData("/test@test_bot  !", "/test@test_bot")]
        public void Should_Parse_Valid_Commands(string text, string commandValue)
        {
            Mock<IBot> mockBot = new Mock<IBot>();
            mockBot.SetupGet(x => x.Username).Returns("Test_Bot");

            IBot bot = mockBot.Object;
            Message message = new Message
            {
                Text = text,
                Entities = new[]
                {
                    new MessageEntity
                    {
                        Type = MessageEntityType.BotCommand,
                        Offset = text.IndexOf(commandValue, StringComparison.OrdinalIgnoreCase),
                        Length = commandValue.Length
                    },
                },
            };

            bool result = bot.CanHandleCommand("test", message);

            Assert.True(result);
        }

        [Theory(DisplayName = "Should reject parsing non-command text messages as command \"/test\"")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("/\t")]
        [InlineData("    ")]
        [InlineData("I AM NOT A COMMAND")]
        [InlineData("/testt")]
        [InlineData("/@test_bot")]
        [InlineData("/tes@test_bot")]
        public void Should_Not_Parse_Invalid_Command_Text(string text)
        {
            Mock<IBot> mockBot = new Mock<IBot>();
            mockBot.SetupGet(x => x.Username).Returns("Test_Bot");

            IBot bot = mockBot.Object;
            Message message = new Message
            {
                Text = text,
            };

            bool result = bot.CanHandleCommand("test", message);

            Assert.False(result);
        }

        [Fact(DisplayName = "Should not accept handling non-text messages")]
        public void Should_Refuse_Handling_Non_Message_Updates()
        {
            Mock<IBot> mockBot = new Mock<IBot>();
            mockBot.SetupGet(x => x.Username).Returns("Test_Bot");

            IBot bot = mockBot.Object;
            Message message = new Message
            {
                Text = null,
            };

            bool result = bot.CanHandleCommand("test", message);

            Assert.False(result);
        }
    }
}