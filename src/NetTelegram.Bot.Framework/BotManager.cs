using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NetTelegram.Bot.Framework.Abstractions;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework
{
    /// <summary>
    /// Manages bot and sends updates to handlers
    /// </summary>
    /// <typeparam name="TBot">Type of bot</typeparam>
    public class BotManager<TBot> : IBotManager<TBot>
        where TBot : BotBase<TBot>
    {
        /// <summary>
        /// Gets webhook's url from bot options provided
        /// </summary>
        public string WebhookUrl { get; }

        private readonly TBot _bot;

        private readonly IUpdateParser<TBot> _updateParser;

        private readonly IBotOptions<TBot> _botOptions;

        private long? _offset;

        /// <summary>
        /// Initializes a new Bot Manager
        /// </summary>
        /// <param name="bot">Bot to be managed</param>
        /// <param name="updateParser">List of update parsers for the bot</param>
        /// <param name="botOptions">Options used to configure the bot</param>
        public BotManager(TBot bot, IUpdateParser<TBot> updateParser, IOptions<BotOptions<TBot>> botOptions)
        {
            _bot = bot;
            _updateParser = updateParser;
            _botOptions = botOptions.Value;
            WebhookUrl = _botOptions.WebhookUrl
                .Replace("{botname}", _botOptions.BotUserName)
                .Replace("{token}", _botOptions.ApiToken);
        }

        /// <summary>
        /// Handle the update
        /// </summary>
        /// <param name="update">Update to be handled</param>
        /// <returns></returns>
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

        /// <summary>
        /// Pulls the updates from Telegram if any and passes them to handlers
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Sets webhook for this bot
        /// </summary>
        /// <returns></returns>
        public async Task SetWebhook()
        {
            await EnsureWebhookDisabledForBot(_bot);

            try
            {
                var file = new FileStream(_botOptions.PathToCertificate, FileMode.Open);
                var req = new SetWebhook(WebhookUrl, new FileToSend(file, "certificate.pem"));
                var response = await _bot.MakeRequestAsync(req);
                if (!response)
                {
                    throw new Exception($"Failed to set webhook. Telegram API response: false");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
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
