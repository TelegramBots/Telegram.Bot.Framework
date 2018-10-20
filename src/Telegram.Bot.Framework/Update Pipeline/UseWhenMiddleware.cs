using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework.Pipeline
{
    internal class UseWhenMiddleware : IUpdateHandler
    {
        private readonly Predicate<IUpdateContext> _predicate;

        private readonly UpdateDelegate _branch;

        public UseWhenMiddleware(Predicate<IUpdateContext> predicate, UpdateDelegate branch)
        {
            _predicate = predicate;
            _branch = branch;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next)
        {
            if (_predicate(context))
            {
                await _branch(context).ConfigureAwait(false);
            }

            await next(context).ConfigureAwait(false);
        }
    }
}
