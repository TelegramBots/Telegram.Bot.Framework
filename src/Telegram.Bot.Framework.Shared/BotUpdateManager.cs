using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    public class BotUpdateManager<TBot> : IBotUpdateManager<TBot>
             where TBot : BotBase
    {
        private readonly UpdateDelegate _chain;
        private readonly IBotServiceProvider _rootProvider;

        private ITelegramBotClient BotClient => _bot.Client;

        private BotBase _bot;

        public BotUpdateManager(UpdateDelegate chain, IBotServiceProvider rootProvider)
        {
            _chain = chain;
            _rootProvider = rootProvider;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            _bot = (TBot)_rootProvider.GetService(typeof(TBot));

            var botUser = await BotClient.GetMeAsync(cancellationToken)
                .ConfigureAwait(false);
            _bot.Username = botUser.Username;

            //if (_rootProvider.TryGetBotOptions(out var options))
            //{
            //    // ToDo throw if invalid url or not HTTPS
            //    throw new NotImplementedException();
            //}
            //else
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
                        var context = new UpdateContext(_bot, update, scopeProvider);
                        // ToDo deep clone bot instance for each update
                        await _chain(context)
                            .ConfigureAwait(false);
                    }
                }

                if (updates.Length > 0)
                {
                    offset = updates[updates.Length - 1].Id + 1;
                }
            }
        }
    }
}