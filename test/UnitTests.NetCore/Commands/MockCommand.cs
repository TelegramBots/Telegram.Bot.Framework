using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace UnitTests.NetCore.Commands
{
    class MockCommand : CommandBase
    {
        private readonly Func<IUpdateContext, UpdateDelegate, string[], CancellationToken, Task> _handler;

        public MockCommand(Func<IUpdateContext, UpdateDelegate, string[], CancellationToken, Task> handler)
        {
            _handler = handler;
        }

        public override Task HandleAsync(IUpdateContext context, UpdateDelegate next, string[] args,
            CancellationToken cancellationToken)
            => _handler(context, next, args, cancellationToken);
    }
}