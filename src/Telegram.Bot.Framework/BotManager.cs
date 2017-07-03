using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
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

        internal IBot Bot => _bot;

        internal BotGameOption[] BotGameOptions => _botOptions.GameOptions;

        private readonly TBot _bot;

        private readonly IUpdateParser<TBot> _updateParser;

        private readonly BotOptions<TBot> _botOptions;

        private int _offset;

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

            string webhook = _botOptions.BaseUrl;
            if (webhook[webhook.Length - 1] != '/')
                webhook += '/';
            webhook += $"{_botOptions.BotUserName}/webhook/{_botOptions.ApiToken}";

            WebhookUrl = webhook;
        }

        /// <summary>
        /// Handle the update
        /// </summary>
        /// <param name="update">Update to be handled</param>
        /// <returns></returns>
        public async Task HandleUpdateAsync(Update update)
        {
            bool anyHandlerExists = false;
            try
            {
                var handlers = _updateParser.FindHandlersForUpdate(_bot, update);

                foreach (IUpdateHandler handler in handlers)
                {
                    anyHandlerExists = true;

                    if (handler is IGameHandler)
                    {
                        IGameHandler gameHandler = handler as IGameHandler;
                        gameHandler.BotBaseUrl = _botOptions.BaseUrl +
                            $"{_botOptions.BotUserName}/";

                        string gameUrl = BotGameOptions
                            .Single(g => g.ShortName == gameHandler.ShortName)
                            .PageUrl;

                        gameHandler.GamePageUrl = gameUrl 
                            ?? _botOptions.BaseUrl + $"{_botOptions.BotUserName}/games/" + "{game}";
                    }

                    var result = await handler.HandleUpdateAsync(_bot, update);
                    if (result == UpdateHandlingResult.Handled)
                    {
                        return;
                    }
                }

                if (!anyHandlerExists)
                {
                    await _bot.HandleUnknownMessage(update);
                }
            }
            catch (Exception e)
            {
                await _bot.HandleFaultedUpdate(update, e);
            }
        }

        /// <summary>
        /// Pulls the updates from Telegram if any and passes them to handlers
        /// </summary>
        /// <returns></returns>
        public async Task GetAndHandleNewUpdatesAsync()
        {
            IEnumerable<Update> updates;
            do
            {
                updates = await _bot.Client.GetUpdatesAsync(_offset);

                foreach (var update in updates)
                {
                    await HandleUpdateAsync(update);
                }

                _offset = updates.LastOrDefault()?.Id + 1 ?? _offset;
            } while (updates.Any());
        }

        /// <summary>
        /// Enables or disables the webhook for this bot
        /// </summary>
        /// <param name="enabled">Whether webhook should be set or deleted</param>
        /// <remarks>
        /// Webhook url will be retrieved from bot's <see cref="BotOptions{TBot}"/>.
        /// Disabling webhook means user wants to use long polling method to get updates.
        /// </remarks>
        public Task SetWebhookStateAsync(bool enabled)
        {
            if (enabled)
            {
                var file = new FileStream(_botOptions.PathToCertificate, FileMode.Open);
                var fileToSend = new FileToSend("certificate.pem", file);
                return _bot.Client.SetWebhookAsync(WebhookUrl, fileToSend);
            }
            else
            {
                return _bot.Client.DeleteWebhookAsync(); // todo check if it always returns `true`
            }
        }

        public (bool Success, IUpdateHandler gameUpdateHandler) TryFindGameUpdateHandler(string gameShortName)
        {
            if (string.IsNullOrWhiteSpace(gameShortName))
                throw new ArgumentNullException(nameof(gameShortName));

            Update gameUpdate = new Update
            {
                CallbackQuery = new CallbackQuery
                {
                    GameShortName = gameShortName,
                },
            };

            IUpdateHandler gameHandler = _updateParser.FindHandlersForUpdate(_bot, gameUpdate)
                .SingleOrDefault();

            return (gameHandler != null, gameHandler);
        }

        internal static class Constants
        {
            public const string BotNamePlaceholder = "{botname}";

            public const string ApiTokenPlaceholder = "{token}";

            public const string GameShortNamePlaceholder = "{game}";
        }
    }
}
