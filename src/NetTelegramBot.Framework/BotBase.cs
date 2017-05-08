using System.Linq;
using System.Threading.Tasks;
using NetTelegramBot.Framework.Abstractions;
using NetTelegramBotApi;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace NetTelegramBot.Framework
{
    public abstract class BotBase<TBot> : IBot
        where TBot : IBot
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

        protected readonly IMessageParser<TBot> MessageParser;

        private User _botUserInfo;

        protected BotBase(IBotOptions<TBot> botOptions, IMessageParser<TBot> messageParser)
        {
            Bot = new TelegramBot(botOptions.ApiToken);
            MessageParser = messageParser;
            MessageParser.SetBot(this);
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
                var handlers = MessageParser.FindMessageHandlers(update).ToArray();
                if (handlers.Any())
                {
                    foreach (var handler in handlers)
                    {
                        handler.Bot = this;
                        await handler.HandleMessageAsync(update);
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
