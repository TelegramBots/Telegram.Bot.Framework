using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RecurrentTasks;
using Telegram.Bot.Framework.Abstractions;

namespace SampleBots
{
    public class BotUpdateGetterTask<TBot> : IRunnable
        where TBot : class, IBot
    {
        private readonly IBotManager<TBot> _botManager;

        private readonly ILogger _logger;

        public BotUpdateGetterTask(IBotManager<TBot> botManager, ILogger<BotUpdateGetterTask<TBot>> logger)
        {
            _botManager = botManager;
            _logger = logger;
        }

        public void Run(ITask currentTask, CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(async () =>
            {
                _logger.LogTrace($"{typeof(TBot).Name}: Checking for updates...");
                await _botManager.GetAndHandleNewUpdatesAsync();
                _logger.LogTrace($"{typeof(TBot).Name}: Handling updates finished");
            }, cancellationToken).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    throw task.Exception.InnerException;
                }
            }, cancellationToken);
        }
    }
}