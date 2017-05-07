using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;
using RecurrentTasks;

namespace NetTelegramBot.Framework
{
    public class BotUpdateManager<TBot> : IBotUpdateManager<TBot>
        where TBot : IBot
    {
        private readonly TBot _bot;

        private long? _offset;

        public BotUpdateManager(TBot bot)
        {
            _bot = bot;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            await _bot.ProcessUpdateAsync(update);
        }

        public void Run(ITask currentTask)
        {
            IEnumerable<Update> updates;
            do
            {
                updates = _bot.MakeRequestAsync(new GetUpdates { Offset = _offset }).Result;

                foreach (var update in updates)
                {
                    HandleUpdateAsync(update).Wait();
                }

                if (updates.Any())
                {
                    _offset = updates.Last()?.UpdateId + 1;
                }
            } while (updates.Any());
        }
    }
}
