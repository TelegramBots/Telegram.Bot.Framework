using System;
using System.Collections.Generic;
using Moq;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Framework.Tests.Helpers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Xunit;

namespace Telegram.Bot.Framework.Tests
{
    public class CommandBaseTests
    {
        [Theory(DisplayName = "Accept handling specific commands")]
        [InlineData("/test", "/test")]
        [InlineData("/test    ", "/test")]
        [InlineData("/test abc", "/test")]
        [InlineData("/TesT", "/tESt")]
        [InlineData("/test@test_bot", "/test@test_bot")]
        [InlineData("/test@test_bot ", "/test@test_bot")]
        [InlineData("/test@test_bot  !", "/test@test_bot")]
        public void Should_Accept_Handling_All(string text, string commandValue)
        {
            const string botUsername = "Test_Bot";
            var mockBot = new Mock<IBot>();
            mockBot.SetupGet(x => x.UserName)
                .Returns(botUsername);

            var update = new Update
            {
                Message = new Message
                {
                    Text = text,
                    Entities = new List<MessageEntity>
                    {
                        new MessageEntity
                        {
                            Type = MessageEntityType.BotCommand,
                            Offset = text.IndexOf(commandValue, StringComparison.OrdinalIgnoreCase),
                            Length = commandValue.Length
                        },
                    },
                },
            };
            var sut = new TestCommand("Test");

            var actual = sut.CanHandleUpdate(mockBot.Object, update);

            Assert.True(actual);
        }

        [Theory(DisplayName = "Do not handle non-command text messages")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("/\t")]
        [InlineData("    ")]
        [InlineData("I AM NOT A COMMAND")]
        [InlineData("/testt")]
        [InlineData("/@test_bot")]
        [InlineData("/tes@test_bot")]
        public void Should_Refuse_Handling_Text_Messages(string text)
        {
            const string botUsername = "Test_Bot";
            var mockBot = new Mock<IBot>();
            mockBot.SetupGet(x => x.UserName)
                .Returns(botUsername);

            var update = new Update
            {
                Message = new Message
                {
                    Text = text,
                },
            };
            var sut = new TestCommand("Test");

            var actual = sut.CanHandleUpdate(mockBot.Object, update);

            Assert.False(actual);
        }

        [Fact(DisplayName = "Should refuse handling non-message updates")]
        public void Should_Refuse_Handling_Non_Message_Updates()
        {
            const string botUsername = "Test_Bot";
            var mockBot = new Mock<IBot>();
            mockBot.SetupGet(x => x.UserName)
                .Returns(botUsername);

            var update = new Update
            {
                CallbackQuery = new CallbackQuery()
            };
            IUpdateHandler sut = new TestCommand("Test");

            var actual = sut.CanHandleUpdate(mockBot.Object, update);

            Assert.False(actual);
        }
    }
}