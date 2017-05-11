using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NetTelegram.Bot.Framework.Abstractions;
using NetTelegramBotApi;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegram.Bot.Framework
{
    /// <summary>
    /// Base class for implementing Bots
    /// </summary>
    /// <typeparam name="TBot">Type of Bot</typeparam>
    public abstract class BotBase<TBot> : IBot
        where TBot : class, IBot
    {
        /// <summary>
        /// Gets this Bot's user information
        /// </summary>
        public User BotUserInfo => _botUserInfo ?? (_botUserInfo = Bot.MakeRequestAsync(new GetMe()).Result);

        /// <summary>
        /// Gets this Bot's webhook information set on Telegram
        /// </summary>
        public WebhookInfo WebhookInfo => _webhookInfo ?? (_webhookInfo = MakeRequestAsync(new GetWebhookInfo()).Result);

        /// <summary>
        /// Instance of Telegram bot
        /// </summary>
        protected readonly TelegramBot Bot;

        /// <summary>
        /// Options used to the configure the bot instance
        /// </summary>
        protected BotOptions<TBot> BotOptions { get; }

        private User _botUserInfo;

        private WebhookInfo _webhookInfo;

        /// <summary>
        /// Initializes a new Bot
        /// </summary>
        /// <param name="botOptions">Options used to configure the bot</param>
        protected BotBase(IOptions<BotOptions<TBot>> botOptions)
        {
            BotOptions = botOptions.Value;
            Bot = new TelegramBot(BotOptions.ApiToken);
        }

        /// <summary>
        /// Responsible for handling bot updates that don't have any handler
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        public abstract Task HandleUnknownMessageAsync(Update update);

        /// <summary>
        /// Sends a HTTPS request to Telegram bot API
        /// </summary>
        /// <typeparam name="T">Type of expected response from Telegram Bot API</typeparam>
        /// <param name="request">Telegram API request call to be sent</param>
        /// <returns>Response from Telegram API</returns>
        public virtual async Task<T> MakeRequestAsync<T>(RequestBase<T> request)
        {
            return await Bot.MakeRequestAsync(request);
        }
    }
}
