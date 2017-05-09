using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NetTelegram.Bot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework
{
    public class BotManager<TBot> : IBotManager<TBot>
        where TBot : BotBase<TBot>
    {
        public string WebhookRoute { get; }

        private readonly TBot _bot;

        private readonly IUpdateParser<TBot> _updateParser;

        private readonly IBotOptions<TBot> _botOptions;

        private long? _offset;

        public BotManager(TBot bot, IUpdateParser<TBot> updateParser, IOptions<BotOptions<TBot>> botOptions)
        {
            _bot = bot;
            _updateParser = updateParser;
            _botOptions = botOptions.Value;
            WebhookRoute = _botOptions.WebhookRoute
                .Replace("{botname}", _botOptions.BotName)
                .Replace("{token}", _botOptions.ApiToken);
        }

        public async Task HandleUpdateAsync(Update update)
        {
            var handlers = _updateParser.FindHandlersForUpdate(_bot, update).ToArray();
            if (handlers.Any())
            {
                foreach (var handler in handlers)
                {
                    var result = await handler.HandleUpdateAsync(_bot, update);
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
            await EnsureWebhookDisabledForBot(_bot);

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
                    _offset = updates.Last().UpdateId + 1;
                }
            } while (updates.Any());
        }

        public async Task SetWebhook(string appBaseUrl)
        {
            if (!appBaseUrl.EndsWith("/"))
            {
                appBaseUrl += '/';
            }

            var webhookRoute = WebhookRoute;
            if (webhookRoute.StartsWith("/"))
            {
                webhookRoute = webhookRoute.Remove(0, 1);
            }

            var req = new SetWebhook(appBaseUrl + webhookRoute);
            var response = await _bot.MakeRequestAsync(req);
            if (!response)
            {
                throw new Exception($"Failed to set webhook. Telegram API response: false");
            }
        }

        private static async Task EnsureWebhookDisabledForBot(IBot bot)
        {
            if (!string.IsNullOrEmpty(bot.WebhookInfo.Url))
            {
                await bot.MakeRequestAsync(new SetWebhook(""));
            }
        }
    }
}
