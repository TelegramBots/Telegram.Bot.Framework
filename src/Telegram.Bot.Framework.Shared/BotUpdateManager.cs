using System;
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
        private readonly IBotServiceProvider<TBot> _provider;

        private ITelegramBotClient BotClient => _bot.Client;

        private TBot _bot;

        public BotUpdateManager(IBotServiceProvider<TBot> provider)
        {
            _provider = provider;
        }

        public Task RunAsync(CancellationToken cancellationToken = default)
        {
            _bot = _provider.GetBot();

            if (_provider.TryGetBotOptions(out var options))
            {
                // ToDo throw if invalid url or not HTTPS
                throw new NotImplementedException();
            }
            else
            {
                return RunLongPollingAsync(cancellationToken);
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
                    using (var provier = _provider.CreateScope())
                    {
                        await HandleUpdateAsync(provier, new UpdateContext(update))
                            .ConfigureAwait(false);
                    }
                }

                if (updates.Length > 0)
                {
                    offset = updates[updates.Length - 1].Id + 1;
                }
            }
        }

        private static async Task HandleUpdateAsync(IBotServiceProvider<TBot> provider, IUpdateContext context)
        {
            var bot = provider.GetBot();
            var accessor = provider.GetHandlerTypes();
            Type[] types = accessor.HandlerTypes;

            UpdateDelegate ResolveNextDelegate(int i)
            {
                if (i < types.Length)
                {
                    var h = provider.GetHandler(types[i]);
                    return c => h.HandleAsync(bot, c, ResolveNextDelegate(i + 1));
                }
                else
                {
                    return _ => Task.FromResult(null as object); // ToDo log information. handler not found
                }
            }

            var firstHandler = provider.GetHandler(types[0]);
            await firstHandler.HandleAsync(bot, context, ResolveNextDelegate(1))
                .ConfigureAwait(false);
        }
    }
}