using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    public class UpdatePollingManager<TBot> : IUpdatePollingManager<TBot>
             where TBot : BotBase
    {
        private readonly UpdateDelegate _updateDelegate;

        private readonly IBotServiceProvider _rootProvider;

        private ITelegramBotClient BotClient => _bot.Client;

        private BotBase _bot;

        public UpdatePollingManager(
            IBotBuilder botBuilder,
            IBotServiceProvider rootProvider
        )
        {
            // ToDo Receive update types array
            _updateDelegate = botBuilder.Build();
            _rootProvider = rootProvider;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            _bot = (TBot)_rootProvider.GetService(typeof(TBot));

            if (string.IsNullOrWhiteSpace(_bot.Username))
            {
                var botUser = await BotClient.GetMeAsync(cancellationToken)
                    .ConfigureAwait(false);
                _bot.Username = botUser.Username;
            }

            await BotClient.DeleteWebhookAsync(cancellationToken)
                .ConfigureAwait(false);

            await StartLongPollingAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        private async Task StartLongPollingAsync(CancellationToken cancellationToken = default)
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
                        var context = new UpdateContext(_bot, update, scopeProvider);
                        // ToDo deep clone bot instance for each update
                        await _updateDelegate(context)
                            .ConfigureAwait(false);
                    }
                }

                if (updates.Length > 0)
                {
                    offset = updates[updates.Length - 1].Id + 1;
                }
            }

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}