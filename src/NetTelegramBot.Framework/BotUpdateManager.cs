using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;
using RecurrentTasks;

namespace NetTelegramBot.Framework
{
    public class BotUpdateManager : IBotUpdateManager<IBot>
    {
        private readonly IBot _bot;

        private long? _offset;

        public BotUpdateManager(IBot bot)
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

                _offset = updates.Last().UpdateId + 1;
            } while (updates.Any());
        }
    }
}
