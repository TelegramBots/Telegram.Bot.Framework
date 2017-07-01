﻿using Moq;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Framework.Tests.Helpers;
using Telegram.Bot.Types;
using Xunit;

namespace Telegram.Bot.Framework.Tests
{
    public class CommandBaseTests
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
            const string botUsername = "Test_Bot";
            var mockBot = new Mock<IBot>();
            mockBot.SetupGet(x => x.UserName)
                .Returns(botUsername);

            var update = new Update
            {
                Message = new Message
                {
                    Text = text,
                }
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
        public void ShouldRefuseHandlingTextMessages(string text)
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
                }
            };
            var sut = new TestCommand("Test");

            var actual = sut.CanHandleUpdate(mockBot.Object, update);

            Assert.False(actual);
        }
    }
}
