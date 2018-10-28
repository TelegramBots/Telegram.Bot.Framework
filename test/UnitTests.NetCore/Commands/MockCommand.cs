using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace UnitTests.NetCore.Commands
{
    class MockCommand : CommandBase
    {
        private readonly Func<IUpdateContext, UpdateDelegate, string[], Task> _handler;

        public MockCommand(Func<IUpdateContext, UpdateDelegate, string[], Task> handler)
        {
            _handler = handler;
        }

        public override Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args)
            => _handler(context, next, args);
    }
}