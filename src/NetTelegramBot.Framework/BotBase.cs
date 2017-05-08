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
        public User BotUserInfo
        {
            get
            {
                if (_botUserInfo == null)
                {
                    _botUserInfo = Bot.MakeRequestAsync(new GetMe()).Result;
                }
                return _botUserInfo;
            }
            protected set => _botUserInfo = value;
        }

        protected readonly TelegramBot Bot;

        private User _botUserInfo;

        private readonly BotOptions<TBot> _botOptions;

        protected BotBase(IOptions<BotOptions<TBot>> botOptions)
        {
            _botOptions = botOptions.Value;
            Bot = new TelegramBot(_botOptions.ApiToken);
        }

        public abstract Task HandleUnknownMessageAsync(Update update);

        public async Task<T> MakeRequestAsync<T>(RequestBase<T> request)
        {
            return await Bot.MakeRequestAsync(request);
        }
    }
}
