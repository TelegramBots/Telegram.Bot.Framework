using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    public class UpdatePollingManager<TBot> : IUpdatePollingManager<TBot>
             where TBot : IBot
    {
        private readonly UpdateDelegate _updateDelegate;

        private readonly IBotServiceProvider _rootProvider;

        public UpdatePollingManager(
            IBotBuilder botBuilder,
            IBotServiceProvider rootProvider
        )
        {
            // ToDo Receive update types array
            _updateDelegate = botBuilder.Build();
            _rootProvider = rootProvider;
        }

        public async Task RunAsync(
            GetUpdatesRequest requestParams = default,
            CancellationToken cancellationToken = default
        )
        {
            var bot = (TBot)_rootProvider.GetService(typeof(TBot));

            await bot.Client.DeleteWebhookAsync(cancellationToken)
                .ConfigureAwait(false);

            requestParams = requestParams ?? new GetUpdatesRequest
            {
                Offset = 0,
                Timeout = 500,
                AllowedUpdates = new UpdateType[0],
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                Update[] updates = await bot.Client.MakeRequestAsync(
                    requestParams,
                    cancellationToken
                ).ConfigureAwait(false);

                foreach (var update in updates)
                {
                    using (var scopeProvider = _rootProvider.CreateScope())
                    {
                        var context = new UpdateContext(bot, update, scopeProvider);
                        // ToDo deep clone bot instance for each update
                        await _updateDelegate(context)
                            .ConfigureAwait(false);
                    }
                }

                if (updates.Length > 0)
                {
                    requestParams.Offset = updates[updates.Length - 1].Id + 1;
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}