using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public abstract class BotBase<TBot> : IBot
        where TBot : class, IBot
    {
        public User BotUserInfo => _botUserInfo ?? (_botUserInfo = Bot.MakeRequestAsync(new GetMe()).Result);

        public WebhookInfo WebhookInfo => _webhookInfo ?? (_webhookInfo = MakeRequestAsync(new GetWebhookInfo()).Result);

        protected readonly TelegramBot Bot;

        private User _botUserInfo;

        private WebhookInfo _webhookInfo;

        protected BotOptions<TBot> BotOptions { get; }

        protected BotBase(IOptions<BotOptions<TBot>> botOptions)
        {
            BotOptions = botOptions.Value;
            Bot = new TelegramBot(BotOptions.ApiToken);
        }

        public abstract Task HandleUnknownMessageAsync(Update update);

        public virtual async Task<T> MakeRequestAsync<T>(RequestBase<T> request)
        {
            return await Bot.MakeRequestAsync(request);
        }
    }
}
