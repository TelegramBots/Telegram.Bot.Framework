using Telegram.Bot.Abstractions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    public abstract class BotBase : IBot
    {
        public ITelegramBotClient Client { get; }

        public User User { get; internal set; }

        internal IBotOptions Options { get; }

        protected BotBase(string apiToken)
        {
            Client = new TelegramBotClient(apiToken);
        }

        protected BotBase(ITelegramBotClient client)
        {
            Client = client;
        }

        protected BotBase(IBotOptions options)
        {
            Options = options;
            Client = new TelegramBotClient(options.ApiToken);
        }

        protected BotBase(ITelegramBotClient client, IBotOptions options)
        {
            Client = client;
            Options = options;
        }
    }
}