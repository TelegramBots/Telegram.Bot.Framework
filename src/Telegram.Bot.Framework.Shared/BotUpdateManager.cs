using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    public class BotUpdateManager<TBot> : IBotUpdateManager<TBot>
             where TBot : class, IBot
    {
        private readonly IBotServiceProvider<TBot> _rootProvider;

        private ITelegramBotClient BotClient => _bot.Client;

        private TBot _bot;

        public BotUpdateManager(IBotServiceProvider<TBot> rootProvider)
        {
            _rootProvider = rootProvider;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            _bot = _rootProvider.GetBot();

            if (_bot.User == null && _bot is BotBase bb)
            {
                bb.User = await BotClient.GetMeAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            if (_rootProvider.TryGetBotOptions(out var options))
            {
                // ToDo throw if invalid url or not HTTPS
                throw new NotImplementedException();
            }
            else
            {
                await RunLongPollingAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        private async Task RunLongPollingAsync(CancellationToken cancellationToken = default)
        {
            int offset = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                Update[] updates = await BotClient.GetUpdatesAsync(
                    offset,
                    timeout: 500,
                    allowedUpdates: new UpdateType[0],
                    cancellationToken: cancellationToken
                ).ConfigureAwait(false);

                foreach (var update in updates)
                {
                    using (var scopeProvider = _rootProvider.CreateScope())
                    {
                        // ToDo deep clone bot instance for each update
                        await HandleUpdateAsync(_bot, scopeProvider, new UpdateContext(update))
                            .ConfigureAwait(false);
                    }
                }

                if (updates.Length > 0)
                {
                    offset = updates[updates.Length - 1].Id + 1;
                }
            }
        }

        private static async Task HandleUpdateAsync(TBot bot, IBotServiceProvider<TBot> scopeProvider, IUpdateContext context)
        {
            var handlersCollection = scopeProvider.GetHandlersCollection();
            var enumerator = handlersCollection.GetEnumerator();

            using (enumerator)
            {
                var next = ResolveNextDelegate(enumerator, scopeProvider, bot, context);
                await next(context)
                    .ConfigureAwait(false);
            }
        }

        private static UpdateDelegate ResolveNextDelegate(
            IEnumerator<IHandlerPredicate> enumerator,
            IBotServiceProvider<TBot> scopeProvider,
            IBot bot,
            IUpdateContext context
        )
        {
            if (enumerator.MoveNext())
            {
                var next = enumerator.Current;
                if (next.CanHandle(bot, context))
                {
                    var nextHandler = scopeProvider.GetHandler(next.Type);
                    return c => nextHandler.HandleAsync(bot, c,
                        ResolveNextDelegate(enumerator, scopeProvider, bot, context)
                    );
                }
                else
                {
                    return ResolveNextDelegate(enumerator, scopeProvider, bot, context);
                }
            }
            else
            {
                return _ => Task.FromResult(null as object); // ToDo log information. handler not found
            }
        }
    }
}