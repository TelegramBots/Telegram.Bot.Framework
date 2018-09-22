using Telegram.Bot.Framework.Abstractions;

namespace Telegram.Bot.Framework
{
    public abstract class BotBase : IBot
    {
        public ITelegramBotClient Client { get; }

        public string Username { get; internal set; }

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