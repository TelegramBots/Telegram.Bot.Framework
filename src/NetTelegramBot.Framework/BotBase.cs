using System.Linq;
using System.Threading.Tasks;
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

        protected readonly IUpdateParser<TBot> UpdateParser;

        private User _botUserInfo;

        protected BotBase(IBotOptions<TBot> botOptions, IUpdateParser<TBot> updateParser)
        {
            Bot = new TelegramBot(botOptions.ApiToken);
            UpdateParser = updateParser;
            UpdateParser.SetBot(this);
        }

        public abstract Task HandleUnknownMessageAsync(Update update);

        public async Task<T> MakeRequestAsync<T>(RequestBase<T> request)
        {
            return await Bot.MakeRequestAsync(request);
        }

        public virtual async Task ProcessUpdateAsync(Update update)
        {
            //if (update?.Message != null)
            {
                var handlers = UpdateParser.FindHandlersFor(update).ToArray();
                if (handlers.Any())
                {
                    foreach (var handler in handlers)
                    {
                        handler.Bot = this;
                        var result = await handler.HandleUpdateAsync(update);
                        if (result == UpdateHandlingResult.Handled)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    await HandleUnknownMessageAsync(update);
                }
            }
        }
    }
}
