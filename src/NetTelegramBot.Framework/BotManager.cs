using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public class BotManager<TBot> : IBotManager<TBot>
        where TBot : BotBase<TBot>
    {
        private readonly TBot _bot;

        private readonly IUpdateParser<TBot> _updateParser;

        private long? _offset;

        public BotManager(TBot bot, IUpdateParser<TBot> updateParser)
        {
            _bot = bot;
            _updateParser = updateParser;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            var handlers = _updateParser.FindHandlersFor(update).ToArray();
            if (handlers.Any())
            {
                foreach (var handler in handlers)
                {
                    handler.Bot = _bot;
                    var result = await handler.HandleUpdateAsync(update);
                    if (result == UpdateHandlingResult.Handled)
                    {
                        break;
                    }
                }
            }
            else
            {
                await _bot.HandleUnknownMessageAsync(update);
            }
        }

        public async Task GetAndHandleNewUpdatesAsync()
        {
            IEnumerable<Update> updates;
            do
            {
                updates = await _bot.MakeRequestAsync(new GetUpdates { Offset = _offset });

                foreach (var update in updates)
                {
                    await HandleUpdateAsync(update);
                }

                if (updates.Any())
                {
                    _offset = updates.Last()?.UpdateId + 1;
                }
            } while (updates.Any());
        }
    }
}
